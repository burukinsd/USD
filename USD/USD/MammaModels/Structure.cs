using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof (EnumDescriptionTypeConverter))]
    public enum Structure
    {
        [Description("����������")] Homogenous,

        [Description("������������")] NonHomogenous
    }
}