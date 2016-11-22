using PPVR.WebApp.Resources;
using PPVR.WebApp.ViewModels.TipoPropaganda;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PPVR.WebApp.ViewModels.Home
{
    public class UploadFotoViewModel
    {
        public IEnumerable<TipoPropagandaViewModel> TiposPropaganda { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.OcorrenciaTipoPropagandaNotNull))]
        public int TipoPropaganda { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationErrorMessage),
            ErrorMessageResourceName = nameof(ValidationErrorMessage.OcorrenciaFotoNotNull))]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }

        public List<string> CandidatosEncontrados { get; set; } = new List<string>();
    }
}