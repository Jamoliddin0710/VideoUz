using Domain.Entities;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Utility;

public static class SD
{
    public static string[] Roles = Enum.GetNames<Role>();
    public static string IsActive(this IHtmlHelper html, string action, string controller, string cssClass = "active")
    {
        var routeData = html.ViewContext.RouteData;
        var actionData = routeData.Values["action"];
        var controllerData = routeData.Values["controller"];

        if (action.Equals(actionData) && controller.Equals(controllerData))
        {
            return cssClass;
        }
        return string.Empty;
    }

    public const int MB = 100;
}