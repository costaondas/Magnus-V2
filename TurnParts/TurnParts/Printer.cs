//using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

//using Microsoft.Office.Interop.Excel;

namespace MagnusSpace
{

    internal class Printer
    {
        char VarDash = ((char)887);
        char VarDashPlus = ((char)888);
        public int fonte = 50;
        public string texto = "";
        public string X = "";
        public string Y = "";
       
        public string newX()
        {
            int x = 0;
            try
            {
                if (X.Contains(VarDash))
                {
                    x = Convert.ToInt32(X.Split(VarDash)[1]);
                }
                else
                {
                    x = Convert.ToInt32(X);
                }
            }
            catch { }
            return (x - width(texto, fonte) / 2).ToString();

        }
        public string newY()
        {
            int y = 0;
            try
            {
                if (Y.Contains(VarDash))
                {
                    y = Convert.ToInt32(Y.Split(VarDash)[1]);
                }
                else
                {
                    y = Convert.ToInt32(Y);
                }
            }
            catch { }
            return (y - fonte / 2).ToString();

        }
        public int width(string text,int fonte)
        {
            double wid = 0;
            foreach (char c in text.ToCharArray())
            {
                Console.WriteLine($"char {c}  wid = {wid}");
                switch (c)
                {
                    case 'A': wid += 0.5458404; break;
                    case 'B': wid += 0.5458404; break;
                    case 'C': wid += 0.533831911; break;
                    case 'D': wid += 0.590443358; break;
                    case 'E': wid += 0.491724223; break;
                    case 'F': wid += 0.491724223; break;
                    case 'G': wid += 0.590443358; break;
                    case 'H': wid += 0.610249567; break;
                    case 'I': wid += 0.27432379; break;
                    case 'J': wid += 0.438543773; break;
                    case 'K': wid += 0.5458404; break;
                    case 'L': wid += 0.474101376; break;
                    case 'M': wid += 0.751544254; break;
                    case 'N': wid += 0.610561476; break;
                    case 'O': wid += 0.572040739; break;
                    case 'P': wid += 0.550986895; break;
                    case 'Q': wid += 0.573756238; break;
                    case 'R': wid += 0.590287404; break;
                    case 'S': wid += 0.533831911; break;
                    case 'T': wid += 0.499677898; break;
                    case 'U': wid += 0.612744838; break;
                    case 'V': wid += 0.533831911; break;
                    case 'X': wid += 0.5458404; break;
                    case 'Y': wid += 0.5458404; break;
                    case 'W': wid += 0.807999746; break;
                    case 'Z': wid += 0.491724223; break;
                    case '0': wid += 0.475037102; break;
                    case '1': wid += 0.475037102; break;
                    case '2': wid += 0.475037102; break;
                    case '3': wid += 0.475037102; break;
                    case '4': wid += 0.475037102; break;
                    case '5': wid += 0.475037102; break;
                    case '6': wid += 0.475037102; break;
                    case '7': wid += 0.475037102; break;
                    case '8': wid += 0.475037102; break;
                    case '9': wid += 0.475037102; break;
                    case 'a': wid += 0.454139213; break;
                    case 'b': wid += 0.490476588; break;
                    case 'c': wid += 0.43667232; break;
                    case 'd': wid += 0.494219494; break;
                    case 'e': wid += 0.474413285; break;
                    case 'f': wid += 0.278690513; break;
                    case 'g': wid += 0.491724223; break;
                    case 'h': wid += 0.491724223; break;
                    case 'i': wid += 0.252646128; break;
                    case 'j': wid += 0.252646128; break;
                    case 'k': wid += 0.43417705; break;
                    case 'l': wid += 0.252646128; break;
                    case 'm': wid += 0.753415706; break;
                    case 'n': wid += 0.49515522; break;
                    case 'o': wid += 0.479403826; break;
                    case 'p': wid += 0.493127813; break;
                    case 'q': wid += 0.493127813; break;
                    case 'r': wid += 0.334678142; break;
                    case 's': wid += 0.414370841; break;
                    case 't': wid += 0.274479744; break;
                    case 'u': wid += 0.49125636; break;
                    case 'v': wid += 0.43667232; break;
                    case 'w': wid += 0.65251321; break;
                    case 'x': wid += 0.43277346; break;
                    case 'y': wid += 0.43277346; break;
                    case 'z': wid += 0.373042925; break;
                    //case ' ': wid += 0.612744838; break;
                    default:  wid += 0.49;  break;

                }
                
            }
            return Convert.ToInt32(wid * fonte);
        }
        
    }
}
