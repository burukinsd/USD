using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModel
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Structure
    {
        [Description("����������")]
        Homogenous,

        [Description("������������")]
        NonHomogenous
    }
}