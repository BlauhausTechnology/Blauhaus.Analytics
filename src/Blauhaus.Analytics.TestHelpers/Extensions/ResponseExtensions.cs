using System;
using System.Linq.Expressions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.Errors;
using Blauhaus.Errors.Extensions;
using Blauhaus.Responses;
using NUnit.Framework;

namespace Blauhaus.Analytics.TestHelpers.Extensions
{
    public static class ResponseExtensions
    {
        public static void VerifyResponseError(this Response result, Error error, AnalyticsServiceMockBuilder mockAnalyticsService, LogSeverity logSeverity = LogSeverity.Error)
        {
            Assert.That(result.Error.Equals(error));
            mockAnalyticsService.VerifyTrace(error.ToString(), logSeverity);
        }

        public static void VerifyResponseError<T>(this Response<T> result, Error error, AnalyticsServiceMockBuilder mockAnalyticsService, LogSeverity logSeverity = LogSeverity.Error)
        {
            Assert.That(result.Error.Equals(error));
            mockAnalyticsService.VerifyTrace(error.ToString(), logSeverity);
        }

        public static void VerifyResponseException<T>(this Response<T> result, Error error, string exceptionMessage, AnalyticsServiceMockBuilder mockAnalyticsService)
        {
            Assert.That(result.Error.Equals(error));
            mockAnalyticsService.VerifyLogExceptionWithMessage(exceptionMessage);
        }

        public static void VerifyResponseException(this Response result, Error error, string exceptionMessage, AnalyticsServiceMockBuilder mockAnalyticsService)
        {
            Assert.That(result.Error.Equals(error));
            mockAnalyticsService.VerifyLogExceptionWithMessage(exceptionMessage);
        }
        public static void VerifyResponseException(this Response result, Error error, Expression<Func<Exception, bool>> predicate, AnalyticsServiceMockBuilder mockAnalyticsService)
        {
            Assert.That(result.Error.Equals(error));
            mockAnalyticsService.VerifyLogExceptionWithMessage(predicate);
        }
    }
}