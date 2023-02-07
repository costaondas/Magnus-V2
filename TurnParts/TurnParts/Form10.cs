using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TurnParts;

namespace MagnusSpace
{
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            Form9 form = new Form9();
            form = Application.OpenForms["Form9"] as Form9;
            // label1.Text = form.cn;
            //textBox3.BackColor = SystemColors.Control;
           // textBox3.Enabled = true;
           // textBox3.BackColor = Color.Transparent;
            textBox3.ForeColor= Color.Green;
            textBox3.ReadOnly= true;

        }

        private void textBox3_EnabledChanged(object sender, EventArgs e)
        {
            //textBox3.ForeColor = sender. == false ? Color.Blue : Color.Red;
           // if(textBox3.Enabled == false)
                 
        }
    }
}
