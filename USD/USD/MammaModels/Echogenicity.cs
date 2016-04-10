using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof (EnumDescriptionTypeConverter))]
    public enum Echogenicity
    {
        [Description("�������������")] Hypo,

        [Description("��������������")] Hyper,

        [Description("������������")] Similar,

        [Description("�����������")] None
    }
}