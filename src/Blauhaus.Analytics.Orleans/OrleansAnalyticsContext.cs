﻿using System;
using System.Collections.Generic;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Ioc.Abstractions;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;

namespace Blauhaus.Analytics.Orleans;

public class OrleansAnalyticsContext : IAnalyticsContext
{
    private Dictionary<string, object>? _analyticsProperties;
    
    private readonly IServiceLocator _serviceLocator;

    public OrleansAnalyticsContext(IServiceLocator serviceLocator)
    {
        _serviceLocator = serviceLocator;
    }

    private Dictionary<string, object> SetProperties()
    {
        var properties = GetProperties();
        RequestContext.Set("AnalyticsProperties", properties);
        return properties;
    }

    public Dictionary<string, object> SetValue(string key, object value)
    {
        GetProperties()[key] = value;
        return SetProperties();
    }

    public Dictionary<string, object> SetValues(Dictionary<string, object> newProperties)
    {
        var properties = GetProperties();
        foreach (var newProperty in newProperties)
        {
            properties[newProperty.Key] = newProperty.Value;
        }
        return SetProperties();
    }
     
    public bool TryGetValue(string key, out object value)
    {
        var properties = GetProperties();
        if (properties.TryGetValue(key, out var cachedValue))
        {
            value = cachedValue;
            return true;
        }

        value = null;
        return false;
    }

    public Dictionary<string, object> GetAllValues()
    {
        return GetProperties();
    }

    public IDisposable BeginScope<T>(Dictionary<string, object>? extraProperties)
    {
        if (extraProperties != null)
        {
            foreach (var extraProperty in extraProperties)
            {
                SetValue(extraProperty.Key, extraProperty.Value);
            }
        }
        var logger = _serviceLocator.Resolve<ILogger<T>>();
        return logger.BeginScope(GetProperties());
    }

    private Dictionary<string, object> GetProperties()
    {
        if (_analyticsProperties == null)
        {
            _analyticsProperties = (Dictionary<string, object>)RequestContext.Get("AnalyticsProperties");
            if (_analyticsProperties == null)
            {
                _analyticsProperties = new Dictionary<string, object>();
                RequestContext.Set("AnalyticsProperties", _analyticsProperties);
            }
        }

        return _analyticsProperties;
    }

}