using Microsoft.Office.Interop.Excel;
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
    public partial class Form19 : Form
    {
        public string listAdress = "";
        public string listName = "";
        public List<string> TPlist = new List<string>();
        public class item
        {
            public string cn = "";
            public int qtd = 0;
        }
        public List<item> TPlist2 = new System.Collections.Generic.List<item>(); 
        public Form19()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "ADD")
            {
                button2.Text = "REMOVE";
                button2.BackColor = Color.DarkRed;
            }
            else
            {
                button2.Text = "ADD";
                button2.BackColor = Color.YellowGreen;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string t = textBox2.Text;
            if (t.StartsWith("@"))
            {
                t = t.Split('@')[1];
            }
            char[] array;
            int n = t.Count();
            if (n != 0)
            {
                array = t.ToCharArray();
                if (array[n - 1] == ' ')
                {
                    textBox2.Text = "";
                    if (button2.Text == "ADD")
                    {
                        button2.Text = "REMOVE";
                        button2.BackColor = Color.DarkRed;
                        return;
                    }
                    if (button2.Text == "REMOVE")
                    {
                        button2.Text = "ADD";
                        button2.BackColor = Color.YellowGreen;
                        return;
                    }
                }
            }
        }

        public List<string> txb_to_list()
        {
            string[] lines = textBox1.Text.Split
           (
              new string[] { Environment.NewLine },
              StringSplitOptions.RemoveEmptyEntries
           );

            return lines.ToList();
        }
        public string pattern(string cn,int qtd)
        {
            return "CN:" + cn + " QTD:" + qtd.ToString();
        }
        public string lcPattern(item i)
        {
            ListClass lc = new ListClass();
            string vd = lc.VarDash.ToString();
            string vdP = lc.VarDashPlus.ToString();
            string retur = "CN" + vd + i.cn + vdP;
            retur += "QTD" + vd + i.qtd.ToString();
            return retur;
        }
        public void build()
        {
            textBox1.Clear();
            foreach (item j in TPlist2)
            {
                textBox1.Text += pattern(j.cn, j.qtd) + "\r\n";
            }
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                List<string> list = new List<string>();
               // list = txb_to_list();
                string text = textBox2.Text;
                if(text.StartsWith("@"))
                {
                    text = text.Split('@')[1];
                }
                textBox2.Text = "";
                if (text == "REMOVE")
                {
                    button2.Text = "REMOVE";
                    button2.BackColor = Color.DarkRed;
                    return;
                }
                if (text == "ADD")
                {
                    button2.Text = "ADD";
                    button2.BackColor = Color.YellowGreen;
                    return;
                }
                if (text == "CHECK" || text == "EDIT" || text == "SEARCH" || text == "PRINT")
                {
                    return;
                }


                if (button2.Text == "REMOVE")
                {
                    int counter = 0;
                    foreach (item i in TPlist2.ToList())
                    {
                        if (i.cn == text)
                        {
                            if (i.qtd > 1)
                            {
                                i.qtd -= 1;
                            }
                            else
                            {
                                TPlist2.RemoveAt(counter);
                            }
                                
                            
                            break;
                        }
                        counter++;
                    }
                    build();
                }
                else //ADD
                {
                    foreach (item i in TPlist2.ToList())
                    {
                        if (i.cn == text)
                        {
                            i.qtd += 1;
                            build();
                            return;
                        }

                    }
                    item j = new item();
                    j.cn = text;
                    j.qtd = 1;
                    TPlist2.Add(j);

                    textBox1.Text += pattern(j.cn,j.qtd) + "\r\n" ;
                }
            }
        }

        private void Form19_Load(object sender, EventArgs e)
        {
            label1.Text = listName;
            ListClass lc = new ListClass();
            ListClass st = new ListClass();
            lc.ListPath = listAdress;
            lc.mainList = lc.readList();
            TPlist2.Clear();
            foreach(string l in lc.mainList.ToList())
            {
                item i = new item ();
                st.mainList = l.Split(lc.VarDashPlus).ToList();
                i.cn = st.stream("CN");
                try
                {
                    i.qtd = Convert.ToInt32(st.stream("QTD"));
                }
                catch
                {
                    i.qtd = 1;
                }
                
                TPlist2.Add(i);
            }
            build();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            lc.ListPath = listAdress;
            //lc.mainList = lc.readList();
            lc.mainList.Clear();
            foreach (item i in TPlist2)
            {
                lc.mainList.Add(lcPattern(i));
            }
            lc.Close();
            Form1 form = new Form1();
            form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;
            form.loadSKUtoolstrip();
            //form.refresh();
            this.Close();
        }
    }
}
