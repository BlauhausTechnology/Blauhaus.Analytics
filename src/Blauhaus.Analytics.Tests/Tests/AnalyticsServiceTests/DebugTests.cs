using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.Tests.Tests._Base;
using Blauhaus.Common.ValueObjects.BuildConfigs;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.Tests.Tests.AnalyticsServiceTests
{
    public class DebugTests : BaseAnalyticsServiceTest
    {
        [Test]
        public void IF_BuildConfig_IS_Debug_SHOULD_log_to_console()
        {
            //Arrange
            CurrentBuildConfig = BuildConfig.Debug;
            var properties = new Dictionary<string, object>{{"Property", "value"}}; 

            //Act
            Sut.Debug("Trace message", properties);

            //Assert 
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("Trace message", LogSeverity.Debug, It.Is<Dictionary<string, string>>(y => y["Property"] == "value")));
        }
        
        [Test]
        public void IF_BuildConfig_IS_not_Debug_SHOULD_ignore()
        {
            //Arrange
            CurrentBuildConfig = BuildConfig.Staging;
            var properties = new Dictionary<string, object>{{"Property", "value"}}; 

            //Act
            Sut.Debug("Trace message", properties);

            //Assert 
            MockConsoleLogger.Mock.Verify(x => x.LogTrace("Trace message", LogSeverity.Debug, It.Is<Dictionary<string, string>>(y => y["Property"] == "value")), Times.Never());
        }
         
    }
}