using System.ComponentModel;
using System.Reflection;

namespace SentinelApi.Monitoring.Application.Helpers;

internal static class EnumHelper
{
    internal static string GetEnumDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? value.ToString();
    }
}
