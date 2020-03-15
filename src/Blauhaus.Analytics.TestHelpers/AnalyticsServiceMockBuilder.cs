using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Common.TestHelpers;
using Moq;

namespace Blauhaus.Analytics.TestHelpers
{
     public class AnalyticsServiceMockBuilder : BaseMockBuilder<AnalyticsServiceMockBuilder, IAnalyticsService>
    {
        public void VerifyLogEvent(string eventName)
        {
            Mock.Verify(x => x.LogEvent(It.IsAny<object>(), eventName, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
        }

        public void VerifyTrace(string traceMessage, LogSeverity severity)
        {
            Mock.Verify(x => x.Trace(It.IsAny<object>(), traceMessage, severity, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
        }
        
        public void VerifyTraceProperty<T>(string key, T value) where T : class
        {
            Mock.Verify(x => x.Trace(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<LogSeverity>(), 
                It.Is<Dictionary<string, object>>(y => 
                    (y[key] as T).Equals(value)), 
                It.IsAny<string>()));
        }

        
        public void VerifyEventProperty<T>(string key, T value) where T : class
        {
            Mock.Verify(x => x.LogEvent(It.IsAny<object>(), It.IsAny<string>(), 
                It.Is<Dictionary<string, object>>(y => 
                    ((T)y[key] ).Equals(value)), It.IsAny<string>()));
        }

        public void VerifyEventProperty<T>(string key, Expression<Func<T,bool>> predicate) where T : class
        {
            Mock.Verify(x => x.LogEvent(It.IsAny<object>(), It.IsAny<string>(), 
                It.Is<Dictionary<string, object>>(y => 
                    (y[key] as T) == It.Is<T>(predicate)), It.IsAny<string>()));
        }


        public void VerifyEventProperty(Expression<Func<Dictionary<string, object>, bool>> match) 
        {
            Mock.Verify(x => x.LogEvent(It.IsAny<object>(), It.IsAny<string>(), 
                It.Is(match), It.IsAny<string>()));
        }
    }
}