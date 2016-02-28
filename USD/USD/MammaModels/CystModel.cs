namespace USD.MammaModels
{
    public class CystModel
    {
        public OutlinesType Outlines { get; set; }
        public string Localization { get; set; }
        public string Size { get; set; }
        public Echogenicity Echogenicity { get; set; }
        public Structure Structure { get; set; }
        public CDK CDK { get; set; }
    }
}