using PPVR.WebApp.DataAccess;
using PPVR.WebApp.ViewModels.Ideologia;
using PPVR.WebApp.ViewModels.Partido;
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

            var partido = _db.Partidos.Find(id);

            if (partido == null)
                return HttpNotFound();

            var partidoViewModel = new PartidoViewModel
            {
                PartidoId = partido.PartidoId,
                Nome = partido.Nome,
                Sigla = partido.Sigla,
                NumeroEleitoral = partido.NumeroEleitoral,
                EspectroPolitico = (EspectroPoliticoViewModel)partido.EspectroPolitico,
                Enabled = partido.Enabled
            };

            return View(partidoViewModel);
        }

        //// POST: Ideologias/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "IdeologiaId, Nome, Enabled")] IdeologiaViewModel ideologiaViewModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var ideologiaExists =
        //                _db.Ideologias.Any(
        //                    i => i.IdeologiaId != ideologiaViewModel.IdeologiaId && i.Nome == ideologiaViewModel.Nome);

        //            if (ideologiaExists)
        //            {
        //                ModelState.AddModelError(nameof(ValidationErrorMessage.IdeologiaNomeJaCadastrado),
        //                    ValidationErrorMessage.IdeologiaNomeJaCadastrado);
        //            }
        //            else
        //            {
        //                var ideologia = _db.Ideologias.Find(ideologiaViewModel.IdeologiaId);

        //                if (ideologia != null)
        //                {
        //                    ideologia.Nome = ideologiaViewModel.Nome;
        //                    ideologia.Enabled = ideologiaViewModel.Enabled;

        //                    _db.Entry(ideologia).State = EntityState.Modified;
        //                    _db.SaveChanges();

        //                    return RedirectToAction("Index", new { q = ideologia.Nome, callbackAction = "Edit" });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //    }

        //    return View(ideologiaViewModel);
        //}

        #endregion
    }
}