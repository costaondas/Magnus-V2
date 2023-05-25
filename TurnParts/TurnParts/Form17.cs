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
    public partial class Form17 : Form
    {
        public Form17()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        public string text = "";
        public string data = "";
        public string testemunha1 = "";
        public string testemunha2 = "";
        public Image Imagetestemunha1 = null;
        public Image Imagetestemunha2 = null;
        public List<string> relação = new List<string>();
        private void Form17_Load(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            lc.Open("Termo");
            foreach(string l in lc.mainList.ToList())
            {
                textBox2.Text += l + "\r\n";
            }
            lc.Close();
            label3.Text = testemunha1;
            label2.Text = testemunha2;
            pictureBox1.Image = Imagetestemunha1;
            pictureBox2.Image = Imagetestemunha2;
            foreach (string l in relação)
            {
                textBox1.Text += l + "\r\n";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
