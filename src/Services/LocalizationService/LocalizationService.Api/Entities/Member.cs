using LocalizationService.Api.Entities.Abstract;

namespace LocalizationService.Api.Entities
{
    public class Member : IEntity
    {
        /// <summary>
        /// Id of the member
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the member
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Key of the member
        /// </summary>
        public string MemberKey { get; set; }
        /// <summary>
        /// Localization Prefix of the member
        /// </summary>
        public string LocalizationPrefix { get; set; }
        /// <summary>
        /// Creation time of the member
        /// </summary>
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Resource> Resources { get; set; }
    }
}
