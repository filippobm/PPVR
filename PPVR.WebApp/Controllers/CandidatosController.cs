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

            var candidatos = _db.Candidatos.Select(c => c).Include(c => c.Partido);

            if (!string.IsNullOrEmpty(q))
                candidatos = candidatos.Where(c => c.NumeroEleitoral.ToString() == q || c.Nome.Contains(q) || c.NomeUrna.Contains(q));

            #region Order By

            ViewBag.CurrentSort = sort;

            ViewBag.SortNome = "nome";
            ViewBag.SortNomeUrna = "nome_urna";
            ViewBag.SortNumeroEleitoral = "numero_eleitoral";
            ViewBag.SortSiglaPartido = "sigla_partido";

            switch (sort)
            {
                case "nome":
                    candidatos = candidatos.OrderBy(c => c.Nome);
                    ViewBag.SortNome = "nome_desc";
                    break;
                case "nome_desc":
                    candidatos = candidatos.OrderByDescending(c => c.Nome);
                    ViewBag.SortNome = "nome";
                    break;

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

                default:
                    candidatos = candidatos.OrderBy(p => p.Nome);
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
                PartidoSigla = c.Partido.Sigla
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
                        Partido = c.Partido.Nome
                    }).SingleOrDefault();

            if (candidato == null)
                return HttpNotFound();

            return View(candidato);
        }
    }
}