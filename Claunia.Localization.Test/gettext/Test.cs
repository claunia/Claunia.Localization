using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Claunia.Localization.Parsers;
using NUnit.Framework;

namespace Claunia.Localization.Test.gettext
{
    [TestFixture]
    public sealed class Test
    {
        [Test]
        public void Parse()
        {
            string testPath = Path.Combine(".", "gettext", "es_ES.po");

            Core.Localization localization = GetText.Parse(testPath, Encoding.GetEncoding("iso8859-1"));

            DateTime                   start   = DateTime.UtcNow;
            Dictionary<string, string> indexed = localization.GetIndex("es_ES");
            DateTime                   end     = DateTime.UtcNow;
            TimeSpan                   elapsed = end - start;
        }
    }
}