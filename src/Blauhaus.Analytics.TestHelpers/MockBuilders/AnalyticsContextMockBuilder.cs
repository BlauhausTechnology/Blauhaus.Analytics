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
        object def = default(object);
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

    public AnalyticsContextMockBuilder Where_BeginScope_returns<T>()
    {
        Mock.Setup(x => x.BeginScope<T>(It.IsAny<Dictionary<string, object>>())).Returns(MockScopeDisposable.Object);
        return this;
    }

    public void VerifyBeginScope<T>(string name, object value)
    {
        Mock.Verify(x => x.BeginScope<T>(It.Is<Dictionary<string, object>>(y => y.ContainsKey(name))), Times.AtLeastOnce, "BeginScope was not called with a property called " + name);
        Mock.Verify(x => x.BeginScope<T>(It.Is<Dictionary<string, object>>(y => y[name] == value)), Times.AtLeastOnce, $"BeginScope property {name} was mot equal to {value}");
    }
}