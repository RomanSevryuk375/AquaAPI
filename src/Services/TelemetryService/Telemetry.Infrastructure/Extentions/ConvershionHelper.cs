namespace Telemetry.Infrastructure.Extentions;

public class ConvershionHelper
{
    public Guid StringToGuid(string value)
    {
        return Guid.TryParse(value, out var guid) 
            ? guid 
            : Guid.Empty;
    }
}
