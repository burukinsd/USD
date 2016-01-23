using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum OutlinesType
    {
        [Description("ровные четкие")]
        SmothClear,

        [Description("ровные нечеткие")]
        SmothNotClear,

        [Description("неровные нечеткие")]
        NotSmothNotClear,

        [Description("не определяются")]
        NotDetermined
    }
}