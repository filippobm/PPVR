using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PPVR.Common.Helpers.OCR
{
    public static class MicrosoftCognitiveServicesHelper
    {
        private const string ApiKey1 = "ENTER_YOUR_APIKEY";
        private const string LanguageCode = "pt";
        //private const string ApiKey2 = "ENTER_YOUR_APIKEY";

        public static async Task<string> UploadAndRecognizeImage(string imageFilePath)
        {
            using (Stream imageStream = File.OpenRead(imageFilePath))
            {
                var visionServiceClient = new VisionServiceClient(ApiKey1);

                var ocrResults = await visionServiceClient.RecognizeTextAsync(imageStream, LanguageCode);
                return GetTextOcrResults(ocrResults);
            }
        }

        /// <summary>
        ///     Get text from the given OCR results object.
        /// </summary>
        /// <param name="ocrResults"></param>
        /// <returns>The OCR results.</returns>
        private static string GetTextOcrResults(OcrResults ocrResults)
        {
            var stringBuilder = new StringBuilder();

            if (ocrResults?.Regions != null)
            {
                foreach (var region in ocrResults.Regions)
                {
                    foreach (var line in region.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            stringBuilder.Append(word.Text);
                            stringBuilder.Append(" ");
                        }
                        stringBuilder.AppendLine();
                    }
                    stringBuilder.AppendLine();
                }
            }
            return stringBuilder.ToString().Trim();
        }
    }
}
