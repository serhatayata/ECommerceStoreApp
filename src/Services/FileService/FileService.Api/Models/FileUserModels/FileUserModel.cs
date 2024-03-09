namespace FileService.Api.Models.FileUserModels;

public class FileUserModel
{
    /// <summary>
    /// Id of file type
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the file type
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the file type
    /// </summary>
    public string Description { get; set; }
}
