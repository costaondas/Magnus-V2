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
    public delegate void myDelegate2();
    public partial class Form3 : Form
    {
        public string model = "empty";
  
        public event myDelegate resizePanel;
        public event myDelegate hideLogPanelLabels;
        public event myDelegate updatePanels;
        public event myDelegate revertPanelColor;
        public event myDelegate moveTasks;
        public event myDelegate disposeTasks;
        int currentSelectedLogPanel = 1;
        int scrollLastValue = 0;
        int totalNumberOfTasks_onDislay = 0;
        int numbOfTasksOnDisplay = 0;
        int textboxIndex = 0;
        string lastTextBoxName = "textB1";
        int selectedTaskIndexOnList = -1;
        int[] taskDisplacemnt = new int[] { 0, 0 };
        List<string> TextboxNamesList = new List<string>();
        List<string> itensList; // lista que gera os paineis
        string dash = ((char)886).ToString();
        char dashC = ((char)886);

        public Form3()
        {
            InitializeComponent();

        }
        private string getTextNames(string k = "")
        {
            string textNames = "";
            int a = 0;
            foreach (string l in TextboxNamesList)
            {
                try
                {
                    foreach (string m in l.Split(dashC).ToList())
                    {
                        if (m == lastTextBoxName)
                        {
                            textNames = l;
                            if (k == "returnIndex")
                            {
                                return (a).ToString();
                            }
                        }
                    }
                }
                catch
                {

                }
                a++;
            }

            return textNames;


        }
        public void getData()
        {
            Folders f = new Folders();
            itensList = f.allItensInModelList(model);
          
        }
        private string newTextIndex()
        {
            textboxIndex++;
            return textboxIndex.ToString();
        }
        public int ReturnTaskIndex()
        {
            int indexask = Convert.ToInt32(getTextNames("returnIndex"));
            return indexask;
        }
        public void loadLayout()
        {
            if (disposeTasks != null)
            {
                disposeTasks();
            }
            TextboxNamesList = new List<string>();
            int taskBoxsStartY = 40;
            int[] sizes = new int[] { 60,400,100,200};
            int numPanels = itensList.Count();
            Point fbP = new Point(10, 10); //location of the first box
            int space_between_boxes_X = 2;
            int space_between_boxes_Y = 2;
            Size boxsize = new Size(100, 40);
            int a = 0;
            int fontSize = 15;
            Color butColor = Color.DarkGreen;
            Color fontColor = Color.White;
            totalNumberOfTasks_onDislay = 0;
            foreach (string l in itensList.ToList())
            {
                
                totalNumberOfTasks_onDislay++;
                if (a == 0)
                {
                    fbP = new Point(10, taskBoxsStartY);
                }

                ///////////////
                //TextBox t1 = new TextBox();
                Button t1 = new Button();
                t1 = statusColor(l.Split(dashC)[3]);
                t1.Location = fbP;
                try
                {
                    if (l != "empty")
                        t1.Text = l.Split(dashC)[0];
                }
                catch { }
                
                t1.Size = new Size(sizes[0], boxsize.Height);
                //t1.Multiline = false;
                t1.Font = new Font("Times New Roman", fontSize, FontStyle.Bold);
               
                t1.Name = "textB" + newTextIndex();
                if (l == "empty")
                {
                    lastTextBoxName = t1.Name;
                }
                t1.Click += (s, args) => {
                    lastTextBoxName = t1.Name;
                    selectedTaskIndexOnList = ReturnTaskIndex();
                };

                t1.KeyDown += (s, args) =>
                {


                };
                disposeTasks += () =>
                {
                    t1.Dispose();
                };
                //t1.

                ///////////////
                //TextBox t2 = new TextBox();
                Button t2 = new Button();
                t2 = statusColor(l.Split(dashC)[3]);
                t2.Location = new Point(t1.Location.X + t1.Width + space_between_boxes_X, fbP.Y);
                try
                {
                    if (l != "empty")
                        t2.Text = l.Split(dashC)[1];
                }
                catch { }
                
                t2.Size = new Size(sizes[1], boxsize.Height);
                //t2.Multiline = false;
                t2.Font = new Font("Times New Roman", fontSize, FontStyle.Bold);
          
                t2.Name = "textB" + newTextIndex();
                t2.Click += (s, args) => {
                    lastTextBoxName = t2.Name;
                    selectedTaskIndexOnList = ReturnTaskIndex();
                };
                disposeTasks += () =>
                {
                    t2.Dispose();
                };
                ///////////////
                //TextBox t3 = new TextBox();
                Button t3 = new Button();
                t3 = statusColor(l.Split(dashC)[3]);
                t3.Location = new Point(t2.Location.X + t2.Width + space_between_boxes_X, fbP.Y);
                t3.Size = new Size(sizes[2], boxsize.Height);
                try
                {
                    if (l != "empty")
                        t3.Text = l.Split(dashC)[2];
                }
                catch { }
                
                //t3.Multiline = false;
                t3.Font = new Font("Times New Roman", fontSize, FontStyle.Bold);
    
                t3.Name = "textB" + newTextIndex();
                t3.Click += (s, args) => {
                    lastTextBoxName = t3.Name;
                    selectedTaskIndexOnList = ReturnTaskIndex();
                };
                disposeTasks += () =>
                {
                    t3.Dispose();
                };
                ///////////////
                //TextBox t4 = new TextBox();
                Button t4 = new Button();
                t4 = statusColor(l.Split(dashC)[3]);
                try
                {
                    //t4.Text = l.Split(dashC)[3];
                }
                catch { }
                
                t4.Location = new Point(t3.Location.X + t3.Width + space_between_boxes_X, fbP.Y);
                t4.Size = new Size(sizes[3], boxsize.Height);
                try
                {
                    if (l != "empty")
                        t4.Text = l.Split(dashC)[3];
                }
                catch { }
                //t4.Multiline = false;
                t4.Font = new Font("Times New Roman", fontSize, FontStyle.Bold);
                t4.Name = "textB" + newTextIndex();
                t4.Click += (s, args) => {
                    lastTextBoxName = t4.Name;
                    selectedTaskIndexOnList = ReturnTaskIndex();
                };
                disposeTasks += () =>
                {
                    t4.Dispose();
                };
                //t4.BackColor = butColor;
                //t4.Enabled = false;
                ///////////////
                ///
                TextboxNamesList.Add(t1.Name + dash + t2.Name + dash + t3.Name + dash + t4.Name + dash);
                groupBox1.Controls.Add(t1);
                groupBox1.Controls.Add(t2);
                groupBox1.Controls.Add(t3);
                groupBox1.Controls.Add(t4);
                fbP = new Point(fbP.X, t1.Location.Y + t1.Height + space_between_boxes_Y);
                numbOfTasksOnDisplay = a;
                a++;

                moveTasks += () =>
                {
                    t1.Location = new Point(t1.Location.X + taskDisplacemnt[0], t1.Location.Y + taskDisplacemnt[1]);
                    t2.Location = new Point(t2.Location.X + taskDisplacemnt[0], t2.Location.Y + taskDisplacemnt[1]);
                    t3.Location = new Point(t3.Location.X + taskDisplacemnt[0], t3.Location.Y + taskDisplacemnt[1]);
                    t4.Location = new Point(t4.Location.X + taskDisplacemnt[0], t4.Location.Y + taskDisplacemnt[1]);

                };
                

            }


        }
        //"Trivial", "Moderado", "Alerta", "Crítico", "Extremamente crítico", "Zerado", "Sem Controle"
        public Button statusColor(string status)
        {
            Button but = new Button();
            switch (status)
            {
                case "Trivial":
                    but.BackColor =  Color.Blue;
                    but.ForeColor = Color.White;
                    break;
                case "Moderado":
                    but.BackColor = Color.DarkGreen;
                    but.ForeColor = Color.White;
                    break;
                case "Alerta":
                    but.BackColor = Color.Yellow;
                    but.ForeColor = Color.Black;
                    break;
                case "Crítico":
                    but.BackColor = Color.DarkRed;
                    but.ForeColor = Color.Black;
                    break;
                case "Extremamente crítico":
                    but.BackColor = Color.Red;
                    but.ForeColor = Color.White;
                    break;
                case "Zerado":
                    but.BackColor =  Color.Black;
                    but.ForeColor = Color.White;
                    break;
                case "Sem Controle":
                    but.BackColor = Color.Gray;
                    but.ForeColor = Color.White;
                    break;
                default:
                    but.BackColor = Color.White;
                    but.ForeColor = Color.Black;
                    break;

            }

            return but;
        }
        public void ganarateItensPanels() //paineis
        {


        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            loadLayout();
            if (updatePanels != null)
            {
                updatePanels();
            }
            label1.Text = model;
            label2.Text = "Total de Itens: " + itensList.Count().ToString();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            vScrollBar1.Maximum = 300;
            vScrollBar1.Minimum = 0;
            int displacement = vScrollBar1.Value - scrollLastValue;
            scrollLastValue = vScrollBar1.Value;
            taskDisplacemnt = new int[] { taskDisplacemnt[0], displacement *(-15)};
            if (moveTasks != null)
            {
                moveTasks();
            }
            taskDisplacemnt = new int[] { 0, 0 };
        }
    }
}
