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
using System.Xml;
using TurnParts;

namespace MagnusSpace
{
    public partial class _messageBox : Form
    {
        public _messageBox()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public void ShowNoDialog(string text)
        {
            label2.Text = text;
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
        public DialogResult Show(string text)
        {
            playsound();
            label2.Text = text;
            //lblText.ForeColor = foreColour;
            return this.ShowDialog();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
