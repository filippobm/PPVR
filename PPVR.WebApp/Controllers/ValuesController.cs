using Microsoft.AspNet.Identity;
using PPVR.WebApp.Utils;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace PPVR.WebApp.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public string Get()
        {
            return User.Identity.GetUserId();
        }

        public async Task<string> GetTextOcr()
        {
            var imageFilePath = @"C:\Projects\Net\ConsoleApplication1\imgs\43_dr guimaraes_prefeito.jpg";

            string text = "";

            using (Stream imageStream = File.OpenRead(imageFilePath))
            {
                text = await OcrHelper.UploadAndRecognizeImage(imageStream);
            }
            return text;
        }
    }
}