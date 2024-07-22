using System.Text;
using Serilog.Core;
using Serilog.Events;

namespace Harlok.Sphere.Core.Serilog;

class SimpleClassEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var typeName = logEvent.Properties.GetValueOrDefault("SourceContext").ToString();
        var pos = typeName.LastIndexOf('.');
        typeName = typeName.Substring(pos + 1, typeName.Length - pos - 2);
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("SourceContext", typeName));
    }
}

public class AbbreviatedSourceContextEnricher : ILogEventEnricher
{
    private readonly int _targetLength;
    private readonly int _padLeft;

    public AbbreviatedSourceContextEnricher(int targetLength, int padLeft)
    {
        _targetLength = targetLength;
        _padLeft = padLeft;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var typeName = logEvent.Properties.GetValueOrDefault("SourceContext").ToString();
        typeName = typeName.Substring(1, typeName.Length - 2);
        // If you prefer to not override the SourceContext property you can create a new property:
        // logEvent.AddOrUpdateProperty( propertyFactory.CreateProperty( "AbbrSourceContext", Abbreviate( typeName ) ) );
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("SourceContext", Abbreviate(typeName)));
        
        var invokedMethod = logEvent.Properties.TryGetValue("InvokedMethod", out var invokedMethodValue)
            ? string.Empty  //invokedMethodValue.ToString().PadLeft(40)
            : string.Empty;

        var customProperty = propertyFactory.CreateProperty("InvokedMethod", invokedMethod, true);
        logEvent.AddOrUpdateProperty(customProperty);

        var newLine = logEvent.Properties.TryGetValue("NewLine", out var newLineValue)
            ? newLineValue.ToString().PadLeft(50)
            : string.Empty;
        
        // logEvent.AddOrUpdateProperty(logEvent.);
    }

    private string Abbreviate(string fqClassName)
    {
        if (fqClassName == null)
        {
            throw new ArgumentNullException(nameof(fqClassName));
        }

        var inLen = fqClassName.Length;
        if (inLen < _targetLength)
        {
            return fqClassName.PadLeft(_padLeft);
        }

        var buf = new StringBuilder(inLen);

        var rightMostDotIndex = fqClassName.LastIndexOf(".");

        if (rightMostDotIndex == -1)
            return fqClassName.PadLeft(_padLeft);

        // length of last segment including the dot
        var lastSegmentLength = inLen - rightMostDotIndex;

        var leftSegments_TargetLen = _targetLength - lastSegmentLength;
        if (leftSegments_TargetLen < 0)
            leftSegments_TargetLen = 0;

        var leftSegmentsLen = inLen - lastSegmentLength;

        // maxPossibleTrim denotes the maximum number of characters we aim to trim
        // the actual number of character trimmed may be higher since segments, when
        // reduced, are reduced to just one character
        var maxPossibleTrim = leftSegmentsLen - leftSegments_TargetLen;

        var trimmed = 0;
        var inDotState = true;

        var i = 0;
        for (; i < rightMostDotIndex; i++)
        {
            char c = fqClassName.ToCharArray()[i];
            if (c == '.')
            {
                // if trimmed too many characters, let us stop
                if (trimmed >= maxPossibleTrim)
                    break;
                buf.Append(c);
                inDotState = true;
            }
            else
            {
                if (inDotState)
                {
                    buf.Append(c);
                    inDotState = false;
                }
                else
                {
                    trimmed++;
                }
            }
        }

        // append from the position of i which may include the last seen DOT
        buf.Append(fqClassName[i..]);
        return buf.ToString().PadLeft(_padLeft);
    }
}