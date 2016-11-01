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

            if (extension != null && (extension.ToLower().EndsWith(".png") || extension.ToLower().EndsWith(".jpg") || extension.ToLower().EndsWith(".jpge")))
            {
                var filePath = "";
                try
                {
                    var geolocationInfo = new GeolocationInfoFileExtractor(file.InputStream);
                    var path = Server.MapPath($"{WebConfigurationManager.AppSettings["DiretorioFotosSantinhos"]}{User.Identity.Name}/");
                    Directory.CreateDirectory(path);
                    filePath = $"{path}{Guid.NewGuid()}{extension}";
                    ViewBag.FilePath = filePath;
                    file.SaveAs(filePath);

                    var googleGeocoder = new GoogleGeocoder(WebConfigurationManager.AppSettings["GoogleMapsAPIKey"]);
                    var enderecos = googleGeocoder.ReverseGeocode(geolocationInfo.Latitude, geolocationInfo.Longitude);
                    var endereco =
                        enderecos.Where(
                            e =>
                                e.LocationType == GoogleLocationType.Rooftop ||
                                e.LocationType == GoogleLocationType.RangeInterpolated).Select(x => new Endereco
                                {
                                    EnderecoFormatado = x.FormattedAddress,
                                    Latitude = x.Coordinates.Latitude,
                                    Longitude = x.Coordinates.Longitude,
                                    Estado = x[GoogleAddressType.AdministrativeAreaLevel1].ShortName,
                                    Cidade = x[GoogleAddressType.Locality].LongName,
                                    CEP = x[GoogleAddressType.PostalCode].LongName,
                                    Bairro = x[GoogleAddressType.SubLocality].LongName
                                }).FirstOrDefault();

                    var imageText = await MicrosoftCognitiveServicesHelper.UploadAndRecognizeImage(filePath);

                    // Busca o(s) candidato(s) da cidade onde a foto foi tirada
                    var candidatos = _db.Candidatos.Where(c => c.DescricaoUnidadeEleitoral == endereco.Cidade).ToList();

                    var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
                    var match = false;

                    foreach (var candidato in candidatos)
                    {
                        var matchByNomeUrna = compareInfo.IndexOf(imageText, candidato.NomeUrna, CompareOptions.IgnoreNonSpace) > -1;
                        var matchByNumeroEleitoral = compareInfo.IndexOf(imageText, candidato.NumeroEleitoral.ToString(), CompareOptions.IgnoreNonSpace) > -1;

                        OcorrenciaTipoMatch? ocorrenciaTipoMatch = null;

                        if (matchByNomeUrna && matchByNumeroEleitoral)
                            ocorrenciaTipoMatch = OcorrenciaTipoMatch.NomeUrnaENumeroEleitoral;
                        else if (matchByNomeUrna)
                            ocorrenciaTipoMatch = OcorrenciaTipoMatch.NomeUrna;
                        else if (matchByNumeroEleitoral)
                            ocorrenciaTipoMatch = OcorrenciaTipoMatch.NumeroEleitoral;

                        if (ocorrenciaTipoMatch.HasValue)
                        {
                            match = true;

                            _db.Ocorrencias.Add(new Ocorrencia
                            {
                                FotoPath = filePath,
                                TipoPropagandaId = 1,
                                OcorrenciaTipoMatch = ocorrenciaTipoMatch,
                                Endereco = endereco,
                                CandidatoId = candidato.CandidatoId
                            });
                        }
                    }

                    if (!match)
                        _db.Ocorrencias.Add(new Ocorrencia { FotoPath = filePath, TipoPropagandaId = 1, Endereco = endereco });

                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    System.IO.File.Delete(filePath);
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