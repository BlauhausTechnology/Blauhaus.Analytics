using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blauhaus.Analytics.Abstractions.Operation;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.Analytics.TestHelpers.MockBuilders
{
     public class AnalyticsServiceMockBuilder : BaseMockBuilder<AnalyticsServiceMockBuilder, IAnalyticsService>
     {

         public AnalyticsServiceMockBuilder()
         {
             Mock.Setup(x => x.CurrentSession).Returns(MockCurrentSession.Object);
             Where_StartOperation_returns_operation();
         }
         
        #region Session

         private AnalyticsSessionMockBuilder _mockCurrentSession;
         public AnalyticsSessionMockBuilder MockCurrentSession => _mockCurrentSession ??= new AnalyticsSessionMockBuilder();

        #endregion

        #region LogEvent

        public AnalyticsServiceMockBuilder VerifyLogEvent(string eventName)
        {
            Mock.Verify(x => x.LogEvent(It.IsAny<object>(), eventName, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            return this;
        }
        
        public AnalyticsServiceMockBuilder VerifyEventProperty<T>(string key, T value) 
        {
            Mock.Verify(x => x.LogEvent(It.IsAny<object>(), It.IsAny<string>(), 
                It.Is<Dictionary<string, object>>(y => 
                    ((T)y[key] ).Equals(value)), It.IsAny<string>()));
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyEventProperty(Expression<Func<Dictionary<string, object>, bool>> match) 
        {
            Mock.Verify(x => x.LogEvent(It.IsAny<object>(), It.IsAny<string>(), It.Is(match), It.IsAny<string>()));
            return this;
        }
         

        #endregion

        #region Trace
        public AnalyticsServiceMockBuilder VerifyTrace(string traceMessage, LogSeverity severity = LogSeverity.Verbose)
        {
            Mock.Verify(x => x.Trace(It.IsAny<object>(), traceMessage, severity, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            return this;
        }
        
        public AnalyticsServiceMockBuilder VerifyTraceProperty<T>(string key, T value)
        {
            Mock.Verify(x => x.Trace(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<LogSeverity>(), 
                It.Is<Dictionary<string, object>>(y =>
                    y.ContainsKey(key) &&
                    ((T)y[key]).Equals(value)),
                It.IsAny<string>()));
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyTraceProperty(Expression<Func<Dictionary<string, object>, bool>> match)
        {
            Mock.Verify(x => x.Trace(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<LogSeverity>(), It.Is(match), It.IsAny<string>()));
            return this;
        }

        #endregion
        
        #region StartTrace

        public AnalyticsServiceMockBuilder VerifyStartTrace(string traceMessage, LogSeverity severity = LogSeverity.Verbose)
        {
            Mock.Verify(x => x.StartTrace(It.IsAny<object>(), traceMessage, severity, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            return this;
        }
        
        public AnalyticsServiceMockBuilder VerifyStartTraceProperty<T>(string key, T value)
        {
            Mock.Verify(x => x.StartTrace(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<LogSeverity>(), 
                It.Is<Dictionary<string, object>>(y =>
                    y.ContainsKey(key) &&
                    ((T)y[key]).Equals(value)),
                It.IsAny<string>()));
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyStartTraceProperty(Expression<Func<Dictionary<string, object>, bool>> match)
        {
            Mock.Verify(x => x.StartTrace(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<LogSeverity>(), It.Is(match), It.IsAny<string>()));
            return this;
        }

        public MockBuilder<IAnalyticsOperation> Where_StartTrace_returns_operation()
        {
            var operation = new MockBuilder<IAnalyticsOperation>();
            
            Mock.Verify(x => x.StartTrace(It.IsAny<object>(), 
                It.IsAny<string>(), It.IsAny<LogSeverity>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));

            return operation;
        }

        #endregion
        
        #region LogException

        public AnalyticsServiceMockBuilder VerifyLogException(Exception exception)
        {
            Mock.Verify(x => x.LogException(It.IsAny<object>(), exception, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyLogExceptionWithMessage(string message)
        {
            Mock.Verify(x => x.LogException(It.IsAny<object>(), It.Is<Exception>(y => y.Message == message), It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyLogExceptionWithMessage(Expression<Func<Exception, bool>> predicate)
        {
            Mock.Verify(x => x.LogException(It.IsAny<object>(), It.Is(predicate), It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyLogException<TException>(string message = null) where TException : Exception
        {
            if (message == null)
            {
                Mock.Verify(x => x.LogException(It.IsAny<object>(), It.IsAny<TException>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            }
            else
            {
                Mock.Verify(x => x.LogException(It.IsAny<object>(), It.Is<TException>(y => y.Message == message), It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            }
            return this;
        }
        
        public AnalyticsServiceMockBuilder VerifyLogExceptionProperty<T>(string key, T value) 
        {
            Mock.Verify(x => x.LogException(It.IsAny<object>(), It.IsAny<Exception>(),
                It.Is<Dictionary<string, object>>(y =>
                    y.ContainsKey(key) &&
                    ((T)y[key]).Equals(value)),
                It.IsAny<string>()));
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyLogExceptionProperty(Expression<Func<Dictionary<string, object>, bool>> match)
        {
            Mock.Verify(x => x.LogException(It.IsAny<object>(), It.IsAny<Exception>(), It.Is(match), It.IsAny<string>()));
            return this;
        }

        #endregion
        
        #region StartOperation

        
        public MockBuilder<IAnalyticsOperation> Where_StartOperation_returns_operation()
        {
            var operation = new MockBuilder<IAnalyticsOperation>();

            Mock.Setup(x => x.StartOperation(It.IsAny<object>(), It.IsAny<string>(),
                It.IsAny<Dictionary<string, object>>(), It.IsAny<string>())).Returns(operation.Object);

            return operation;
        }


        public AnalyticsServiceMockBuilder VerifyStartOperation(string operationName)
        {
            Mock.Verify(x => x.StartOperation(It.IsAny<object>(), operationName, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            return this;
        }
        
        public AnalyticsServiceMockBuilder VerifyStartOperationProperty<T>(string key, T value) 
        {
            Mock.Verify(x => x.StartOperation(It.IsAny<object>(), It.IsAny<string>(), 
                It.Is<Dictionary<string, object>>(y =>
                    y.ContainsKey(key) &&
                    ((T)y[key]).Equals(value)),
                It.IsAny<string>()));
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyStartOperationProperty(Expression<Func<Dictionary<string, object>, bool>> match) 
        {
            Mock.Verify(x => x.StartOperation(It.IsAny<object>(), It.IsAny<string>(), It.Is(match), It.IsAny<string>()));
            return this;
        }

        #endregion
         
        #region StartPageViewOperation
        
        public MockBuilder<IAnalyticsOperation> Where_StartPageViewOperation_returns_operation()
        {
            var operation = new MockBuilder<IAnalyticsOperation>();

            Mock.Setup(x => x.StartPageViewOperation(It.IsAny<object>(), It.IsAny<string>(),
                It.IsAny<Dictionary<string, object>>(), It.IsAny<string>())).Returns(operation.Object);

            return operation;
        }


        public AnalyticsServiceMockBuilder VerifyStartPageViewOperation(string pageName = "")
        {
            if (string.IsNullOrEmpty(pageName))
            {
                Mock.Verify(x => x.StartPageViewOperation(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            }
            else
            {
                Mock.Verify(x => x.StartPageViewOperation(It.IsAny<object>(), pageName, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            }
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyStartPageViewOperation(object page)
        {
            Mock.Verify(x => x.StartPageViewOperation(It.IsAny<object>(), page.GetType().Name, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
            return this;
        }
        
        public AnalyticsServiceMockBuilder VerifyStartPageViewOperationProperty<T>(string key, T value) 
        {
            Mock.Verify(x => x.StartPageViewOperation(It.IsAny<object>(), It.IsAny<string>(), 
                It.Is<Dictionary<string, object>>(y =>
                    y.ContainsKey(key) &&
                    ((T)y[key]).Equals(value)), It.IsAny<string>()));
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyStartPageViewOperationProperty(Expression<Func<Dictionary<string, object>, bool>> match) 
        {
            Mock.Verify(x => x.StartPageViewOperation(It.IsAny<object>(), It.IsAny<string>(), It.Is(match), It.IsAny<string>()));
            return this;
        }

        #endregion
        
        #region StartRequestOperation

        public AnalyticsServiceMockBuilder VerifyStartRequestOperation(string operationName)
        {
            Mock.Verify(x => x.StartRequestOperation(It.IsAny<object>(), operationName, It.IsAny<IDictionary<string, string>>(), It.IsAny<string>()));
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyStartRequestOperationHeader(string key, string value)
        {
            Mock.Verify(x => x.StartRequestOperation(It.IsAny<object>(), It.IsAny<string>(), 
                It.Is<Dictionary<string, string>>(y => 
                    y[key].Equals(value)), It.IsAny<string>()));
            return this;
        }

        public AnalyticsServiceMockBuilder VerifyStartRequestOperationProperty(Expression<Func<Dictionary<string, string>, bool>> match) 
        {
            Mock.Verify(x => x.StartRequestOperation(It.IsAny<object>(), It.IsAny<string>(), It.Is(match), It.IsAny<string>()));
            return this;
        }

        #endregion
    }
}