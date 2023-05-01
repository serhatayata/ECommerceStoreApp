using IdentityServer.Api.Dtos.Base.Abstract;
using Newtonsoft.Json;

namespace IdentityServer.Api.Dtos.UserDtos
{
    public class UserUpdateDto : IUpdateDto
    {
        /// <summary>
        /// Name of the user
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Surname of the user
        /// </summary>
        [JsonProperty("surname")]
        public string Surname { get; set; }
        /// <summary>
        /// Username of the user
        /// </summary>
        [JsonProperty("userName")]
        public string UserName { get; set; }
        /// <summary>
        /// Current username of the user
        /// </summary>
        [JsonProperty("currentUserName")]
        public string CurrentUserName { get; set; }
    }
}
