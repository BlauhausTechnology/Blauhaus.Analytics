using System;
using Blauhaus.AppInsights.Abstractions.ConsoleLoggers;
using Blauhaus.Common.TestHelpers;
using NUnit.Framework;

namespace Blauhaus.AppInsights.Tests.Tests._Base
{
    public abstract class BaseAppInsightsTest<TSut> : BaseUnitTest<TSut> where TSut : class
    {

        protected MockBuilder<ITraceProxy> MockTraceProxy;

        [SetUp]
        public virtual void Setup()
        {
            Cleanup();
            MockTraceProxy = new MockBuilder<ITraceProxy>();
        }

    }
}
