using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Errors;
using Blauhaus.Errors.Extensions;
using CSharpFunctionalExtensions;
using NUnit.Framework;

namespace Blauhaus.Analytics.TestHelpers.Extensions
{
    public static class ResultExtensions
    {
        public static void VerifyResultError(this Result result, Error error, AnalyticsServiceMockBuilder mockAnalyticsService)
        {
            Assert.That(result.Error.ToError().Code, Is.EqualTo(error.Code));
            mockAnalyticsService.VerifyTrace(error.Code, LogSeverity.Error);
        }

        public static void VerifyResultError<T>(this Result<T> result, Error error, AnalyticsServiceMockBuilder mockAnalyticsService)
        {
            Assert.That(result.Error.ToError().Code, Is.EqualTo(error.Code));
            mockAnalyticsService.VerifyTrace(error.Code, LogSeverity.Error);
        }

        public static void VerifyResultException<T>(this Result<T> result, Error error, string exceptionMessage, AnalyticsServiceMockBuilder mockAnalyticsService)
        {
            Assert.That(result.Error.ToError().Code, Is.EqualTo(error.Code));
            mockAnalyticsService.VerifyLogExceptionWithMessage(exceptionMessage);
        }

        public static void VerifyResultException(this Result result, Error error, string exceptionMessage, AnalyticsServiceMockBuilder mockAnalyticsService)
        {
            Assert.That(result.Error.ToError().Code, Is.EqualTo(error.Code));
            mockAnalyticsService.VerifyLogExceptionWithMessage(exceptionMessage);
        }
    }
}