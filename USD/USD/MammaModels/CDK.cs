using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof (EnumDescriptionTypeConverter))]
    public enum CDK
    {
        [Description("�����������")] None,
        [Description("���������������")] Intranodulyarny,
        [Description("��������������")] Perinodulyarny,
        [Description("���������")] Mix
    }
}