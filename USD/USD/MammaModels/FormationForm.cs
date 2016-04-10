using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof (EnumDescriptionTypeConverter))]
    public enum FormationForm
    {
        [Description("��������")] Circum,
        [Description("��������")] Oval,
        [Description("������������")] Irregular
    }
}