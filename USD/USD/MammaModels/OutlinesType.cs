using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof (EnumDescriptionTypeConverter))]
    public enum OutlinesType
    {
        [Description("������ ������")] SmothClear,

        [Description("������ ��������")] SmothNotClear,

        [Description("�������� ������")] NotSmothClear,

        [Description("�������� ��������")] NotSmothNotClear,

        [Description("�� ������������")] NotDetermined
    }
}