using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.Errors;
using Blauhaus.Errors.Extensions;
using Blauhaus.Responses;
using Moq;
using NUnit.Framework;

namespace Blauhaus.Analytics.TestHelpers.Extensions
{
    public static class ResponseExtensions
    {
        public static void VerifyExactResponseError(this Response result, Error error, AnalyticsServiceMockBuilder mockAnalyticsService, LogSeverity logSeverity = LogSeverity.Error)
        {
            Assert.That(result.Error.Code.Equals(error.Code));
            Assert.That(result.Error.Description.Equals(error.Description));
            mockAnalyticsService.Mock.Verify(x => x.Trace(It.IsAny<object>(), error.ToString(), logSeverity, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
        }

        public static void VerifyExactResponseError<T>(this Response<T> result, Error error, AnalyticsServiceMockBuilder mockAnalyticsService, LogSeverity logSeverity = LogSeverity.Error)
        {
            Assert.That(result.Error.Code.Equals(error.Code));
            Assert.That(result.Error.Description.Equals(error.Description));
            mockAnalyticsService.Mock.Verify(x => x.Trace(It.IsAny<object>(), error.ToString(), logSeverity, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
        }


        public static void VerifyResponseError(this Response result, Error error, AnalyticsServiceMockBuilder mockAnalyticsService, LogSeverity logSeverity = LogSeverity.Error)
        {
            if(result.IsSuccess)
                Assert.Fail($"Expected {error} but got Success");
            else if(result.Error.Code!=error.Code)
                Assert.Fail($"Expected {error.Code} but got {result.Error.Code}");
            else
            {
                try
                {
                    mockAnalyticsService.Mock.Verify(x => x.Trace(It.IsAny<object>(), It.Is<string>(y => y.StartsWith(error.Code)), logSeverity, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
                }
                catch (Exception)
                {
                    Assert.Fail($"{error} was returned but not logged to analytics");
                }
            }

        }

        public static void VerifyResponseError<T>(this Response<T> result, Error error, AnalyticsServiceMockBuilder mockAnalyticsService, LogSeverity logSeverity = LogSeverity.Error)
        {
            if(result.IsSuccess)
                Assert.Fail($"Expected {error} but got Success");
            else if(result.Error.Code!=error.Code)
                Assert.Fail($"Expected {error.Code} but got {result.Error.Code}");
            else
            {
                try
                {
                    mockAnalyticsService.Mock.Verify(x => x.Trace(It.IsAny<object>(), It.Is<string>(y => y.StartsWith(error.Code)), logSeverity, It.IsAny<Dictionary<string, object>>(), It.IsAny<string>()));
                }
                catch (Exception)
                {
                    Assert.Fail($"{error} was returned but not logged to analytics");
                }
            }
        }

        public static void VerifyResponseException<T>(this Response<T> result, Error error, string exceptionMessage, AnalyticsServiceMockBuilder mockAnalyticsService)
        {
            if(result.IsSuccess)
                Assert.Fail($"Expected {error} but got Success");
            else if(result.Error.Code!=error.Code)
                Assert.Fail($"Expected {error.Code} but got {result.Error.Code}");

            mockAnalyticsService.VerifyLogExceptionWithMessage(exceptionMessage);
        }

        public static void VerifyResponseException(this Response result, Error error, string exceptionMessage, AnalyticsServiceMockBuilder mockAnalyticsService)
        {
            if(result.IsSuccess)
                Assert.Fail($"Expected {error} but got Success");
            else if(result.Error.Code!=error.Code)
                Assert.Fail($"Expected {error.Code} but got {result.Error.Code}");

            mockAnalyticsService.VerifyLogExceptionWithMessage(exceptionMessage);
        }

        public static void VerifyResponseException(this Response result, Error error, Expression<Func<Exception, bool>> predicate, AnalyticsServiceMockBuilder mockAnalyticsService)
        {
            if(result.IsSuccess)
                Assert.Fail($"Expected {error} but got Success");
            else if(result.Error.Code!=error.Code)
                Assert.Fail($"Expected {error.Code} but got {result.Error.Code}");

            mockAnalyticsService.VerifyLogExceptionWithMessage(predicate);
        }
    }
}