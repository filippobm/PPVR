using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace PPVR.WebApp.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sobre()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadPicture(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0)
                return RedirectToAction("Index");

            var extension = Path.GetExtension(file.FileName);

            if (extension != null &&
                (extension.ToLower().EndsWith(".png") || extension.ToLower().EndsWith(".jpg") ||
                 extension.ToLower().EndsWith(".jpge")))
            {
                var path = Server.MapPath($"~/Content/images/santinhos/{User.Identity.Name}/");
                Directory.CreateDirectory(path);
                var filePath = $"{path}{Guid.NewGuid()}{extension}";
                ViewBag.FilePath = filePath;
                file.SaveAs(filePath);
                return RedirectToAction("Index");
            }
            throw new Exception("AAAAA");
        }
    }
}