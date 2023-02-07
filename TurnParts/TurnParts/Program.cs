using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TurnParts
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MagnusSpace.Form2());
           Application.Run(new MagnusSpace.Form10());
          // Application.Run(new MagnusSpace.LoadScreen()); // main
            //Application.Run(new MagnusSpace.testForm());
        }
    }
}
