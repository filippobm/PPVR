namespace PPVR.OCRBenchmark.Entities
{
    public class SantinhoPolitico
    {
        public string ImageFilePath { get; set; }
        public int NumeroEleitoral { get; set; }
        public string NomeCandidato { get; set; }
        public string TextoMicrosoftCognitiveServices { get; set; }
        public MatchType? MatchMicrosoftCognitiveServices { get; set; }
        public string TextoTesseract { get; set; }
        public MatchType? MatchTesseract { get; set; }
    }
}