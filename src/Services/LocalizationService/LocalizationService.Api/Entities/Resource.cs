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
        public int? LanguageId { get; set; }
        /// <summary>
        /// Member Id using the resource
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// Tag of the resource
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// Value of the resource
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// ResourceCode of the resource
        /// </summary>
        public string ResourceCode { get; set; }
        /// <summary>
        /// LanguageCode of the resource
        /// </summary>
        public string LanguageCode { get; set; }
        /// <summary>
        /// Creation date of the resource
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Status of the resource
        /// </summary>
        public bool Status { get; set; }

        public virtual Language Language { get; set; }
        public virtual Member Member { get; set; }
    }
}
