using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace MagnusSpace
{
    public partial class Form14 : Form
    {
        public string title = "";
        public string CN = "";
        public List<string> displayList = new List<string>();
        public List<string> headList = new List<string>();
        public List<string> PendenciasAlllist = new List<string>();
         public List<string> PendenciasLinhalist = new List<string>();
        List<int> rowsChanged= new List<int>();
        public Form14()
        {
            InitializeComponent();
            dataGridView1.Columns.Clear();
            dataGridView1.DefaultCellStyle.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            
            }
        public void loadLayout()
        {
            textBox1.Location = new Point(panel1.Width - textBox1.Width - 30, panel1.Height / 2 - textBox1.Height / 2);
            label25.Location = new Point(textBox1.Location.X - label25.Width,panel1.Height/2 - label25.Height/2);
        }
        public static void loadPendencias(Label l1,Label l2,Label l3,string cn)
        {

            Item item = new Item();
            ListClass lc = new ListClass();
            item.Open(cn);
            if (item.grupo == "")
            {
                return;
            }
                
            
            List<string> list = new List<string>();
            list = item.groupList;
            char VarDash = lc.VarDash;
            int outNumber = 0;
            int pendencias = 0;
            foreach (string l in list.ToList())
            {

                string fxCN = l.Split(VarDash)[0];
                Item item2 = new Item();
                item2.Open(fxCN);
                if (item2.maintanenceExpired())
                {
                    if (l.Split(VarDash)[1] == "OUT")
                    {
                        outNumber++;
                        
                        
                    }
                    pendencias++;
                }

                Application.DoEvents();
            }
            //  Invoke(() => label3.Text = pendencias.ToString(););
            l1.Invoke((MethodInvoker)(() => l1.Text = pendencias.ToString()));
            l2.Invoke((MethodInvoker)(() => l2.Text = outNumber.ToString()));
            l3.Invoke((MethodInvoker)(() => l3.Text = item.total_group.ToString()));
            //l1.Text = pendencias.ToString();
            //l2.Text = outNumber.ToString();
           // l3.Text = item.total_group.ToString();
            //Console.WriteLine($"Calculando async {l3.Text}");
            //label3.Text = pendencias.ToString();
           // label4.Text = outNumber.ToString();
            //label6.Text = item.total_group.ToString();
        }
        public async Task PrintCount(Label l1,Label l2,Label l3,string cn)
        {

            loadPendencias(l1,l2,l3,cn);
        }
        private async void Form14_Load(object sender, EventArgs e)
        {
            Item item = new Item();
            item.Open(CN);
            label9.Text = item.ItemModelo;
            loadLayout();
            Console.WriteLine("LOAD FORM");
            ListClass lc = new ListClass();
            int totalColumns = headList.Count();
            dataGridView1.DataSource = lc.toDataTable(displayList, headList);
            try
            {
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                for (int a = 0; a < totalColumns; a++)
                {
                    dataGridView1.Columns[a].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                
            }
            catch { }
            dataGridView1.Enabled = false;

            ListClass lc2 = new ListClass();
            ListClass lc3 = new ListClass();
            lc3.mainList = displayList; //main
            Folders folder = new Folders();
            string data = DateTime.Now.ToString().Split(' ')[0];
            //string listCN = dataGridView1.Rows[a].Cells[1].Value.ToString(); //ToString();
            lc2.Open("maintenance", folder.itemFolder(CN));
            int rowsTotal = dataGridView1.Rows.Count;
            for(int a = 0; a< rowsTotal-1; a++)
            {
                string listCN = dataGridView1.Rows[a].Cells[1].Value.ToString();
                dataGridView1.Rows[a].Cells[2].Value = lc2.streamPlus(listCN, "maintDate");
                dataGridView1.Rows[a].Cells[3].Value = lc2.streamPlus(listCN, "technician");
                int dias = 0;
                try
                {
                    DateTime dt = Convert.ToDateTime(lc2.streamPlus(listCN, "maintDate"));
                    dias = Convert.ToInt32((DateTime.Now.Date - dt.Date).TotalDays);
                    dataGridView1.Rows[a].Cells[4].Value = dias.ToString() + " dias";

                    string status = "OK";
                    string diasValidade = lc3.streamPlus(listCN, "dias_validade");
                    if (diasValidade == "")
                    {
                        status = "NOT OK";
                    }
                    else
                    {
                        int diasV = Convert.ToInt32(diasValidade);
                        dt = dt.AddDays(diasV);
                        if(DateTime.Now.Date > dt.Date)
                        {
                            status = "NOT OK";
                        }

                    }
                    dataGridView1.Rows[a].Cells[0].Value = status;

                }
                catch
                {
                    dataGridView1.Rows[a].Cells[4].Value = "";
                    dataGridView1.Rows[a].Cells[0].Value = "NOT OK";
                }
                
                //dias_validade
            }


            reShape();
            var task = Task.Run(() => PrintCount(label3, label4, label6, CN));
            await task;
           // task.Start();
            Console.WriteLine("Step 1");
           // ; ; Task.Run(() => PrintCount(l1, l2, l3, cn));
            //await task;
            // PrintCount(label3,label4,label6,CN);
            //Task.Run((Action)PrintCount(label3, label4, label6, CN));
        }
        private async Task DoVariousThingsFromTheUIThreadAsync(Label l1,Label l2,Label l3,string cn)
        {
            // I have a bunch of async work to do, and I am executed on the UI thread.
            
           
        }

        public void reShape()
        {
            int totalColumns = headList.Count();
            if (totalColumns == 0)
                return;
            dataGridView1.AutoSize = false;
            int cWidth = dataGridView1.Size.Width / totalColumns;
            dataGridView1.ClientSize = dataGridView1.Size;
            try
            {
                for (int a = 0; a < totalColumns; a++)
                {
                    //dataGridView1.Columns[a].Width = cWidth;
                    if(a == 1)
                    {
                        dataGridView1.Columns[a].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dataGridView1.Columns[a].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }
                        
                    else
                    {
                        dataGridView1.Columns[a].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                }

                //  dataGridView1.Columns[1].Width = cWidth;
            }
            catch { }
            
        }
        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
            Console.WriteLine("SIZE CHANGED");
            reShape();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        bool locked = true;
        private void button2_Click(object sender, EventArgs e)
        {
            if (locked)
            {
                button2.BackgroundImage = new Bitmap(MagnusSpace.Properties.Resources.lockOpen);
                locked = false;
                dataGridView1.Enabled = true;
                button1.Visible = true;
            }
            else
            {
                button2.BackgroundImage = new Bitmap(MagnusSpace.Properties.Resources.lockClose);
                locked = true;
                dataGridView1.Enabled = false;
                button1.Visible = false;
            }
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if ((Keys)e.KeyValue == Keys.Enter)
            {
                string tecn = textBox1.Text;
                textBox1.Text = "";
                Item item = new Item();
                item.Open(CN);
                Folders folder = new Folders();
                ListClass lc2 = new ListClass();
                string data2 = DateTime.Now.ToString();
                lc2.Open("maintenanceLogs", folder.itemFolder(CN));
                foreach (int a in rowsChanged)
                {
                    string listCN = dataGridView1.Rows[a].Cells[1].Value.ToString(); //ToString();
                    dataGridView1.Rows[a].Cells[3].Value = tecn;
                    item.markMaintanance(listCN,"OK", tecn);
                    string log = listCN + lc2.VarDashPlus;
                    log += tecn + lc2.VarDashPlus;
                    log += data2;
                    lc2.mainList.Add(log);
                }
                lc2.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[1].Value.ToString() == "")
                return;
            string Inspection_item = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            dataGridView1.CurrentRow.Cells[0].Value = "OK";
            dataGridView1.CurrentRow.Cells[2].Value = DateTime.Now.ToString().Split(' ')[0];
            dataGridView1.CurrentRow.Cells[4].Value = "0 dias";
            int index = dataGridView1.CurrentRow.Index;
            rowsChanged.Add(index);
            rowsChanged = rowsChanged.Distinct().ToList();
            
                
        }
    }
}
