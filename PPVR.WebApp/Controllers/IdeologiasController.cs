﻿using PPVR.WebApp.DataAccess;
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
    public class IdeologiasController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: Ideologias
        public ActionResult Index(string q, string sort, int? page, string callbackAction)
        {
            ViewBag.CallbackAction = callbackAction;
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
                        var ideologia = new Ideologia
                        {
                            IdeologiaId = ideologiaViewModel.IdeologiaId,
                            Nome = ideologiaViewModel.Nome,
                            Enabled = ideologiaViewModel.Enabled
                        };

                        _db.Entry(ideologia).State = EntityState.Modified;
                        _db.SaveChanges();

                        return RedirectToAction("Index", new { q = ideologia.Nome, callbackAction = "Edit" });
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
    }
}