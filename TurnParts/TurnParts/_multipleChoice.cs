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
        public _multipleChoice()
        {
            InitializeComponent();
        }

        private void _multipleChoice_Load(object sender, EventArgs e)
        {

        }
        public bool allunchecked()
        {
            if (checkBox1.Checked) { }
                else
                return false;
            if (checkBox2.Checked) { }
            else
                return false;
            if (checkBox3.Checked) { }
            else
                return false;
            if (checkBox4.Checked) { }
            else
                return false;
            if (checkBox5.Checked) { }
            else
                return false;
            if (checkBox6.Checked) { }
            else
                return false;
            if (checkBox7.Checked) { }
            else
                return false;
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
            if (checkBox1.Checked)
            {
                justificativa = checkBox1.Text;
                lastChecked= 1;
                button2.Enabled = true;

                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
            }
            else
            {
                if(lastChecked == 1)
                    button2.Enabled = false;
            }
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                justificativa = checkBox2.Text;
                lastChecked = 2;
                button2.Enabled = true;

                checkBox1.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
            }
            else
            {
                if (lastChecked == 2)
                    button2.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                justificativa = checkBox4.Text;
                lastChecked = 4;
                button2.Enabled = true;

                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox1.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
            }
            else
            {
                if (lastChecked == 4)
                    button2.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                lastChecked = 3;
                button2.Enabled = true;
                justificativa = checkBox3.Text;
                checkBox2.Checked = false;
                checkBox1.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
            }
            else
            {
                if (lastChecked == 3)
                    button2.Enabled = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                lastChecked = 5;
                button2.Enabled = true;
                justificativa = checkBox5.Text;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox1.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
            }
            else
            {
                if (lastChecked == 5)
                    button2.Enabled = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                lastChecked = 6;
                button2.Enabled = true;
                justificativa = checkBox6.Text;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox1.Checked = false;
                checkBox7.Checked = false;
            }
            else
            {
                if (lastChecked == 6)
                    button2.Enabled = false;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                lastChecked = 7;
                button2.Enabled = true;
                justificativa = checkBox7.Text;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox1.Checked = false;
            }
            else
            {
                if (lastChecked == 7)
                    button2.Enabled = false;
            }
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
    }
}
