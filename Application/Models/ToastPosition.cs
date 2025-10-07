using System.ComponentModel;

namespace IIA.Components.Enums
{
    public enum ToastPosition
    {
        [Description("right-top")]
        TopRight,
        [Description("right-bottom")]
        BottomRight,
        [Description("left-bottom")]
        BottomLeft,
        [Description("left-top")]
        TopLeft,
        [Description("center-top")]
        TopCenter,
        [Description("center-bottom")]
        BottomCenter
    }
}
