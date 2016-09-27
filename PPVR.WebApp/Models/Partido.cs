using PPVR.WebApp.Utils;
using Resources;
using System;
using System.Collections.Generic;

namespace PPVR.WebApp.Models
{
    public class Partido
    {
        #region Private Fields

        private string _nome;
        private byte _numeroEleitoral;
        private string _sigla;
        private EspectroPolitico _espectroPolitico;

        #endregion

        #region Properties

        public byte PartidoId { get; set; }

        public string Nome
        {
            get { return _nome; }
            set
            {
                AssertionConcern.AssertArgumentNotNull(value,
                    ValidationErrorMessage.PartidoNomeNotNull);

                AssertionConcern.AssertArgumentLength(value, 1, 60,
                    ValidationErrorMessage.PartidoNomeInvalidLength);

                _nome = value;
            }
        }

        public string Sigla
        {
            get { return _sigla; }
            set
            {
                AssertionConcern.AssertArgumentNotNull(value,
                    ValidationErrorMessage.PartidoSiglaNotNull);

                AssertionConcern.AssertArgumentLength(value, 1, 10,
                    ValidationErrorMessage.PartidoSiglaInvalidLength);

                _sigla = value;
            }
        }

        public byte NumeroEleitoral
        {
            get { return _numeroEleitoral; }
            set
            {
                AssertionConcern.AssertArgumentRange(value, 1, 99, 
                    ValidationErrorMessage.PartidoNumeroEleitoralInvalidRange);

                _numeroEleitoral = value;
            }
        }

        public EspectroPolitico EspectroPolitico
        {
            get { return _espectroPolitico; }
            set
            {
                AssertionConcern.AssertStateTrue(Enum.IsDefined(typeof(EspectroPolitico), value),
                    ValidationErrorMessage.PartidoEspectroPoliticoInvalidValue);

                _espectroPolitico = value;
            }
        }

        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Candidato> Candidatos { get; set; }
        public virtual ICollection<Ideologia> Ideologias { get; set; }

        #endregion
    }
}