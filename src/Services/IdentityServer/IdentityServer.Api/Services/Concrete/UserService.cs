using AutoMapper;
using IdentityModel;
using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.IncludeOptions.User;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Services.Redis.Abstract;
using IdentityServer.Api.Utilities.Results;
using IdentityServer.Api.Utilities.Security.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityServer.Api.Services.Concrete
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IMapper _mapper;
        private IRedisService _redisCacheService;
        private IConfiguration _configuration;
        private IJwtHelper _jwtHelper;
        private ILogger<UserService> _logger;

        private LoginOptions loginOptions;

        public UserService(UserManager<User> userManager, 
                           SignInManager<User> signInManager,
                           IRedisService redisCacheService,
                           IMapper mapper,
                           IConfiguration configuration,
                           IJwtHelper jwtHelper,
                           ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _redisCacheService = redisCacheService;
            _mapper = mapper;
            _configuration = configuration;
            _jwtHelper = jwtHelper;
            _logger = logger;

            this.loginOptions = _configuration.GetSection("LoginOptions").Get<LoginOptions>();
        }

        public async Task<DataResult<UserLoginResponse>> GetLoginCodeAsync(UserLoginModel model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync();
            if (user == null)
                return new ErrorDataResult<UserLoginResponse>("User not found");

            var checkUser = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
            if (checkUser.IsLockedOut)
                return new ErrorDataResult<UserLoginResponse>("You entered wrong credentials for 3 times, wait 180 seconds and try again");
            else if (!checkUser.Succeeded)
                return new ErrorDataResult<UserLoginResponse>("Invalid username or password");

            int verifyCodeDuration = loginOptions.VerifyCodeDuration;
            string verifyCodeRole = loginOptions.VerifyCodeRole;

            int databaseId = loginOptions.DatabaseId;

            var userRoles = await _userManager.GetRolesAsync(user);
            var userClaims = ClaimExtensions.GetClaims(userRoles.ToList());

            var accessToken = _jwtHelper.CreateToken(user, new List<Claim>() { new Claim(JwtClaimTypes.Role, verifyCodeRole) }, verifyCodeDuration, false);

            var code = RandomExtensions.RandomCode(6);
            await _redisCacheService.SetAsync($"{loginOptions.Prefix}{user.UserName}", code, verifyCodeDuration, databaseId);

            var response = new UserLoginResponse(user.UserName, code, accessToken);

            return new SuccessDataResult<UserLoginResponse>(response);
        }

        public async Task<DataResult<UserModel>> AddAsync(UserAddModel model)
        {
            var existingMail = await _userManager.FindByEmailAsync(model.Email);
            if (existingMail != null)
                return new ErrorDataResult<UserModel>();

            var existingUserName = await _userManager.FindByNameAsync(model.UserName);
            if (existingUserName != null)
                return new ErrorDataResult<UserModel>();

            var addedUser = _mapper.Map<User>(model);
            addedUser.TwoFactorEnabled = true;
            var result = await _userManager.CreateAsync(addedUser, model.Password);
            if (!result.Succeeded)
                return new ErrorDataResult<UserModel>();

            var resultValue = _mapper.Map<UserModel>(model);
            return new SuccessDataResult<UserModel>(resultValue);
        }

        public async Task<DataResult<UserModel>> UpdateAsync(UserUpdateModel model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.CurrentUserName);
            if (existingUser == null)
                return new ErrorDataResult<UserModel>();

            var updatedUser = _mapper.Map<UserUpdateModel,User>(model, existingUser);
            var result = await _userManager.UpdateAsync(updatedUser);
            if (!result.Succeeded)
                return new ErrorDataResult<UserModel>();

            var resultValue = _mapper.Map<UserModel>(updatedUser);
            return new SuccessDataResult<UserModel>(resultValue);
        }

        public async Task<Result> DeleteAsync(StringModel model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.Value);
            if (existingUser == null)
                return new ErrorResult();

            var result = await _userManager.DeleteAsync(existingUser);
            if (!result.Succeeded)
                return new ErrorResult();
            return new SuccessResult();
        }

        public async Task<DataResult<UserModel>> GetAsync(StringModel model, UserIncludeOptions options)
        {
            var existingUser = await _userManager.FindByNameAsync(model.Value);
            if (existingUser == null)
                return new ErrorDataResult<UserModel>();

            var returnValue = _mapper.Map<UserModel>(existingUser);
            return new SuccessDataResult<UserModel>(returnValue);
        }

        public async Task<DataResult<List<UserModel>>> GetAllAsync(UserIncludeOptions options)
        {
            var users = await _userManager.Users.ToListAsync();
            if (users == null)
                return new ErrorDataResult<List<UserModel>>();

            var returnValue = _mapper.Map<List<UserModel>>(users);
            return new SuccessDataResult<List<UserModel>>(returnValue);
        }
    }
}
