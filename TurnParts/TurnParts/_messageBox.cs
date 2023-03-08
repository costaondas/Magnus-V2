using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

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
        public DialogResult Show(string text)
        {
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
