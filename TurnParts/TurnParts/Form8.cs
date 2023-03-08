using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MagnusSpace
{
    public partial class Form8 : Form
    {
        char VarDash = ((char)887);
        char VarDashPlus = ((char)888);
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
        List<string> GruposList = new List<string>();
        public string lastLoadedMode = "HISTORICO";
        bool trig = true;
        public Form8()
        {
            Form9 form = new Form9();
            form = Application.OpenForms["Form9"] as Form9;
            cn = form.cn;
            qtd = form.qtd;
            qtdCompra = form.qtdCompra;
            qtdFIM = form.qtdFIM;
            placasProduzidas = form.placasProduzidas;
            forecast = form.forecast;
            data = form.data;
            turnsLinha = form.turnsLinha;
            scraps = form.scraps;
            inicioIntervalo = form.inicioIntervalo;
            fimIntervalo = form.fimIntervalo;
            cycleLife = form.cycleLife;
            InitializeComponent();
        }
        public void save()
        {
            Item item = new Item();
            item.Open(cn);
            item.stream("QTDcompra", qtdCompra);
            item.stream("QTDFinalizar", qtdFIM);
            item.stream("PlacasProd", placasProduzidas);
            item.stream("Forcast", forecast);
            item.stream("dataDoCalculo", data);
            item.stream("inicioIntervalo", inicioIntervalo);
            item.stream("fimIntervalo", fimIntervalo);
            item.stream("CycleLife", cycleLife);
            item.Close();
        }
        public void clearLabels()
        {
            label19.Text = "";
            label20.Text = "";
            label21.Text = "";
            label22.Text = "";
            label23.Text = "";
            label24.Text = "";
            label25.Text = "";
            label26.Text = "";
            label28.Text = "";
            label29.Text = "";
            label30.Text = "";
        }
        public void loadVars()
        {
            label19.Text = forecast;
            label20.Text = placasProduzidas;
            label21.Text = qtdCompra;
            label22.Text = qtd;
            label23.Text = inicioIntervalo;
            label24.Text = scraps;
            label25.Text = turnsLinha;
            label26.Text = data;
            label28.Text = cycleLife;
            label30.Text = qtdFIM;
            label29.Text = fimIntervalo;
        }
        public void enablePanel(string panel)
        {
            switch (panel)
            {
                case "HISTORICO":
                    lastLoadedMode = panel;
                    trig = false;
                    groupBox6.Enabled= false;
                    groupBox2.Enabled = false;
                    groupBox5.Enabled = false;
                    groupBox1.Enabled = true;
                    checkBox4.Checked= false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    trig = true; 
                    textBox1.Focus();
                    break;
                case "INTERVALO":
                    lastLoadedMode = panel;
                    trig = false;
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox5.Enabled = false;
                    groupBox6.Enabled = true;
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    trig = true;
                    textBox7.Focus();
                    break;
                case "PROJEÇÃO":
                    lastLoadedMode = panel;
                    trig = false;
                    groupBox1.Enabled = false;
                    groupBox6.Enabled = false;
                    groupBox5.Enabled = false;
                    groupBox2.Enabled = true;
                    checkBox1.Checked = false;
                    checkBox4.Checked = false;
                    checkBox3.Checked = false;
                    trig = true;
                    textBox2.Focus();
                    break;
                case "EMPIRICO":
                    lastLoadedMode = panel;
                    trig = false;
                    groupBox6.Enabled = false;
                    groupBox2.Enabled = false;
                    groupBox1.Enabled = false;
                    groupBox5.Enabled = true;
                    checkBox4.Checked = false;
                    checkBox2.Checked = false;
                    checkBox1.Checked = false;
                    trig = true;
                    textBox6.Focus();
                    break;
            }
        }
        public int scrapsRange(string inicio, string fim)
        {
            int scrapsR = 1;
            return scrapsR;
        }
        private void Form8_Load(object sender, EventArgs e)
        {
            int offset = groupBox7.Width/2 - label27.Width/2;
            int offsety = label27.Height / 2;
            label27.Location = new Point(groupBox7.Location.X + offset, groupBox7.Location.Y);
            enablePanel(lastLoadedMode);
            clearLabels();
            loadVars();
            Item item = new Item();
            item.Open(cn);
            string valeu = item.stream("lock");
            if (valeu == "true")
            {
                bloquearToolStripMenuItem.Text = "Desbloquear";
                label11.Visible = true;
            }
            else
            {
                bloquearToolStripMenuItem.Text = "Bloquear";
                label11.Visible = false;
            }
            Grupo gp = new Grupo();

            GruposList = gp.allGrupos();
            
            List<string> gps = new  List<string>();
            foreach(string l2 in GruposList.ToList())
            {
                gps.Add(l2.Split(VarDashPlus)[0].Split(VarDash)[1]);
            }
            comboBox1.DataSource = gps;

            Item item2 = new Item();
            item2.Open(cn);
            string fc = item2.getForecast().ToString();
            textBox3.Text = fc;
            textBox5.Text = fc;
            textBox4.Text = fc;

        }
        public int getTurnsLinha(string grup)
        {
            int outLine = 0;
           foreach(string l in GruposList.ToList())
            {
                if (l.StartsWith("CN" + VarDash.ToString() + grup + VarDashPlus.ToString()))
                {
                    outLine = 0;
                    List<string> list = new List<string>();
                    list = l.Split(VarDashPlus).ToList();
                    list.RemoveAt(0);
                    foreach(string tp in list)
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
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked && trig)
            {
                enablePanel("HISTORICO");
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked && trig)
            {
                enablePanel("INTERVALO");
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked && trig)
            {
                enablePanel("PROJEÇÃO");
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked && trig)
            {
                enablePanel("EMPIRICO");
            }
            
        }

        private void oPÇÔESToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            salvarToolStripMenuItem.Text = "Salvar";
            float cycle = 0;
            float qtdF = 0;
            float qtdFimF = 0;
            float qtdCompraF = 0;
            float scrapF = 0;
            float placasProduzidasF = 0;
            float forecastF = 0;
            float turnLinhaF = 0;
            int sc = 0;
            if (turnsLinha == "")
                turnsLinha = "0";
            qtdCompra = "";
            placasProduzidas= "0";
            forecast = "0";
            qtdFIM= "0";
            data = "0";
            inicioIntervalo= "";
            fimIntervalo= "";
            cycleLife = "";
                
            switch (lastLoadedMode)
            {

                case "HISTORICO":
                    try
                    {
                        placasProduzidas = Convert.ToInt32(textBox1.Text).ToString();
                    }
                    catch(Exception ex) { MessageBox.Show(ex.Message); }
                    try
                    {
                        forecast = Convert.ToInt32(textBox3.Text).ToString();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    sc = Convert.ToInt32(scraps);
                    data = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString().Split(' ')[0];
                    inicioIntervalo = "NPI";
                    string time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString();
                    time = time.Split(' ')[0];
                    fimIntervalo = time;
                    scrapF = (float)(sc);
                    placasProduzidasF = (float)Convert.ToInt32(placasProduzidas);
                    if (checkBox5.Checked)
                    {
                        turnLinhaF = (float)Convert.ToInt32(label25.Text);
                    }
                    else
                    {
                        turnLinhaF = 0;
                    }
                    
                    cycle = placasProduzidasF / (scrapF - turnLinhaF);
                    qtdF = (float)Convert.ToInt32(qtd);
                    forecastF = (float)Convert.ToInt32(forecast);
                    qtdFimF = forecastF / cycle;
                    qtdCompraF = qtdFimF - qtdF;
                    turnsLinha = turnLinhaF.ToString();
                    scraps = scrapF.ToString("#.##");
                    cycleLife = cycle.ToString("#.##");
                    qtdCompra = qtdCompraF.ToString("#.##");
                    qtdFIM = qtdFimF.ToString("#.##");

                    clearLabels();
                    loadVars();
                    
                    break;
                case "INTERVALO":
                    //FilterLogList()
                    try
                    {
                        placasProduzidas = Convert.ToInt32(textBox7.Text).ToString();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    try
                    {
                        forecast = Convert.ToInt32(textBox5.Text).ToString();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }



                    data = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString().Split(' ')[0];

                    DateTime dtini = dateTimePicker1.Value;
                    DateTime dtfim = dateTimePicker2.Value;
                    inicioIntervalo = TimeZoneInfo.ConvertTime(dtini, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString().Split(' ')[0];
                    fimIntervalo = TimeZoneInfo.ConvertTime(dtfim, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString().Split(' ')[0];

                    Item item = new Item();
                    item.Open(cn);
                    sc = item.getScraps(dtini,dtfim);

                    //sc = scrapsRange(inicioIntervalo,fimIntervalo);
                    scrapF = (float)(sc);
                    placasProduzidasF = (float)Convert.ToInt32(placasProduzidas);
                    if (checkBox5.Checked)
                    {
                        turnLinhaF = (float)Convert.ToInt32(label25.Text);
                    }
                    else
                    {
                        turnLinhaF = 0;
                    }
                    cycle = placasProduzidasF / (scrapF - turnLinhaF);
                    qtdF = (float)Convert.ToInt32(qtd);
                    forecastF = (float)Convert.ToInt32(forecast);
                    qtdFimF = forecastF / cycle;
                    qtdCompraF = qtdFimF - qtdF;
                    turnsLinha = turnLinhaF.ToString();
                    scraps = scrapF.ToString("#.##");
                    cycleLife = cycle.ToString("#.##");
                    qtdCompra = qtdCompraF.ToString("#.##");
                    qtdFIM = qtdFimF.ToString("#.##");
                    clearLabels();
                    loadVars();



                    break;
                case "PROJEÇÃO":

                    try
                    {
                        cycleLife = float.Parse(textBox2.Text.Replace('.', ',')).ToString();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    try
                    {
                        forecast = Convert.ToInt32(textBox4.Text).ToString();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    data = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString().Split(' ')[0];

                   
                    //placasProduzidasF = (float)Convert.ToInt32(placasProduzidas);
                    if (checkBox5.Checked)
                    {
                        turnLinhaF = (float)Convert.ToInt32(label25.Text);
                    }
                    else
                    {
                        turnLinhaF = 0;
                    }
                    cycle = float.Parse(cycleLife.Replace('.',','));
                    qtdF = (float)Convert.ToInt32(qtd);
                    forecastF = (float)Convert.ToInt32(forecast);
                    qtdFimF = forecastF / cycle - turnLinhaF;
                    qtdCompraF = qtdFimF - qtdF;
                    turnsLinha = turnLinhaF.ToString();
                    cycleLife = cycle.ToString("#.##");
                    qtdCompra = qtdCompraF.ToString("#.##");
                    qtdFIM = qtdFimF.ToString("#.##");
                    clearLabels();
                    loadVars();


                    break;
                case "EMPIRICO":
                    try
                    {
                        cycleLife = float.Parse(textBox6.Text.Replace('.', ',')).ToString();
                        Console.WriteLine(cycleLife);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                    data = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString().Split(' ')[0];

                    cycle = float.Parse(cycleLife);
                    scraps = scrapF.ToString("#.##");
                    cycleLife = cycle.ToString("#.##");
                    clearLabels();
                    loadVars();
                    break;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                label25.Text = getTurnsLinha(comboBox1.Text).ToString();
                groupBox9.Enabled = true;
                checkBox5.ForeColor = Color.FromArgb(20,20,20) ;
            }
            else
            {
                groupBox9.Enabled= false;
                checkBox5.ForeColor = Color.Gray;
            }
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
            salvarToolStripMenuItem.Text = "Salvo";
        }

        private void groupBox9_Enter(object sender, EventArgs e)
        {

        }

        private void bloquearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Item item = new Item();
            item.Open(cn);
            string valeu = item.stream("lock");
            if(valeu == "true")
            {
                item.stream("lock","false");
                label11.Visible = false;
                bloquearToolStripMenuItem.Text = "Bloquear";
            }
            else
            {
                item.stream("lock", "true");
                label11.Visible = true;
                bloquearToolStripMenuItem.Text = "Desbloquear";
            }
            item.Close();
            
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label25.Text = getTurnsLinha(comboBox1.Text).ToString();
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }
    }
}
