using PPVR.WebApp.Resources;
using PPVR.WebApp.Utils;
using System.Collections.Generic;

namespace PPVR.WebApp.Models
{
    public class Ideologia
    {
        #region Private Fields

        private string _nome;

        #endregion        

        #region Properties

        public short IdeologiaId { get; set; }

        public string Nome
        {
            get { return _nome; }
            set
            {
                AssertionConcern.AssertArgumentNotNull(value,
                    ValidationErrorMessage.IdeologiaNomeNotNull);

                AssertionConcern.AssertArgumentLength(value, 1, 30,
                    ValidationErrorMessage.IdeologiaNomeInvalidLength);

                _nome = value;
            }
        }

        public bool Enabled { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Partido> Partidos { get; set; }

        #endregion
    }
}