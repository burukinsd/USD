using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Novacode;
using USD.MammaModels;

namespace USD.WordExport
{
    public static class MammaExporter
    {
        public static void Export(MammaModel model)
        {
            var directoryFullPath = ExportDirectoryCreator.EnsureDirectory();

            var fileFullPath =
                $"{directoryFullPath}\\{model.VisitDate:dd.MM.yyyy} {model.FIO} {model.BirthYear}.docx";

            try
            {
                using (var document = DocX.Load(@"Templates\MammaTemplate.docx"))
                {
                    document.ReplaceText("%VisitDate%", model.VisitDate.ToShortDateString());

                    document.ReplaceText("%FIO%", model.FIO ?? string.Empty);

                    document.ReplaceText("%BirthYear%", model.BirthYear ?? string.Empty);

                    document.ReplaceText("%Status%", MakeStatus(model));

                    document.ReplaceText("%Skin%", MakeSkin(model));

                    document.ReplaceText("%Tissue%", MakeTissue(model));

                    document.ReplaceText("%Grandular%", MakeGrandular(model));

                    document.ReplaceText("%ActualToPhase%", MakeActualToPhase(model));

                    document.ReplaceText("%Canals%", MakeCanals(model));

                    document.ReplaceText("%DiffuseChanges%", MakeDiffuseCahnges(model));

                    document.ReplaceText("%NippleArea%", MakeNippleArea(model));

                    document.ReplaceText("%Cyst%", MakeCysts(model));

                    document.ReplaceText("%FocalFormation%", MakeFocalFormations(model));

                    document.ReplaceText("%LymphNodes%", MakeLymphNodes(model));

                    document.ReplaceText("%AdditionalInfo%",
                        MakeAdditionalInfo(model));

                    document.ReplaceText("%Conclusion%", ConclusionMaker.MakeConclusion(model));

                    document.ReplaceText("%Recomendation%",
                        model.Recomendation == MammaSpecialists.None
                            ? string.Empty
                            : $"\r\nРекомендована консультация {model.Recomendation.EnumDescription()}, маммография");

                    document.SaveAs(fileFullPath);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Не удается сохранить заключение. Возможно оно откртыто в Word.", "УЗД молочных желез",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Process.Start(fileFullPath);
        }

        private static string MakeAdditionalInfo(MammaModel model)
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(model.AdditionalDesc) || model.IsLypomAdditionalInfo)
            {
                builder.AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(model.AdditionalDesc))
            {
                builder.AppendLine(model.AdditionalDesc);
            }
            if (model.IsLypomAdditionalInfo)
            {
                builder.AppendLine("УЗ признаки доброкачественной инволютивной перестройки жировой ткани по типу липомы.");
            }
            return builder.ToString();
        }

        private static string MakeActualToPhase(MammaModel model)
        {
            if (model.PhisiologicalStatus == PhisiologicalStatus.Normal)
                return model.ActualToPhase
                    ? "\r\nСтроение соответствует фазе менструального цикла."
                    : "\r\nСтроение не соответствует фазе менструального цикла.";
            return string.Empty;
        }

        private static string MakeGrandular(MammaModel model)
        {
            if (model.MaxThicknessGlandularLayer.HasValue)
            {
                return $"{model.MaxThicknessGlandularLayer} мм.";
            }
#pragma warning disable 618
            var val = Math.Max(model.LeftThicknessGlandularLayer ?? 0, model.RightThicknessGlandularLayer ?? 0);
#pragma warning restore 618
            return $"{val} мм.";
        }

        private static string MakeLymphNodes(MammaModel model)
        {
            var builder = new StringBuilder();
            if (model.IsDeterminateLymphNodes)
            {
                builder.Append("определяются: ");
                builder.Append(model.LymphNodesDesc ?? string.Empty);
            }
            else
            {
                builder.Append("не определяются.");
            }
            return builder.ToString();
        }

        private static string MakeFocalFormations(MammaModel model)
        {
            var builder = new StringBuilder();
            if (model.AreFocalFormations)
            {
                builder.AppendLine("выявляются:");
                if (model.FocalFormations == null) return builder.ToString();
                foreach (var formation in model.FocalFormations)
                {
                    var innerBuilder = new StringBuilder();

                    var number = model.FocalFormations.IndexOf(formation) + 1;

                    innerBuilder.Append(number);
                    innerBuilder.Append(". ");
                    innerBuilder.Append(formation.Localization ?? string.Empty);
                    innerBuilder.Append(", ");
                    innerBuilder.Append("форма: ");
                    innerBuilder.Append(formation.Form.EnumDescription());
                    innerBuilder.Append(", ");
                    innerBuilder.Append(formation.Size ?? string.Empty);
                    innerBuilder.Append("мм, ");
                    innerBuilder.Append("контуры ");
                    innerBuilder.Append(formation.Outlines.EnumDescription());
                    innerBuilder.Append(", ");
                    innerBuilder.Append(formation.Echogenicity.EnumDescription());
                    innerBuilder.Append(", ");
                    innerBuilder.Append("внутренняя структура ");
                    innerBuilder.Append(formation.Structure.EnumDescription());
                    innerBuilder.Append(", ");
                    innerBuilder.Append("кровоток при ЦДК ");
                    innerBuilder.Append(formation.CDK.EnumDescription());
                    innerBuilder.Append(number != model.FocalFormations.Count ? ";" : ".");

                    builder.AppendLine(innerBuilder.ToString());
                }
            }
            else
            {
                builder.Append("не выявляются.");
            }
            return builder.ToString();
        }

        private static string MakeCysts(MammaModel model)
        {
            var builder = new StringBuilder();
            if (model.AreCysts)
            {
                if (!string.IsNullOrEmpty(model.CystsDesc))
                {
                    builder.Append("выявляются ");
                    builder.Append(model.CystsDesc ?? string.Empty);
                    if (model.CystsDesc != null && !model.CystsDesc.EndsWith("."))
                    {
                        builder.Append(".");
                    }
                }
                else
                {
                    builder.AppendLine("выявляются:");
                    if (model.Cysts == null) return builder.ToString();
                    foreach (var cyst in model.Cysts)
                    {
                        var innerBuilder = new StringBuilder();

                        var number = model.Cysts.IndexOf(cyst) + 1;

                        innerBuilder.Append(number);
                        innerBuilder.Append(". ");
                        innerBuilder.Append(cyst.Localization ?? string.Empty);
                        innerBuilder.Append(", ");
                        innerBuilder.Append("форма: ");
                        innerBuilder.Append(cyst.Form.EnumDescription());
                        innerBuilder.Append(", ");
                        innerBuilder.Append(cyst.Size ?? string.Empty);
                        innerBuilder.Append("мм, ");
                        innerBuilder.Append("контуры ");
                        innerBuilder.Append(cyst.Outlines.EnumDescription());
                        innerBuilder.Append(", ");
                        innerBuilder.Append(cyst.Echogenicity.EnumDescription());
                        innerBuilder.Append(", ");
                        innerBuilder.Append("внутренняя структура ");
                        innerBuilder.Append(cyst.Structure.EnumDescription());
                        innerBuilder.Append(", ");
                        innerBuilder.Append("кровоток при ЦДК ");
                        innerBuilder.Append(cyst.CDK.EnumDescription());
                        innerBuilder.Append(number != model.FocalFormations.Count ? ";" : ".");

                        builder.AppendLine(innerBuilder.ToString());
                    }
                }
            }
            else
            {
                builder.Append("не выявляются");
            }
            return builder.ToString();
        }

        private static string MakeNippleArea(MammaModel model)
        {
            var builder = new StringBuilder();
            switch (model.VisualizatioNippleArea)
            {
                case VisualizatioNippleArea.Good:
                    builder.Append("хорошая.");
                    break;
                case VisualizatioNippleArea.Imposible:
                    builder.Append("невозможна.");
                    break;
                case VisualizatioNippleArea.ObliqueProjection:
                    builder.Append("только в косых проекциях.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return builder.ToString();
        }


        private static string MakeTissue(MammaModel model)
        {
            var builder = new StringBuilder();
            builder.Append(model.Adipose.EnumDescription());
            builder.Append(" жировой, ");
            builder.Append(model.Grandular.EnumDescription());
            builder.Append(" железистой. ");
            return builder.ToString();
        }

        private static string MakeSkin(MammaModel model)
        {
            var builder = new StringBuilder();
            if (model.IsSkinChanged)
            {
                builder.Append("изменены, ");
                builder.Append(model.SkinChangedDesc ?? string.Empty);
            }
            else
            {
                builder.Append("не изменены");
            }
            return builder.ToString();
        }

        private static string MakeCanals(MammaModel model)
        {
            var builder = new StringBuilder();
            if (model.CanalsExpandingType == CanalsExpandingType.ExpandingSpecific)
            {
                builder.Append("расширены до ");
                builder.Append(model.CanalsExpandingDesc ?? string.Empty);
            }
            else
            {
                builder.Append(model.CanalsExpandingType.EnumDescription());
            }
            return builder.ToString();
        }

        private static string MakeStatus(MammaModel model)
        {
            var builder = new StringBuilder();
            switch (model.PhisiologicalStatus)
            {
                case PhisiologicalStatus.Normal:
                    builder.Append("1-й день последней менстуруации: ");
                    builder.Append(model.FirstDayOfLastMenstrualCycle.ToShortDateString());
                    break;
                case PhisiologicalStatus.Pregant:
                    builder.Append("Беременность");
                    break;
                case PhisiologicalStatus.Lactation:
                    builder.Append("Лактация");
                    break;
                case PhisiologicalStatus.Menopause:
                    builder.Append("Менопауза: ");
                    builder.Append(model.MenopauseText ?? string.Empty);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder.ToString();
        }

        private static string MakeDiffuseCahnges(MammaModel model)
        {
            var builder = new StringBuilder();
            switch (model.DiffuseChanges)
            {
                case DiffuseChanges.Moderate:
                    builder.Append("умеренные диффузные фиброзные изменения железистой ткани.");
                    break;
                case DiffuseChanges.Expressed:
                    builder.Append("выраженные диффузные фиброзные изменения железистой ткани.");
                    break;
                case DiffuseChanges.Minor:
                    builder.Append("незначительные диффузные фиброзные изменения железистой ткани.");
                    break;
                case DiffuseChanges.None:
                    builder.Append("нет.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (!string.IsNullOrEmpty(model.DiffuseChangesFeatures))
            {
                builder.Append(" ");
                builder.Append(model.DiffuseChangesFeatures);
            }
            return builder.ToString();
        }
    }
}