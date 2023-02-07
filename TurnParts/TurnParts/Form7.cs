using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnusSpace
{
    public partial class Form7 : Form
    {
        public string grupo = "";
        public string cn = "";
        string dash = ((char)886).ToString();
        char VarDash = ((char)887);
        char VarDashPlus = ((char)888);
        public Form7()
        {
            InitializeComponent();
            
           // List<string> list = new List<string>();


        }

        public string getVersao(string grupo)
        {
            string versao = "";
            ListClass lc = new ListClass();
            Folders folder = new Folders();
            lc.Open(grupo, folder.versoesFX);
            string retString = "";
            try
            {
                retString = lc.mainList.Last();
            }
            catch
            {
                return "";
            }
             
            if (retString.Contains(VarDash))
            {
                return lc.mainList.Last().Split(VarDash)[0];
            }
            else
            {
                return "";
            }
            
            
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //SALVAR COMO NOVO
            ListClass lc = new ListClass();
            Folders folder = new Folders();
            lc.Open(grupo,folder.versoesFX);
            string time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString();
            time = time.Split(' ')[0];
            string texttoAdd = textBox1.Text + VarDash + textBox2.Text + VarDash + time;
            lc.mainList.Add(texttoAdd);
            lc.Close();
            
            


            


            string vsAtual = "";
            vsAtual = lc.mainList.Last().ToString().Split(VarDash)[0];
            label4.Text = vsAtual;
            Item item = new Item();
            item.Open(cn);
            foreach (string l in item.groupList.ToList())
            {
                string CNnow = "";
                CNnow = l.Split(VarDash)[0];


                
                Item item3 = new Item();

                item3.Open(CNnow);
                item3.stream("versaoGrupo", vsAtual);
                item3.Close();

            }

            loadTextBox();


        }
        void loadTextBox()
        {
            textBox3.Text = "";
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ListClass lc = new ListClass();
            Folders folder = new Folders();
            lc.Open(grupo, folder.versoesFX);
            Console.WriteLine(grupo);
            Console.WriteLine(folder.versoesFX);
            foreach (string l in lc.mainList)
            {
                textBox3.Text += "(" + l.Split(VarDash)[2] + ")   Versão: " + l.Split(VarDash)[0] + "     " + l.Split(VarDash)[1];
                if (lc.mainList.IndexOf(l) != lc.mainList.Count() - 1)
                {
                    textBox3.Text += "\r\n";
                }
                else
                {
                    label4.Text = l.Split(VarDash)[0];
                }
                Console.WriteLine("ADD " + l);
            }
            lc.Close();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            loadTextBox();
            label4.Location = new Point(label3.Location.X + label3.Width + 5 , label3.Location.Y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            Folders folder = new Folders();
            lc.Open(grupo, folder.versoesFX);
            string time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString();
            time = time.Split(' ')[0];
            string texttoAdd = textBox1.Text + VarDash + textBox2.Text + VarDash + time;
            lc.mainList[lc.mainList.Count() -1]  = texttoAdd;
            lc.Close();
            loadTextBox();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_Click(object sender, EventArgs e)
        {
          //  this.Focus();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void upgradeTodosOsFixturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Item item = new Item();
            item.Open(cn);
            string versao = getVersao(item.grupo);
            if (versao == "")
            {
                MessageBox.Show("Nenhuma versão cadastrada");
                return;
            }

            string text = "Atualizar todos os fixtures do grupo " + grupo+" para a versão atual?";
            // MessageBox.Show("Atualizar para versão atual?");
            DialogResult dialogResult = MessageBox.Show(text, "UPGRADE para " + versao, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString();
                foreach (string l in item.groupList.ToList())
                {
                    Item item3 = new Item();
                    item3.Open(l.Split(VarDash)[0]);
                    item3.stream("versao", versao);
                    time = time.Split(' ')[0];
                    item3.stream("DATA_MANUT", time);
                    item3.Close();
                }
                
             



            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }
    }
}
