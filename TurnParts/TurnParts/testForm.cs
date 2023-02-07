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
    public partial class testForm : Form
    {
        public testForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            lc.Open("Mestra", "ListaGeral");
            lc.streamPlus(textBox1.Text, textBox2.Text,textBox3.Text);
            lc.Close();
        }
    }
}
