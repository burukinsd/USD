using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum CanalsExpandingType
    {
        [Description("не расширены")]
        Not,
        [Description("расширены не равномерно до 2-3мм дифузно справа и слева")]
        Expanding23Mm,
        [Description("расширены до")]
        ExpandingSpecific
    }
}