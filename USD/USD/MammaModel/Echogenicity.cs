using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModel
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Echogenicity
    {
        [Description("�������������")]
        Hypo,

        [Description("��������������")]
        Hyper,

        [Description("������������")]
        Similar,

        [Description("�����������")]
        None
    }
}