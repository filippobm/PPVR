using PPVR.WebApp.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Text;

namespace PPVR.WebApp.DataAccess.InitConfig
{
    public class ContextInitializer : DropCreateDatabaseAlways<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            var candidatos = ImportarCandidatos();

            context.SaveChanges();
        }

        private List<Candidato> ImportarCandidatos()
        {
            var candidatos = new List<Candidato>();

            var path = @"C:\Projects\PPVR\src\PPVR.WebApp\bin\Migrations\dataset\consulta_cand_2016\consulta_cand_2016_MG.txt";

            using (var streamReader = new StreamReader(path, Encoding.GetEncoding("ISO-8859-1")))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var x = line.Split(';');

                    if (x.Length != 46)
                    {
                        string i = "";
                    }

                }

                //[2] Ano eleição
                //[3] Turno
                //[4] Descrição Eleição
                //[5] SiglaUF
                //[6] SiglaUE
                //[7] NomeMunicipio
                //[12] Número Candidato Urna
                //[10] Nome Candidato
                //[14] Nome Urna Candidato
                //[16] Descrição Situação Candidato
                //[17] Número Partido
                //[18] Nome Partido
            }

            return candidatos;
        }
    }
}