using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof (EnumDescriptionTypeConverter))]
    public enum MammaSpecialists
    {
        [Description(" ")] None,
        [Description("���������")] Mammalogist,
        [Description("��������-���������")] Oncologist,
        [Description("����������")] Gynecologist,
        [Description("�������")] Surgeo�
    }
}