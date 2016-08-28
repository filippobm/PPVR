using PPVR.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace PPVR.WebApp.DAL
{
    public class ContextInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            #region Ideologias        

            new List<Ideologia>
            {
                new Ideologia {Nome = "Ambientalismo", Enabled = true},
                new Ideologia {Nome = "Brizolismo", Enabled = true},
                new Ideologia {Nome = "Capitalismo", Enabled = true},
                new Ideologia {Nome = "Centrismo", Enabled = true},
                new Ideologia {Nome = "Comunismo", Enabled = true},
                new Ideologia {Nome = "Conservadorismo", Enabled = true},
                new Ideologia {Nome = "Conservadorismo liberal", Enabled = true},
                new Ideologia {Nome = "Conservadorismo social", Enabled = true},
                new Ideologia {Nome = "Democracia cristã", Enabled = true},
                new Ideologia {Nome = "Democracia liberal", Enabled = true},
                new Ideologia {Nome = "Desenvolvimentismo", Enabled = true},
                new Ideologia {Nome = "Direita cristã", Enabled = true},
                new Ideologia {Nome = "Direitos da mulher", Enabled = true},
                new Ideologia {Nome = "Distributismo", Enabled = true},
                new Ideologia {Nome = "Ecossocialismo", Enabled = true},
                new Ideologia {Nome = "Federalismo", Enabled = true},
                new Ideologia {Nome = "Getulismo", Enabled = true},
                new Ideologia {Nome = "Humanismo", Enabled = true},
                new Ideologia {Nome = "Intervencionismo", Enabled = true},
                new Ideologia {Nome = "Khrushchevismo", Enabled = true},
                new Ideologia {Nome = "Libertarianismo Bleeding-Heart", Enabled = true},
                new Ideologia {Nome = "Libertarismo", Enabled = true},
                new Ideologia {Nome = "Liberalismo económico", Enabled = true},
                new Ideologia {Nome = "Liberalismo social", Enabled = true},
                new Ideologia {Nome = "Lulismo", Enabled = true},
                new Ideologia {Nome = "Maoismo", Enabled = true},
                new Ideologia {Nome = "Marxismo", Enabled = true},
                new Ideologia {Nome = "Marxismo-leninismo", Enabled = true},
                new Ideologia {Nome = "Mobilização", Enabled = true},
                new Ideologia {Nome = "Nacionalismo", Enabled = true},
                new Ideologia {Nome = "Participalismo", Enabled = true},
                new Ideologia {Nome = "Parlamentarismo", Enabled = true},
                new Ideologia {Nome = "Populismo", Enabled = true},
                new Ideologia {Nome = "Progressismo", Enabled = true},
                new Ideologia {Nome = "Protecionismo", Enabled = true},
                new Ideologia {Nome = "Reformismo", Enabled = true},
                new Ideologia {Nome = "Republicanismo", Enabled = true},
                new Ideologia {Nome = "Sincretismo político", Enabled = true},
                new Ideologia {Nome = "Socialismo", Enabled = true},
                new Ideologia {Nome = "Socialismo científico", Enabled = true},
                new Ideologia {Nome = "Socialismo democrático", Enabled = true},
                new Ideologia {Nome = "Social-democracia", Enabled = true},
                new Ideologia {Nome = "Sustentabilidade", Enabled = true},
                new Ideologia {Nome = "Terceira via", Enabled = true},
                new Ideologia {Nome = "Trabalhismo", Enabled = true},
                new Ideologia {Nome = "Trotskismo", Enabled = true}
            }.ForEach(x => context.Ideologias.AddOrUpdate(x));

            #endregion

            #region Partidos

            var partidos = new List<Partido>
            {
                new Partido
                {
                    Nome = "Democratas",
                    NumeroEleitoral = 25,
                    Sigla = "DEM",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroDireita
                },
                new Partido
                {
                    Nome = "Partido Comunista Brasileiro",
                    NumeroEleitoral = 21,
                    Sigla = "PCB",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.ExtremaEsquerda
                },
                new Partido
                {
                    Nome = "Partido Comunista do Brasil",
                    NumeroEleitoral = 65,
                    Sigla = "PCdoB",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.EsquerdaAExtremaEsquerda
                },
                new Partido
                {
                    Nome = "Partido da Causa Operária",
                    NumeroEleitoral = 29,
                    Sigla = "PCO",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.ExtremaEsquerda
                },
                new Partido
                {
                    Nome = "Partido da Mobilização Nacional",
                    NumeroEleitoral = 33,
                    Sigla = "PMN",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerda
                },
                new Partido
                {
                    Nome = "Partido da Mulher Brasileira",
                    NumeroEleitoral = 35,
                    Sigla = "PMB",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerda
                },
                new Partido
                {
                    Nome = "Partido da República",
                    NumeroEleitoral = 22,
                    Sigla = "PR",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroDireita
                },
                new Partido
                {
                    Nome = "Partido da Social Democracia Brasileira",
                    NumeroEleitoral = 45,
                    Sigla = "PSDB",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerda
                },
                new Partido
                {
                    Nome = "Partido Democrático Trabalhista",
                    NumeroEleitoral = 12,
                    Sigla = "PDT",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerda
                },
                new Partido
                {
                    Nome = "Partido do Movimento Democrático Brasileiro",
                    NumeroEleitoral = 15,
                    Sigla = "PMDB",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.Centro
                },
                new Partido
                {
                    Nome = "Partido dos Trabalhadores",
                    NumeroEleitoral = 13,
                    Sigla = "PT",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.Esquerda
                },
                new Partido
                {
                    Nome = "Partido Ecológico Nacional",
                    NumeroEleitoral = 51,
                    Sigla = "PEN",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroDireita
                },
                new Partido
                {
                    Nome = "Partido Humanista da Solidariedade",
                    NumeroEleitoral = 31,
                    Sigla = "PHS",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroDireitaADireita
                },
                new Partido
                {
                    Nome = "Partido Novo",
                    NumeroEleitoral = 30,
                    Sigla = "NOVO",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.Centro
                },
                new Partido
                {
                    Nome = "Partido Pátria Livre",
                    NumeroEleitoral = 54,
                    Sigla = "PPL",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerdaAEsquerda
                },
                new Partido
                {
                    Nome = "Partido Popular Socialista",
                    NumeroEleitoral = 23,
                    Sigla = "PPS",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerda
                },
                new Partido
                {
                    Nome = "Partido Progressista",
                    NumeroEleitoral = 11,
                    Sigla = "PP",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroDireitaADireita
                },
                new Partido
                {
                    Nome = "Partido Renovador Trabalhista Brasileiro",
                    NumeroEleitoral = 28,
                    Sigla = "PRTB",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.Direita
                },
                new Partido
                {
                    Nome = "Partido Republicano Brasileiro",
                    NumeroEleitoral = 10,
                    Sigla = "PRB",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroDireita
                },
                new Partido
                {
                    Nome = "Partido Republicano da Ordem Social",
                    NumeroEleitoral = 90,
                    Sigla = "PROS",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerda
                },
                new Partido
                {
                    Nome = "Partido Republicano Progressista",
                    NumeroEleitoral = 44,
                    Sigla = "PRP",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroDireita
                },
                new Partido
                {
                    Nome = "Partido Social Cristão",
                    NumeroEleitoral = 20,
                    Sigla = "PSC",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.Direita
                },
                new Partido
                {
                    Nome = "Partido Social Democrata Cristão",
                    NumeroEleitoral = 27,
                    Sigla = "PSDC",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.DireitaAExtremaDireita
                },
                new Partido
                {
                    Nome = "Partido Social Democrático",
                    NumeroEleitoral = 55,
                    Sigla = "PSD",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.Centro
                },
                new Partido
                {
                    Nome = "Partido Social Liberal",
                    NumeroEleitoral = 17,
                    Sigla = "PSL",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroDireita
                },
                new Partido
                {
                    Nome = "Partido Socialismo e Liberdade",
                    NumeroEleitoral = 50,
                    Sigla = "PSOL",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.EsquerdaAExtremaEsquerda
                },
                new Partido
                {
                    Nome = "Partido Socialista Brasileiro",
                    NumeroEleitoral = 40,
                    Sigla = "PSB",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerdaAEsquerda
                },
                new Partido
                {
                    Nome = "Partido Socialista dos Trabalhadores Unificado",
                    NumeroEleitoral = 16,
                    Sigla = "PSTU",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.ExtremaEsquerda
                },
                new Partido
                {
                    Nome = "Partido Trabalhista Brasileiro",
                    NumeroEleitoral = 14,
                    Sigla = "PTB",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.Centro
                },
                new Partido
                {
                    Nome = "Partido Trabalhista Cristão",
                    NumeroEleitoral = 36,
                    Sigla = "PTC",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroDireitaADireita
                },
                new Partido
                {
                    Nome = "Partido Trabalhista do Brasil",
                    NumeroEleitoral = 70,
                    Sigla = "PTdoB",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerda
                },
                new Partido
                {
                    Nome = "Partido Trabalhista Nacional",
                    NumeroEleitoral = 19,
                    Sigla = "PTN",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerda
                },
                new Partido
                {
                    Nome = "Partido Verde",
                    NumeroEleitoral = 43,
                    Sigla = "PV",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerda
                },
                new Partido
                {
                    Nome = "Rede Sustentabilidade",
                    NumeroEleitoral = 18,
                    Sigla = "REDE",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerda
                },
                new Partido
                {
                    Nome = "Solidariedade",
                    NumeroEleitoral = 77,
                    Sigla = "SD",
                    Enabled = true,
                    EspectroPolitico = EspectroPolitico.CentroEsquerda
                }
            };

            partidos.ForEach(x => x.CreatedAt = DateTime.Now);
            partidos.ForEach(x => context.Partidos.AddOrUpdate(x));

            #endregion

            context.SaveChanges();
        }
    }
}