using System.Linq;
using System.Web.Mvc;
using PPVR.WebApp.DataAccess;
using X.PagedList;

namespace PPVR.WebApp.Controllers
{
    public class PartidosController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        // GET: Partidos
        public ActionResult Index(string orderBy, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = orderBy;
            ViewBag.NomeSortParm = string.IsNullOrEmpty(orderBy) ? "nome_desc" : "";
            ViewBag.SiglaSortParm = string.IsNullOrEmpty(orderBy) ? "sigla_desc" : "";
            ViewBag.NumeroEleitoralSortParm = string.IsNullOrEmpty(orderBy) ? "num_eleitoral_desc" : "";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var q = db.Partidos.Select(p => p);

            if (!string.IsNullOrEmpty(searchString))
                q =
                    q.Where(
                        p =>
                            p.Nome.Contains(searchString) || p.Sigla.Contains(searchString) ||
                            p.NumeroEleitoral.ToString() == searchString);

            #region Order By

            switch (orderBy)
            {
                case "nome_desc":
                    q = q.OrderByDescending(p => p.Nome);
                    break;
                case "sigla_desc":
                    q = q.OrderByDescending(p => p.Sigla);
                    break;
                case "sigla":
                    q = q.OrderBy(p => p.Sigla);
                    break;
                case "num_eleitoral":
                    q = q.OrderBy(p => p.NumeroEleitoral);
                    break;
                case "num_eleitoral_desc":
                    q = q.OrderByDescending(p => p.NumeroEleitoral);
                    break;
                default:
                    q = q.OrderBy(p => p.Nome);
                    break;
            }

            #endregion

            var pagedPartidos = q.ToPagedList(page ?? 1, 10);
            return View(pagedPartidos);
        }
    }
}