using IdentityServer.Api.Models.Base.Abstract;
using Newtonsoft.Json;

namespace IdentityServer.Api.Models.UserModels
{
    public class UserAddModel : IAddModel
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
        /// E-mail of the user
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
        /// <summary>
        /// Phone number of the user
        /// </summary>
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Password of the user
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
        /// <summary>
        /// Confirm Password of the user
        /// </summary>
        [JsonProperty("confirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
