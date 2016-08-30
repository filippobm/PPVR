namespace PPVR.OCRBenchmark.Entities
{
    public class SantinhoPolitico
    {
        public string ImageFilePath { get; set; }
        public int NumeroEleitoral { get; set; }
        public string NomeCandidato { get; set; }

        // Microsoft Cognitive Services
        public string TextoMicrosoftCognitiveServices { get; set; }
        public MatchType? MatchMicrosoftCognitiveServices { get; set; }

        // Tesseract OCR
        public string TextoTesseract { get; set; }
        public MatchType? MatchTesseract { get; set; }

        // OCR.Space
        public string TextoOCRSpace { get; set; }
        public MatchType? MatchOCRSpace { get; set; }
    }
}