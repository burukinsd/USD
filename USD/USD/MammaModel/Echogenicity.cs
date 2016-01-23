using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModel
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Echogenicity
    {
        [Description("гипоэхогенное")]
        Hypo,

        [Description("гиперэхогенное")]
        Hyper,

        [Description("изоэхогенное")]
        Similar,

        [Description("анэхогенное")]
        None
    }
}