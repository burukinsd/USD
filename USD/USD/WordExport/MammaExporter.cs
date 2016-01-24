﻿using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using Novacode;
using USD.MammaModels;

namespace USD.WordExport
{
    public static class MammaExporter
    {
        public static void Export(MammaModel model)
        {
            var directoryFullPath = EnsureDirectory();

            var fileFullPath =
                $"{directoryFullPath}\\{model.VisitDate.ToShortDateString()} {model.FIO} {model.BirthYear}.docx";

            using (DocX document = DocX.Load(@"Templates\MammaTemplate.docx"))
            {
                document.ReplaceText("%VisitDate%", model.VisitDate.ToShortDateString());

                document.ReplaceText("%FIO%", model.FIO ?? String.Empty);

                document.ReplaceText("%BirthYear%", model.BirthYear ?? String.Empty);

                document.ReplaceText("%Status%", MakeStatus(model));

                document.ReplaceText("%Skin%", MakeSkin(model));

                document.ReplaceText("%Tissue%", MakeTissue(model));

                document.ReplaceText("%Grandular%", $"справа - {model.RightThicknessGlandularLayer ?? 0}мм, слева - {model.LeftThicknessGlandularLayer ?? 0}мм.");

                document.ReplaceText("%Canals%", MakeCanals(model));

                document.ReplaceText("%DiffuseChanges%", MakeDiffuseCahnges(model));

                document.ReplaceText("%NippleArea%", MakeNippleArea(model));

                document.ReplaceText("%Cyst%", MakeCysts(model));

                document.ReplaceText("%FocalFormation%", MakeFocalFormations(model));

                document.ReplaceText("%LymphNodes%", MakeLymphNodes(model));

                document.ReplaceText("%AdditionalInfo%", String.IsNullOrEmpty(model.AdditionalDesc) ? String.Empty : $"\r\n{model.AdditionalDesc}");

                document.ReplaceText("%Conclusion%", ConclusionMaker.MakeConclusion(model));

                document.ReplaceText("%Recomendation%", model.Recomendation == MammaSpecialists.None ? String.Empty : $"\r\nРекомендована консультация: {model.Recomendation.EnumDescription()}");

                document.SaveAs(fileFullPath);
            }

            Process.Start(fileFullPath);
        }

        private static string EnsureDirectory()
        {
            var direc = new DirectoryInfo("Узи молочной железы");
            if (!direc.Exists)
            {
                direc.Create();
            }
            return direc.FullName;
        }

        private static string MakeLymphNodes(MammaModel model)
        {
            var builder = new StringBuilder();
            if (model.IsDeterminateLymphNodes)
            {
                builder.Append("определяются: ");
                builder.Append(model.LymphNodesDesc ?? String.Empty);
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
                builder.Append("выявляются:");
                if (model.FocalFormations == null) return builder.ToString();
                foreach (var formation in model.FocalFormations)
                {
                    var innerBuilder = new StringBuilder();

                    var number = model.FocalFormations.IndexOf(formation) + 1;

                    innerBuilder.Append(number);
                    innerBuilder.Append(". ");
                    innerBuilder.Append(formation.Localization ?? String.Empty);
                    innerBuilder.Append(", ");
                    innerBuilder.Append(formation.Size ?? 0);
                    innerBuilder.Append("мм, ");
                    innerBuilder.Append("контуры ");
                    innerBuilder.Append(formation.Outlines.EnumDescription());
                    innerBuilder.Append(", ");
                    innerBuilder.Append(formation.Echogenicity.EnumDescription());
                    innerBuilder.Append(", ");
                    innerBuilder.Append("внутренняя структура ");
                    innerBuilder.Append(formation.Structure.EnumDescription());
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
                builder.Append("выявляются ");
                builder.Append(model.CystsDesc ?? String.Empty);
                if (model.CystsDesc != null && !model.CystsDesc.EndsWith("."))
                {
                    builder.Append(".");
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
            switch (model.TissueRatio)
            {
                case TissueRatio.MoreGlandularLessAdipose:
                    builder.Append("много железистой и мало жировой (подкожной).");
                    break;
                case TissueRatio.EnoughGlandularMoreAdipose:
                    builder.Append("достаточно железистой и много жировой (подкожной и в центральном отделе).");
                    break;
                case TissueRatio.LessGlandular:
                    builder.Append("мало железистой в виде единичных включений между жировой клетчаткой.");
                    break;
                case TissueRatio.MoreAdipose:
                    builder.Append("много жировой (подкожной, в центральнх, задних отделах).");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return builder.ToString();
        }

        private static string MakeSkin(MammaModel model)
        {
            var builder = new StringBuilder();
            if (model.IsSkinChanged)
            {
                builder.Append("изменены, ");
                builder.Append(model.SkinChangedDesc ?? String.Empty);
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
            if (model.IsCanalsExpanded)
            {
                builder.Append("расширены до");
                builder.Append(model.CanalsExpandingDesc ?? String.Empty);
            }
            else
            {
                builder.Append("не расширены");
            }
            return builder.ToString();
        }

        private static string MakeStatus(MammaModel model)
        {
            var builder = new StringBuilder();
            switch (model.PhisiologicalStatus)
            {
                case PhisiologicalStatus.Normal:
                    builder.Append("1-й день менстурального цикла: ");
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
                    builder.Append(model.MenopauseText ?? String.Empty);
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