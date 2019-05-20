using System;
using Eto;
using Eto.Forms;

namespace Claunia.Localization.Desktop
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Application(Platform.Detect).Run(new MainForm());
        }
    }
}