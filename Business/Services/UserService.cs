using Business.Handlers;
using Business.Models;
using Data.Entitites;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Domain.Responses;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Business.Services;

public interface IUserService
{
    Task<UserResult> AddUserToRoleAsync(UserEntity user, string roleName);
    Task<UserResult<User?>> CreateMemberAsync(AddMemberFormData formData, string roleName = "User");
    Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User");
    Task<string> GetDisplayNameAsync(string userId);
    Task<UserResult<User>> GetUserByIdAsync(string id);
    Task<UserResult<IEnumerable<User>>> GetUsersAsync();
    Task<UserResult> UserExistsByEmailAsync(string email);
}

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager, IImageHandler imageHandler) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IImageHandler _imageHandler = imageHandler;

    public async Task<UserResult<IEnumerable<User>>> GetUsersAsync()
    {
        var repositoryResult = await _userRepository.GetAllAsync
            (
                orderByDescending: false,
                sortByColumn: x => x.FirstName!
            );

        var users = repositoryResult.Result;

        return new UserResult<IEnumerable<User>> { Succeeded = true, StatusCode = 200, Result = users };
    }
    public async Task<UserResult<User>> GetUserByIdAsync(string id)
    {
        var repositoryResult = await _userRepository.GetAsync(x => x.Id == id);

        var user = repositoryResult.Result;
        if (user == null)
            return new UserResult<User> { Succeeded = false, StatusCode = 404, Error = $"User with id '{id}' was not found." };

        return new UserResult<User> { Succeeded = true, StatusCode = 200, Result = user };
    }
    public async Task<UserResult> UserExistsByEmailAsync(string email)
    {
        var existsResult = await _userRepository.ExistsAsync(x => x.Email == email);
        if (existsResult.Succeeded)
            return new UserResult { Succeeded = true, StatusCode = 200, Error = "A user with the specified email address exists." };

        return new UserResult { Succeeded = false, StatusCode = 404, Error = "User was not found." };
    }
    public async Task<UserResult> AddUserToRoleAsync(UserEntity user, string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded ? new UserResult { Succeeded = true, StatusCode = 200 } : new UserResult { Succeeded = false, StatusCode = 500, Error = "Unable to add user to role" };
        }

        return new UserResult { Succeeded = false, StatusCode = 404, Error = "Role does not exist" };
    }

    public async Task<string> GetDisplayNameAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return "";

        var user = await _userManager.FindByIdAsync(userId);
        return user == null ? "" : $"{user.FirstName} {user.LastName}";
    }

    public async Task<UserResult> CreateUserAsync(SignUpFormData formData, string roleName = "User")
    {
        if (formData == null)
            return new UserResult { Succeeded = false, StatusCode = 400, Error = "Form data can't be null." };

        var exists = await _userRepository.ExistsAsync(x => x.Email == formData.Email);
        if (exists.Succeeded)
            return new UserResult { Succeeded = false, StatusCode = 409, Error = "User with same email already exists." };
        try
        {
            var userEntity = formData.MapTo<UserEntity>();
            userEntity.UserName = formData.Email;
            var result = await _userManager.CreateAsync(userEntity, formData.Password);

            if (result.Succeeded)
            {
                var addToRoleResult = await AddUserToRoleAsync(userEntity, roleName);

                return result.Succeeded ?
                new UserResult { Succeeded = true, StatusCode = 201 }
            : new UserResult { Succeeded = false, StatusCode = 201, Error = "User successfully created but not added to role." };
            }

            return new UserResult { Succeeded = false, StatusCode = 500, Error = "Unable to create user" };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new UserResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }


    public async Task<UserResult<User?>> CreateMemberAsync(AddMemberFormData formData, string roleName = "User")
    {
        if (formData == null)
            return new UserResult<User?> { Succeeded = false, StatusCode = 400, Error = "Form data can't be null." };

        var exists = await _userRepository.ExistsAsync(x => x.Email == formData.Email);
        if (exists.Succeeded)
            return new UserResult<User?> { Succeeded = false, StatusCode = 409, Error = "User with same email already exists." };


        try
        {
            var userEntity = formData.MapTo<UserEntity>();

            var imageFileName = await _imageHandler.SaveProfileImageAsync(formData.Image!);
            userEntity.Image = imageFileName;
            userEntity.UserName = formData.Email;

            var result = await _userManager.CreateAsync(userEntity);

            if (result.Succeeded)
            {
                var addToRoleResult = await AddUserToRoleAsync(userEntity, roleName);

                return result.Succeeded ?
                new UserResult<User?> { Succeeded = true, StatusCode = 201 }
                : new UserResult<User?> { Succeeded = false, StatusCode = 201, Error = "User successfully created but not added to role." };
            }

            return new UserResult<User?> { Succeeded = false, StatusCode = 500, Error = "Unable to create user" };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new UserResult<User?> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }

    }
}
