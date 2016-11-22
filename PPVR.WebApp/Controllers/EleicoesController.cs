using PPVR.WebApp.DataAccess;
using PPVR.WebApp.ViewModels.Eleicao;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using X.PagedList;

namespace PPVR.WebApp.Controllers
{
    [Authorize]
    public class EleicoesController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: Eleicoes
        public ActionResult Index(string q, string sort, int? page, string callbackAction)
        {
            ViewBag.CallbackAction = callbackAction;
            ViewBag.CurrentFilter = q;

            var eleicoes = _db.Eleicoes.Select(e => e);

            if (!string.IsNullOrEmpty(q))
                if (callbackAction == "Create" || callbackAction == "Edit")
                    eleicoes = eleicoes.Where(e => e.Descricao == q);
                else
                    eleicoes = eleicoes.Where(e => e.Descricao.Contains(q));

            #region Order By

            ViewBag.CurrentSort = sort;

            ViewBag.SortDescricao = "descricao";
            ViewBag.SortAno = "ano";
            ViewBag.SortTurno = "turno";
            ViewBag.SortCreatedAt = "created_at";
            ViewBag.SortUpdatedAt = "updated_at";

            switch (sort)
            {
                case "descricao":
                    eleicoes = eleicoes.OrderBy(e => e.Descricao);
                    ViewBag.SortDescricao = "descricao_desc";
                    break;
                case "descricao_desc":
                    eleicoes = eleicoes.OrderByDescending(e => e.Descricao);
                    ViewBag.SortDescricao = "descricao";
                    break;

                case "ano":
                    eleicoes = eleicoes.OrderBy(e => e.Ano);
                    ViewBag.SortAno = "ano_desc";
                    break;
                case "ano_desc":
                    eleicoes = eleicoes.OrderByDescending(e => e.Ano);
                    ViewBag.SortAno = "ano";
                    break;

                case "turno":
                    eleicoes = eleicoes.OrderBy(e => e.Turno);
                    ViewBag.SortTurno = "turno_desc";
                    break;
                case "turno_desc":
                    eleicoes = eleicoes.OrderByDescending(e => e.Turno);
                    ViewBag.SortTurno = "turno";
                    break;

                case "created_at":
                    eleicoes = eleicoes.OrderBy(e => e.CreatedAt);
                    ViewBag.SortCreatedAt = "created_at_desc";
                    break;
                case "created_at_desc":
                    eleicoes = eleicoes.OrderByDescending(e => e.CreatedAt);
                    ViewBag.SortCreatedAt = "created_at";
                    break;

                case "updated_at":
                    eleicoes = eleicoes.OrderBy(e => e.UpdatedAt);
                    ViewBag.SortUpdatedAt = "updated_at_desc";
                    break;
                case "updated_at_desc":
                    eleicoes = eleicoes.OrderByDescending(e => e.UpdatedAt);
                    ViewBag.SortUpdatedAt = "updated_at";
                    break;

                default:
                    eleicoes = eleicoes.OrderBy(e => e.Descricao);
                    break;
            }

            #endregion

            var pagedEleicoes = eleicoes.ToPagedList(page ?? 1, 10);

            var eleicaoViewModel = eleicoes.Select(e => new EleicaoViewModel
            {
                EleicaoId = e.EleicaoId,
                Descricao = e.Descricao,
                Ano = e.Ano,
                Turno = e.Turno,
                Enabled = e.Enabled,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            });

            var pagedViewModel = new StaticPagedList<EleicaoViewModel>(eleicaoViewModel, pagedEleicoes.GetMetaData());

            return View(pagedViewModel);
        }

        #region Edit

        // GET: Eleicoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var eleicao = _db.Eleicoes.Find(id);

            if (eleicao == null)
                return HttpNotFound();

            var eleicaoViewModel = new EleicaoViewModel
            {
                EleicaoId = eleicao.EleicaoId,
                Descricao = eleicao.Descricao,
                Ano = eleicao.Ano,
                Turno = eleicao.Turno,
                CreatedAt = eleicao.CreatedAt,
                UpdatedAt = eleicao.UpdatedAt,
                Enabled = eleicao.Enabled
            };

            return View(eleicaoViewModel);
        }

        // POST: Eleicoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EleicaoId, Enabled")] EleicaoViewModel eleicaoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var eleicao = _db.Eleicoes.Find(eleicaoViewModel.EleicaoId);

                    if (eleicao != null)
                    {
                        eleicao.Enabled = eleicaoViewModel.Enabled;

                        _db.Entry(eleicao).State = EntityState.Modified;
                        _db.SaveChanges();

                        return RedirectToAction("Index", new
                        {
                            q = eleicao.Descricao,
                            callbackAction = "Edit"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(eleicaoViewModel);
        }

        #endregion
    }
}