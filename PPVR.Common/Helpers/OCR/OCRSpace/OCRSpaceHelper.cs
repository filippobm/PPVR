using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PPVR.Common.Helpers.OCR.OCRSpace
{
    public static class SpaceHelper
    {
        private const string ApiKey = "ENTER_YOUR_APIKEY";
        private const string Language = "por";

        public static async Task<string> UploadAndRecognizeImage(string imageFilePath)
        {
            var httpClient = new HttpClient
            {
                Timeout = new TimeSpan(1, 1, 1)
            };

            var form = new MultipartFormDataContent
            {
                {new StringContent(ApiKey), "apikey"},
                {new StringContent(Language), "language"}
            };

            var imageData = File.ReadAllBytes(imageFilePath);
            form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.jpg");

            var response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
            var strContent = await response.Content.ReadAsStringAsync();
            var ocrResult = JsonConvert.DeserializeObject<RootObject>(strContent);

            var stringBuilder = new StringBuilder();

            if (ocrResult.OCRExitCode == 1)
            {
                foreach (var parsedResult in ocrResult.ParsedResults)
                {
                    stringBuilder.Append(parsedResult.ParsedText);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
