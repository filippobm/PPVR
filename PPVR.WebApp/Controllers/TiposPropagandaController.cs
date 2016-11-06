using PPVR.WebApp.DataAccess;
using PPVR.WebApp.Models;
using PPVR.WebApp.Resources;
using PPVR.WebApp.ViewModels.TipoPropaganda;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;

namespace PPVR.WebApp.Controllers
{
    public class TiposPropagandaController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: TiposPropaganda
        public ActionResult Index(string q, string sort, int? page, string callbackAction)
        {
            ViewBag.CallbackAction = callbackAction;
            ViewBag.CurrentFilter = q;

            var tiposPropaganda = _db.TiposPropaganda.Select(tp => tp);

            if (!string.IsNullOrEmpty(q))
                if (callbackAction == "Create" || callbackAction == "Edit")
                    tiposPropaganda = tiposPropaganda.Where(tp => tp.Descricao == q);
                else
                    tiposPropaganda = tiposPropaganda.Where(tp => tp.Descricao.Contains(q));

            #region Order By

            ViewBag.CurrentSort = sort;
            ViewBag.SortDescricao = "descricao";
            ViewBag.SortValorMedio = "valor_medio";

            switch (sort)
            {
                case "descricao":
                    tiposPropaganda = tiposPropaganda.OrderBy(tp => tp.Descricao);
                    ViewBag.SortDescricao = "descricao_desc";
                    break;
                case "descricao_desc":
                    tiposPropaganda = tiposPropaganda.OrderByDescending(tp => tp.Descricao);
                    ViewBag.SortDescricao = "descricao";
                    break;

                case "valor_medio":
                    tiposPropaganda = tiposPropaganda.OrderBy(tp => tp.ValorMedio);
                    ViewBag.SortValorMedio = "valor_medio_desc";
                    break;
                case "valor_medio_desc":
                    tiposPropaganda = tiposPropaganda.OrderByDescending(tp => tp.ValorMedio);
                    ViewBag.SortValorMedio = "valor_medio";
                    break;

                default:
                    tiposPropaganda = tiposPropaganda.OrderBy(tp => tp.Descricao);
                    break;
            }

            #endregion

            var pagedTiposPropaganda = tiposPropaganda.ToPagedList(page ?? 1, 10);

            var tipoPropagandaViewModel = pagedTiposPropaganda.Select(tp => new TipoPropagandaViewModel
            {
                TipoPropagandaId = tp.TipoPropagandaId,
                Descricao = tp.Descricao,
                ValorMedio = tp.ValorMedio
            });

            var pagedViewModel = new StaticPagedList<TipoPropagandaViewModel>(tipoPropagandaViewModel,
                pagedTiposPropaganda.GetMetaData());

            return View(pagedViewModel);
        }

        #region Create

        // GET: TiposPropaganda/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposPropaganda/Create
        [HttpPost]
        public ActionResult Create(
            [Bind(Include = "Descricao, ValorMedio")] TipoPropagandaViewModel tipoPropagandaViewModel)
        {
            if (ModelState.IsValid)
            {
                var tipoPropagandaExists =
                    _db.TiposPropaganda.Any(tp => tp.Descricao == tipoPropagandaViewModel.Descricao);

                if (tipoPropagandaExists)
                {
                    ModelState.AddModelError(nameof(ValidationErrorMessage.TipoPropagandaJaCadastrada),
                        ValidationErrorMessage.IdeologiaNomeJaCadastrado);
                }
                else
                {
                    _db.TiposPropaganda.Add(new TipoPropaganda
                    {
                        Descricao = tipoPropagandaViewModel.Descricao,
                        ValorMedio = tipoPropagandaViewModel.ValorMedio
                    });

                    _db.SaveChanges();

                    return RedirectToAction("Index",
                        new { q = tipoPropagandaViewModel.Descricao, callbackAction = "Create" });
                }
            }
            return View(tipoPropagandaViewModel);
        }

        #endregion
    }
}