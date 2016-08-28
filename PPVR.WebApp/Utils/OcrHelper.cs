using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PPVR.WebApp.Utils
{
    public class OcrHelper
    {
        public static async Task<string> UploadAndRecognizeImage(Stream imageStream)
        {
            var visionServiceClient =
                new VisionServiceClient(ConfigurationManager.AppSettings["MicrosoftComputerVisionAPIKey1"]);

            var ocrResults = await visionServiceClient.RecognizeTextAsync(imageStream, "pt");
            return GetTextOcrResults(ocrResults);
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