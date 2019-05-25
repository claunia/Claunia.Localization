using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Claunia.Localization.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Summary parserBenchmark    = BenchmarkRunner.Run<Parser>();
            Summary retrievalBenchmark = BenchmarkRunner.Run<Retriever>();
            Summary indexBenchmark     = BenchmarkRunner.Run<Index>();
        }
    }
}