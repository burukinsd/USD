using System;
using System.Text;

namespace USD.MammaModels
{
    public class ConclusionMaker
    {
        public static string MakeConclusion(MammaModel mammaModel)
        {
            var conclusionStringBuilder = new StringBuilder();
            if (mammaModel.IsNotPatalogyConclusion)
            {
                conclusionStringBuilder.Append("�� ������ �� ��������� �������� ����� �� ��������. ");
            }
            if (mammaModel.IsCystsConclusion)
            {
                conclusionStringBuilder.Append("�� �������� ��������-��������� �������");
                if (!String.IsNullOrEmpty(mammaModel.CystConslusionDesc))
                {
                    conclusionStringBuilder.Append(": ");
                    conclusionStringBuilder.Append(mammaModel.CystConslusionDesc);
                }
                conclusionStringBuilder.Append(". ");
            }
            if (mammaModel.IsInvolutionConclusion)
            {
                conclusionStringBuilder.Append("��������-������� ���������. ");
            }
            if (mammaModel.IsSpecificConclusion)
            {
                conclusionStringBuilder.Append(mammaModel.SpecificConclusionDesc ?? String.Empty);
            }

            return conclusionStringBuilder.ToString();
        }
    }
}