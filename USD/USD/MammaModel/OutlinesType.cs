using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModel
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum OutlinesType
    {
        [Description("������ ������")]
        SmothClear,

        [Description("������ ��������")]
        SmothNotClear,

        [Description("�������� ��������")]
        NotSmothNotClear,

        [Description("�� ������������")]
        NotDetermined
    }
}