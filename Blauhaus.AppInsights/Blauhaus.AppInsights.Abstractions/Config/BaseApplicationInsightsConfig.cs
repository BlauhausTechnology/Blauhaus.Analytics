namespace Blauhaus.AppInsights.Abstractions.Config
{
    public abstract class BaseApplicationInsightsConfig : IApplicationInsightsConfig
    {
        protected BaseApplicationInsightsConfig(string instrumentationKey)
        {
            InstrumentationKey = instrumentationKey;
        }

        public string InstrumentationKey { get; }
    }
}