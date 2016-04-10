using System;
using System.Collections.Generic;
using LiteDB;

namespace USD.MammaModels
{
    public class MammaModel
    {
        public ObjectId Id { get; set; }
        public DateTime VisitDate { get; set; }
        public string FIO { get; set; }
        public string BirthYear { get; set; }
        public PhisiologicalStatus PhisiologicalStatus { get; set; }
        public DateTime FirstDayOfLastMenstrualCycle { get; set; }
        public string MenopauseText { get; set; }
        public bool IsSkinChanged { get; set; }
        public string SkinChangedDesc { get; set; }

        [Obsolete("Переехало в отдельные поля по тканям")]
        public TissueRatio TissueRatio { get; set; }

        public TissueQuanity Grandular { get; set; }
        public TissueQuanity Adipose { get; set; }

        [Obsolete("Переехало на одно поле MaxThicknessGlandularLayer")]
        public decimal? LeftThicknessGlandularLayer { get; set; }

        [Obsolete("Переехало на одно поле MaxThicknessGlandularLayer")]
        public decimal? RightThicknessGlandularLayer { get; set; }

        public decimal? MaxThicknessGlandularLayer { get; set; }
        public bool ActualToPhase { get; set; } = true;
        public bool IsCanalsExpanded { get; set; }
        public string CanalsExpandingDesc { get; set; }
        public DiffuseChanges DiffuseChanges { get; set; }
        public string DiffuseChangesFeatures { get; set; }
        public VisualizatioNippleArea VisualizatioNippleArea { get; set; }
        public bool AreCysts { get; set; }
        public string CystsDesc { get; set; }
        public bool AreFocalFormations { get; set; }
        public List<FocalFormationModel> FocalFormations { get; set; }
        public bool IsDeterminateLymphNodes { get; set; }
        public string LymphNodesDesc { get; set; }
        public string AdditionalDesc { get; set; }
        public bool IsNotPatalogyConclusion { get; set; }
        public bool IsCystsConclusion { get; set; }
        public string CystConslusionDesc { get; set; }
        public bool IsInvolutionConclusion { get; set; }
        public bool IsSpecificConclusion { get; set; }
        public bool IsAdenosisConclusion { get; set; }
        public bool IsFocalFormationConclusion { get; set; }
        public FocalFormationConclusionPosition FocalFormationConclusionPosition { get; set; }
        public string SpecificConclusionDesc { get; set; }
        public MammaSpecialists Recomendation { get; set; }
        public List<CystModel> Cysts { get; set; }
    }
}