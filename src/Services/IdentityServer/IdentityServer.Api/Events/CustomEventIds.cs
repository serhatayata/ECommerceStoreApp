namespace IdentityServer.Api.Events
{
    public static class CustomEventIds
    {
        private const int AuthenticationEventsStart = 1000;

        public const int UserLoginSuccess = 1000;

        public const int UserLoginFailure = 1001;

        public const int UserLogoutSuccess = 1002;

        public const int UserVerifyCodeFailure = 1003;

        public const int ClientAuthenticationSuccess = 1010;

        public const int ClientAuthenticationFailure = 1011;

        public const int ApiAuthenticationSuccess = 1020;

        public const int ApiAuthenticationFailure = 1021;

        private const int TokenEventsStart = 2000;

        public const int TokenIssuedSuccess = 2000;

        public const int TokenIssuedFailure = 2001;

        public const int TokenRevokedSuccess = 2010;

        public const int TokenIntrospectionSuccess = 2020;

        public const int TokenIntrospectionFailure = 2021;

        private const int ErrorEventsStart = 3000;

        public const int UnhandledException = 3000;

        public const int InvalidClientConfiguration = 3001;

        private const int GrantsEventsStart = 4000;

        public const int ConsentGranted = 4000;

        public const int ConsentDenied = 4001;

        public const int GrantsRevoked = 4002;

        private const int DeviceFlowEventsStart = 5000;

        public const int DeviceAuthorizationSuccess = 5000;

        public const int DeviceAuthorizationFailure = 5001;
    }
}
