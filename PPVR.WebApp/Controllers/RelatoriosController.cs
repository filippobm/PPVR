using PPVR.WebApp.DataAccess;
using PPVR.WebApp.ViewModels.Relatorios;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PPVR.WebApp.Controllers
{
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
    }
}