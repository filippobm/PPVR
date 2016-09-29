using PPVR.WebApp.DataAccess;
using PPVR.WebApp.ViewModels.Ideologia;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using X.PagedList;

namespace PPVR.WebApp.Controllers
{
    public class IdeologiasController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: Ideologias
        public ActionResult Index(string q, string sort, int? page)
        {
            ViewBag.CurrentFilter = q;

            var ideologias = _db.Ideologias.Select(i => i);

            if (!string.IsNullOrEmpty(q))
                ideologias = ideologias.Where(i => i.Nome.Contains(q));

            #region Order By

            ViewBag.CurrentSort = sort;
            ViewBag.SortNome = string.IsNullOrEmpty(sort) ? "nome_desc" : "";

            switch (sort)
            {
                case "nome_desc":
                    ideologias = ideologias.OrderByDescending(i => i.Nome);
                    break;
                default:
                    ideologias = ideologias.OrderBy(i => i.Nome);
                    break;
            }

            #endregion

            var pagedIdeologias = ideologias.ToPagedList(page ?? 1, 10);
            return View(pagedIdeologias);
        }

        // GET: Ideologias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var ideologia = _db.Ideologias.Include(i => i.Partidos)
                .Where(i => i.IdeologiaId == id)
                .Select(i => new IdeologiaDetailsViewModel
                {
                    Ideologia = new IdeologiaViewModel
                    {
                        IdeologiaId = i.IdeologiaId,
                        Nome = i.Nome,
                        Enabled = i.Enabled
                    },
                    QtdePartidosAssociados = i.Partidos.Count,
                    Partidos = i.Partidos
                        .Where(p => p.Enabled)
                        .Select(p => new IdeologiaPartidoViewModel
                        {
                            PartidoId = p.PartidoId,
                            Nome = p.Nome,
                            Sigla = p.Sigla
                        })
                        .OrderBy(p => p.Nome).ToList()
                })
                .SingleOrDefault();

            if (ideologia == null)
                return HttpNotFound();

            return View(ideologia);
        }
    }
}