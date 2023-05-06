// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityModel.Client;
using IdentityServer.Api.Attributes;
using IdentityServer.Api.Data.SeedData;
using IdentityServer.Api.Extensions.Authentication;
using IdentityServer.Api.Models.Account;
using IdentityServer.Api.Models.ApiResourceModels;
using IdentityServer.Api.Models.ApiScopeModels;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.ClientModels;
using IdentityServer.Api.Models.IdentityResourceModels;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Services.Concrete;
using IdentityServer.Api.Utilities;
using IdentityServer.Api.Utilities.Results;
using IdentityServer.Api.ViewModels.Account;
using IdentityServer4;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.Api.Controllers
{
    /// <summary>
    /// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
    /// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
    /// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
    /// </summary>
    [SecurityHeaders]
    public class AccountController : Controller
    {
        private readonly TestUserStore _users;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly IClientService _clientService;
        private readonly IApiResourceService _apiResourceService;
        private readonly IApiScopeService _apiScopeService;
        private readonly IIdentityResourceService _identityResourceService;
        private readonly IUserService _userService;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IClientService clientService,
            IApiResourceService apiResourceService,
            IApiScopeService apiScopeService,
            IIdentityResourceService identityResourceService,
            IUserService userService,
            TestUserStore users = null)
        {
            // if the TestUserStore is not in DI, then we'll just use the global users collection
            // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
            _users = users ?? new TestUserStore(TestUsers.Users);

            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _clientService = clientService;
            _apiResourceService = apiResourceService;
            _apiScopeService = apiScopeService;
            _identityResourceService = identityResourceService;
            _userService = userService;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildLoginViewModelAsync(returnUrl);

            if (vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
            }

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "login")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                // validate username/password against in-memory store
                if (_users.ValidateCredentials(model.Username, model.Password))
                {
                    var user = _users.FindByUsername(model.Username);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username, clientId: context?.Client.ClientId));

                    // only set explicit expiration here if user chooses "remember me". 
                    // otherwise we rely upon expiration configured in cookie middleware.
                    AuthenticationProperties props = null;
                    if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    };

                    // issue authentication cookie with subject ID and username
                    var isuser = new IdentityServerUser(user.SubjectId)
                    {
                        DisplayName = user.Username
                    };

                    await HttpContext.SignInAsync(isuser, props);

                    if (context != null)
                    {
                        if (context.IsNativeClient())
                        {
                            // The client is native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage("Redirect", model.ReturnUrl);
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        // user might have clicked on a malicious link - should be logged
                        throw new Exception("invalid return URL");
                    }
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }
        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();

                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if (vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region User
        [HttpPost]
        [Route("get-login-code")]
        [ProducesResponseType(typeof(DataResult<UserLoginResponse>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<UserLoginResponse>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public async Task<IActionResult> GetLoginCode([FromBody] UserLoginModel model)
        {
            var result = await _userService.GetLoginCodeAsync(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(DataResult<UserModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<UserModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserAddModel model)
        {
            var result = await _userService.AddAsync(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("update-user")]
        [ProducesResponseType(typeof(DataResult<UserModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<UserModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public async Task<IActionResult> Update([FromBody] UserUpdateModel model)
        {
            var result = await _userService.UpdateAsync(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete]
        [Route("delete-user")]
        [ProducesResponseType(typeof(Result), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public async Task<IActionResult> Delete([FromBody] StringModel model)
        {
            var result = await _userService.DeleteAsync(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet]
        [Route("get-user")]
        [ProducesResponseType(typeof(DataResult<UserModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<UserModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public async Task<IActionResult> GetUser([FromBody] StringModel model)
        {
            var result = await _userService.GetAsync(model, new Models.IncludeOptions.User.UserIncludeOptions());
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet]
        [Route("get-all-users")]
        [ProducesResponseType(typeof(DataResult<List<UserModel>>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<List<UserModel>>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllAsync(new Models.IncludeOptions.User.UserIncludeOptions());
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        #endregion
        #region Client
        [HttpPost]
        [Route("add-client")]
        [ProducesResponseType(typeof(DataResult<ClientModel>),(int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<ClientModel>),(int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult AddClient([FromBody] ClientAddModel client)
        {
            var result = _clientService.Add(client);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete]
        [Route("delete-client")]
        [ProducesResponseType(typeof(Result), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult DeleteClient([FromBody] StringModel model)
        {
            var result = _clientService.Delete(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-client")]
        [ProducesResponseType(typeof(DataResult<ClientModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<ClientModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult GetClient([FromBody] StringModel model)
        {
            var result = _clientService.Get(model, new Models.IncludeOptions.Account.ClientIncludeOptions());
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-clients")]
        [ProducesResponseType(typeof(DataResult<List<ClientModel>>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<List<ClientModel>>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult GetClients()
        {
            var result = _clientService.GetAll(new Models.IncludeOptions.Account.ClientIncludeOptions());
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        #endregion
        #region ApiScope
        [HttpPost]
        [Route("add-apiscope")]
        [ProducesResponseType(typeof(DataResult<ApiScopeModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<ApiScopeModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult AddApiScope([FromBody] ApiScopeAddModel apiScope)
        {
            var result = _apiScopeService.Add(apiScope);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete]
        [Route("delete-apiscope")]
        [ProducesResponseType(typeof(Result), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult DeleteApiScope([FromBody] StringModel model)
        {
            var result = _apiScopeService.Delete(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-apiscope")]
        [ProducesResponseType(typeof(DataResult<ApiScopeModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<ApiScopeModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult GetApiScope([FromBody]StringModel model)
        {
            var result = _apiScopeService.Get(model, new Models.IncludeOptions.Account.ApiScopeIncludeOptions());
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-apiscopes")]
        [ProducesResponseType(typeof(DataResult<List<ApiScopeModel>>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<List<ApiScopeModel>>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult GetApiScopes()
        {
            var result = _apiScopeService.GetAll(new Models.IncludeOptions.Account.ApiScopeIncludeOptions());
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        #endregion
        #region ApiResource
        [HttpPost]
        [Route("add-apiresource")]
        [ProducesResponseType(typeof(DataResult<ApiResourceModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<ApiResourceModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult AddApiResource([FromBody] ApiResourceAddModel apiResource)
        {
            var result = _apiResourceService.Add(apiResource);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete]
        [Route("delete-apiresource")]
        [ProducesResponseType(typeof(Result), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult DeleteApiResource([FromBody] StringModel model)
        {
            var result = _apiResourceService.Delete(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-apiresource")]
        [ProducesResponseType(typeof(DataResult<ApiResourceModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<ApiResourceModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult GetApiResource([FromBody] StringModel model)
        {
            var result = _apiResourceService.Get(model, new Models.IncludeOptions.Account.ApiResourceIncludeOptions());
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-apiresources")]
        [ProducesResponseType(typeof(DataResult<List<ApiResourceModel>>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<List<ApiResourceModel>>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult GetApiResources()
        {
            var result = _apiResourceService.GetAll(new Models.IncludeOptions.Account.ApiResourceIncludeOptions());
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        #endregion
        #region IdentityResource
        [HttpPost]
        [Route("add-identityresource")]
        [ProducesResponseType(typeof(DataResult<IdentityResourceModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<IdentityResourceModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult AddIdentityResource([FromBody] IdentityResourceAddModel apiResource)
        {
            var result = _identityResourceService.Add(apiResource);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete]
        [Route("delete-identityresource")]
        [ProducesResponseType(typeof(Result), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult DeleteIdentityResource([FromBody] StringModel model)
        {
            var result = _identityResourceService.Delete(model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-identityresource")]
        [ProducesResponseType(typeof(DataResult<IdentityResourceModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<IdentityResourceModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult GetIdentityResource([FromBody] StringModel model)
        {
            var result = _identityResourceService.Get(model, new IdentityResourceIncludeOptions());
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-identityresources")]
        [ProducesResponseType(typeof(DataResult<List<IdentityResourceModel>>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<List<IdentityResourceModel>>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(LocalApi.PolicyName)]
        public IActionResult GetIdentityResources()
        {
            var result = _identityResourceService.GetAll(new IdentityResourceIncludeOptions());
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        #endregion

        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/

        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }
        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }
        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }
        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        if (vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}
