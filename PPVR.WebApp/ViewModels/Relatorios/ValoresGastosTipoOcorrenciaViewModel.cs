using System.Collections.Generic;

namespace PPVR.WebApp.ViewModels.Relatorios
{
    public class ValoresGastosTipoOcorrenciaViewModel
    {
        public string name { get; set; }
        public List<decimal> data { get; set; } = new List<decimal>();
    }
}