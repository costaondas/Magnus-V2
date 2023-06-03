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
    public partial class Form21 : Form
    {
        public List<string> list = new List<string>();
        public Form21()
        {
            InitializeComponent();
        }
        public class fixture
        {
            string cn = "";
            int diasCorridos = 0;
            int diasTotais = 0;
            int scale = 0;
        }
        private void Form21_Load(object sender, EventArgs e)
        {

        }
    }
}
