namespace Blauhaus.Analytics.Abstractions.Http
{
    public static class AnalyticsHeaders
    {
        private const string _prefix = "X-Analytics-";
        public const string Prefix = _prefix + "Extra-";

        public static class Operation
        {
            
            public static readonly string Id = $"{_prefix}{nameof(Operation)}{nameof(Id)}";
            public static readonly string Name = $"{_prefix}{nameof(Operation)}{nameof(Name)}";
        }

        public static class Session
        {
            
            public static readonly string Id =  $"{_prefix}{nameof(Session)}{nameof(Id)}";
            public static readonly string UserId =  $"{_prefix}{nameof(UserId)}";
            public static readonly string AccountId =  $"{_prefix}{nameof(AccountId)}";
            public static readonly string DeviceId =  $"{_prefix}{nameof(DeviceId)}";
            public static readonly string AppVersion =  $"{_prefix}{nameof(AppVersion)}";
        }

    }
}