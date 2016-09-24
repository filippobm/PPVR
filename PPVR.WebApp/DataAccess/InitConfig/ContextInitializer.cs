using PPVR.WebApp.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

namespace PPVR.WebApp.DataAccess.InitConfig
{
    public class ContextInitializer : DropCreateDatabaseAlways<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            var path =
                @"C:\Projects\PPVR\src\PPVR.WebApp\bin\Migrations\dataset\consulta_cand_2016\consulta_cand_2016_MG.txt";            

            using (var streamReader = new StreamReader(path, Encoding.GetEncoding("ISO-8859-1")))
            {
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    var c = line.Split(';');

                    if (c.Length != 46)
                        throw new Exception("aaa");

                    for (var i = 0; i < c.Length; i++)
                        c[i] = c[i].Substring(1, c[i].Length - 2);

                    #region Eleição

                    var eleicaoAno = int.Parse(c[2]);
                    var eleicaoDescricao = c[4];
                    var eleicaoTurno = byte.Parse(c[3]);

                    var eleicao =
                        context.Eleicoes.FirstOrDefault(
                            x => x.Ano == eleicaoAno && x.Descricao == eleicaoDescricao && x.Turno == eleicaoTurno);

                    if (eleicao == null)
                    {
                        eleicao = new Eleicao
                        {
                            Descricao = eleicaoDescricao,
                            Ano = eleicaoAno,
                            Turno = eleicaoTurno,
                            Enabled = true
                        };
                    }

                    #endregion

                    #region Partido

                    var numeroPartido = byte.Parse(c[17]);
                    var partido = context.Partidos.FirstOrDefault(x => x.NumeroEleitoral == numeroPartido);

                    if (partido == null)
                    {
                        partido = new Partido
                        {
                            NumeroEleitoral = numeroPartido,
                            Nome = c[18]
                        };
                    }

                    #endregion

                    //[12] Número Candidato Urna
                    //[16] Descrição Situação Candidato

                    var candidato = new Candidato
                    {
                        SiglaUnidadeFederacao = c[5],
                        SiglaUnidadeEleitoral = c[6],
                        DescricaoUnidadeEleitoral = c[7],
                        Nome = c[10],
                        NomeUrna = c[14],
                        Partido = partido,
                        Eleicao = eleicao,
                        Enabled = true
                    };

                    context.Candidatos.Add(candidato);
                    //context.SaveChanges();
                }
            }
        }

        private
            void ImportarCandidatos(string path)
        {
        }
    }
}