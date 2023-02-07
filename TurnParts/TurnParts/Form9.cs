using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnusSpace
{
    public partial class Form9 : Form
    {
        public string cn = "";
        public string qtd = "";
        public string qtdCompra = "";
        public string qtdFIM = "";
        public string placasProduzidas = "";
        public string forecast = "";
        public string data = "";
        public string turnsLinha = "1";
        public string scraps = "";
        public string inicioIntervalo = "";
        public string fimIntervalo = "";
        public string cycleLife = "";




        Color colorButton = Color.Green;//Color.FromArgb(49, 160, 95);
        Color GraycolorButton = Color.FromArgb(192, 192, 192);
        private List<string> _myVar = new List<string>();
        public List<string> MyVar
        {
            get
            {
                return _myVar;
            }
            set
            {
                if (_myVar != value)
                    _myVar = value;
            }
        }
        public Form9()
        {
            InitializeComponent();
        }
        public void loadForm(object Form)
        {
            while(this.panel2.Controls.Count > 0)
            {
                this.panel2.Controls.RemoveAt(0);
            }
            
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            panel2.Controls.Add(f);
            panel2.Tag = f;
            f.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MyVar.Add("AAA");
            loadForm(new Form10());
            button1.BackColor = colorButton;
            button2.BackColor = GraycolorButton;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.BackColor = GraycolorButton;
            button2.BackColor = colorButton;
            loadForm(new Form8());
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            button1.BackColor = colorButton;
            button2.BackColor = GraycolorButton;
            loadForm(new Form10());
        }
    }
}
