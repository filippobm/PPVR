using PPVR.WebApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace PPVR.WebApp.ViewModels.Partido
{
    public enum EspectroPoliticoViewModel : byte
    {
        NaoInformado = 0,

        [Display(Name = nameof(Labels.EspectroPoliticoCentro), ResourceType = typeof(Labels))]
        Centro = 1,

        [Display(Name = nameof(Labels.EspectroPoliticoCentroDireita), ResourceType = typeof(Labels))]
        CentroDireita = 2,

        [Display(Name = nameof(Labels.EspectroPoliticoCentroDireitaADireita), ResourceType = typeof(Labels))]
        CentroDireitaADireita = 3,

        [Display(Name = nameof(Labels.EspectroPoliticoDireita), ResourceType = typeof(Labels))]
        Direita = 4,

        [Display(Name = nameof(Labels.EspectroPoliticoDireitaAExtremaDireita), ResourceType = typeof(Labels))]
        DireitaAExtremaDireita = 5,

        [Display(Name = nameof(Labels.EspectroPoliticoExtremaDireita), ResourceType = typeof(Labels))]
        ExtremaDireita = 6,

        [Display(Name = nameof(Labels.EspectroPoliticoCentroEsquerda), ResourceType = typeof(Labels))]
        CentroEsquerda = 7,

        [Display(Name = nameof(Labels.EspectroPoliticoCentroEsquerdaAEsquerda), ResourceType = typeof(Labels))]
        CentroEsquerdaAEsquerda = 8,

        [Display(Name = nameof(Labels.EspectroPoliticoEsquerda), ResourceType = typeof(Labels))]
        Esquerda = 9,

        [Display(Name = nameof(Labels.EspectroPoliticoEsquerdaAExtremaEsquerda), ResourceType = typeof(Labels))]
        EsquerdaAExtremaEsquerda = 10,

        [Display(Name = nameof(Labels.EspectroPoliticoExtremaEsquerda), ResourceType = typeof(Labels))]
        ExtremaEsquerda = 11
    }
}