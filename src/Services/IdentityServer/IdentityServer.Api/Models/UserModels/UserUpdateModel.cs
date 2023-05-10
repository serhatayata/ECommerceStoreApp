using IdentityServer.Api.Models.Base.Abstract;
using Newtonsoft.Json;

namespace IdentityServer.Api.Models.UserModels
{
    public class UserUpdateModel : IUpdateModel
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
        /// Current username of the user
        /// </summary>
        public string CurrentUserName { get; set; }
    }
}
