using PPVR.WebApp.DataAccess;
using PPVR.WebApp.ViewModels.Relatorios;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PPVR.WebApp.Controllers
{
    [Authorize]
    public class RelatoriosController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: Relatorios
        public ActionResult Index()
        {
            return View();
        }

        #region Relatório Ocorrências por Tipo de Propaganda

        // GET: Relatorios/OcorrenciasTipoPropaganda
        public ActionResult OcorrenciasTipoPropaganda()
        {
            return View();
        }

        // GET: Relatorios/OcorrenciasTipoPropagandaJson
        public JsonResult OcorrenciasTipoPropagandaJson()
        {
            var ocorrenciasTipoPropaganda = _db.Ocorrencias.Select(o => o)
                .Include(o => o.TipoPropaganda)
                .GroupBy(g => g.TipoPropaganda.Descricao)
                .Select(o => new OcorrenciasTipoPropagandaViewModel
                {
                    name = o.Key,
                    y = o.Count()
                }).ToList();

            return Json(ocorrenciasTipoPropaganda, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Relatórios Valores Gastos por Tipo de Ocorrência

        // GET: Relatorios/ValoresGastosTipoOcorrencia
        public ActionResult ValoresGastosTipoOcorrencia()
        {
            return View();
        }

        // GET: Relatorios/ValoresGastosTipoOcorrenciaJson
        public JsonResult ValoresGastosTipoOcorrenciaJson()
        {
            var valoresGastosTipoOcorrenciaViewModel = new List<ValoresGastosTipoOcorrenciaViewModel>
            {
                new ValoresGastosTipoOcorrenciaViewModel {name = "Teste 1", data = new[] {1}},
                new ValoresGastosTipoOcorrenciaViewModel {name = "Teste 2", data = new[] {2}},
                new ValoresGastosTipoOcorrenciaViewModel {name = "Teste 3", data = new[] {150}},
                new ValoresGastosTipoOcorrenciaViewModel {name = "Teste 4", data = new[] {7}},
                new ValoresGastosTipoOcorrenciaViewModel {name = "Teste 5", data = new[] {9}},
                new ValoresGastosTipoOcorrenciaViewModel {name = "Teste 6", data = new[] {7}},
                new ValoresGastosTipoOcorrenciaViewModel {name = "Teste 7", data = new[] {5}}
            };

            return Json(valoresGastosTipoOcorrenciaViewModel, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}