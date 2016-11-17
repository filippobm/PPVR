using PPVR.WebApp.DataAccess;
using PPVR.WebApp.Models;
using PPVR.WebApp.Resources;
using PPVR.WebApp.ViewModels.TipoPropaganda;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            ViewBag.SortCreatedAt = "created_at";
            ViewBag.SortUpdatedAt = "updated_at";

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

                case "created_at":
                    tiposPropaganda = tiposPropaganda.OrderBy(tp => tp.CreatedAt);
                    ViewBag.SortCreatedAt = "created_at_desc";
                    break;
                case "created_at_desc":
                    tiposPropaganda = tiposPropaganda.OrderByDescending(tp => tp.CreatedAt);
                    ViewBag.SortCreatedAt = "created_at";
                    break;

                case "updated_at":
                    tiposPropaganda = tiposPropaganda.OrderBy(tp => tp.UpdatedAt);
                    ViewBag.SortUpdatedAt = "updated_at_desc";
                    break;
                case "updated_at_desc":
                    tiposPropaganda = tiposPropaganda.OrderByDescending(tp => tp.UpdatedAt);
                    ViewBag.SortUpdatedAt = "updated_at";
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
                ValorMedio = tp.ValorMedio,
                Enabled = tp.Enabled,
                CreatedAt = tp.CreatedAt,
                UpdatedAt = tp.UpdatedAt
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
                        ValidationErrorMessage.TipoPropagandaJaCadastrada);
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

        #region Edit

        // GET: TiposPropaganda/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tipoPropaganda = _db.TiposPropaganda.Find(id);

            if (tipoPropaganda == null)
                return HttpNotFound();

            var tipoPropagandaViewModel = new TipoPropagandaViewModel
            {
                TipoPropagandaId = tipoPropaganda.TipoPropagandaId,
                Descricao = tipoPropaganda.Descricao,
                ValorMedio = tipoPropaganda.ValorMedio,
                Enabled = tipoPropaganda.Enabled
            };

            return View(tipoPropagandaViewModel);
        }

        // POST: TiposPropaganda/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "TipoPropagandaId, Descricao, ValorMedio, Enabled")] TipoPropagandaViewModel
                tipoPropagandaViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tipoPropagandaExists =
                        _db.TiposPropaganda.Any(
                            tp =>
                                tp.TipoPropagandaId != tipoPropagandaViewModel.TipoPropagandaId &&
                                tp.Descricao == tipoPropagandaViewModel.Descricao);

                    if (tipoPropagandaExists)
                    {
                        ModelState.AddModelError(nameof(ValidationErrorMessage.TipoPropagandaJaCadastrada),
                            ValidationErrorMessage.TipoPropagandaJaCadastrada);
                    }
                    else
                    {
                        var tipoPropaganda = _db.TiposPropaganda.Find(tipoPropagandaViewModel.TipoPropagandaId);

                        if (tipoPropaganda != null)
                        {
                            tipoPropaganda.Descricao = tipoPropagandaViewModel.Descricao;
                            tipoPropaganda.ValorMedio = tipoPropagandaViewModel.ValorMedio;
                            tipoPropaganda.Enabled = tipoPropagandaViewModel.Enabled;

                            _db.Entry(tipoPropaganda).State = EntityState.Modified;
                            _db.SaveChanges();

                            return RedirectToAction("Index", new { q = tipoPropaganda.Descricao, callbackAction = "Edit" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(tipoPropagandaViewModel);
        }

        #endregion
    }
}