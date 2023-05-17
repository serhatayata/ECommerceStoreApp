using LocalizationService.Api.Entities.Abstract;

namespace LocalizationService.Api.Entities
{
    public class Resource : IEntity
    {
        /// <summary>
        /// Id of the resource
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Language Id of the resource
        /// </summary>
        public int LanguageId { get; set; }
        /// <summary>
        /// Member Id using the resource
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// Tag of the resource
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Code of the resource
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Creation date of the resource
        /// </summary>
        public DateTime CreateDate { get; set; }

        public Language Language { get; set; }
        public Member Member { get; set; }
    }
}
