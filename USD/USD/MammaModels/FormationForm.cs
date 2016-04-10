using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof (EnumDescriptionTypeConverter))]
    public enum FormationForm
    {
        [Description("округлая")] Circum,
        [Description("овальная")] Oval,
        [Description("неправильная")] Irregular
    }
}