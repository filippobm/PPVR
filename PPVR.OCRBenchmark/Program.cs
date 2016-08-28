using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PPVR.OCRBenchmark
{
    internal class Program
    {
        private static readonly IEnumerable<SantinhoPoliticoTest> SantinhosPoliticos = new List<SantinhoPoliticoTest>
        {
            new SantinhoPoliticoTest
            {
                Candidato = "Dr. Guimarães",
                Texto = @"EFEITO @ \nDrGulmaraes \nO CANDIDATO DO povo \nSOBRAL PARA TODOS OS SOBRALENSES",
                NumeroEleitoral = 43
            },
            new SantinhoPoliticoTest
            {
                Candidato = "Professora Narayana de Pontakayana",
                Texto =
                    @"COLIGAÇÃO AMOR À TRINDADE - \nPP - PC do B - PTN - PCB \n—-„.YEREADORA \nsso \nOÑTAKAYÀNA \n65065 \nPREFEITO \nVice:",
                NumeroEleitoral = 65065
            },
            new SantinhoPoliticoTest
            {
                Candidato = "Zé Bom Bril",
                NumeroEleitoral = 693333,
                Texto =
                    @"ACABE coM A SUJEIRA. VOTE NO \nZÉ BOM BRIL \nVEREADOR COM 1001 UTILIDADES. \nPD2 \nMANTENDO \nE so ATÉ A URNA E ""APERTAR\n69.333\nÉ NA FITA, MANO' FIRMEZA'"
            },
            new SantinhoPoliticoTest
            {
                Candidato = "Márcio Aníbal",
                NumeroEleitoral = 25789,
                Texto = @"Democratas \n$oante \nue \nVereador \nMárcio Aníbal \n'25789 \nAtitude que faz a diferença!"
            }
        };

        public static void Main(string[] args)
        {
            var t = MainAsync(args);
            t.Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            //// Process the list of files found in the directory.
            //var imageFilePaths = Directory.GetFiles(@"C:\Projects\PPVR\docs\analise_ocrs\dataset_santinhos_politicos");

            //Console.WriteLine("{0,-20} {1,50}\n", "Número Eleitoral", "Nome");

            //foreach (var imageFilePath in imageFilePaths)
            //{
            //    var fileInfo = new FileInfo(imageFilePath);
            //    var candidato = fileInfo.Name.Split('_');
            //    Console.WriteLine("{0,-20} {1,50}", candidato[0], candidato[1]);

            //    var text = await MicrosoftCognitiveServicesHelper.UploadAndRecognizeImage(imageFilePath);
            //}


            foreach (var santinhoPolitico in SantinhosPoliticos)
            {
                PesquisarCandidatoTexto(santinhoPolitico);
            }
            Console.ReadKey();
        }

        private static void PesquisarCandidatoTexto(SantinhoPoliticoTest santinhoPolitico)
        {
            var containsNumeroEleitoral = false;
            var regex = new Regex(@"\d+");
            var palavras = santinhoPolitico.Texto.Replace(".", "").Replace(",", "").Split(' ');

            foreach (var palavra in palavras)
            {
                var match = regex.Match(palavra);
                if (match.Success)
                {
                    if (match.Value == santinhoPolitico.NumeroEleitoral.ToString())
                    {
                        Console.WriteLine("O candidato {0} - {1} foi encontrado pelo Número Eleitoral",
                            santinhoPolitico.Candidato, santinhoPolitico.NumeroEleitoral);
                        containsNumeroEleitoral = true;
                    }
                }
            }
            if (!containsNumeroEleitoral)
            {
                if (santinhoPolitico.Texto.Contains(santinhoPolitico.Candidato, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("O candidato {0} - {1} foi encontrado pelo Nome.",
                        santinhoPolitico.Candidato, santinhoPolitico.NumeroEleitoral);
                }
            }
        }
    }

    public class SantinhoPoliticoTest
    {
        public int NumeroEleitoral { get; set; }
        public string Candidato { get; set; }
        public string Texto { get; set; }
    }

    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}