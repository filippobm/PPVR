using PPVR.WebApp.DataAccess;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;

namespace PPVR.WebApp.Controllers
{
    public class CandidatosController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: Candidatos
        public ActionResult Index(string q, string sort, int? page)
        {
            ViewBag.CurrentFilter = q;

            var candidatos = _db.Candidatos.Select(c => c);

            if (!string.IsNullOrEmpty(q))
                candidatos =
                    candidatos.Where(
                        c => c.NumeroEleitoral.ToString() == q || c.Nome.Contains(q) || c.NomeUrna.Contains(q));

            #region Order By

            ViewBag.CurrentSort = sort;

            if (string.IsNullOrEmpty(sort))
            {
                ViewBag.SortNome = "nome_desc";
                ViewBag.SortNomeUrna = "nome_urna_desc";
                ViewBag.SortNumeroEleitoral = "numero_eleitoral_desc";
            }
            else
            {
                ViewBag.SortNome = "";
                ViewBag.SortNomeUrna = "";
                ViewBag.SortNumeroEleitoral = "";
            }

            switch (sort)
            {
                case "nome_desc":
                    candidatos = candidatos.OrderByDescending(c => c.Nome);
                    break;
                case "nome_urna":
                    candidatos = candidatos.OrderBy(c => c.NomeUrna);
                    break;
                case "nome_urna_desc":
                    candidatos = candidatos.OrderByDescending(p => p.NomeUrna);
                    break;
                case "numero_eleitoral":
                    candidatos = candidatos.OrderBy(c => c.NumeroEleitoral);
                    break;
                case "numero_eleitoral_desc":
                    candidatos = candidatos.OrderByDescending(p => p.NumeroEleitoral);
                    break;
                default:
                    candidatos = candidatos.OrderBy(p => p.Nome);
                    break;
            }

            #endregion

            var pagedCandidatos = candidatos.ToPagedList(page ?? 1, 10);
            return View(pagedCandidatos);
        }
    }
}