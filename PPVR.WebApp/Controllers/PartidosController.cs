using PPVR.WebApp.DataAccess;
using PPVR.WebApp.ViewModels.Partido;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;

namespace PPVR.WebApp.Controllers
{
    [Authorize]
    public class PartidosController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: Partidos
        public ActionResult Index(string q, string sort, int? page, string callbackAction)
        {
            ViewBag.CallbackAction = callbackAction;
            ViewBag.CurrentFilter = q;

            var partidos = _db.Partidos.Select(p => p).Include(p => p.Candidatos);

            #region Filters

            if (!string.IsNullOrEmpty(q))
            {
                partidos =
                    partidos.Where(p => p.Nome.Contains(q) || p.Sigla.Contains(q) || p.NumeroEleitoral.ToString() == q);
            }

            #endregion

            #region Order By

            ViewBag.CurrentSort = sort;

            ViewBag.SortNome = "nome";
            ViewBag.SortSigla = "sigla_desc";
            ViewBag.SortNumeroEleitoral = "numero_eleitoral";
            ViewBag.SortQtdeCandidatosAssociados = "qtde_candidatos_associados";

            switch (sort)
            {
                case "nome":
                    partidos = partidos.OrderBy(p => p.Nome);
                    ViewBag.SortNome = "nome_desc";
                    break;
                case "nome_desc":
                    partidos = partidos.OrderByDescending(p => p.Nome);
                    ViewBag.SortNome = "nome";
                    break;

                case "sigla":
                    partidos = partidos.OrderBy(p => p.Sigla);
                    ViewBag.SortSigla = "sigla_desc";
                    break;
                case "sigla_desc":
                    partidos = partidos.OrderByDescending(p => p.Sigla);
                    ViewBag.SortSigla = "sigla";
                    break;

                case "numero_eleitoral":
                    partidos = partidos.OrderBy(p => p.NumeroEleitoral);
                    ViewBag.SortNumeroEleitoral = "numero_eleitoral_desc";
                    break;
                case "numero_eleitoral_desc":
                    partidos = partidos.OrderByDescending(p => p.NumeroEleitoral);
                    ViewBag.SortNumeroEleitoral = "numero_eleitoral";
                    break;

                case "qtde_candidatos_associados":
                    partidos = partidos.OrderBy(p => p.Candidatos.Count);
                    ViewBag.SortQtdeCandidatosAssociados = "qtde_candidatos_associados_desc";
                    break;
                case "qtde_candidatos_associados_desc":
                    partidos = partidos.OrderByDescending(p => p.Candidatos.Count);
                    ViewBag.SortQtdeCandidatosAssociados = "qtde_candidatos_associados";
                    break;

                default:
                    partidos = partidos.OrderBy(p => p.Nome);
                    ViewBag.SortNome = "nome_desc";
                    break;
            }

            #endregion

            var pagedPartidos = partidos.ToPagedList(page ?? 1, 10);

            var partidoViewModel = pagedPartidos.Select(p => new PartidoViewModel
            {
                PartidoId = p.PartidoId,
                Nome = p.Nome,
                Sigla = p.Sigla,
                NumeroEleitoral = p.NumeroEleitoral,
                QtdeCandidatosAssociados = p.Candidatos.Count,
                Enabled = p.Enabled,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            });

            var pagedViewModel = new StaticPagedList<PartidoViewModel>(partidoViewModel, pagedPartidos.GetMetaData());
            return View(pagedViewModel);
        }
    }
}