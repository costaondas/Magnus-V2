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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace MagnusSpace
{
    public partial class Form10 : Form
    {
        public string cn = "";
        public string qtd = "";
        public string qtdCompra = "";
        public string qtdFIM = "";
        public string placasProduzidas = "";
        public string forecast = "";
        public string data = "";
        public string turnsLinha = "1";
        public string scraps = "";
        public string inicioIntervalo = "";
        public string fimIntervalo = "";
        public string cycleLife = "";
        string modelo = "";
        char VarDash = ((char)887);
        char VarDashPlus = ((char)888);
        int currentForecast = 0;
        float cycle = 0;
        float qtdF = 0;
        float qtdFimF = 0;
        float qtdCompraF = 0;
        float scrapF = 0;
        float placasProduzidasF = 0;
        float forecastF = 0;
        float turnLinhaF = 0;
        int sc = 0;
        List<string> GruposList = new List<string>();
        public Form10()
        {
            InitializeComponent();
        }
        
        private void Form10_Load(object sender, EventArgs e)
        {
            
            Form9 form = new Form9();
            form = System.Windows.Forms.Application.OpenForms["Form9"] as Form9;

            /*
             Form1 form = new Form1();
             form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;
             */

            // label1.Text = form.cn;
            //textBox3.BackColor = SystemColors.Control;
            // textBox3.Enabled = true;
            // textBox3.BackColor = Color.Transparent;
            //textBox3.ForeColor= Color.Green;
            textBox3.ReadOnly= true;
            DateTime date = DateTime.Now;
            loadYear(label4.Text);
            label4.Text = date.Year.ToString();
            textBox16.Text = textBox2.Text;
            modelo = form.modelo;
            label1.Text = modelo;
            cn = form.cn;

            Grupo gp = new Grupo();

            GruposList = gp.allGrupos();

            List<string> gps = new List<string>();
            foreach (string l2 in GruposList.ToList())
            {
                gps.Add(l2.Split(VarDashPlus)[0].Split(VarDash)[1]);
            }
            comboBox1.DataSource = gps;

            GroupTest();

        }
        public void GroupTest()
        {
           
        }
        public void loadYear(string yr)
        {
            int year = 0;
            try
            {
                year = Convert.ToInt32(yr);
            }
            catch { }
            ListClass lc = new ListClass();
            Folders folder = new Folders();
            
            lc.Open(modelo, folder.Forecast);
            textBox4.Text = lc.streamPlus(yr,"1");
            textBox5.Text = lc.streamPlus(yr, "2");
            textBox6.Text = lc.streamPlus(yr, "3");
            textBox9.Text = lc.streamPlus(yr, "4");
            textBox8.Text = lc.streamPlus(yr, "5");
            textBox7.Text = lc.streamPlus(yr, "6");
            textBox12.Text = lc.streamPlus(yr, "7");
            textBox11.Text = lc.streamPlus(yr, "8");
            textBox10.Text = lc.streamPlus(yr, "9");
            textBox15.Text = lc.streamPlus(yr, "10");
            textBox14.Text = lc.streamPlus(yr, "11");
            textBox13.Text = lc.streamPlus(yr, "12");
            try
            {
                currentForecast = Convert.ToInt32(textBox16.Text);
            }
            catch { currentForecast = 0; }
            
        }
        public void writeMonth(string t2, int month)
        {
            calendar(t2, month, false);
        }
        public void loadForecast()
        {
            Item item = new Item();
            item.Open(cn);
            textBox16.Text = item.getForecast().ToString();
            //calendar("1", 1, true);
        }
        public void calendar(string t2,int month,bool loadOnly = false)
        {
            int forcast = 0;
            string t = "";
            if (t2 == "")
                t = "0";
            else
            {
                t = t2;
            }
            int var = 0;
            try
            {
                var = Convert.ToInt32(t);
            }
            catch { return; }
            ListClass lc = new ListClass();
            Folders folder = new Folders();
            lc.Open(modelo, folder.Forecast);
            bool cnFound = false;
            if (loadOnly)
            {
                cnFound = true;
            }
            if (!cnFound)
            {
                foreach (string l in lc.mainList)
                {
                    if (l.StartsWith("CN" + VarDash.ToString() + label4.Text + VarDashPlus.ToString()))
                    {
                        cnFound = true;
                        //break;
                    }

                }
            }
            
            if (!cnFound)
                lc.mainList.Add("CN" + VarDash.ToString() + label4.Text);
            lc.streamPlus(label4.Text, month.ToString(), var.ToString());
            foreach(string l in lc.mainList)
            {
                bool bk = false;
                if (l.StartsWith("CN"))
                {
                    string currentCN = l.Split(VarDashPlus)[0].Split(VarDash)[1];
                    int CCN = 0;
                    try
                    {
                        CCN = Convert.ToInt32(currentCN);
                    }
                    catch { bk = true; }
                    if (bk == false)
                    {

                        DateTime dt = DateTime.Now;
                        int yr = dt.Year;
                        if (CCN >= yr)
                        {
                            List<string> list = new List<string>();
                            list = l.Split(VarDashPlus).ToList();
                            list.RemoveAt(0);
                            if (CCN == yr)
                            {
                                int mon = dt.Month;
                                int cm = 0;
                                foreach (string m in list)
                                {
                                    cm = Convert.ToInt32(m.Split(VarDash)[0]);
                                    if (cm >= mon)
                                    {
                                        forcast += Convert.ToInt32(m.Split(VarDash)[1]);
                                    }
                                }
                            }
                            if (CCN > yr)
                            {
                                foreach (string m in list)
                                {
                                    forcast += Convert.ToInt32(m.Split(VarDash)[1]);
                                }
                            }
                        }
                    }
                }

            }
            if(!loadOnly)
                lc.Close();
            textBox16.Text = forcast.ToString();
            currentForecast = forcast;
        }

        private void textBox3_EnabledChanged(object sender, EventArgs e)
        {
            //textBox3.ForeColor = sender. == false ? Color.Blue : Color.Red;
           // if(textBox3.Enabled == false)
                 
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (int.TryParse(label4.Text, out int var))
            {
                label4.Text = (var + 1).ToString();
                loadYear(label4.Text);
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (int.TryParse(label4.Text, out int var))
            {
                label4.Text = (var - 1).ToString();
                loadYear(label4.Text);
            }
               
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox4.Text, 1);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox5.Text, 2);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox6.Text, 3);
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox9.Text, 4);
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox8.Text, 5);
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox7.Text, 6);
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox12.Text, 7);
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox11.Text, 8);
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox10.Text, 9);
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox15.Text, 10);
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox14.Text, 11);
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            writeMonth(textBox13.Text, 12);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox16.Text = textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString().Split(' ')[0];
            inicioIntervalo = "NPI";
            string time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString();
            time = time.Split(' ')[0];
            fimIntervalo = time;
            scrapF = (float)(sc);
            try
            {
                placasProduzidasF = (float)Convert.ToInt32(textBox1.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

            Item item = new Item();
            item.Open(cn);
            List<string> list = new List<string>();
            list = item.modelToList();
            DateTime ini = DateTime.Now;
            DateTime fim = ini;
            if (checkBox4.Checked)
            {
                ini = dateTimePicker1.Value;
                fim = dateTimePicker2.Value;
            }
            progressBar1.Maximum = list.Count;
            progressBar1.Value = 0;
            foreach (string l in list)
            {
                Item item2 = new Item();
                item2.Open(l.Split(VarDash)[0]);
                string locked = item2.stream("lock");
                if(locked == "true")
                {
                    progressBar1.Value++;
                    continue;
                }

                scrapF = (float)(item2.getScraps(ini, fim));
                if (checkBox5.Checked)
                {
                    turnLinhaF = (float)Convert.ToInt32(label25.Text);
                }
                else
                {
                    turnLinhaF = 0;
                }
                cycle = placasProduzidasF / (scrapF - turnLinhaF);
                qtdF = (float)Convert.ToInt32(item.QTD("get"));
                forecastF = (float)Convert.ToInt32(textBox16.Text);
                forecast = textBox16.Text;


                qtdFimF = forecastF / cycle;
                qtdCompraF = qtdFimF - qtdF;

                scraps = scrapF.ToString("#.##");
                cycleLife = cycle.ToString("#.##");
                qtdCompra = qtdCompraF.ToString("#.##");
                qtdFIM = qtdFimF.ToString("#.##");

                placasProduzidas = textBox1.Text;



                item2.stream("QTDcompra", qtdCompra);
                item2.stream("QTDFinalizar", qtdFIM);
                item2.stream("PlacasProd", placasProduzidas);
                item2.stream("Forcast", forecast);
                item2.stream("dataDoCalculo", data);
                item2.stream("inicioIntervalo", inicioIntervalo);
                item2.stream("fimIntervalo", fimIntervalo);
                item2.stream("CycleLife", cycleLife);

                item2.Close();
                progressBar1.Value++;
            }
            progressBar1.Value = progressBar1.Maximum;

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                groupBox2.Enabled= true;
            }
            else
            {
                groupBox2.Enabled= false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                groupBox1.Enabled= true;
                groupBox3.Enabled= false;
                textBox16.Text = textBox2.Text;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                loadForecast();
                loadYear(label4.Text);
                checkBox1.Checked = false;
                groupBox3.Enabled= true;
                groupBox1.Enabled = false;
                //atualizar forecast

            }
        }
        public int getTurnsLinha(string grup)
        {
            int outLine = 0;
            foreach (string l in GruposList.ToList())
            {
                if (l.StartsWith("CN" + VarDash.ToString() + grup + VarDashPlus.ToString()))
                {
                    outLine = 0;
                    List<string> list = new List<string>();
                    list = l.Split(VarDashPlus).ToList();
                    list.RemoveAt(0);
                    foreach (string tp in list)
                    {
                        if (tp.Split(VarDash)[1] == "OUT")
                        {
                            outLine++;
                        }
                    }
                    break;
                }
            }
            return outLine;

        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                label25.Text = getTurnsLinha(comboBox1.Text).ToString();
                groupBox16.Enabled = true;
                checkBox5.ForeColor = Color.FromArgb(20, 20, 20);
            }
            else
            {
                groupBox16.Enabled = false;
                checkBox5.ForeColor = Color.Gray;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label25.Text = getTurnsLinha(comboBox1.Text).ToString();
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
