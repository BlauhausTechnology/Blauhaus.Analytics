using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blauhaus.Errors;
using Blauhaus.Responses;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.TestHelpers.MockBuilders;

public class LoggerMockBuilder<T> : BaseMockBuilder<LoggerMockBuilder<T>, ILogger<T>>
{

    public LoggerMockBuilder()
    {
        MockScopeDisposable = new Mock<IDisposable>();
        Mock.Setup(x => x.BeginScope(It.IsAny<Dictionary<string, object>>())).Returns(MockScopeDisposable.Object);
    }

    public Mock<IDisposable> MockScopeDisposable { get; }

    public void VerifyLogError(string message, Exception? e = null) => VerifyLog(message, LogLevel.Error, e);
    public void VerifyLogError(Error error, Exception? e = null) => VerifyLog(error.ToString(), LogLevel.Error, e);

    public void VerifyLogErrorResponse(Error expectedError, Response actualResponse, Exception? e = null)
    {
        Assert.That(actualResponse.Error, NUnit.Framework.Is.EqualTo(expectedError));
        VerifyLog(expectedError.ToString(), LogLevel.Error, e);
    }
    public void VerifyLogErrorResponse<TResponse>(Error expectedError, Response<TResponse> actualResponse, Exception? e = null)
    {
        Assert.That(actualResponse.Error, NUnit.Framework.Is.EqualTo(expectedError));
        VerifyLog(expectedError.ToString(), LogLevel.Error, e);
    }

    public void VerifyLog(string message, LogLevel? logLevel = null, Exception? e = null)
    {
        Mock.Verify(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), message, It.IsAny<Exception?>(), It.IsAny<Func<string, Exception?, string>>()));
        if (logLevel != null)
        {
            Mock.Verify(x => x.Log(logLevel.Value, It.IsAny<EventId>(), It.IsAny<string>(), It.IsAny<Exception?>(), It.IsAny<Func<string, Exception?, string>>()));
        }
        if (e != null)
        {
            Mock.Verify(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<string>(), It.Is<Exception?>(ex => ex.Message == e.Message), It.IsAny<Func<string, Exception?, string>>()));
        }
    }

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