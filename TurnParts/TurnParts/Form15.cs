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
    public partial class Form15 : Form
    {
        List<string> Itenslist = new List<string>();
        public Form15()
        {
            InitializeComponent();
        }

        private void Form15_Load(object sender, EventArgs e)
        {
            loadCombo();

            //comboBox1.Items = 
        }
        public void loadCombo()
        {
            comboBox1.Items.Clear();
            Item item = new Item();
            List<string> list = new List<string>();
            //list = item.maintanenceList(); // lista global
            ListClass lc = new ListClass();
           // ListClass lc = new ListClass();
            lc.Open("Itens de Manutenção");
            foreach (string l in lc.mainList)
            {
                if (l.StartsWith("CN"))
                {
                    list.Add(l);
                }
            }
            char vd = lc.VarDash;
            char vdP = lc.VarDashPlus;
            string itemCN = "";
            string dias = "";
            foreach (string l in list)
            {
                if (l == "")
                    continue;
                itemCN = l.Split(vdP)[0].Split(vd)[1];
                comboBox1.Items.Add(itemCN);
                Itenslist.Add(l);

            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            lc.Open("Itens de Manutenção");
            char vd = lc.VarDash;
            char vdP = lc.VarDashPlus;
            string itemCN = comboBox1.Text;
            string dias = "";
            //lc.mainList = Itenslist;
            dias = lc.streamPlus(itemCN, "dias_validade");
            textBox1.Text = dias;
           // comboBox1.Text
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            textBox1.Text = "";
            lc.Open("Itens de Manutenção");
            char vd = lc.VarDash;
            char vdP = lc.VarDashPlus;
            int c = 0;
            foreach (string l in lc.mainList.ToList())
            {
                if(l.StartsWith("CN"+ vd+ comboBox1.Text + vdP))
                {
                    lc.mainList.RemoveAt(c);
                    break;
                }
                c++;
            }
            lc.Close();
            loadCombo();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
            //loadCombo();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keys)e.KeyValue == Keys.Enter)
            {
                int days = 0;
                try
                {
                    days = Convert.ToInt32(textBox1.Text);
                }
                catch { return; }
                if (comboBox1.Text == "")
                    return;
                ListClass lc = new ListClass();
                lc.Open("Itens de Manutenção");
                char vd = lc.VarDash;
                char vdP = lc.VarDashPlus;
                lc.streamPlus(comboBox1.Text, "dias_validade", days.ToString());
                lc.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int days = 0;
            try
            {
                days = Convert.ToInt32(textBox2.Text);
            }
            catch { return; }
            ListClass lc = new ListClass();
            lc.Open("Itens de Manutenção");
            char vd = lc.VarDash;
            char vdP = lc.VarDashPlus;
            lc.streamPlus(textBox3.Text, "dias_validade", days.ToString(),true);
            lc.Close();
            textBox3.Text = "";
            textBox2.Text = "";
            textBox1.Text = "";
            loadCombo();
        }
    }
}
