using Blauhaus.Analytics.Abstractions;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Blauhaus.Analytics.TestHelpers.MockBuilders;

public class AnalyticsLoggerMockBuilder : BaseAnalyticsLoggerMockBuilder<AnalyticsLoggerMockBuilder, IAnalyticsLogger>
{
}
public class AnalyticsLoggerMockBuilder<T> : BaseAnalyticsLoggerMockBuilder<AnalyticsLoggerMockBuilder<T>, IAnalyticsLogger<T>, T>
{
}

public abstract class BaseAnalyticsLoggerMockBuilder<TBuilder, TMock, T> : BaseAnalyticsLoggerMockBuilder<TBuilder, TMock>
    where TBuilder : BaseAnalyticsLoggerMockBuilder<TBuilder, TMock, T>
    where TMock : class, IAnalyticsLogger<T>
{

}

public abstract class BaseAnalyticsLoggerMockBuilder<TBuilder, TMock> : BaseLoggerMockBuilder<TBuilder, TMock>
    where TBuilder : BaseAnalyticsLoggerMockBuilder<TBuilder, TMock>
    where TMock : class, IAnalyticsLogger
{
    protected BaseAnalyticsLoggerMockBuilder()
    {
        Mock.Setup(x => x.BeginScope()).Returns(MockScopeDisposable.Object);
        Mock.Setup(x => x.LogTimed(It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<object[]>())).Returns(MockScopeDisposable.Object);
        Mock.Setup(x => x.BeginTimedScope(It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<object[]>())).Returns(MockScopeDisposable.Object);

        Mock.Setup(x => x.SetValues(It.IsAny<Dictionary<string, object>>())).Returns(Mock.Object);
        Mock.Setup(x => x.SetValue(It.IsAny<string>(), It.IsAny<object>())).Returns(Mock.Object);
    }
    public void VerifyBeginTimedScope(LogLevel logLevel, string message)
    {
        Mock.Verify(x => x.BeginTimedScope(logLevel, message, It.IsAny<object[]>()));
    }
    public void VerifyBeginTimedScope(string message)
    {
        Mock.Verify(x => x.BeginTimedScope(It.IsAny<LogLevel>(), message, It.IsAny<object[]>()));
    }
    public void VerifyBeginTimedScope()
    {
        Mock.Verify(x => x.BeginTimedScope(It.IsAny<LogLevel>(), It.IsAny<string>(), It.IsAny<object[]>()));
    }
    public void VerifySetValue(string key, object value)
    {
        Mock.Verify(x => x.SetValue(key, It.IsAny<object>()), Times.AtLeastOnce, $"SetValue was not called with a value {key}");
        Mock.Verify(x => x.SetValue(It.IsAny<string>(), value), Times.AtLeastOnce, $"SetValue was called with a value {key} but it was not {value}");

    }
    public void VerifySetValues(string key, object value)
    {
        Mock.Verify(x => x.SetValues(It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce, "SetValues was never called");

        Mock.Verify(x => x.SetValues(It.Is<Dictionary<string, object>>(y => 
            y.ContainsKey(key))), Times.AtLeastOnce, $"SetValues was called but not with a property named {key}");

        var invocations = Mock.Invocations.Where(x => x.Arguments.FirstOrDefault(y => y is Dictionary<string, object>)!=null);
        foreach (var invocation in invocations)
        {
            foreach (var invocationArgument in invocation.Arguments)
            {
                if (invocationArgument is Dictionary<string, object> values)
                {
                    if (values.TryGetValue(key, out var foundValue))
                    {
                        if (foundValue.Equals(value))
                        {
                            Assert.Pass($"Value {value} was found for {key}");
                            return;
                        }
                    }
                }
            }
        }

        Assert.Fail($"SetValues was called with a property named {key} but the value of it was not {value}");
    }

    public void VerifySetValues(string key, Expression<Func<object,bool>> predicate)
    {
        Mock.Verify(x => x.SetValues(It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce, "SetValues was never called");

        Mock.Verify(x => x.SetValues(It.Is<Dictionary<string, object>>(y => 
            y.ContainsKey(key))), Times.AtLeastOnce, $"SetValues was called but not with a property named {key}");

        var invocations = Mock.Invocations.Where(x => x.Arguments.FirstOrDefault(y => y is Dictionary<string, object>)!=null);
        foreach (var invocation in invocations)
        {
            foreach (var invocationArgument in invocation.Arguments)
            {
                if (invocationArgument is Dictionary<string, object> values)
                {
                    if (values.TryGetValue(key, out var foundValue))
                    {
                        if (predicate.Compile().Invoke(foundValue))
                        {
                            Assert.Pass($"Value was found for {key}");
                            return;
                        }
                    }
                }
            }
        }
        Assert.Fail( $"SetValues was called with a property named {key} but the value did not match {predicate.Body}");
    }

    public void VerifyLogTimed(string message, LogLevel? logLevel = null)
    {
        Mock.Verify(x => x.LogTimed(
                It.IsAny<LogLevel>(),message), Times.AtLeastOnce, 
            $"LogTimed was not called with message {message}");
         
        if (logLevel != null)
        {
            Mock.Verify(x => x.Log(
                    logLevel.Value, 
                    It.IsAny<string>()), Times.AtLeastOnce, 
                $"LogTimed was not called with LogLevel {logLevel}");
        } 
    }
}