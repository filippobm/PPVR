using PPVR.WebApp.DataAccess;
using PPVR.WebApp.Models;
using PPVR.WebApp.Resources;
using PPVR.WebApp.ViewModels.Ideologia;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using X.PagedList;

namespace PPVR.WebApp.Controllers
{
    [Authorize]
    public class IdeologiasController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: Ideologias
        public ActionResult Index(string q, string sort, int? page, string callbackAction)
        {
            ViewBag.CallbackAction = callbackAction;
            ViewBag.CurrentFilter = q;

            var ideologias = _db.Ideologias.Include(i => i.Partidos);

            if (!string.IsNullOrEmpty(q))
                ideologias = ideologias.Where(i => i.Nome.Contains(q));

            #region Order By

            ViewBag.CurrentSort = sort;

            ViewBag.SortNome = "nome";
            ViewBag.SortCreatedAt = "created_at";
            ViewBag.SortUpdatedAt = "updated_at";
            ViewBag.SortQtdePartidosAssociados = "qtde_partidos_associados";

            switch (sort)
            {
                case "nome":
                    ideologias = ideologias.OrderBy(i => i.Nome);
                    ViewBag.SortNome = "nome_desc";
                    break;
                case "nome_desc":
                    ideologias = ideologias.OrderByDescending(i => i.Nome);
                    ViewBag.SortNome = "nome";
                    break;

                case "qtde_partidos_associados":
                    ideologias = ideologias.OrderBy(i => i.Partidos.Count);
                    ViewBag.SortQtdePartidosAssociados = "qtde_partidos_associados_desc";
                    break;
                case "qtde_partidos_associados_desc":
                    ideologias = ideologias.OrderByDescending(i => i.Partidos.Count);
                    ViewBag.SortQtdePartidosAssociados = "qtde_partidos_associados";
                    break;

                case "created_at":
                    ideologias = ideologias.OrderBy(i => i.CreatedAt);
                    ViewBag.SortCreatedAt = "created_at_desc";
                    break;
                case "created_at_desc":
                    ideologias = ideologias.OrderByDescending(i => i.CreatedAt);
                    ViewBag.SortCreatedAt = "created_at";
                    break;

                case "updated_at":
                    ideologias = ideologias.OrderBy(i => i.UpdatedAt);
                    ViewBag.SortUpdatedAt = "updated_at_desc";
                    break;
                case "updated_at_desc":
                    ideologias = ideologias.OrderByDescending(i => i.UpdatedAt);
                    ViewBag.SortUpdatedAt = "updated_at";
                    break;

                default:
                    ideologias = ideologias.OrderBy(i => i.Nome);
                    break;
            }

            #endregion

            var pagedIdeologias = ideologias.ToPagedList(page ?? 1, 10);

            var ideologiaGridViewModel = pagedIdeologias.Select(i => new IdeologiaGridViewModel
            {
                Ideologia = new IdeologiaViewModel
                {
                    IdeologiaId = i.IdeologiaId,
                    Nome = i.Nome,
                    Enabled = i.Enabled
                },
                QtdePartidosAssociados = i.Partidos.Count,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            });

            var pagedViewModel = new StaticPagedList<IdeologiaGridViewModel>(ideologiaGridViewModel,
                pagedIdeologias.GetMetaData());

            return View(pagedViewModel);
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

        #region Create

        // GET: Ideologias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ideologias/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Nome")] IdeologiaViewModel ideologiaViewModel)
        {
            if (ModelState.IsValid)
            {
                var ideologiaExists = _db.Ideologias.Any(i => i.Nome == ideologiaViewModel.Nome);

                if (ideologiaExists)
                {
                    ModelState.AddModelError(nameof(ValidationErrorMessage.IdeologiaNomeJaCadastrado),
                        ValidationErrorMessage.IdeologiaNomeJaCadastrado);
                }
                else
                {
                    _db.Ideologias.Add(new Ideologia { Nome = ideologiaViewModel.Nome });
                    _db.SaveChanges();

                    return RedirectToAction("Index", new { q = ideologiaViewModel.Nome, callbackAction = "Create" });
                }
            }
            return View(ideologiaViewModel);
        }

        #endregion

        #region Edit

        // GET: Ideologias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var ideologia = _db.Ideologias.Find(id);

            if (ideologia == null)
                return HttpNotFound();

            var ideologiaViewModel = new IdeologiaViewModel
            {
                IdeologiaId = ideologia.IdeologiaId,
                Nome = ideologia.Nome,
                Enabled = ideologia.Enabled
            };

            return View(ideologiaViewModel);
        }

        // POST: Ideologias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdeologiaId, Nome, Enabled")] IdeologiaViewModel ideologiaViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ideologiaExists =
                        _db.Ideologias.Any(
                            i => i.IdeologiaId != ideologiaViewModel.IdeologiaId && i.Nome == ideologiaViewModel.Nome);

                    if (ideologiaExists)
                    {
                        ModelState.AddModelError(nameof(ValidationErrorMessage.IdeologiaNomeJaCadastrado),
                            ValidationErrorMessage.IdeologiaNomeJaCadastrado);
                    }
                    else
                    {
                        var ideologia = _db.Ideologias.Find(ideologiaViewModel.IdeologiaId);

                        if (ideologia != null)
                        {
                            ideologia.Nome = ideologiaViewModel.Nome;
                            ideologia.Enabled = ideologiaViewModel.Enabled;

                            _db.Entry(ideologia).State = EntityState.Modified;
                            _db.SaveChanges();

                            return RedirectToAction("Index", new { q = ideologia.Nome, callbackAction = "Edit" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(ideologiaViewModel);
        }

        #endregion
    }
}