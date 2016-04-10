namespace USD.MammaModels
{
    public class FocalFormationModel
    {
        public OutlinesType Outlines { get; set; }
        public string Localization { get; set; }
        public string Size { get; set; }
        public Echogenicity Echogenicity { get; set; }
        public Structure Structure { get; set; }
        public CDK CDK { get; set; }
        public FormationForm Form { get; set; }
    }
}