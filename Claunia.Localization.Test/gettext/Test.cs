using System.IO;
using System.Text;
using NUnit.Framework;

namespace Claunia.Localization.Test.gettext
{
    [TestFixture]
    public sealed class Test
    {
        [Test]
        public void Parse()
        {
            var testPath = Path.Combine(".", "gettext","es_ES.po");

            var localization = Claunia.Localization.Parsers.GetText.Parse(testPath, Encoding.GetEncoding("iso8859-1"));
        }
    }
}