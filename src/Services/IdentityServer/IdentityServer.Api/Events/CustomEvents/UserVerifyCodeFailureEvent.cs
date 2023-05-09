using IdentityServer4.Events;

namespace IdentityServer.Api.Events.CustomEvents
{
    public class UserVerifyCodeFailureEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of custom event
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="error">The error.</param>
        /// <param name="clientId">The client id</param>
        public UserVerifyCodeFailureEvent(string username, string error, string clientId = null) 
            : base(CustomEventCategories.Authentication,                  
                  "User Verify Code Failure",
                  EventTypes.Failure,
                  CustomEventIds.UserVerifyCodeFailure,
                  error)
        {
            Username = username;
            ClientId = clientId;
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        /// <value>
        /// The client id.
        /// </value>
        public string ClientId { get; set; }
    }
}
