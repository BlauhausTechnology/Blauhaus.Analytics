using System;

namespace Blauhaus.Analytics.Abstractions.Attributes;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
public sealed class MessageFormatMethod : Attribute
{
    public MessageFormatMethod(string messageTemplateParameterName)
    {
        MessageTemplateParameterName = messageTemplateParameterName;
    }

    public string MessageTemplateParameterName { get; private set; }
}
