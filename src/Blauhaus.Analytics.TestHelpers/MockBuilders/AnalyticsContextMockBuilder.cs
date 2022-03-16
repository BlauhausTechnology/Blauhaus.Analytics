using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.Analytics.TestHelpers.MockBuilders;

public class AnalyticsContextMockBuilder : BaseMockBuilder<AnalyticsContextMockBuilder, IAnalyticsContext>
{
    public Mock<IDisposable> MockScopeDisposable = new();
    public AnalyticsContextMockBuilder()
    {
        Where_GetAllValues_returns(new Dictionary<string, object>());
    }

    public AnalyticsContextMockBuilder Where_TryGetValue_returns(object value, string key)
    {
        Mock.Setup(x => x.TryGetValue(key, out value)).Returns(true);
        return this;
    }
    public AnalyticsContextMockBuilder Where_TryGetValue_fails(string key)
    {
        var def = default(object);
        Mock.Setup(x => x.TryGetValue(key, out def)).Returns(false);
        return this;
    }

    public AnalyticsContextMockBuilder Where_GetAllValues_returns(Dictionary<string, object> properties)
    { 
        Mock.Setup(x => x.GetAllValues()).Returns(properties);
        return this;
    }
    public AnalyticsContextMockBuilder Where_GetAllValues_returns(string name, object value)
    { 
        Mock.Setup(x => x.GetAllValues()).Returns(new  Dictionary<string, object>{[name] = value});
        return this;
    }

    public AnalyticsContextMockBuilder Where_BeginScope_returns<T>(IDisposable? disposable = null)
    {
        if (disposable != null)
        {
            Mock.Setup(x => x.BeginScope<T>()).Returns(disposable);
        }
        else
        {
            Mock.Setup(x => x.BeginScope<T>()).Returns(MockScopeDisposable.Object);
        }
        return this;
    }

    public void VerifyBeginTimedScope<T>(string message)
    {
        Mock.Verify(x => x.BeginTimedScope<T>(message, It.IsAny<object[]>()));
    }
    public void VerifyBeginTimedScope<T>()
    {
        Mock.Verify(x => x.BeginTimedScope<T>(It.IsAny<string>(), It.IsAny<object[]>()));
    }

    public void VerifySetValues(string name, object value)
    {
        Mock.Verify(x => x.SetValues(It.Is<Dictionary<string, object>>(y => y.ContainsKey(name))), Times.AtLeastOnce, "SetValues was not called with a property called " + name);
        Mock.Verify(x => x.SetValues(It.Is<Dictionary<string, object>>(y => y[name] == value)), Times.AtLeastOnce, $"SetValues property {name} was mot equal to {value}");
    }
}