using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum MammaSpecialists
    {
        [Description(" ")]
        None,
        [Description("маммолога")]
        Mammalogist,
        [Description("онколога")]
        Oncologist,
        [Description("гинеколога")]
        Gynecologist,
        [Description("хирурга")]
        Surgeoт
    }
}