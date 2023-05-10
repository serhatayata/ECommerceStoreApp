using IdentityServer.Api.Models.Base.Abstract;
using Newtonsoft.Json;

namespace IdentityServer.Api.Models.UserModels
{
    public class UserAddModel : IAddModel
    {
        /// <summary>
        /// Name of the user
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Surname of the user
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Username of the user
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// E-mail of the user
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Phone number of the user
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Password of the user
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Confirm Password of the user
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}
