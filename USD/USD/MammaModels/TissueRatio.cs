using System;

namespace USD.MammaModels
{
    [Obsolete("Используется TissueQuanity для каждой ткани отдельно.")]
    public enum TissueRatio
    {
        EnoughAll,
        MoreGlandularLessAdipose,
        EnoughGlandularMoreAdipose,
        LessGlandular,
        MoreAdipose
    }
}