using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum CDK
    {
        [Description("аваскул€рное")]
        Avascular
    }
}