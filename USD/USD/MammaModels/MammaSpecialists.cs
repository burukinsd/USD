using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum MammaSpecialists
    {
        [Description(" ")]
        None,
        [Description("маммолог")]
        Mammalogist,
        [Description("онколог")]
        Oncologist,
        [Description("гинеколог")]
        Gynecologist,
        [Description("хирург")]
        Surgeoт
    }
}