namespace Blauhaus.Analytics.Abstractions.Http
{
    public static class AnalyticsHeaders
    {
        public const string Prefix = "X-Analytics-";

        public static class Operation
        {
            
            public static readonly string Id = $"{Prefix}{nameof(Operation)}{nameof(Id)}";
            public static readonly string Name = $"{Prefix}{nameof(Operation)}{nameof(Name)}";
        }

        public static class Session
        {
            
            public static readonly string Id =  $"{Prefix}{nameof(Session)}{nameof(Id)}";
            public static readonly string UserId =  $"{Prefix}{nameof(UserId)}";
            public static readonly string AccountId =  $"{Prefix}{nameof(AccountId)}";
            public static readonly string DeviceId =  $"{Prefix}{nameof(DeviceId)}";
            public static readonly string AppVersion =  $"{Prefix}{nameof(AppVersion)}";
        }

    }
}