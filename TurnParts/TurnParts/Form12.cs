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
using TurnParts;

namespace MagnusSpace
{

    public partial class Form12 : Form
    {
        public List<string> displayList = new List<string>();
        public List<string> headList = new List<string>();
        DataTable mainDATA= new DataTable();
        string searchText = "  search";
        Size formSize = new Size();
        public bool FixSize = false;
        public bool changeWithEnter = false;
        public bool dontFocus = false;
        public bool call20 = false;

        public Form12()
        {

         
            InitializeComponent();
            dataGridView1.Columns.Clear();
            textBox1.Text = searchText;
        }
        public void size(Size s)
        {
            formSize = s;
            FixSize= true;
        }
        
        private void Form12_Load(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            /*
            lc.Open("Mestra");
            List<string> list = new List<string>();
            List<string> newlist = new List<string>();
            list.Add("CN" + lc.VarDash.ToString() + "CN");
            list.Add("Modelo" + lc.VarDash.ToString() + "MODELO");
            list.Add("QTD" + lc.VarDash.ToString() + "Quantidade");
            list.Add("P/N" + lc.VarDash.ToString() + "PN");
            headList = list;
            dataGridView1.Columns.Clear();
            displayList = lc.mainList;
            */

            Console.WriteLine("get main data");
            mainDATA = lc.toDataTable(displayList, headList);
            Console.WriteLine("main data ok");
            displayList = lc.filterList(displayList, headList);
            Console.WriteLine("Filtered");
            dataGridView1.DataSource = mainDATA;

            //AutoSize = true;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.White;//Color.FromArgb(22, 87, 20);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;//Color.FromArgb(130, 255, 132);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.AutoSize = false;
            if (FixSize)
            {
                dataGridView1.Size = new Size(formSize.Width, formSize.Height - getOffset() - 4);
                dataGridView1.Location = new Point(0, panel1.Height);
            }
            else
            {
                setSize();
            }
            
         
            //addCollums(dataGridView1);
            
        }
        public void addCollums(DataGridView dt)
        {
            int numbColums = 0;
            int Xloc = 0;
            Xloc = dt.RowHeadersWidth;
            numbColums = dt.Columns.Count;
            for(int a = 0; a < numbColums; a++)
            {
                TextBox tb = new TextBox();
                int index = a;
                tb.Location = new Point(Xloc, 0);
                panel1.Controls.Add(tb);
                tb.BorderStyle = BorderStyle.FixedSingle;
                tb.BackColor = SystemColors.Control;
                tb.Size = new Size(dt.Columns[a].Width, panel1.Height );
                tb.TextChanged += (s,EventArgs) =>
                {
                    textBox1.Text = index.ToString();
                };

                Xloc += dt.Columns[a].Width;
                
            }
        }
        void sizeDGV(DataGridView dgv, int offset)
        {
            int heightOffset = 4;
            DataGridViewElementStates states = DataGridViewElementStates.None;
            //dgv.ScrollBars = ScrollBars.None;
            var totalHeight = 0;// Form.ActiveForm.Height;//dgv.Rows.GetRowsHeight(states) + dgv.ColumnHeadersHeight;
            totalHeight += dgv.Rows.Count * 4; // a correction I need
            var totalWidth = dgv.Columns.GetColumnsWidth(states) + dgv.RowHeadersWidth;
            dgv.ClientSize= new Size(totalWidth + offset, totalHeight);
        }
        public int getOffset()
        {
            int offset = 0;
            foreach (Control ctrl in dataGridView1.Controls)
                if (ctrl.GetType() == typeof(VScrollBar))
                {
                    offset = ctrl.Width;
                    //break;
                }
            return offset;
        }
        public void setSize()
        {
            int offset = 0;
            offset = getOffset();
            int offset2 = 0;
            offset2 = offset;
            offset = 0;
            sizeDGV(dataGridView1, offset);
            this.Width = dataGridView1.Width;
            this.Height = Screen.FromControl(this).Bounds.Height - 100;
            int X1 = Screen.FromControl(this).Bounds.Width;
            int Y1 = Screen.FromControl(this).Bounds.Height;
            this.MaximumSize = new Size(X1 , Y1);
            // this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            
            Size refe = this.Size;
            if(this.Width != X1)
            {
                offset2 = 0;
            }

            dataGridView1.Location = new Point(0, panel1.Height);
            dataGridView1.Size = new Size(refe.Width - offset2, refe.Height - offset - panel1.Height);
            offset = 0;
            this.Size = new Size(refe.Width + offset + offset2, refe.Height + offset);
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void focus()
        {
            if (dontFocus)
                return;
            Form1 form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;
            string mode=  form.searchButtonReturnToOriginal();
            form.buttonMode(mode);
            form.focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void search()
        {
            if (textBox1.Text != "" && textBox1.Text != searchText && textBox1.Focused)
            {
                List<string> l1 = new List<string>();
                ListClass lc = new ListClass();
                l1 = lc.search(displayList, textBox1.Text);

                dataGridView1.DataSource = lc.toDataTable(l1, headList);
            }
            else
            {
                dataGridView1.DataSource = mainDATA;
            }
                focus();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!changeWithEnter)
            {
                search();
            }
            
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = searchText;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == searchText)
                textBox1.Text = "";
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && changeWithEnter)
            {
                search();
            }
        }
        public string clickCN = "";
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            if (call20)
            {
                Form20 form20 = System.Windows.Forms.Application.OpenForms["Form20"] as Form20;
                form20.callDisplay(text); // procurar em EOL tambem
                return;
            }
            if (dontFocus)
            {
                clickCN= text;
                return;
            }
            
            //int row = dataGridView1.CurrentCell.RowIndex;
            Form1 form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;
            form.callDisplay(text); // procurar em EOL tambem
            //MessageBox.Show(text);


        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dontFocus)
                return;
                focus();
        }

        private void Form12_SizeChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
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
            lc.Open(listName,adress);
            lc.mainList.Add(String.Join(lc.VarDashPlus.ToString(),headList));
            lc.mainList.AddRange(displayList);
            lc.Close();
        }
    }
}
