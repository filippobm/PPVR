using System.Threading.Tasks;

namespace PPVR.OCRBenchmark
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var t = MainAsync(args);
            t.Wait();
        }

        private static async Task MainAsync(string[] args)
        {
        }
    }
}