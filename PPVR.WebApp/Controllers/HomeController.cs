using Geocoding.Google;
using PPVR.Common.Extensions;
using PPVR.Common.Helpers.Geocoding;
using PPVR.Common.Helpers.OCR;
using PPVR.WebApp.DataAccess;
using PPVR.WebApp.Models;
using PPVR.WebApp.Resources;
using PPVR.WebApp.ViewModels.Home;
using PPVR.WebApp.ViewModels.TipoPropaganda;
using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            var uploadFotoViewModel = new UploadFotoViewModel
            {
                TiposPropaganda = _db.TiposPropaganda
                    .Where(tp => tp.Enabled)
                    .Select(tp => new TipoPropagandaViewModel
                    {
                        TipoPropagandaId = tp.TipoPropagandaId,
                        Descricao = tp.Descricao
                    })
                    .OrderBy(tp => tp.Descricao)
                    .ToList()
            };

            return View(uploadFotoViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(UploadFotoViewModel viewModel)
        {
            var validImageTypes = new[] { "image/jpeg", "image/pjpeg", "image/png" };

            if (viewModel.ImageUpload == null || viewModel.ImageUpload.ContentLength == 0)
                ModelState.AddModelError("ImageUpload", ValidationErrorMessage.OcorrenciaFotoNotNull);
            else if (!validImageTypes.Contains(viewModel.ImageUpload.ContentType))
                ModelState.AddModelError("ImageUpload", ValidationErrorMessage.OcorrenciaFotoInvalidFormat);

            viewModel.TiposPropaganda = _db.TiposPropaganda.Select(x => new TipoPropagandaViewModel
            {
                TipoPropagandaId = x.TipoPropagandaId,
                Descricao = x.Descricao
            }).ToList();

            if (ModelState.IsValid)
            {
                var extension = Path.GetExtension(viewModel.ImageUpload.FileName);
                var filePath = "";

                try
                {
                    var geolocationInfo = new GeolocationInfoFileExtractor(viewModel.ImageUpload.InputStream);
                    var path =
                        Server.MapPath(
                            $"{WebConfigurationManager.AppSettings["DiretorioFotosSantinhos"]}{User.Identity.Name}/");
                    Directory.CreateDirectory(path);
                    filePath = $"{path}{Guid.NewGuid()}{extension}";
                    ViewBag.FilePath = filePath;
                    viewModel.ImageUpload.SaveAs(filePath);

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
                    var candidatos = _db.Candidatos.Include(c => c.Eleicao)
                        .Where(c => c.DescricaoUnidadeEleitoral == endereco.Cidade && c.Eleicao.Enabled)
                        .ToList();

                    var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
                    var match = false;

                    foreach (var candidato in candidatos)
                    {
                        //var matchByNomeUrna = compareInfo.IndexOf(imageText, candidato.NomeUrna, CompareOptions.IgnoreNonSpace) > -1;
                        //var matchByNumeroEleitoral = compareInfo.IndexOf(imageText, candidato.NumeroEleitoral.ToString(), CompareOptions.IgnoreNonSpace) > -1;

                        //if (matchByNomeUrna && matchByNumeroEleitoral)
                        //    ocorrenciaTipoMatch = OcorrenciaTipoMatch.NomeUrnaENumeroEleitoral;
                        //else if (matchByNomeUrna)
                        //    ocorrenciaTipoMatch = OcorrenciaTipoMatch.NomeUrna;
                        //else if (matchByNumeroEleitoral)
                        //    ocorrenciaTipoMatch = OcorrenciaTipoMatch.NumeroEleitoral;

                        var ocorrenciaTipoMatch = PesquisarCandidatoTexto(candidato.NomeUrna, candidato.NumeroEleitoral, imageText);

                        if (ocorrenciaTipoMatch.HasValue)
                        {
                            match = true;
                            viewModel.CandidatosEncontrados.Add(candidato.NomeUrna);

                            _db.Ocorrencias.Add(new Ocorrencia
                            {
                                FotoPath = filePath,
                                TipoPropagandaId = viewModel.TipoPropaganda,
                                OcorrenciaTipoMatch = ocorrenciaTipoMatch,
                                Endereco = endereco,
                                CandidatoId = candidato.CandidatoId
                            });
                        }
                    }

                    if (!match)
                    {
                        _db.Ocorrencias.Add(new Ocorrencia
                        {
                            FotoPath = filePath,
                            TipoPropagandaId = viewModel.TipoPropaganda,
                            Endereco = endereco
                        });
                    }

                    _db.SaveChanges();
                    ViewBag.FotoSalva = true;
                }
                catch (GoogleGeocodingException)
                {
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    ViewBag.ErrorMessage = ValidationErrorMessage.UploadFotoGoogleGeocodingException;
                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                    ViewBag.ErrorMessage = ex.Message;
                    return View(viewModel);
                }
            }

            viewModel.CandidatosEncontrados = viewModel.CandidatosEncontrados.OrderBy(c => c).ToList();
            return View(viewModel);
        }

        public ActionResult Sobre()
        {
            return View();
        }

        private static OcorrenciaTipoMatch? PesquisarCandidatoTexto(string nomeCandidato, int numeroEleitoral,
            string texto)
        {
            OcorrenciaTipoMatch? matchType = null;
            var containsNumeroEleitoral = false;
            var regex = new Regex(@"\d+");
            var palavras = texto.Replace(".", "").Replace(",", "").Split(' ');

            foreach (var palavra in palavras)
            {
                var match = regex.Match(palavra);
                if (match.Success)
                {
                    if (match.Value == numeroEleitoral.ToString())
                    {
                        matchType = OcorrenciaTipoMatch.NumeroEleitoral;
                        containsNumeroEleitoral = true;
                    }
                }
            }

            if (containsNumeroEleitoral)
            {
                if (texto.Contains(nomeCandidato, StringComparison.OrdinalIgnoreCase))
                    matchType = OcorrenciaTipoMatch.NomeUrnaENumeroEleitoral;
            }
            else
            {
                if (texto.Contains(nomeCandidato, StringComparison.OrdinalIgnoreCase))
                    matchType = OcorrenciaTipoMatch.NomeUrna;
            }

            return matchType;
        }
    }
}