using System.ComponentModel;
using USD.ViewTools;

namespace USD.MammaModels
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum FocalFormationConclusionPosition
    {
        [Description("левой молочной железы")]
        Left,
        [Description("правой молочной железы")]
        Right,
        [Description("обеих молочных желез")]
        Both
    }
}