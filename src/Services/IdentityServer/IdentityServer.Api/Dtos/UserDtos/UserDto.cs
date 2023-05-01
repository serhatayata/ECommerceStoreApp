using IdentityServer.Api.Dtos.Base.Abstract;

namespace IdentityServer.Api.Dtos.UserDtos
{
    public class UserDto : IDto
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
        public virtual string UserName { get; set; }
        /// <summary>
        /// E-mail of the user
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// Phone number of the user
        /// </summary>
        public virtual string PhoneNumber { get; set; }
    }
}
