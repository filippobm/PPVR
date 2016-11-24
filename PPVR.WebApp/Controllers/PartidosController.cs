using PPVR.WebApp.DataAccess;
using PPVR.WebApp.Models;
using PPVR.WebApp.Resources;
using PPVR.WebApp.ViewModels.Ideologia;
using PPVR.WebApp.ViewModels.Partido;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
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

        // GET: Partidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var partido = _db.Partidos.Select(p => p)
                .Include(p => p.Candidatos)
                .Include(p => p.Ideologias)
                .Where(p => p.PartidoId == id)
                .Select(p => new PartidoViewModel
                {
                    PartidoId = p.PartidoId,
                    Nome = p.Nome,
                    Sigla = p.Sigla,
                    NumeroEleitoral = p.NumeroEleitoral,
                    EspectroPolitico = (EspectroPoliticoViewModel)p.EspectroPolitico,
                    QtdeCandidatosAssociados = p.Candidatos.Count,
                    Ideologias = p.Ideologias.Where(i => i.Enabled)
                        .Select(i => new IdeologiaViewModel
                        {
                            IdeologiaId = i.IdeologiaId,
                            Nome = i.Nome
                        })
                        .ToList(),
                    Enabled = p.Enabled,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).SingleOrDefault();

            if (partido == null)
                return HttpNotFound();

            return View(partido);
        }

        #region Edit

        // GET: Partidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var ideologias = _db.Ideologias.Where(i => i.Enabled).ToList();

            var partido = _db.Partidos.Select(p => p)
                .Where(p => p.PartidoId == id)
                .Include(p => p.Ideologias)
                .FirstOrDefault();

            if (partido == null)
                return HttpNotFound();

            var partidoViewModel = new PartidoViewModel
            {
                PartidoId = partido.PartidoId,
                Nome = partido.Nome,
                Sigla = partido.Sigla.TrimEnd(),
                NumeroEleitoral = partido.NumeroEleitoral,
                EspectroPolitico = (EspectroPoliticoViewModel)partido.EspectroPolitico,
                Enabled = partido.Enabled
            };

            foreach (var ideologia in ideologias)
            {
                var partidoIdeologiaViewModel = new PartidoIdeologiaViewModel
                {
                    IdeologiaId = ideologia.IdeologiaId,
                    NomeIdeologia = ideologia.Nome,
                    Checked = partido.Ideologias.Any(i => i.IdeologiaId == ideologia.IdeologiaId)
                };
                partidoViewModel.PartidoIdeologias.Add(partidoIdeologiaViewModel);
            }

            partidoViewModel.PartidoIdeologias = partidoViewModel.PartidoIdeologias.OrderBy(i => i.NomeIdeologia).ToList();

            return View(partidoViewModel);
        }

        // POST: Ideologias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PartidoId, Nome, Sigla, Enabled, NumeroEleitoral, EspectroPolitico, PartidoIdeologias")] PartidoViewModel partidoViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isValid = true;

                    // Verifica se já existe outro partido com o nome informado.
                    if (_db.Partidos.Any(p => p.PartidoId != partidoViewModel.PartidoId && p.Nome == partidoViewModel.Nome))
                    {
                        ModelState.AddModelError(nameof(ValidationErrorMessage.PartidoNomeJaCadastrado),
                            ValidationErrorMessage.PartidoNomeJaCadastrado);
                        isValid = false;
                    }

                    // Verifica se já existe outro partido com a sigla informada.
                    if (_db.Partidos.Any(p => p.PartidoId != partidoViewModel.PartidoId && p.Sigla == partidoViewModel.Sigla))
                    {
                        ModelState.AddModelError(nameof(ValidationErrorMessage.PartidoSiglaJaCadastrado),
                            ValidationErrorMessage.PartidoSiglaJaCadastrado);
                        isValid = false;
                    }

                    // Verifica se já existe outro partido com o número eleitoral informado.
                    if (_db.Partidos.Any(p => p.PartidoId != partidoViewModel.PartidoId && p.NumeroEleitoral == partidoViewModel.NumeroEleitoral))
                    {
                        ModelState.AddModelError(nameof(ValidationErrorMessage.PartidoNumeroEleitoralJaCadastrado),
                            ValidationErrorMessage.PartidoNumeroEleitoralJaCadastrado);
                        isValid = false;
                    }

                    if (isValid)
                    {
                        var partido = _db.Partidos.Select(p => p)
                            .Include(p => p.Ideologias)
                            .FirstOrDefault(p => p.PartidoId == partidoViewModel.PartidoId);

                        if (partido != null)
                        {
                            partido.Nome = partidoViewModel.Nome;
                            partido.Sigla = partidoViewModel.Sigla;
                            partido.EspectroPolitico = (EspectroPolitico)partidoViewModel.EspectroPolitico;
                            partido.NumeroEleitoral = partidoViewModel.NumeroEleitoral;
                            partido.Enabled = partidoViewModel.Enabled;

                            partido.Ideologias.Clear();

                            foreach (var partidoIdeologia in partidoViewModel.PartidoIdeologias.Where(i => i.Checked))
                            {
                                var ideologia = _db.Ideologias.Find(partidoIdeologia.IdeologiaId);
                                partido.Ideologias.Add(ideologia);
                            }

                            _db.Entry(partido).State = EntityState.Modified;
                            _db.SaveChanges();

                            return RedirectToAction("Index", new { q = partido.Nome, callbackAction = "Edit" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(partidoViewModel);
        }

        #endregion
    }
}