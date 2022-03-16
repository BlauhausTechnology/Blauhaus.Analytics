using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blauhaus.Errors;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.Logging;
using Moq;

namespace Blauhaus.Analytics.TestHelpers.MockBuilders;

public class LoggerMockBuilder<T> : BaseMockBuilder<LoggerMockBuilder<T>, ILogger<T>>
{

    public LoggerMockBuilder()
    {
        MockScopeDisposable = new Mock<IDisposable>();
        Mock.Setup(x => x.BeginScope(It.IsAny<Dictionary<string, object>>())).Returns(MockScopeDisposable.Object);
    }

    public Mock<IDisposable> MockScopeDisposable { get; }

    public void VerifyLogError(string message) => Mock.Verify(x => x.Log(LogLevel.Error, message));
    public void VerifyLogError(Error error) => Mock.Verify(x => x.Log(LogLevel.Error, error.ToString()));
    public void VerifyLogError(Error error, Exception e) => Mock.Verify(x => x.Log(LogLevel.Error, It.Is<Exception>(y => y.Message == e.Message), error.ToString()));

    public void VerifyLogInformation(string message) => Mock.Verify(x => x.Log(LogLevel.Information, message));
    public void VerifyLogTrace(string message) => Mock.Verify(x => x.Log(LogLevel.Trace, message));
    public void VerifyLogWarning(string message) => Mock.Verify(x => x.Log(LogLevel.Warning, message));
    public void VerifyLogDebug(string message) => Mock.Verify(x => x.Log(LogLevel.Debug, message));
    public void VerifyLogCritical(string message) => Mock.Verify(x => x.Log(LogLevel.Critical, message));
    public void VerifyLog(LogLevel logLevel, string message)=> Mock.Verify(x => x.Log(logLevel, message));
    public void VerifyLog(LogLevel logLevel, string message, Exception e)=> Mock.Verify(x => x.Log(logLevel, e, message));
    public void VerifyLog(string message)=> Mock.Verify(x => x.Log(It.IsAny<LogLevel>(), message));

    public LoggerMockBuilder<T> VerifyBeginScope(string name, Expression<Func<object, bool>> predicate)
    {
        Mock.Verify(x => x.BeginScope(It.IsAny<Dictionary<string, object>>()), Times.AtLeastOnce, 
            "BeginScope was not called with any Dictionary<string,object?");
        
        Mock.Verify(x => x.BeginScope(It.Is<Dictionary<string, object>>(y => y.ContainsKey(name))), Times.AtLeastOnce,
            $"BeginScope was called with a Dictionary that did not include a value for {name}");
        
        Mock.Verify(x => x.BeginScope(It.Is<Dictionary<string, object>>(y => predicate.Compile().Invoke(y[name]))), Times.AtLeastOnce,
            $"BeginScope was called with a for {name} that did not match the predicate {predicate.Body}");

        return this;
    }

}