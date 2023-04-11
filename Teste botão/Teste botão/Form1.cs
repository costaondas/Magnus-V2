using System.Configuration;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Teste_botão
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int a = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string name = "name";
            string barcode = "barcode";
            var sb = new StringBuilder();
            PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
            sb.AppendLine();
            sb.AppendLine("N");
            sb.AppendLine("q425");
            sb.AppendLine("Q300,26");
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                "A36,96,0,4,1,1,N,\"{0}\"",name));
            //sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
              //  "B50,13,0,1A,2,3,50,B,\"{0}\"", barcode));
            sb.AppendLine("P1,1");
            if(DialogResult.OK == pd.ShowDialog(this))
            {
                RawPrinterHelper.SendFileToPrinter(pd.PrinterSettings.PrinterName, sb.ToString());
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string s = "^XA^LH30,30\n^FO20,10^ADN,90,50^AD^FDHello World^FS\n^XZ";
            string intro = "^XA";
            string final = "^XZ";
            /*
            string Segunda = "^FO50,50";
            string terceira = "^A0N50,120";
            string quarta = "^FDHello, World!^FS";
            string final = "^XZ";
            string s2 = intro + Segunda + terceira + quarta;

            Segunda = "^100,120";
            terceira = "^A0N50,60";
            quarta = "^FDHello, World!^FS";

            s2 += Segunda + terceira + quarta + final;

            string s = "";
            s = s2;
            */
            string s = "";
            s += intro;
            s += textBox1.Text;
            s += textBox2.Text;
            s += textBox3.Text;
            //string s3 = s;
            s += final;
            /*

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(s3);
            sb.AppendLine(s);
            s = sb.ToString();

            */

            PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new PrinterSettings();
            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);
            
            
            /*
            if (DialogResult.OK == pd.ShowDialog(this))
            {
                RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);
            }

            */
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
          //  go();
        }
        public void printString(string s)
        {
            PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new PrinterSettings();
            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);
        }
        public void go()
        {
            //string s = "^XA^LH30,30\n^FO20,10^ADN,90,50^AD^FDHello World^FS\n^XZ";
            string intro = "^XA";
            string final = "^XZ";
            /*
            string Segunda = "^FO50,50";
            string terceira = "^A0N50,120";
            string quarta = "^FDHello, World!^FS";
            string final = "^XZ";
            string s2 = intro + Segunda + terceira + quarta;

            Segunda = "^100,120";
            terceira = "^A0N50,60";
            quarta = "^FDHello, World!^FS";

            s2 += Segunda + terceira + quarta + final;

            string s = "";
            s = s2;
            */
            string s = "";
            s += intro;
            s += textBox1.Text;
            s += textBox2.Text;
            s += textBox3.Text;
            //string s3 = s;
            s += final;
            /*

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(s3);
            sb.AppendLine(s);
            s = sb.ToString();

            */

            PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new PrinterSettings();
            RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);


            /*
            if (DialogResult.OK == pd.ShowDialog(this))
            {
                RawPrinterHelper.SendStringToPrinter(pd.PrinterSettings.PrinterName, s);
            }

            */
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                go();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string tp = "";
                Apend ap = new Apend();
                ap.modelo = "WARLOCK";
                ap.Name = "CABO LVDS 44p";
                ap.PN = "NPIAPAKDE78";
                ap.CN = "1098";
                ap.ID = "503";
                ap.QTD = "1500";
                ap.ADRESS = "BUTITI_A C 4";
                tp = ap.Quantidade_em_pacote();

                tp = ap.Close();
                Console.WriteLine(tp);
                printString(tp);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime a = DateTime.Now;
            ListClass lc = new ListClass();
            string name = "test";
            string folder = "C:\\Zebra test";
            lc.Open(name, folder);
            lc.mainList.Add(a.ToString());
            lc.Close();
        }
        

        private void button4_Click(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            string name = "test";
            string folder = "C:\\Zebra test";
            lc.Open(name,folder);
            textBox1.Text = lc.mainList[0].ToString();

        }
    }
}