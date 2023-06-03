using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
        public string itemCN = "";
        public string id = "";
        public bool smallFonte = false;
        public bool leftIliment = false;
        public bool maintananceWindow = false;
        public bool maintananceStatus = false;
        public string currentTechnician = "";
        public Form13()
        {
            
            InitializeComponent();
           
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            int fonte = 20;
            if (smallFonte)
                fonte = 12;
            dataGridView1.Columns.Clear();
            dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", fonte, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", fonte, FontStyle.Bold);
            ListClass lc = new ListClass();
            var loc = DataGridViewContentAlignment.MiddleCenter;
            var loc1 = DataGridViewContentAlignment.MiddleCenter;
            if (leftIliment)
            {
                loc = DataGridViewContentAlignment.MiddleLeft;
            }
            Console.WriteLine("Form 13 to data table");
            dataGridView1.DataSource = lc.toDataTable(displayList, headList);
            try
            {
                dataGridView1.Columns[0].DefaultCellStyle.Alignment = loc;
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = loc1;
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch { }
            if (maintananceStatus)
            {
                Item item = new Item();
                ListClass lc2 = new ListClass();
                Folders folder = new Folders();
                lc2.Open("maintenance", folder.itemFolder(itemCN));
                for (int a = 0; a < dataGridView1.Rows.Count - 1; a++)
                {
                    //if(a)
                    try
                    {
                        string itemCN_ = dataGridView1.Rows[a].Cells[0].Value.ToString();
                        string status = lc2.streamPlus(itemCN_, "Status");
                        if(status == "")
                        {
                            status = "NADA CONSTA";
                        }
                        Console.WriteLine($"STATUS<{status}>");
                        dataGridView1.Rows[a].Cells[1].Value = status;
                    }
                    catch { }

                }
            }
            if (maintananceWindow)
            {
                Item item = new Item();
                ListClass lc2 = new ListClass();
                Folders folder = new Folders();
                lc2.Open("maintenance", folder.itemFolder(itemCN));
                for (int a = 0; a < dataGridView1.Rows.Count - 1; a++)
                {
                    //if(a)
                    try
                    {
                        string itemCN_ = dataGridView1.Rows[a].Cells[0].Value.ToString();
                        dataGridView1.Rows[a].Cells[1].Value = lc2.streamPlus(itemCN_, "maintDate");
                    }
                    catch { }

                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
     
        }

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
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
            if (maintananceWindow)
            {
                Console.WriteLine("CELL CLICK");
                Form14 f = new Form14();
                Console.WriteLine($"Open 14 with cn {itemCN}");
                
                f.CN = itemCN;
                ListClass lc = new ListClass();
                // f.itemCN = currentCN;

                f.TopLevel = true;
                //f.Dock = DockStyle.Fill;

                f.headList = lc.RowTitle(13);
                f.displayList = displayList;
                f.title = "ITENS DE MANUTENÇÃO";
                f.FormBorderStyle = FormBorderStyle.None;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.BringToFront();
                f.Show();
            }
            if (maintananceStatus)
            {
                string itemCNInspect = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string status = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                if(status == "NADA COSNTA")
                {
                    if (currentTechnician == "")
                    {
                        return;
                    }
                    Item item = new Item();
                    item.Open(itemCN);
                    item.markMaintanance(itemCNInspect,"OK",id);
                    dataGridView1.CurrentRow.Cells[1].Value = "OK";

                }
            }
            

        }

        private void tabletToolStripMenuItem_Click(object sender, EventArgs e)
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
