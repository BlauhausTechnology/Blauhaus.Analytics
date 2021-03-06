﻿using System.Net.Http.Headers;
using Blauhaus.Analytics.Abstractions.Http;
using Blauhaus.Analytics.Abstractions.Session;
using Blauhaus.Analytics.Orleans.Context;

namespace Blauhaus.Analytics.Orleans.Session
{
    public class OrleansSessionFactory : IAnalyticsSessionFactory
    {
        private readonly IOrleansRequestContext _requestContext;

        public OrleansSessionFactory(IOrleansRequestContext requestContext)
        {
            _requestContext = requestContext;
        }
        
        
        public IAnalyticsSession CreateSession()
        {
            
            var session = AnalyticsSession.FromExisting((string) _requestContext.Get(AnalyticsHeaders.Session.Id));
            session.UserId = (string)_requestContext.Get(AnalyticsHeaders.Session.UserId);
            session.DeviceId = (string)_requestContext.Get(AnalyticsHeaders.Session.DeviceId);
            session.AppVersion = (string)_requestContext.Get(AnalyticsHeaders.Session.AppVersion);
            session.AccountId = (string)_requestContext.Get(AnalyticsHeaders.Session.AccountId);
            
            session.SetProperty("OperationId", (string)_requestContext.Get(AnalyticsHeaders.Operation.Id));
            session.SetProperty("OperationName", (string)_requestContext.Get(AnalyticsHeaders.Operation.Name));

            return session;
        }
    }
}