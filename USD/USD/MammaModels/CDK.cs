using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof (EnumDescriptionTypeConverter))]
    public enum CDK
    {
        [Description("отсутствует")] None,
        [Description("интранодул€рный")] Intranodulyarny,
        [Description("перинодул€рный")] Perinodulyarny,
        [Description("смешанный")] Mix
    }
}