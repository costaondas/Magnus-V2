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
    public delegate void myDelegate();
    public partial class Form2 : Form
    {
        public event myDelegate moveTasks;
        public event myDelegate disposeTasks;
        public event myDelegate disposeFixtures;
        public event myDelegate moveFixtures;
        Color c1 = Color.FromArgb(114, 84, 143);
        Color c2 = Color.FromArgb(160, 85, 191);
        Color c3 = Color.FromArgb(200, 127, 234);
        Color c4 = Color.FromArgb(86, 86, 94);
        Color c5 = Color.FromArgb(95, 94, 112);
        Color c6 = Color.FromArgb(111, 103, 143);
        Color cEnter = Color.FromArgb(255, 253, 212);
        Color cLeave = Color.FromArgb(255, 255, 255);
        string dash = ((char)886).ToString();
        char dashC = ((char)886);
        char VarDash = ((char)887);
        public char vd()
        {
            return VarDash;
        }
        List<string> taskList = new List<string>();
        int[] taskDisplacemnt = new int[] { 0, 0 };
        int scrollLastValue = 0;
        int textboxIndex = 0;
        string lastTextBoxName = "textB1";
        string lastTextBoxFixtureName = "textF1";
        List<string> TextboxNamesList = new List<string>();
        List<string> TextboxFIXTURESNamesList = new List<string>();
        string currentDate = "date";
        int selectedTaskIndexOnList = -1;
        int numbOfTasksOnDisplay = 0;
        int numbOfFixturesOnDisplay = 0;
        int addBounusTaskon_getTaskList = 0;
        int totalNumberOfTasks_onDislay = 0;
        int totalNumberOfFixtures_onDislay = 0;
        List<string> fixtureList = new List<string>();
        public Form2()
        {
            InitializeComponent();
          
           
            textBox5.ScrollBars = ScrollBars.Both;
        }
       
       
    
        public int ReturnTaskIndex()
        {
            int indexask = Convert.ToInt32(getTextNames("returnIndex"));
            //label2.Text = indexask.ToString();
            return indexask;
        }
        public int ReturnFixtureIndex()
        {
            int indexask = Convert.ToInt32(getFixturesTextNames("returnIndex"));
            //label2.Text = indexask.ToString();
            return indexask;
        }
        private void displayTasks(string action = "")
        {
            int index = ReturnTaskIndex();
            switch (action)
            {
                case "next":
                    index++;
                    if (index == totalNumberOfTasks_onDislay)
                        index = 0;
                    break;
                case "previous":
                    index--;
                    if (index == -1)
                        index = totalNumberOfTasks_onDislay - 1;
                    break;
            }
            string line = "";
            ListClass list = new ListClass();
            list.Open(currentDate, "Tasks");

            line = list.mainList[index];
            list.Close();

            Console.WriteLine("line:" + line.ToString());

            try { textBox1.Text = line.Split(dashC)[0]; } catch { }
            try { textBox2.Text = line.Split(dashC)[2]; } catch { }
            try { textBox3.Text = line.Split(dashC)[3]; } catch { }
            try { textBox4.Text = line.Split(dashC)[1]; } catch { }
            
            
            
            


        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        private string newTextIndex()
        {
            textboxIndex++;
            return textboxIndex.ToString();
        }
        private Button boxMode(string mode)
        {
            var t = new Button();
            switch (mode)
            {
                case "inProgress":
                    t.Text = "IN PROGRESS";
                    t.ForeColor = Color.Black;
                    t.BackColor = Color.Yellow;
                    break;
                case "Done":
                    t.Text = "DONE";
                    t.ForeColor = Color.Black;
                    t.BackColor = Color.Green;
                    break;
                case "waiting":
                    t.Text = "WAITING";
                    t.ForeColor = Color.Black;
                    t.BackColor = Color.LightSalmon;
                    break;

            }
            return t;

        }
       
        private void createTask(string name, string description, string ID, string title)
        {
            ListClass list = new ListClass();
            list.Open(currentDate, "Tasks");
            list.mainList.Add(name + dash + description + dash + ID + dash + title);
            list.Close();
        }
        private void editTask(string name, string description, string ID, string title)
        {
            if(selectedTaskIndexOnList == -1)
            {
                createTask(name, description, ID, title);
                return;
            }
            ListClass list = new ListClass();
            list.Open(currentDate, "Tasks");
            list.mainList[selectedTaskIndexOnList] = name + dash + description + dash + ID + dash + title;
            list.Close();
        }
        private List<string> getTaskList()
        {
            List<string> taskList = new List<string>();
            ListClass list = new ListClass();
            list.Open(currentDate, "Tasks");
            taskList = list.mainList;
            list.Close();
            while(addBounusTaskon_getTaskList != 0)
            {
                taskList.Insert(0, "empty");
                addBounusTaskon_getTaskList--;
            }
            return taskList;
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

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
        private string getFixturesTextNames(string k = "")
        {
            string textNames = "";
            int a = 0;
            foreach (string l in TextboxFIXTURESNamesList)
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


        private void but2Modes(string mode)
        {
            switch (mode)
            {
                case "NOVA TAREFA":
                    button2.Text = "NOVA TAREFA";
                    break;
                case "ADICIONAR":
                    button2.Text = "ADICIONAR";
                    break;
                case "SALVAR":
                    button2.Text = "SALVAR";
                    break;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string descrip = "";
            List<string> Create = new List<string>();
            Create.Add("");
            Create[0] += textBox1.Text + vd().ToString(); // CN
            Create[0] += textBox2.Text + vd().ToString(); // CN
            Create[0] += textBox3.Text + vd().ToString(); // CN
            Create[0] += textBox4.Text + vd().ToString(); // CN

             // CN
            descrip = textBox5.Text;
            char[] array = descrip.ToCharArray();
            int b = 0;
            foreach (char a in array)
            {
                if (a == '\n')
                {
                    array[b - 1] = (char)886;
                    array[b] = (char)886;
                }
                b++;
            }
            descrip = new string(array);

            Create[0] += descrip + vd().ToString();

            Create[0] += textBox6.Text + vd().ToString(); // CN
            Create[0] += textBox7.Text + vd().ToString(); // CN
            Create[0] += textBox8.Text + vd().ToString(); // CN
            Create[0] += textBox9.Text + vd().ToString(); // CN
            Create[0] += textBox10.Text + vd().ToString(); // CN
            Create[0] += textBox11.Text + vd().ToString(); // CN

            Item item = new Item();
            
            item.createItemFromList(Create);
            item.Close();


        
            /*
            switch (button2.Text)
            {
                case "NOVA TAREFA":
                    selectedTaskIndexOnList = -1;
                    addBounusTaskon_getTaskList++;

                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    loadLayout();
                    but2Modes("ADICIONAR");
                    button3.Visible = true;
                    break;
                case "ADICIONAR":
                    createTask(textBox1.Text, textBox4.Text, textBox2.Text, textBox3.Text);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    but2Modes("NOVA TAREFA");
                    button3.Visible = false;
                    loadLayout();
                    break;

                case "SALVAR":
                    editTask(textBox1.Text, textBox4.Text, textBox2.Text, textBox3.Text);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    button3.Visible = false;
                    loadLayout();
                    but2Modes("NOVA TAREFA");
                    break;
            }
            */
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            try { if (numbOfTasksOnDisplay != 0) { groupBox1.Controls[getTextNames().Split(dashC)[0]].Text = textBox1.Text; } }

            catch { }



            //

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            if (numbOfTasksOnDisplay != 0)
            {
                groupBox1.Controls[getTextNames().Split(dashC)[1]].Text = textBox2.Text;
            }
        }

        

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
            if (numbOfTasksOnDisplay != 0)
            {
                groupBox1.Controls[getTextNames().Split(dashC)[2]].Text = textBox3.Text;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            but2Modes("SALVAR");
          
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            but2Modes("SALVAR");
            
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            but2Modes("SALVAR");
         
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            but2Modes("SALVAR");
    
        }

        private void button3_Click(object sender, EventArgs e)
        {
      
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            label4.ForeColor = cEnter; 
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            label4.ForeColor = cLeave;
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            label5.ForeColor = cEnter;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            label5.ForeColor = cLeave;
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            label6.ForeColor = cEnter;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            label6.ForeColor = cLeave;
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            label7.ForeColor = cEnter;
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            label7.ForeColor = cLeave;
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            label8.ForeColor = cEnter;
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            label8.ForeColor = cLeave;
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            label9.ForeColor = cEnter;
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            label9.ForeColor = cLeave;
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            label10.ForeColor = cEnter;
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            label10.ForeColor = cLeave;
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            label11.ForeColor = cEnter;
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            label11.ForeColor = cLeave;
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            label12.ForeColor = cEnter;
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            label12.ForeColor = cLeave;
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            label13.ForeColor = cEnter;
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            label13.ForeColor = cLeave;
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            label14.ForeColor = cEnter;
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            label14.ForeColor = cLeave;
        }
    }
}
