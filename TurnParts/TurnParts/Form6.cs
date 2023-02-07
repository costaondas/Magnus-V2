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
   
    public partial class Form6 : Form
    {
        List<string> CNList = TurnParts.Form1.CNList.ToList();
        public int totalButtons = 0;
        public bool launch = false;
        char VarDash = ((char)887);
        public List<string> arrayList = new List<string>();
        public Form6()
        {
            InitializeComponent();
            timer1.Interval = 100;
            timer1.Start();
        
        }
        public void loadSerach(string text)
        {
            //label11.Text = "Search: " + text;
            int a = 0;
            bool containsAllStrings = true;
            
            //textBox1.Text = "";
         
            List<string> resolts = new List<string>();
            foreach (string l in CNList)
            {
                StringComparison comp = StringComparison.OrdinalIgnoreCase;
                try
                {
 
                    if(text.Contains(' '))
                    {
                        containsAllStrings = true; ;
                        foreach (string l2 in text.Split(' ').ToList())
                        {
                            if (!l.Contains(l2, comp))
                            {
                                containsAllStrings = false;
                            }
                        }
                        if (containsAllStrings)
                        {
                            resolts.Add(l);
                        }
                    }
                    else
                    {
                        if (l.Contains(text, comp))
                        {
                            resolts.Add(l);

                        }
                    }
                }
                catch
                {

                }

               
            }
          
            loadButtonArray(resolts);
        }
        Point p1 = new Point(10, 10);
        int butNumber = 0;
        int butSpace = 4;
        
        public void loadButtonArray(List<string> list)
        {
            arrayList = list;
            launch = true;
            int line = 1;
            int collum = 1;
            int numColumns = 3;
            string text1 = "";
            int total = list.Count();
            numColumns = total / 8;
            numColumns++;
            totalButtons = list.Count;
            int b = 0;
            foreach (string l in list)
            {
               
                text1 = "";
                



                Button but = new Button();
                string cn = "";
                string grupo = "";
                string position = "";
                but.Size = new Size((panel1.Width - 6)/ numColumns, 30);
                but.Location = new Point((but.Width*(collum-1)), p1.Y + (but.Height + butSpace) * line);
                but.Font = new Font("Times New Roman", 14);
                but.ForeColor = Color.White;
                but.BackColor = Color.FromArgb(45,70,80);
                butNumber++;
                
                int a = 0;
                

                foreach (string l2 in l.Split(VarDash).ToList())
                {
                    if(a!=0)
                        text1 += " "+ l2;
                    else
                    {
                        text1 += l2;
                    }
                    if (a == 0)
                    {
                        cn = l2;
                    }
                    if (a == 15)//grupo
                    {
                        grupo = l2;
                    }
                    if (a == 11)//posição
                    {
                        position = l2;
                    }
                    a++;
                }
                a = 0;
                but.Text = cn;
                if(grupo != "")
                {
                    if(position == "OUT")
                    {
                        but.BackColor = Color.Green;
                    }
                    if (position == "IN")
                    {
                        but.BackColor = Color.Blue;
                    }
                }
                but.Click += (s, args) =>
                {
                    TurnParts.Form1.callDisplayCN = cn;
            
                    TurnParts.Form1.callDisplayBoll = true;
                };


                
                panel1.Controls.Add(but);
                collum++;
                if (collum == numColumns + 1)
                {
                    collum = 1;
                    line++;
                }
                b++;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (TurnParts.Form1.loadNewSearch == false)
            {
                return;
            }
            CNList = TurnParts.Form1.CNList.ToList();
            loadSerach(TurnParts.Form1.searchText);
            TurnParts.Form1.searchList = arrayList;
            TurnParts.Form1.searchText = "";
            TurnParts.Form1.loadNewSearch = false;
            timer1.Stop();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    public static class StringExtensions
    {
        public static bool Contains(this String str, String substring,
                                    StringComparison comp)
        {
            if (substring == null)
                throw new ArgumentNullException("substring",
                                             "substring cannot be null.");
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
                throw new ArgumentException("comp is not a member of StringComparison",
                                         "comp");

            return str.IndexOf(substring, comp) >= 0;
        }
    }
}
