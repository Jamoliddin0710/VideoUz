using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Helpers;

public static class EnumHelper
{
    public static IEnumerable<SelectListItem> GetEnumSelectList<TEnum>(bool isNullable = false)
        where TEnum : struct
    {
        IList<SelectListItem> selectLists = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(c => new SelectListItem()
        {
            Text = GetDisplayName(c),
            Value = c.ToString()
        }).ToList();

        if (isNullable)
        {
            selectLists.Add(new SelectListItem());
        }

        return selectLists.OrderBy(c => c.Value);
    }

    public static string GetDisplayName<TEnum>(TEnum enumVal)
    {
        DisplayAttribute attr = GetAttribute<DisplayAttribute>(enumVal);

        if (attr != null)
        {
            return attr.Name;
        }

        return enumVal?.ToString() ?? string.Empty;
    }

    public static TEnum GetAttribute<TEnum>(object enumVal) where TEnum : Attribute
    {
        if (enumVal == null)
        {
            return default;
        }

        Type type = enumVal.GetType();
        MemberInfo[] memInfo = type.GetMember(enumVal.ToString());

        if (memInfo.Length == 0)
        {
            return null;
        }

        object[] attributes = memInfo[0].GetCustomAttributes(typeof(TEnum), false);
        return attributes.Length > 0 ? (TEnum)attributes[0] : null;
    }
}