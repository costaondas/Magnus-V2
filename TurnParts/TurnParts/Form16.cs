using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TurnParts;
using static System.Net.Mime.MediaTypeNames;

namespace MagnusSpace
{
    public partial class Form16 : Form
    {
        public Form16()
        {
            InitializeComponent();
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
        public List<string> txb_to_list()
        {
            string[] lines = textBox1.Text.Split
           (
              new string[] { Environment.NewLine },
              StringSplitOptions.RemoveEmptyEntries
           );

            return lines.ToList();
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            

                if ((Keys)e.KeyChar == Keys.Enter)
            {
                List<string> list = new List<string>();
                list = txb_to_list();
                string text = textBox2.Text;
                textBox2.Text = "";
                if(text == "REMOVE")
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
                if(text == "CHECK" || text == "EDIT" || text == "SEARCH" || text == "PRINT")
                {
                    return;
                }


                if (button2.Text == "REMOVE")
                {
                    int counter = 0;
                    foreach (string l in list.ToList())
                    {
                        if (l == text)
                        {
                            list.RemoveAt(counter);
                            break;
                        }
                        counter++;
                    }
                    textBox1.Text = string.Join(Environment.NewLine, list);
                }
                else
                {
                    foreach (string l in list.ToList())
                    {
                        if (l == text)
                        {
                            return;
                        }
                        
                    }
                    if (textBox1.Text == "")
                        textBox1.Text = text;
                    else
                    {
                        textBox1.Text += "\r\n" + text;
                    }
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            
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

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;
            string adress = "";
            adress = form.config("requestAdress"); //ok
            if(adress == "")
            {
                form.config("requestAdress", "R:",true);
            }
            adress += "\\Requisições";
            string listName = DateTime.Now.ToString().Replace(':', '_');
            listName = listName.Replace('/', '_');
            List<string> list3 = new List<string>();
            ListClass lc = new ListClass();
            if (!Directory.Exists(adress))
            {
                System.Media.SystemSounds.Hand.Play();
                _messageBox ms = new _messageBox();
                ms.Show("Diretorio de requisição inexistente");
                return;
            }
            lc.Open(listName, adress);
            lc.mainList = txb_to_list(); //esse
            List<string> cnlist = new List<string>();
            cnlist.AddRange(lc.mainList);
            foreach(string cn in cnlist)
            {
                List<string> list2 = new List<string>();
                string log = "";
                Item item = new Item();
                item.Open(cn);
                Console.WriteLine($"CN<{cn}>");
                if (!item.itemExists)
                    continue;
                list2 = item.logsList[item.logsList.Count - 1].Split(' ').ToList();
                string log1 = item.logsList[item.logsList.Count - 1];
                List<string> log2 = log1.Split(' ').ToList().GetRange(0, 6);
                log1 = string.Join(" ", log2);
                list3.Add(log1);
                bool hasREQ = false;
                int count = 0;
                foreach(string l in list2)
                {
                    if (l.StartsWith("REQ:"))
                    {
                        hasREQ = true;
                        break;
                    }
                    count++;
                }
                if (!hasREQ)
                {
                    item.logsList[item.logsList.Count - 1] += " REQ:" + listName.Replace(' ', '-');
                }
                else
                {
                    list2[count] = "REQ:" + listName.Replace(' ', '-');
                    item.logsList[item.logsList.Count - 1] = string.Join(" ", list2);
                }
                
                item.Close();

            }

            
            lc.mainList = list3;
            lc.Close();

            form.refresh();
            this.Close();
        }

        private void Form16_Load(object sender, EventArgs e)
        {

        }
    }
}
