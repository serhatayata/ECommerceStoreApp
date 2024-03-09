using FileService.Api.Models.ImageModels;

namespace FileService.Api.Entities;

public class Image : IEntity
{
    /// <summary>
    /// Id of image
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// FK - File type is actually used for grouping the files, for example Category or product
    /// </summary>
    public int FileUserId { get; set; }

    /// <summary>
    /// Entity id for example CategoryId or ProductId
    /// </summary>
    public int EntityId { get; set; }

    /// <summary>
    /// Entity id for example CategoryId or ProductId
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Created date
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Image type
    /// </summary>
    public ImageType Type { get; set; }

    public FileUser FileUser { get; set; }
}
