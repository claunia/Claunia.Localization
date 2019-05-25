using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using Claunia.Localization.Parsers;

// ReSharper disable ClassCanBeSealed.Global

namespace Claunia.Localization.Benchmark
{
    public class Parser
    {
        [Benchmark]
        public Core.Localization ParseGetText() =>
            GetText.Parse(Path.Combine(".", "gettext", "es_ES.po"), Encoding.GetEncoding("iso8859-1"));
    }
}