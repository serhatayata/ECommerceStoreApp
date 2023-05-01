using AutoMapper;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Dtos.Base.Concrete;
using IdentityServer.Api.Dtos.UserDtos;
using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Models.IncludeOptions.User;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Services.Base;
using IdentityServer.Api.Utilities.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Services.Concrete
{
    public class UserService : IUserService
    {
        private AppIdentityDbContext _identityDbContext;
        private UserManager<User> _userManager;
        private IMapper _mapper;

        public UserService(AppIdentityDbContext identityDbContext, UserManager<User> userManager, IMapper mapper)
        {
            _identityDbContext = identityDbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public UserService(AppIdentityDbContext identityDbContext, IMapper mapper)
        {
            _identityDbContext = identityDbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="model">User add model</param>
        /// <returns><see cref="DataResult{T}"/></returns>
        public async Task<DataResult<UserDto>> AddAsync(UserAddDto model)
        {
            var existingMail = await _userManager.FindByEmailAsync(model.Email);
            if (existingMail != null)
                return new ErrorDataResult<UserDto>();

            var existingUserName = await _userManager.FindByNameAsync(model.UserName);
            if (existingUserName != null)
                return new ErrorDataResult<UserDto>();

            var addedUser = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(addedUser, model.Password);
            if (!result.Succeeded)
                return new ErrorDataResult<UserDto>();

            var resultValue = _mapper.Map<UserDto>(model);
            return new SuccessDataResult<UserDto>(resultValue);
        }

        /// <summary>
        /// Update the specified user
        /// </summary>
        /// <param name="model">User update model</param>
        /// <returns><see cref="{T}"/></returns>
        public async Task<DataResult<UserDto>> UpdateAsync(UserUpdateDto model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.CurrentUserName);
            if (existingUser == null)
                return new ErrorDataResult<UserDto>();

            var updatedUser = _mapper.Map<UserUpdateDto,User>(model, existingUser);
            var result = await _userManager.UpdateAsync(updatedUser);
            if (!result.Succeeded)
                return new ErrorDataResult<UserDto>();

            var resultValue = _mapper.Map<UserDto>(updatedUser);
            return new SuccessDataResult<UserDto>(resultValue);
        }

        /// <summary>
        /// Delete the specified user
        /// </summary>
        /// <param name="model">Delete model</param>
        /// <returns><see cref="{R}"/></returns>
        public async Task<Result> DeleteAsync(StringDto model)
        {
            var existingUser = await _userManager.FindByNameAsync(model.Value);
            if (existingUser == null)
                return new ErrorResult();

            var result = await _userManager.DeleteAsync(existingUser);
            if (!result.Succeeded)
                return new ErrorResult();
            return new SuccessResult();
        }

        /// <summary>
        /// Get specified user
        /// </summary>
        /// <param name="id">id of the user</param>
        /// <param name="options">get include options</param>
        /// <returns><see cref="{T}"/></returns>
        public async Task<DataResult<UserDto>> GetAsync(StringDto model, UserIncludeOptions options)
        {
            var existingUser = await _userManager.FindByNameAsync(model.Value);
            if (existingUser == null)
                return new ErrorDataResult<UserDto>();

            var returnValue = _mapper.Map<UserDto>(existingUser);
            return new SuccessDataResult<UserDto>(returnValue);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="options">get include options</param>
        /// <returns><see cref="List{T}"/></returns>
        public async Task<DataResult<List<UserDto>>> GetAllAsync(UserIncludeOptions options)
        {
            var users = await _userManager.Users.ToListAsync();
            if (users == null)
                return new ErrorDataResult<List<UserDto>>();

            var returnValue = _mapper.Map<List<UserDto>>(users);
            return new SuccessDataResult<List<UserDto>>(returnValue);
        }
    }
}
