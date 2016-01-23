namespace USD.MammaModels
{
    public class FocalFormationModel
    {
        public OutlinesType Outlines { get; set; }
        public string Localization { get; set; }
        public decimal? Size { get; set; }
        public Echogenicity Echogenicity { get; set; }
        public Structure Structure { get; set; }
    }
}