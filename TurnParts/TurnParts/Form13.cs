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
    public partial class Form13 : Form
    {
        public string title = "";
        public List<string> displayList = new List<string>();
        public List<string> headList = new List<string>();
        public Form13()
        {
            InitializeComponent();
            dataGridView1.Columns.Clear();
            dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", 20, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 20, FontStyle.Bold);
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            dataGridView1.DataSource = lc.toDataTable(displayList, headList);
            try
            {
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch { }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
     
        }

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
            Console.WriteLine($"Resize {dataGridView1.Size}");
            dataGridView1.AutoSize = false;
            int cWidth = dataGridView1.Size.Width / 2;
            dataGridView1.ClientSize = dataGridView1.Size;
            try
            {
                dataGridView1.Columns[0].Width = cWidth;
                dataGridView1.Columns[1].Width = cWidth;
            }
            catch { }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    public class MyTextBox : TextBox
    {
        public MyTextBox()
        {
            SelectionHighlightEnabled = true;
        }
        const int WM_SETFOCUS = 0x0007;
        const int WM_KILLFOCUS = 0x0008;
        [DefaultValue(true)]
        public bool SelectionHighlightEnabled { get; set; }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SETFOCUS && !SelectionHighlightEnabled)
                m.Msg = WM_KILLFOCUS;

            base.WndProc(ref m);
        }
    }
}
