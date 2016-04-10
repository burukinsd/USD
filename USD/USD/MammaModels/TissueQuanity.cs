using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof (EnumDescriptionTypeConverter))]
    public enum TissueQuanity
    {
        [Description("достаточно")] Enogh,
        [Description("мало")] Few,
        [Description("много")] Many
    }
}