namespace PPVR.Common.Helpers.OCR.OCRSpace
{
    public class ParsedResult
    {
        public object FileParseExitCode { get; set; }
        public string ParsedText { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
    }
}