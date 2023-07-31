using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TurnParts;

namespace MagnusSpace
{
    public partial class _multipleChoice : Form
    {
        public string justificativa = "";
        CheckBox cbMain = new CheckBox();
        bool editMode = false;
        public _multipleChoice()
        {
            InitializeComponent();
        }
        private void build()
        {
           // MessageBox.Show("");
            ListClass lc = new ListClass();
            lc.Open("Choices");
            string line = "";
            if (lc.mainList.Count == 0)
            {
                line = "CN" + lc.VarDash + "ENTRADA NPI" + lc.VarDashPlus;
                line += "scrapCount" + lc.VarDash + "FALSE";
                lc.mainList.Add(line);

                line = "CN" + lc.VarDash + "CHANGE OVER" + lc.VarDashPlus;
                line += "scrapCount" + lc.VarDash + "TRUE";
                lc.mainList.Add(line);

                line = "CN" + lc.VarDash + "ENVIO" + lc.VarDashPlus;
                line += "scrapCount" + lc.VarDash + "TRUE";
                lc.mainList.Add(line);

                lc.Close();
            }
            Point p = new Point(10, 10);
            int count = 0;
            foreach (string l in lc.mainList)
            {
                CheckBox cb = new CheckBox();
                panel1.Controls.Add(cb);
                cb.Location = new Point(p.X, p.Y + cb.Height * count);
                cb.Text = l.Split(lc.VarDashPlus)[0].Split(lc.VarDash)[1];
                count++;
                cb.Click += (s, args) =>
                {
                    textBox2.Text = cb.Text;
                    if (cb.Checked)
                    {
                        justificativa = cb.Text;
                    }
                };
                
                cb.CheckedChanged += (s,args) =>
                {
                    if (cb.Checked)
                    {
                        button2.Enabled = true;
                        foreach (Control control in panel1.Controls)
                        {
                            if (control is CheckBox chk)
                            {
                                if(chk.Text != cb.Text)
                                    chk.Checked = false;
                            }
                                
                        }
                    }
                };
                
            }
        }
        private void _multipleChoice_Load(object sender, EventArgs e)
        {

            build();
        }
        public bool allunchecked()
        {

            return true;
        }
        int lastChecked = 0;
        public DialogResult Show()
        {
            playsound();
            return this.ShowDialog();
        }
        public void playsound()
        {
            ListClass lc = new ListClass();
            Form1 form = new Form1();
            form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;
            string adress = "";
            adress = form.config("requestAdress"); //ok
            if (adress == "")
            {
                form.config("requestAdress", "R:", true);
                return;
            }
            adress += "\\Listas";
            if (!Directory.Exists(adress))
            {
                try
                {
                    Directory.CreateDirectory(adress);
                }
                catch
                {
                    return;
                }

            }
            string listName = DateTime.Now.ToString().Replace(':', '_');
            listName = listName.Replace('/', '_');
            listName = "Alert " + listName;
            lc.Open(listName, adress);
            lc.Close();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;
            form.justificativa_saida = justificativa;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void eDITToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editMode = !editMode;
            if (editMode)
            {
                eDITToolStripMenuItem.BackColor = Color.Green;
               // eDITToolStripMenuItem.Text = "SALVAR";
                panel2.Visible = true;
            }
            else
            {
               // eDITToolStripMenuItem.Text = "EDITAR";
                eDITToolStripMenuItem.BackColor = Color.Silver;
                panel2.Visible = false;

            }

        }

        private void checkBox2_Click(object sender, EventArgs e)
        {


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {

        }

        private void checkBox3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox5_Click(object sender, EventArgs e)
        {

        }

        private void checkBox6_Click(object sender, EventArgs e)
        {

        }

        private void checkBox7_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            lc.Open("Choices");
            string line = "";
            line = "CN" + lc.VarDash + textBox2.Text + lc.VarDashPlus;
            if (checkBox1.Checked)
            {
                line += "scrapCount" + lc.VarDash + "TRUE";
            }
            else
            {
                line += "scrapCount" + lc.VarDash + "FALSE";
            }

            lc.mainList.Add(line);
            textBox2.Text = "";
            checkBox1.Checked = false;
            lc.Close();
            panel1.Controls.Clear();
            build();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            lc.Open("Choices");
            int count = 0;
            foreach(string l in lc.mainList.ToList())
            {
                if (l.Split(lc.VarDashPlus)[0].Split(lc.VarDash)[1] == textBox2.Text)
                {
                    lc.mainList.RemoveAt(count);
                    textBox2.Text = "";
                    checkBox1.Checked = false;
                    break;
                }
                count++;
            }
            lc.Close();
            panel1.Controls.Clear();
            build();

        }
    }
}
