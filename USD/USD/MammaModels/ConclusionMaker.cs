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
                conclusionStringBuilder.Append("�� ������ �� ��������� �������� ����� �� ��������. ");
            }
            if (mammaModel.IsCystsConclusion)
            {
                conclusionStringBuilder.Append("�� �������� ��������-��������� �������");
                if (!string.IsNullOrEmpty(mammaModel.CystConslusionDesc))
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
            if (mammaModel.IsAdenosisConclusion)
            {
                conclusionStringBuilder.Append("�� �������� ����������������. ");
            }
            if (mammaModel.IsFocalFormationConclusion)
            {
                conclusionStringBuilder.Append(mammaModel.FocalFormationConclusionPosition ==
                                               FocalFormationConclusionPosition.Both
                    ? "�� �������� �������� ����������� "
                    : "�� �������� ��������� ����������� ");
                conclusionStringBuilder.Append(mammaModel.FocalFormationConclusionPosition.EnumDescription());
            }
            if (mammaModel.IsSpecificConclusion)
            {
                conclusionStringBuilder.Append(mammaModel.SpecificConclusionDesc ?? string.Empty);
            }

            return conclusionStringBuilder.ToString();
        }
    }
}