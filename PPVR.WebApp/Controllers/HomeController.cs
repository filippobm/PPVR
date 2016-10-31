using Geocoding.Google;
using PPVR.Common.Helpers.Geocoding;
using PPVR.Common.Helpers.OCR;
using PPVR.WebApp.DataAccess;
using PPVR.WebApp.Models;
using PPVR.WebApp.Resources;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace PPVR.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

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
        public async Task<ActionResult> UploadPicture(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0)
                return RedirectToAction("Index");

            var extension = Path.GetExtension(file.FileName);

            if (extension != null &&
                (extension.ToLower().EndsWith(".png") || extension.ToLower().EndsWith(".jpg") ||
                 extension.ToLower().EndsWith(".jpge")))
            {
                try
                {
                    var geolocationInfo = new GeolocationInfoFileExtractor(file.InputStream);
                    var path =
                        Server.MapPath(
                            $"{WebConfigurationManager.AppSettings["DiretorioFotosSantinhos"]}{User.Identity.Name}/");
                    Directory.CreateDirectory(path);
                    var filePath = $"{path}{Guid.NewGuid()}{extension}";
                    ViewBag.FilePath = filePath;
                    file.SaveAs(filePath);

                    var ocorrencia = new Ocorrencia
                    {
                        FotoPath = filePath,
                        Verificada = false,
                        TipoPropagandaId = 1
                    };

                    var googleGeocoder = new GoogleGeocoder(WebConfigurationManager.AppSettings["GoogleMapsAPIKey"]);
                    var enderecos = googleGeocoder.ReverseGeocode(geolocationInfo.Latitude, geolocationInfo.Longitude);

                    ocorrencia.Endereco = enderecos
                        .Where(
                            e =>
                                e.LocationType == GoogleLocationType.Rooftop ||
                                e.LocationType == GoogleLocationType.RangeInterpolated)
                        .Select(x => new Endereco
                        {
                            EnderecoFormatado = x.FormattedAddress,
                            Latitude = x.Coordinates.Latitude,
                            Longitude = x.Coordinates.Longitude,
                            Estado = x[GoogleAddressType.AdministrativeAreaLevel1].ShortName,
                            Cidade = x[GoogleAddressType.Locality].LongName,
                            CEP = x[GoogleAddressType.PostalCode].LongName,
                            Bairro = x[GoogleAddressType.SubLocality].LongName
                        }).FirstOrDefault();

                    #region Busca o(s) candidato(s) da foto

                    var candidatos = _db.Candidatos
                        .Where(c => c.DescricaoUnidadeEleitoral == ocorrencia.Endereco.Cidade)
                        .ToList();

                    if (candidatos.Any())
                    {
                        var imageText = await MicrosoftCognitiveServicesHelper.UploadAndRecognizeImage(filePath);

                        var compareInfo = CultureInfo.InvariantCulture.CompareInfo;

                        foreach (var candidato in candidatos)
                        {
                            var matchByNomeUrna = compareInfo.IndexOf(imageText, candidato.NomeUrna, CompareOptions.IgnoreNonSpace) > -1;
                            var matchByNumeroEleitoral = compareInfo.IndexOf(imageText, candidato.NumeroEleitoral.ToString(), CompareOptions.IgnoreNonSpace) > -1;

                            string matchType = "";

                            if (matchByNomeUrna && matchByNumeroEleitoral)
                                matchType = "NomeUrnaENumeroEleitoral";
                            else if (matchByNomeUrna)
                                matchType = "NomeUrna";
                            else if (matchByNumeroEleitoral)
                                matchType = "NumeroEleitoral";

                            return new HttpStatusCodeResult(HttpStatusCode.OK, matchType);
                        }
                    }

                    #endregion

                    _db.Ocorrencias.Add(ocorrencia);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                    ValidationErrorMessage.OcorrenciaFotoInvalidFormat);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}