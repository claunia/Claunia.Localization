using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using Claunia.Localization.Core;
using Claunia.Localization.Parsers;

namespace Claunia.Localization.Benchmark
{
    public class Index
    {
        readonly Core.Localization localization;

        public Index()
        {
            localization = GetText.Parse(Path.Combine(".", "gettext", "es_ES.po"), Encoding.GetEncoding("iso8859-1"));
        }

        [Benchmark]
        public void LinqIndex()
        {
            Dictionary<string, string> index = new Dictionary<string, string>();
            foreach(Message message in localization.Messages)
            {
                LocalizedString translated = message.Translations.FirstOrDefault(l => l.Locale == "es_ES");

                if(translated is null)
                {
                    // TODO: Requested "es" but only "es_ES" exists
                    int underscore = "es_ES".IndexOf('_');
                    if(underscore > 0)
                        translated =
                            message.Translations.FirstOrDefault(l => l.Locale == "es_ES".Substring(underscore));
                }

                try { index.Add(message.Id, translated is null ? message.Source.Singular : translated.Singular); }
                catch(ArgumentException e)
                {
                    Debug.WriteLine(e);
                    throw;
                }
            }
        }
    }
}