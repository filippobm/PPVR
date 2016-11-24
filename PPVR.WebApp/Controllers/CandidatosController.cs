using PPVR.WebApp.DataAccess;
using PPVR.WebApp.ViewModels.Candidato;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using X.PagedList;

namespace PPVR.WebApp.Controllers
{
    [Authorize]
    public class CandidatosController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: Candidatos
        public ActionResult Index(string q, string sort, int? page, string callbackAction)
        {
            ViewBag.CallbackAction = callbackAction;
            ViewBag.CurrentFilter = q;

            var candidatos =
                _db.Candidatos.Select(c => c)
                    .Include(c => c.Partido)
                    .Include(c => c.Eleicao)
                    .Where(c => c.Enabled && c.Eleicao.Enabled);

            #region Filters

            if (!string.IsNullOrEmpty(q))
                candidatos =
                    candidatos.Where(
                        c => c.NumeroEleitoral.ToString() == q || c.Nome.Contains(q) || c.NomeUrna.Contains(q));

            #endregion

            #region Order By

            ViewBag.CurrentSort = sort;

            ViewBag.SortNomeUrna = "nome_urna";
            ViewBag.SortNumeroEleitoral = "numero_eleitoral";
            ViewBag.SortSiglaPartido = "sigla_partido";
            ViewBag.SortUnidadeEleitoral = "unidade_eleitoral";

            switch (sort)
            {
                case "nome_urna":
                    candidatos = candidatos.OrderBy(c => c.NomeUrna);
                    ViewBag.SortNomeUrna = "nome_urna_desc";
                    break;
                case "nome_urna_desc":
                    candidatos = candidatos.OrderByDescending(c => c.NomeUrna);
                    ViewBag.SortNomeUrna = "nome_urna";
                    break;

                case "numero_eleitoral":
                    candidatos = candidatos.OrderBy(c => c.NumeroEleitoral);
                    ViewBag.SortNumeroEleitoral = "numero_eleitoral_desc";
                    break;
                case "numero_eleitoral_desc":
                    candidatos = candidatos.OrderByDescending(c => c.NumeroEleitoral);
                    ViewBag.SortNumeroEleitoral = "numero_eleitoral";
                    break;

                case "sigla_partido":
                    candidatos = candidatos.OrderBy(c => c.Partido.Sigla);
                    ViewBag.SortSiglaPartido = "sigla_partido_desc";
                    break;
                case "sigla_partido_desc":
                    candidatos = candidatos.OrderByDescending(c => c.Partido.Sigla);
                    ViewBag.SortSiglaPartido = "sigla_partido";
                    break;

                case "unidade_eleitoral":
                    candidatos = candidatos.OrderBy(c => c.DescricaoUnidadeEleitoral);
                    ViewBag.SortUnidadeEleitoral = "unidade_eleitoral_desc";
                    break;
                case "unidade_eleitoral_desc":
                    candidatos = candidatos.OrderByDescending(c => c.DescricaoUnidadeEleitoral);
                    ViewBag.SortUnidadeEleitoral = "unidade_eleitoral";
                    break;

                default:
                    candidatos = candidatos.OrderBy(c => c.NomeUrna);
                    ViewBag.SortNomeUrna = "nome_urna_desc";
                    break;
            }

            #endregion

            var pagedCandidatos = candidatos.ToPagedList(page ?? 1, 10);

            var candidatoViewModel = pagedCandidatos.Select(c => new CandidatoViewModel
            {
                CandidatoId = c.CandidatoId,
                Nome = c.Nome,
                NomeUrna = c.NomeUrna,
                NumeroEleitoral = c.NumeroEleitoral,
                Enabled = c.Enabled,
                PartidoSigla = c.Partido.Sigla,
                UnidadeEleitoral = c.DescricaoUnidadeEleitoral
            });

            var pagedViewModel = new StaticPagedList<CandidatoViewModel>(candidatoViewModel,
                pagedCandidatos.GetMetaData());

            return View(pagedViewModel);
        }

        // GET: Candidatos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var candidato =
                _db.Candidatos.Where(c => c.CandidatoId == id)
                    .Include(c => c.Partido)
                    .Include(c => c.Ocorrencias.Select(o => o.TipoPropaganda))
                    .Select(c => new CandidatoViewModel
                    {
                        CandidatoId = c.CandidatoId,
                        Nome = c.Nome,
                        NomeUrna = c.NomeUrna,
                        NumeroEleitoral = c.NumeroEleitoral,
                        UnidadeEleitoral = c.DescricaoUnidadeEleitoral,
                        UnidadeFederacao = c.SiglaUnidadeFederacao,
                        Enabled = c.Enabled,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        Partido = c.Partido.Nome,
                        Gastos = c.Ocorrencias.GroupBy(g => g.TipoPropagandaId).Select(o => new GastoCandidatoViewModel
                        {
                            TipoPropagandaId = o.Key,
                            TipoPropagandaDescricao = o.Select(x => x.TipoPropaganda.Descricao).FirstOrDefault(),
                            QtdeOcorrencias = o.Count(),
                            ValorMedio = o.Select(x => x.TipoPropaganda.ValorMedio).FirstOrDefault()
                        }).ToList()
                    }).SingleOrDefault();

            if (candidato == null)
                return HttpNotFound();

            return View(candidato);
        }
    }
}