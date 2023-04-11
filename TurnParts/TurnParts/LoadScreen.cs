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
    public partial class LoadScreen : Form
    {
        public LoadScreen()
        {
            InitializeComponent();
            timer1.Interval = 3;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == 100)
            {
                timer1.Stop();
                //Form1 form = new Form1();
               // form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;
                //if(form!=null)
                 //   form.Close();
                TurnParts.Form1 newForm1 = new TurnParts.Form1();
                this.Hide();
                newForm1.ShowDialog();

                this.Close();

                return;
            }
                
            progressBar1.Value += 1;


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void LoadScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
