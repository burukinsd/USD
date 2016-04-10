using System;
using System.Text;
using Novacode;

namespace USD.MammaModels
{
    public class ConclusionMaker
    {
        public static string MakeConclusion(MammaModel mammaModel)
        {
            var conclusionStringBuilder = new StringBuilder();
            if (mammaModel.IsNotPatalogyConclusion)
            {
                conclusionStringBuilder.Append("УЗ данных за патологию молочных желез не получено. ");
            }
            if (mammaModel.IsCystsConclusion)
            {
                conclusionStringBuilder.Append("УЗ признаки фиброзно-кистозной болезни");
                if (!String.IsNullOrEmpty(mammaModel.CystConslusionDesc))
                {
                    conclusionStringBuilder.Append(": ");
                    conclusionStringBuilder.Append(mammaModel.CystConslusionDesc);
                }
                conclusionStringBuilder.Append(". ");
            }
            if (mammaModel.IsInvolutionConclusion)
            {
                conclusionStringBuilder.Append("Фиброзно-жировая инволюция. ");
            }
            if (mammaModel.IsAdenosisConclusion)
            {
                conclusionStringBuilder.Append("УЗ признаки фиброаденоматоза. ");
            }
            if (mammaModel.IsFocalFormationConclusion)
            {
                conclusionStringBuilder.Append(mammaModel.FocalFormationConclusionPosition ==
                                               FocalFormationConclusionPosition.Both
                    ? "УЗ признаки очаговых образований "
                    : "УЗ признаки очагового образования ");
                conclusionStringBuilder.Append(mammaModel.FocalFormationConclusionPosition.EnumDescription());
            }
            if (mammaModel.IsSpecificConclusion)
            {
                conclusionStringBuilder.Append(mammaModel.SpecificConclusionDesc ?? String.Empty);
            }

            return conclusionStringBuilder.ToString();
        }
    }
}