using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;


namespace Teste_botão
{
    public partial class Zebra : Form
    {
        char VarDash = (char)(887);
        char VarDashPlus = (char)(888);
       //string directoryMonitor = "T:\\Turn_Parts\\Magnus\\Zebra";
       string directoryMonitor = "Z:";
        public Zebra()
        {
            InitializeComponent();
        }

        private void Zebra_Load(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            // lc.Open();
            timer1.Interval = 500;
            timer1.Start();

        }
        public void printString(string s)
        {
            PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new PrinterSettings();
            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);
        }
        public int cv(string txt)
        {
            int r = 0;
            try
            {
                r = Convert.ToInt32(txt);
            }
            catch
            {
                r = 0;
            }
            return r;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            string[] files = new string[1];
            try
            {
                files = Directory.GetFiles(directoryMonitor);
            }
            catch { Console.WriteLine("No Network"); }
            
            foreach (string file in files)
            {
                try
                {
                    if (file.Contains(".txt"))
                    {
                        Console.WriteLine(file);
                        ListClass lc = new ListClass();
                        lc.Open(file);
                        //Console.WriteLine("content" + lc.mainList[0]);
                        if (lc.mainList.Count() > 0)
                        {
                            string printLine = "";
                            string tp = "";
                            Apend ap = new Apend();
                            string location3 = "";
                            string location2 = "";
                            string location = "";
                            if (lc.mainList[0].StartsWith("PRINT" + VarDashPlus))
                            {
                                int counter = 0;
                                int pX = 0;
                                int pY = 0;
                                int fontSize = 0;
                                string font = "A0";
                                int qtdL = 1;
                                foreach (string l in lc.mainList[0].Split(VarDashPlus).ToList())
                                {
                                    if (counter == 0)
                                    {
                                        counter++;
                                        ap.Open();
                                        ap.font = font;
                                        ap.fontSize = fontSize;
                                        ap.x = pX;
                                        ap.y = pY;
                                        continue;
                                    }
                                    if (l.Split(VarDash)[0] == "X")
                                    {
                                        ap.x = cv(l.Split(VarDash)[1]);
                                    }
                                    if (l.Split(VarDash)[0] == "Y")
                                    {
                                        ap.y = cv(l.Split(VarDash)[1]);
                                    }
                                    if (l.Split(VarDash)[0] == "Size")
                                    {
                                        ap.fontSize = cv(l.Split(VarDash)[1]);
                                    }
                                    if (l.Split(VarDash)[0] == "Font")
                                    {
                                        ap.font = l.Split(VarDash)[1];
                                    }
                                    if (l.Split(VarDash)[0] == "txt")
                                    {
                                        ap.Add(l.Split(VarDash)[1]);
                                    }
                                    if (l.Split(VarDash)[0] == "qtdLabels")
                                    {
                                        qtdL = cv(l.Split(VarDash)[1]);
                                    }

                                }
                                tp = ap.Close();
                                while (qtdL > 0)
                                {
                                    printString(tp);
                                    qtdL--;
                                }
                                //File.Delete(file);
                            }
                            else

                            {
                                foreach (string l in lc.mainList[0].Split(VarDashPlus).ToList())
                                {
                                    if (l.Split(VarDash)[0] == "CN")
                                    {
                                        ap.CN = l.Split(VarDash)[1];
                                    }
                                    if (l.Split(VarDash)[0] == "Modelo")
                                    {
                                        ap.modelo = l.Split(VarDash)[1];
                                    }
                                    if (l.Split(VarDash)[0] == "Name")
                                    {
                                        ap.Name = l.Split(VarDash)[1];
                                    }
                                    if (l.Split(VarDash)[0] == "P/N")
                                    {
                                        ap.PN = l.Split(VarDash)[1];
                                    }
                                    if (l.Split(VarDash)[0] == "grupo")
                                    {
                                        ap.grupo = l.Split(VarDash)[1];
                                    }
                                    if (l.Split(VarDash)[0] == "Descrição")
                                    {
                                        string desc1 = "";
                                        string desc2 = "";
                                        string desc3 = "";
                                        try { desc1 = l.Split(VarDash)[1].Split((char)886)[0]; }
                                        catch { }
                                        try { desc2 = l.Split(VarDash)[1].Split((char)886)[1]; }
                                        catch { }
                                        try { desc3 = l.Split(VarDash)[1].Split((char)886)[2]; }
                                        catch { }

                                        ap.desc1 = desc1;
                                        ap.desc2 = desc2;
                                        ap.desc3 = desc3;
                                    }
                                    if (l.Split(VarDash)[0] == "ID")
                                    {
                                        ap.ID = l.Split(VarDash)[1];
                                    }
                                    if (l.Split(VarDash)[0] == "printQTD")
                                    {
                                        ap.QTD = l.Split(VarDash)[1];
                                    }
                                    if (l.Split(VarDash)[0] == "qtdLabels")
                                    {
                                        ap.nLabels = l.Split(VarDash)[1];
                                    }
                                    if (l.Split(VarDash)[0] == "groupPosition")
                                    {
                                        ap.position = l.Split(VarDash)[1];
                                    }
                                    if (l.Split(VarDash)[0] == "location3")
                                    {
                                        location3 = l.Split(VarDash)[1];
                                        ap.ADRESS = location3 + " " + location2 + " " + location;
                                    }
                                    if (l.Split(VarDash)[0] == "location2")
                                    {
                                        location2 = l.Split(VarDash)[1];
                                        ap.ADRESS = location3 + " " + location2 + " " + location;
                                    }
                                    if (l.Split(VarDash)[0] == "location")
                                    {
                                        location = l.Split(VarDash)[1];
                                        ap.ADRESS = location3 + " " + location2 + " " + location;
                                    }

                                }
                                if (ap.QTD == "")
                                {
                                    if (ap.Name == "Fixture" || ap.Name == "fixture" || ap.Name == "FIXTURE")
                                    {
                                        tp = ap.etiquetaFixture();
                                    }
                                    else
                                    {
                                        if (ap.grupo == "")
                                        {
                                            tp = ap.etiquetaBin();
                                        }
                                        else
                                        {
                                            tp = ap.grupoEtiqueta();
                                        }
                                        //Console.WriteLine("Print BIN");

                                    }

                                }
                                else
                                {
                                    tp = ap.Quantidade_em_pacote();
                                }
                                tp = ap.Close();
                                Console.WriteLine(tp);
                                int number = 1;
                                try
                                {
                                    number = Convert.ToInt32(ap.nLabels);
                                }
                                catch
                                {
                                    number = 1;
                                }
                                while (number > 0)
                                {
                                    printString(tp);
                                    number--;
                                }
                            }

                           
                            
                        }
                        File.Delete(file);
                    }
                }
                catch
                {
                    Console.WriteLine("no content");
                    continue;
                }
                
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
