using PPVR.WebApp.DataAccess;
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
    }
}