using System.ComponentModel;
using System.Reflection;

using AMNApi.Common.Dtos.Response;

namespace AMNApi.Helpers;

public static class EnumHelper
{
    public static IEnumerable<EnumValueResponseDto> GetEnumItems<T>() where T : Enum
    {
        var type = typeof(T);
        var values = Enum.GetValues(type);
        foreach (var value in values)
        {
            var field = type.GetField(value.ToString()!);
            var attribute = (DescriptionAttribute)field!.GetCustomAttribute(typeof(DescriptionAttribute))!;
            var description = attribute != null ? attribute.Description : value.ToString();
            yield return new EnumValueResponseDto { Value = (int)value, Description = description! };
        }
    }

    public static IEnumerable<EnumStringValueResponseDto> GetEnumItemsString<T>() where T : Enum
    {
        var type = typeof(T);
        var values = Enum.GetValues(type);
        foreach (var value in values)
        {
            var field = type.GetField(value.ToString()!);
            var attribute = (DescriptionAttribute)field!.GetCustomAttribute(typeof(DescriptionAttribute))!;
            var description = attribute != null ? attribute.Description : value.ToString();
            yield return new EnumStringValueResponseDto { Value = ((int)value).ToString(), Description = description! };
        }
    }

    public static string GetDescription<T>(Enum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);

        if (name == null)
            return string.Empty;
        
        var field = type.GetField(name);
        if (field == null)
            return string.Empty;

        var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute != null ? attribute.Description : name;
    }
}