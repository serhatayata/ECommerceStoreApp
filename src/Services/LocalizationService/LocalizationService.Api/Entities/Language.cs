using LocalizationService.Api.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalizationService.Api.Entities
{
    public class Language : IEntity
    {
        /// <summary>
        /// Id of the language
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Code of the language
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Display name of the language
        /// </summary>
        public string DisplayName { get; set; }

        public virtual ICollection<Resource> Resources { get; set; }
    }
}
