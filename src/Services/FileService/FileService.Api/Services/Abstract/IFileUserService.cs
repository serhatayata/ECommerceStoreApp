using FileService.Api.Models.FileUserModels;
using FileService.Api.Utilities.Results;

namespace FileService.Api.Services.Abstract;

public interface IFileUserService
{
    /// <summary>
    /// Adds a file user
    /// </summary>
    /// <param name="model">file user model</param>
    /// <returns>Url value</returns>
    Task<Result> AddAsync(FileUserModel model);

    /// <summary>
    /// Updates a file user
    /// </summary>
    /// <param name="model">file user model</param>
    /// <returns>Url value</returns>
    Task<Result> UpdateAsync(FileUserModel model);

    /// <summary>
    /// Delete file user by id
    /// </summary>
    /// <param name="id">id of the file user</param>
    /// <returns>If the file user deleted or not</returns>
    Task<Result> DeleteAsync(int id);

    /// <summary>
    /// Get file user by id
    /// </summary>
    /// <param name="id">file user id</param>
    /// <returns>image data</returns>
    Task<DataResult<FileUserModel>> GetByIdAsync(int id);

    /// <summary>
    /// Get file user by id
    /// </summary>
    /// <param name="id">file user id</param>
    /// <returns>image data</returns>
    Task<DataResult<List<FileUserModel>>> GetByNameAsync(string name);
}
