using PPVR.WebApp.Resources;
using PPVR.WebApp.Utils;
using System;
using System.Collections.Generic;

namespace PPVR.WebApp.Models
{
    public class TipoPropaganda
    {
        #region Private Fields

        private string _descricao;
        private decimal _valorMedio;

        #endregion

        #region Properties

        public int TipoPropagandaId { get; set; }

        public string Descricao
        {
            get { return _descricao; }
            set
            {
                AssertionConcern.AssertArgumentNotNull(value, ValidationErrorMessage.TipoPropagandaDescricaoNotNull);

                AssertionConcern.AssertArgumentLength(value, 1, 30,
                    ValidationErrorMessage.TipoPropagandaDescricaoInvalidLength);

                _descricao = value;
            }
        }

        public decimal ValorMedio
        {
            get { return _valorMedio; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(ValidationErrorMessage.TipoPropagandaValorMedioInvalidValue);

                _valorMedio = value;
            }
        }

        public bool Enabled { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Ocorrencia> Ocorrencias { get; set; }

        #endregion
    }
}