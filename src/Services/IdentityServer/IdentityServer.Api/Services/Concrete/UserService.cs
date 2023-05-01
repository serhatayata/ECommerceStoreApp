using AutoMapper;
using IdentityModel.Client;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.IncludeOptions.User;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Utilities.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Services.Concrete
{
    public class UserService : IUserService
    {
        private AppIdentityDbContext _identityDbContext;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;

        public UserService(AppIdentityDbContext identityDbContext, 
                           UserManager<User> userManager, 
                           IMapper mapper, 
                           SignInManager<User> signInManager)
        {
            _identityDbContext = identityDbContext;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
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
