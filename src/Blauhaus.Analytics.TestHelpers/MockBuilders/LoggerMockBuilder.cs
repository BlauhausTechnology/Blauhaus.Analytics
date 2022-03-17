using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Blauhaus.Errors;
using Blauhaus.Responses;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

#pragma warning disable CS8620

namespace Blauhaus.Analytics.TestHelpers.MockBuilders;

public class LoggerMockBuilder : BaseLoggerMockBuilder<LoggerMockBuilder, ILogger>
{
}
public class LoggerMockBuilder<T> : BaseLoggerMockBuilder<LoggerMockBuilder<T>, ILogger<T>, T>
{
}

public abstract class BaseLoggerMockBuilder<TBuilder, TMock, T> : BaseLoggerMockBuilder<TBuilder, TMock>
    where TBuilder : BaseLoggerMockBuilder<TBuilder, TMock, T>
    where TMock : class, ILogger<T>
{
}

public abstract class BaseLoggerMockBuilder<TBuilder, TMock> : BaseMockBuilder<TBuilder, TMock>
    where TBuilder :BaseLoggerMockBuilder<TBuilder, TMock>
    where TMock : class, ILogger
{
    protected BaseLoggerMockBuilder()
    {
        MockScopeDisposable = new Mock<IDisposable>();
        Mock.Setup(x => x.BeginScope(It.IsAny<Dictionary<string, object>>()))
            .Returns(MockScopeDisposable.Object);
    }

    public Mock<IDisposable> MockScopeDisposable { get; }

    public void VerifyLogError(string message, Exception? e = null) => VerifyLog(message, LogLevel.Error, e);
    public void VerifyLogError(Error error, Exception? e = null) => VerifyLog(error.Code, LogLevel.Error, e);

    public void VerifyLogErrorResponse(Error expectedError, Response actualResponse, Exception? e = null)
    {
        Assert.That(actualResponse.Error, NUnit.Framework.Is.EqualTo(expectedError));
        VerifyLog(expectedError.Code, LogLevel.Error, e);
    }
    public void VerifyLogErrorResponse<TResponse>(Error expectedError, Response<TResponse> actualResponse, Exception? e = null)
    {
        Assert.That(actualResponse.Error, NUnit.Framework.Is.EqualTo(expectedError));
        VerifyLog(expectedError.Code, LogLevel.Error, e);
    }

    public void VerifyLog(string message, LogLevel? logLevel = null, Exception? e = null)
    {
        Mock.Verify(x => x.Log(
            It.IsAny<LogLevel>(), 
            It.IsAny<EventId>(), 
            It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains(message)),
            It.IsAny<Exception>(), 
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.AtLeastOnce, 
            $"Log was not called with message {message}");
         
        if (logLevel != null)
        {
            Mock.Verify(x => x.Log(
                logLevel.Value, 
                It.IsAny<EventId>(), 
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(), 
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.AtLeastOnce, 
                $"Log was not called with LogLevel {logLevel}");
        }
        if (e != null)
        {
            var invocation = Mock.Invocations.FirstOrDefault(x => x.Arguments.FirstOrDefault(y => y is Exception)!=null);
            if (invocation != null)
            {
                var ex = (Exception)invocation.Arguments[3];
                Assert.That(ex.Message, NUnit.Framework.Is.EqualTo(e.Message));
                return;
            }

            Assert.Fail("No exceptions where passed to the logger"); 
        }
    }

    public TBuilder VerifyBeginScope(string name, Expression<Func<object, bool>> predicate)
    {
        Mock.Verify(x => x.BeginScope(It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce, 
            "BeginScope was not called with any Dictionary<string,object?");
        
        Mock.Verify(x => x.BeginScope(It.Is<Dictionary<string, object>>(y => y.ContainsKey(name))), Times.AtLeastOnce,
            $"BeginScope was called with a Dictionary that did not include a value for {name}");
        
        Mock.Verify(x => x.BeginScope(It.Is<Dictionary<string, object>>(y => predicate.Compile().Invoke(y[name]))), Times.AtLeastOnce,
            $"BeginScope was called with a for {name} that did not match the predicate {predicate.Body}");

        return (TBuilder)this;
    }

}