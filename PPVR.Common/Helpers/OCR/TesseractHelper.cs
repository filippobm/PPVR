using Tesseract;

namespace PPVR.Common.Helpers.OCR
{
    public static class TesseractHelper
    {
        public static string UploadAndRecognizeImage(string imageFilePath)
        {
            var text = "";

            using (var engine = new TesseractEngine(@"./tessdata", "por", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(imageFilePath))
                {
                    using (var page = engine.Process(img))
                    {
                        text = page.GetText();
                    }
                }
            }
            return text;
        }
    }
}