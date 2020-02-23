//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Headers;
//using System.Runtime.CompilerServices;
//using Blauhaus.Analytics.Abstractions;
//using Blauhaus.Analytics.Abstractions.Config;
//using Blauhaus.Analytics.Abstractions.Http;
//using Blauhaus.Analytics.Abstractions.Operation;
//using Blauhaus.Analytics.Abstractions.Service;
//using Blauhaus.Analytics.Abstractions.Session;
//using Blauhaus.Analytics.Common.Service;
//using Blauhaus.Analytics.Common.Telemetry;
//using Blauhaus.Analytics.Console.ConsoleLoggers;
//using Blauhaus.Common.ValueObjects.BuildConfigs;
//using Microsoft.ApplicationInsights.DataContracts;

//namespace Blauhaus.Analytics.Server.Service
//{
//    public class AspNetCoreAnalyticsService : AnalyticsService, IAnalyticsServerService
//    {

//        public AspNetCoreAnalyticsService(
//            IApplicationInsightsConfig config, 
//            IConsoleLogger appInsightsLogger, 
//            ITelemetryClientProxy telemetryClient,
//            ITelemetryDecorator telemetryDecorator,
//            IBuildConfig currentBuildConfig)
//            : base(config, appInsightsLogger, telemetryClient, telemetryDecorator, currentBuildConfig)
//        {
//        }


//    }
//}