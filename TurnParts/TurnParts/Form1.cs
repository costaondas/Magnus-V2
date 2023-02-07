using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MagnusSpace;
using System.Diagnostics;
using System.Management;
using System.Windows;
using System.IO.Ports;


namespace TurnParts
{
    public delegate void myDelegate();
    public partial class Form1 : Form
    {
        public event myDelegate resizePanel;
        public event myDelegate updatePanels;
        public event myDelegate hideLogPanelLabels;
        public event myDelegate deleteChartPanel;
        public event myDelegate disposeChartLabels;
        public event myDelegate reapearChartpanel;
        public event myDelegate relocateBut;
        public event myDelegate revertPanelColor;
        public event myDelegate deleteItensPanel;
        public event myDelegate reapearItempanel;
        public event myDelegate deleteItemPanel;
        public event myDelegate butDispose;
        public event myDelegate listbuttons;
        public event myDelegate resizeMissin;
        public event myDelegate switchtem;
        public event myDelegate showform1;
        public event myDelegate shutBlinkQTD;
        public event myDelegate shutBlinkVersao;
        public event myDelegate shutBlinkStatus;
        public event myDelegate makeitVisible;
        public event myDelegate invisibleLabels;
        public event myDelegate loadAllCNs;
        public event myDelegate closeSearchForm;
        public event myDelegate addItemlogPanelButtonDispose;

        SerialPort mainPort = new SerialPort("COM9", 9600);
        string lastCOM = "";

        bool burlarLicensa = true;

        bool habilitarLicensa = true;
        bool licenceFound = false; //false
        private string[] keysMaster = { @"USB\VID_14CD&PID_1212\121220160204",
            @"USB\VID_18F8&PID_0F99\6&117AE5A&0&4",
            @"USB\VID_18F8&PID_0F99\5&20BDEAD2&0&3",
            @"USB\VID_C031&PID_3412\5928271398D679F4",
            @"USB\VID_05E3&PID_0751\6&117AE5A&0&3" };

        public Form1()
        {
            InitializeComponent();
            ganarateLogPanels();
            cycleLifeLayouy("hide");
            timer1.Interval = 100;
            timer1.Start();
            timer2.Interval = 50;
            timer2.Start();
            timer3.Start();
            int b = 0;
            foreach (bool a in OIpins)
            {
                OIpins[b] = false;
            }
            loadPlacasProduzidas();
            // Folders f = new Folders();
            //itensMissing(f.missingList(), missingConfig);
        }
        bool isITdone = false;
        bool zoom = false;
        public static bool callDisplayBoll = false;
        public static string callDisplayCN = "";
        int[] picdefaultSize = { 0, 0 };
        bool[] OIpins = new bool[8];
        Point p11 = new Point(0, 0);
        Point p12 = new Point(0, 0);
        Point p21 = new Point(0, 0);
        Point p22 = new Point(0, 20);
        Point p31 = new Point(0, 0);
        Point p32 = new Point(0, 0);
        int[] missingConfig = { 7, 2 };
        int currentSelectedLogPanel = 1;
        bool ChartPanelVisible = true;
        bool PI = true;
        public string searchButtonReturnTo = "";
        public string scrapSemanalListPath = "";
        StringComparison comp = StringComparison.OrdinalIgnoreCase;
        private List<string> modelsFixturePatterns = new List<string>();
        private List<string> idPatterns = new List<string>();
        private List<string> prefixs = new List<string>(); // lista de preixos para comando de local
        private List<string> settingsList = new List<string>();
        public static List<string> CNList = new List<string>();
        public List<string> ModelList = new List<string>();
        public List<string> PlascasProduzidas = new List<string>();
        public static List<string> searchList = new List<string>();
        string currentCN = "";
        string lastCN = "!!!!!!!!";
        int missing_page = 1;
        string lastCharsScreem = "";
        /// <settings>////////////////////
        int proporcao_do_primeiro_painel_em_porcentagem = 65;
        int proporcao_do_segundo_painel_em_porcentagem = 65;
        int proporcao_do_terceiro_painel_em_porcentagem = 60;
        int textbox1_offset = 80;
        int description_font_size = 18;
        bool chartLabelVisible = false;
        char VarDash = ((char)887);
        char VarDashPlus = ((char)888);
        bool group_layout = false;
        bool loadGroupsOnBoot = true;
        int chartW = 300;
        int chartH = 200;
        int chartLabel_offset = -50;
        public static bool loadNewSearch = false;
        public static string searchText = "";
        public string COM_port = "COM6";
        public static DateTime time1 = DateTime.Now;
        public static DateTime time2 = DateTime.Now;

        ///////////////////////////// 

        private void validade_License()
        {
            if (burlarLicensa)
            {
                licenceFound = true;
                return;
            }
            if (!habilitarLicensa) //false = não checa licensa
            {
                licenceFound = true;
                panel2.BackColor = Color.Pink;
                return;
            }
            var usbDevices = GetUSBDevices();

            foreach (var usbDevice in usbDevices)
            {

                licenceFound = false;
                foreach (var keys in keysMaster)
                {

                    if (usbDevice.DeviceID == keys)
                    {

                        licenceFound = true;
                        break;
                    }

                }
                if (licenceFound)
                {
                    break;
                }


            }
        }
        public void IO(int outNumber, string state)
        {
            string sulfix = "\n";
            string comand = "out";
            comand += outNumber.ToString();
            if (state == "low" || state == "LOW")
            {
                OIpins[outNumber - 1] = false;
                //comand += "low"+ sulfix;
            }
            if (state == "high" || state == "HIGH")
            {
                OIpins[outNumber - 1] = true;
            }
        }
        public void sendIO()
        {
            string tosend = "";
            foreach (bool a in OIpins)
            {
                if (a)
                {
                    tosend += "1";
                }
                else
                {
                    tosend += "0";
                }
            }
            tosend += "\n";


            if (mainPort.IsOpen)
            {

                mainPort.Write(tosend);
                // MessageBox.Show(comand);
            }
        }
        public void IO_mode(string mode)
        {
            switch (mode)
            {
                case "REMOVE":
                    IO(1, "LOW");
                    IO(2, "HIGH");
                    IO(3, "HIGH");
                    sendIO();

                    break;
                case "CHECK":
                    IO(1, "HIGH");
                    IO(2, "LOW");
                    IO(3, "HIGH");
                    sendIO();
                    break;
                case "ADD":
                    IO(1, "HIGH");
                    IO(2, "HIGH");
                    IO(3, "LOW");
                    sendIO();
                    break;
                case "IDLE":
                    IO(1, "LOW");
                    IO(2, "LOW");
                    IO(3, "LOW");
                    sendIO();
                    break;
                case "ALARME":
                    alarmBuzz();
                    break;

            }
        }

        //configuração
        public string config(string varName, string value = "null")
        {
            ListClass list2 = new ListClass();
            list2.Open("Settings", "settings");
            string text = "";
            text = list2.stream(varName, value);
            list2.Close();
            return text;

        }
        //private 

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void print(string qtd = "",string qtdLabels = "1")
        {
            string printString = "";
            Folders f = new Folders();
            string path = "";
            path = config("printAdress");
            if (!Directory.Exists(path))
            {
                path = config("printAdress", f.printPath);
            }
            ListClass list2 = new ListClass();
            list2.Open(DateTime.Now.ToString().Replace(':',' ').Replace('/','_') +" " +RandomString(8), path);


           

            ListClass lc = new ListClass();
            lc.Open("Mestra", "ListaGeral");

            int a = 0;
            foreach (string l in lc.mainList.ToList())
            {
                List<string> subList = l.Split(VarDashPlus).ToList();
                foreach (string l2 in subList)
                {
                    if (l2.Split(VarDash)[0] == "CN" && l2.Split(VarDash)[1] == currentCN)
                    {
                        printString = l;

                  
                        break;
                    }
                }
                a++;
            }
            int b = 0;
            if(qtd != "")
            {
                try
                {
                    b = Convert.ToInt32(qtd);
                }
                catch
                {
                    return;
                }
            }
            try
            {
                Convert.ToInt32(qtdLabels);
            }
            catch
            {
                qtdLabels = 1.ToString();
            }

            printString += VarDashPlus.ToString() + "printQTD" + VarDash.ToString() + qtd + VarDashPlus.ToString() + "qtdLabels" + VarDash.ToString() + qtdLabels;
            list2.mainList.Add(printString);
            list2.Close();


            
           

        }
        public void loadForm(object Form)
        {
            if (this.panel8.Controls.Count > 0)
            {
                this.panel8.Controls.RemoveAt(0);
            }
            if (closeSearchForm != null)
            {
                closeSearchForm();
            }
            Form f = Form as Form;
            closeSearchForm += () =>
            {
                f.Close();
            };
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            panel8.Controls.Add(f);
            panel8.Tag = f;

            f.Show();
        }

        private void code(string code = "")
        {
            loadForm(new Form6());
            return;

        }
        bool blinkQTD = false;
        bool blinkStatus = false;
        bool blinkVersao = false;
        private void loadAllFGroups()
        {
            return;
            if (!loadGroupsOnBoot)
            {
                return;
            }
            ListClass lc = new ListClass();
            bool itemPresente = false;
            string cn_Atual = "";
            int b = 0;
            string line = "";
            foreach (string l3 in CNList.ToList())
            {
                Item item = new Item();
                cn_Atual = l3.Split(vd())[0];
                itemPresente = false;
                item.grupo = "";
                item.Open(cn_Atual);

                if (item.grupo != "")
                {
                    lc.Open(item.grupo, "Grupo");
                    b = 0;
                    foreach (string item_naLista_do_grupo in lc.mainList.ToList()) //lista do que ta no grupo
                    {
                        if (item_naLista_do_grupo.Split(VarDash)[0] == cn_Atual)
                        {
                            line = "";
                            line += cn_Atual + VarDash.ToString();
                            line += item_naLista_do_grupo.Split(VarDash)[1].ToString() + VarDash.ToString();
                            line += item.ItemPN + VarDash.ToString();
                            line += item.Item_description + VarDash.ToString();
                            line += item.location + VarDash.ToString();
                            line += item.respValidação + VarDash.ToString();
                            line += item.dataVistoria + VarDash.ToString();
                            line += item.proxVistoria;
                            lc.mainList[b] = line;
                            itemPresente = true;
                            break;
                        }
                        b++;
                    }
                    if (!itemPresente)
                    {
                        line = "";
                        line += cn_Atual + VarDash.ToString();
                        line += "IN" + VarDash.ToString();
                        line += item.ItemPN + VarDash.ToString();
                        line += item.Item_description + VarDash.ToString();
                        line += item.location + VarDash.ToString();
                        line += item.respValidação + VarDash.ToString();
                        line += item.dataVistoria + VarDash.ToString();
                        line += item.proxVistoria;
                        lc.mainList.Add(line);
                    }
                    lc.Close();
                }
                else
                {
                    item.Close();
                    continue;
                }
                item.Close();
            }

        }
        private async void alarmBuzz()
        {
            IO(4, "HIGH");
            sendIO();
            await Task.Delay(1000);
            IO(4, "LOW");
            sendIO();

        }
        private async void stopBlink()
        {
            await Task.Delay(500);
            if (!blinkQTD)
                labelQTD_value.ForeColor = Color.White;
            if (!blinkStatus)
                label12.ForeColor = Color.White;
            if (!blinkVersao)
                label12.ForeColor = Color.BlanchedAlmond;
        }
        private async void BlinkQTD(string mode = "ON")
        {
            if (shutBlinkQTD != null)
            {
                shutBlinkQTD();
            }
            if (mode == "ON")
                blinkQTD = true;

            while (blinkQTD)
            {
                await Task.Delay(500);
                //labelQTD_value.ForeColor = labelQTD_value.ForeColor == Color.White ? Color.Red : Color.White;
                labelQTD_value.ForeColor = labelQTD_value.ForeColor == Color.Red ? Color.White : Color.Red;
                shutBlinkQTD += () =>
                {
                    blinkQTD = false;
                    labelQTD_value.ForeColor = Color.White;

                    return;
                };
            }
        }
        private async void BlinkStatus(string mode = "ON")
        {
            if (shutBlinkStatus != null)
            {
                shutBlinkStatus();
            }
            if (mode == "ON")
                blinkStatus = true;

            while (blinkStatus)
            {
                await Task.Delay(500);
                label12.ForeColor = label12.ForeColor == Color.White ? Color.Red : Color.White;
                shutBlinkQTD += () =>
                {
                    blinkStatus = false;
                    label12.ForeColor = Color.White;
                    return;

                };

            }
        }

        private async void BlinkVersao(string mode = "ON")
        {
            if (shutBlinkVersao != null)
            {
                shutBlinkVersao();
            }
            if (mode == "ON")
                blinkVersao = true;

            while (blinkVersao)
            {
                await Task.Delay(500);
                label59.ForeColor = label59.ForeColor == Color.White ? Color.Red : Color.White;
                shutBlinkVersao += () =>
                {
                    blinkVersao = false;
                    label59.ForeColor = Color.BlanchedAlmond;
                    return;

                };

            }
        }

        static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new USBDeviceInfo(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
            return devices;
        }


        class USBDeviceInfo
        {
            public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
            {
                this.DeviceID = deviceID;
                this.PnpDeviceID = pnpDeviceID;
                this.Description = description;
            }
            public string DeviceID { get; private set; }
            public string PnpDeviceID { get; private set; }
            public string Description { get; private set; }
        }



        List<string> LastChart = new List<string>();
        private void load_panel_layout()
        {
            if (group_layout)
            {
                label2.AutoSize = true;
                label12.Visible = true;
                progressBar1.Visible = true;
                label2.Font = new Font("Times New Roman", 20);
                label2.Size = new Size(panel5.Width, label2.Height);
            }
            else
            {
                label2.Font = new Font("Times New Roman", 13);
                label2.Text = "Quantidade";
                label2.AutoSize = true;
                label12.Visible = true;
                progressBar1.Visible = true;
            }
            int[] colunas = new int[2] { 30, 30 };
            int linha1 = proporcao_do_primeiro_painel_em_porcentagem;//60
            int linha2 = proporcao_do_segundo_painel_em_porcentagem;
            int linha3 = proporcao_do_terceiro_painel_em_porcentagem;
            panel1.Size = new Size(this.Size.Width, this.Size.Height);
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int totalWidth = screenWidth;//panel1.Size.Width;
            int totalHeight = panel1.Size.Height;
            int hc = 1; // heightcorrection
            p11 = new Point(panel1.Location.X, panel1.Location.Y - menuStrip1.Size.Height);
            p12 = new Point(p11.X, p11.Y + (totalHeight * linha1) / 100);
            p21 = new Point(p11.X + (totalWidth * colunas[0]) / 100, p11.Y);
            p22 = new Point(p21.X, p21.Y + (totalHeight * linha2) / 100);
            p31 = new Point(p11.X + (totalWidth * (colunas[0] + colunas[1])) / 100, p11.Y);
            p32 = new Point(p31.X, p31.Y + (totalHeight * linha3) / 100);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(panel6);
            panel1.Controls.Add(panel7);
            panel2.Location = p11;
            panel3.Location = p12;
            panel4.Location = p21;
            panel5.Location = p22;
            panel6.Location = p31;
            panel7.Location = p32;
            panel2.Size = new Size(p21.X - p11.X, p12.Y - p11.Y);
            panel3.Size = new Size(p21.X - p11.X, (totalHeight * (100 - linha1)) / 100 + hc);
            panel4.Size = new Size(p31.X - p21.X, p22.Y - p21.Y);
            panel5.Size = new Size(p31.X - p21.X, (totalHeight * (100 - linha2)) / 100 + hc);
            panel6.Size = new Size(totalWidth - panel2.Width - panel4.Width, p32.Y - p31.Y);
            panel7.Size = new Size(totalWidth * (100 - (colunas[0] + colunas[1])), (totalHeight * (100 - linha3)) / 100 + hc);
            panel5.Controls.Add(labelQTD_value);
            panel5.Controls.Add(label2);

            label2.BringToFront();
            resize_pic();
            int lab1_wid = panel2.Width;


            label10.Size = new Size(lab1_wid, label10.Height);
            label10.Location = new Point(0, 10);//new Point(p11.X,p11.Y);
            label10.Font = new Font("Times New Roman", 30);//40
            label1.Size = new Size(lab1_wid, label1.Height);
            label1.Location = new Point(label10.Location.X + 10, label10.Location.Y + label10.Height);//new Point(p11.X,p11.Y);
            Point[] array = new Point[2] { new Point(0, 0), new Point(0, 0) };
            array = schrink(panel2.Size, 90, 90, "widthOnly");
            //textBox2.Location = array[0];
            int text2Y = pictureBox1.Location.Y + pictureBox1.Size.Height + 10;
            textBox2.Location = new Point(pictureBox1.Location.X, text2Y);

            textBox2.Size = new Size(pictureBox1.Width, textBox2.Size.Height);
            panel2.Controls.Add(button13);
            button13.Location = new Point(textBox2.Location.X, textBox2.Location.Y + textBox2.Height);
            int shadow = panel2.Height - textBox2.Location.Y;
            int boxSize = 120;

            if (shadow < boxSize)
            {
                textBox2.Size = new Size(textBox2.Size.Width, shadow - 10);
            }
            else
            {
                textBox2.Size = new Size(textBox2.Size.Width, boxSize);
            }

            array = schrink(panel3.Size, 90, 90, "widthOnly");
            textBox1.Location = new Point(array[0].X, panel3.Size.Height - textbox1_offset);/////////////////////////
            //textBox1.Location = new Point(array[0].X, panel3.Size.Height - 80);
            int TextBheight = textBox1.Size.Height;
            textBox1.Size = new Size(array[1].X - array[0].X, TextBheight);
            button1.Size = new Size(textBox1.Size.Width, button1.Size.Height);
            button1.Location = new Point(textBox1.Location.X, textBox1.Location.Y - button1.Size.Height - 5);
            panel5.Controls.Add(label2);
            label2.Location = new Point(0, 0);

            if (resizePanel != null)
            {
                resizePanel();
            }
            panel6.Controls.Add(panel8);

            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(label43);
            groupBox1.Controls.Add(label44);
            groupBox1.Controls.Add(label45);
            groupBox1.Controls.Add(button15);
            groupBox1.Controls.Add(textBox7);
            groupBox1.Controls.Add(label59);
            textBox7.Height = button15.Height;
            int spc = 1;
            label11.Location = new Point(4, 15);
            label43.Location = new Point(label11.Location.X + label11.Width + spc, label11.Location.Y);
            label44.Location = new Point(label43.Location.X + label43.Width + spc, label11.Location.Y);
            label45.Location = new Point(label44.Location.X + label44.Width + spc, label11.Location.Y);

            button15.Location = new Point(label11.Location.X , label11.Location.Y + label11.Height + 5);
            textBox7.Location = new Point(button15.Location.X + button15.Width + 5, button15.Location.Y + button15.Height/2 - textBox7.Height/2);
            label59.Location = new Point(textBox7.Location.X + textBox7.Width + 5, button15.Location.Y);

            // label11.Size = new Size();
            int charPanelw = panel6.Width - 40;
            panel8.Size = new Size(charPanelw, (charPanelw * 9) / 16);
            //panel8.Location = new Point(0,0);


            panel6.Controls.Add(groupBox1);
            groupBox1.Size = new Size(charPanelw, 100);
            groupBox1.Location = new Point(panel6.Width / 2 - groupBox1.Width / 2, 5);

            panel8.Location = new Point(panel6.Width / 2 - panel8.Width / 2, groupBox1.Location.Y + groupBox1.Height + 5);//
            panel6.Controls.Add(button2);
            panel6.Controls.Add(button3);
            int ButHeight = 50;
            button2.Location = new Point(panel8.Location.X, panel8.Location.Y + panel8.Size.Height);
            button2.Size = new Size(panel8.Size.Width / 2, ButHeight);
            button3.Location = new Point(panel8.Location.X + panel8.Width / 2, panel8.Location.Y + panel8.Size.Height);
            button3.Size = new Size(panel8.Size.Width / 2, ButHeight);
            if (relocateBut != null)
            {
                relocateBut();
            }
            panel4.Controls.Add(label3);
            panel4.Controls.Add(label4);
            panel4.Controls.Add(label5);
            panel4.Controls.Add(label6);
            panel4.Controls.Add(label7);
            panel4.Controls.Add(label8);
            panel4.Controls.Add(label13);
            panel4.Controls.Add(label14);
            panel4.Controls.Add(label15);
            panel4.Controls.Add(label16);
            panel4.Controls.Add(label27);
            panel4.Controls.Add(label28);
            panel4.Controls.Add(label29);
            panel4.Controls.Add(label30);
            panel4.Controls.Add(label33);
            panel4.Controls.Add(label34);

            LabInfoLoca();

            //panel8.Controls.Add(dateTimePicker1);
            //panel8.Controls.Add(dateTimePicker2);
           // panel8.Controls.Add(button4);
            //panel8.Controls.Add(button5);



            if (label8.Text != "" || label8.Text != "0")
            {
                label7.ForeColor = Color.Salmon;
                label8.ForeColor = Color.Salmon;
            }

           // dateTimePicker1.Font = new Font("Times New Roman", 20);
           // dateTimePicker2.Font = new Font("Times New Roman", 20);
            //dateTimePicker1.Size = new Size(50, dateTimePicker1.Size.Height);
            //dateTimePicker2.Size = new Size(50, dateTimePicker2.Size.Height);
         
            //label24.Text = "None";
            //label25.Text = "None";
            //label26.Text = "None";
            panel9.Controls.Add(label46);
            panel9.Controls.Add(label47);
            panel9.Controls.Add(label48);
            panel9.Controls.Add(label49);
            panel9.Controls.Add(label50);
            panel9.Controls.Add(label51);
            panel9.Controls.Add(label52);
            label48.Location = new Point(chart2.Location.X, chart2.Location.Y + chart2.Height + chartLabel_offset);
            label49.Location = new Point(chart3.Location.X, chart3.Location.Y + chart3.Height + chartLabel_offset);
            label50.Location = new Point(chart1.Location.X, chart1.Location.Y + chart1.Height + chartLabel_offset);

            label46.Location = new Point(label48.Location.X, label48.Location.Y + label48.Height);
            label47.Location = new Point(label49.Location.X, label49.Location.Y + label49.Height);

            label51.Location = new Point(label50.Location.X, label46.Location.Y + label46.Height + 10);
            label52.Location = new Point(label51.Location.X + label51.Width + 10, label51.Location.Y);

            int gap1 = 0;

           

            // button11.Location = new Point(label23.Location.X, label23.Location.Y + label23.Height);

            panel8.Controls.Add(groupBox2);
            groupBox2.Location = new Point(10, panel8.Height - groupBox2.Height - 10);




            //dateTimePicker1.Location = new Point(0, button11.Location.Y + dateTimePicker1.Height);
           //dateTimePicker2.Location = new Point(dateTimePicker1.Location.X, dateTimePicker1.Location.Y + dateTimePicker1.Height);
           // button4.Location = new Point(dateTimePicker1.Location.X + dateTimePicker1.Width, dateTimePicker1.Location.Y);
          //  button5.Location = new Point(dateTimePicker2.Location.X + dateTimePicker2.Width, dateTimePicker2.Location.Y);
            panel3.Controls.Add(button6);
            panel3.Controls.Add(button7);
            button6.Size = new Size(button1.Width / 2, button6.Height);
            button7.Size = new Size(button1.Width / 2, button7.Height);
            button6.Location = new Point(button1.Location.X, button1.Location.Y - button6.Height - 3);
            panel3.Controls.Add(button14_EXTRAIR_LISTA_DO_MODELO);
            panel3.Controls.Add(button14_EXTRAIR_TODOS_LOGS);
            int spaceBetwwemButtonPrcima = 4;
            int spacetothesideofbuttons = 4;
            button14_EXTRAIR_LISTA_DO_MODELO.Location = new Point(button6.Location.X, button6.Location.Y - button14_EXTRAIR_LISTA_DO_MODELO.Height - spaceBetwwemButtonPrcima);
            button14_EXTRAIR_TODOS_LOGS.Location = new Point(button14_EXTRAIR_LISTA_DO_MODELO.Location.X + button14_EXTRAIR_TODOS_LOGS.Width + spacetothesideofbuttons, button14_EXTRAIR_LISTA_DO_MODELO.Location.Y);
            button7.Location = new Point(button1.Location.X + button6.Width, button6.Location.Y);
            panel7.Controls.Add(panel9);
            panel7.Controls.Add(button8);
            panel7.Controls.Add(button9);
            panel9.Controls.Add(chart1);
            panel9.Controls.Add(chart2);
            panel9.Controls.Add(chart3);
            label9.Location = new Point(panel9.Width / 2 - label9.Width / 2, 0);
            panel9.Location = new Point(0, label9.Height);
            chart1.Location = new Point(10, 10);
            chart2.Location = new Point(chart1.Location.X + chart1.Width, chart1.Location.Y);
            chart3.Location = new Point(chart2.Location.X + chart2.Width, chart2.Location.Y);
            label46.Font = new Font("Times New Roman", 12);
            label47.Font = new Font("Times New Roman", 12);
            label48.Font = new Font("Times New Roman", 12);
            label49.Font = new Font("Times New Roman", 12);
            label50.Font = new Font("Times New Roman", 12);
            label51.Font = new Font("Times New Roman", 12);
            label52.Font = new Font("Times New Roman", 12);
            float factor = 0;
            factor = chartW / chartH;
            int d = panel9.Width / 3;
            int c = Convert.ToInt32((d * factor));

            chart1.Size = new Size(d, c);
            chart2.Size = new Size(d, c);
            chart3.Size = new Size(d, c);
            button8.Size = new Size(panel6.Width / 2, 30);
            button9.Size = new Size(panel6.Width / 2, 30);
            int butGap = 10;
            panel9.Size = new Size(panel6.Width, panel7.Height - label9.Height - button8.Height - butGap);

            button8.Location = new Point(0, panel7.Height - button8.Height - butGap);
            button9.Location = new Point(button8.Width, panel7.Height - button8.Height - butGap);

            panel5.Controls.Add(label12);
            label12.Location = new Point(panel5.Width - label12.Width, progressBar1.Location.Y + progressBar1.Height);
            button14.Location = new Point(textBox1.Location.X + textBox1.Width + 3, textBox1.Location.Y);
            button14.Size = new Size(textBox1.Height, textBox1.Height);
            label12.Font = new Font("Times New Roman", 35);
            textBox2.Font = new Font("Times New Roman", description_font_size);
            panel6.Controls.Add(dateTimePicker3);
            panel6.Controls.Add(dateTimePicker4);
            panel3.Controls.Add(button14);
            dateTimePicker3.Location = new Point(0, 0);
            dateTimePicker4.Location = new Point(dateTimePicker3.Location.X + dateTimePicker3.Width, dateTimePicker3.Location.Y);
            if (currentCN == "")
            {
                turnPartToolStripMenuItem.Enabled = false;
            }
            int spacelabels = 5;
            panel3.Controls.Add(label57);
            panel3.Controls.Add(label58);
            panel3.Controls.Add(label35);
            panel3.Controls.Add(label36);
            panel3.Controls.Add(label37);
            panel3.Controls.Add(label38);
            panel3.Controls.Add(label39);
            panel3.Controls.Add(label40);

            label39.Location = new Point(button14_EXTRAIR_LISTA_DO_MODELO.Location.X, button14_EXTRAIR_LISTA_DO_MODELO.Location.Y - label39.Height - spacelabels);
            label37.Location = new Point(label39.Location.X, label39.Location.Y - label37.Height - spacelabels);
            label35.Location = new Point(label37.Location.X, label37.Location.Y - label35.Height - spacelabels);

            label57.Location = new Point(label35.Location.X, label35.Location.Y - label35.Height - spacelabels);
            label58.Location = new Point(panel3.Width - label58.Width, label57.Location.Y);



            label40.Location = new Point(panel3.Width - label40.Width, label39.Location.Y);
            label38.Location = new Point(panel3.Width - label38.Width, label37.Location.Y);
            label36.Location = new Point(panel3.Width - label36.Width, label35.Location.Y);

            label41.Location = new Point(button14_EXTRAIR_TODOS_LOGS.Location.X + button14_EXTRAIR_TODOS_LOGS.Width + spacelabels, label39.Location.Y + label41.Height + spacelabels);
            label42.Location = new Point(panel3.Width - label42.Width, label41.Location.Y);
            //panel7.Controls.Add(label9);
            //label9.Location = new Point(panel6.Width / 2 - label9.Width / 2, 0);

        } //layout do form
        private void loadNoItemLayout()
        {
            label12.Text = "";
            currentCN = "";
            label1.Text = "";
            label10.Text = "";
            labelQTD_value.Text = "";
            label2.Text = "";
            pictureBox1.Image = null;
            textBox2.Text = "";
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label13.Text = "";
            label14.Text = "";
            label15.Text = "";
            label16.Text = "";
            label27.Text = "";
            label28.Text = "";
            label29.Text = "";
            label30.Text = "";
           
       
            label35.Text = "";
            label36.Text = "";
            label37.Text = "";
            label38.Text = "";
            label39.Text = "";
            label40.Text = "";
            label41.Text = "";
            label42.Text = "";
            label43.Text = "";
            label44.Text = "";
            label45.Text = "";
            label11.Text = "";
            label46.Text = "";
            label47.Text = "";
            label48.Text = "";
            label49.Text = "";
            label50.Text = "";
            label51.Text = "";
            label52.Text = "";
            label57.Text = "";
            label58.Text = "";


            if (hideLogPanelLabels != null)
            {
                hideLogPanelLabels();
            }
        }
        public void cycleLifeLayouy(string action = "show")
        {
            switch (action)
            {
                case "show":
                   
                    button11.Visible = true;
                    groupBox2.Visible = true;
                    dateTimePicker1.Visible = true;
                    dateTimePicker2.Visible = true;
                    //button4.Visible = true;
                 //   button5.Visible = true;
                    break;
                case "hide":
                    
                    button11.Visible = false;
                    groupBox2.Visible = false;
                    dateTimePicker1.Visible = false;
                    dateTimePicker2.Visible = false;
                    button4.Visible = false;
                    button5.Visible = false;
                    
                    break;
            }
        }
        private void LabInfoLoca()
        {
            //label3.Location = new Point(0, label3.Height);
            label3.Location = new Point(0, 5);


            label4.Location = new Point(panel4.Width - label4.Width, label3.Location.Y);
            label5.Location = new Point(label3.Location.X, label3.Location.Y + label3.Height);
            label6.Location = new Point(panel4.Width - label6.Width, label5.Location.Y);
            label7.Location = new Point(label3.Location.X, label3.Location.Y + label3.Height + label5.Height);
            label8.Location = new Point(panel4.Width - label8.Width, label7.Location.Y);
            label13.Location = new Point(label3.Location.X, label3.Location.Y + label3.Height + label5.Height + label7.Height);
            label14.Location = new Point(panel4.Width - label14.Width, label13.Location.Y);
            label15.Location = new Point(label3.Location.X, label13.Location.Y + label15.Height);
            label16.Location = new Point(panel4.Width - label16.Width, label15.Location.Y);
            label27.Location = new Point(label3.Location.X, label15.Location.Y + label27.Height);
            label28.Location = new Point(panel4.Width - label28.Width, label27.Location.Y);
            label29.Location = new Point(label3.Location.X, label27.Location.Y + label29.Height);
            label30.Location = new Point(panel4.Width - label30.Width, label29.Location.Y);
            
            label33.Location = new Point(label3.Location.X, label29.Location.Y + label29.Height);
            label34.Location = new Point(panel4.Width - label34.Width, label33.Location.Y);

            //labelQTD_value.Location = new Point(panel5.Size.Width - labelQTD_value.Size.Width, label2.Size.Height);  //////////////////////////////
            qtdLabelLocation();
            progressBar1.Size = new Size(panel5.Width - 20, progressBar1.Height);
            progressBar1.Location = new Point((panel5.Width - progressBar1.Width) / 2, labelQTD_value.Location.Y + labelQTD_value.Height + 10);
        }
        public void qtdLabelLocation()
        {
            labelQTD_value.Location = new Point(panel5.Size.Width - labelQTD_value.Size.Width, label2.Size.Height);
        }
        private void resize_pic()
        {
            int picWidth = panel2.Width - 30;
            int proporcao_pic = 15; // 100 is all
            int[] pic_proportion = new int[2] { 16, 9 };
            // pictureBox1.Size = new Size(picWidth,(picWidth*9)/16 ) ;
            Point[] array = new Point[2] { new Point(0, 0), new Point(0, 0) };
            array = schrink(panel1.Size, 90, 90, "widthOnly");
            pictureBox1.Size = new Size(picWidth, (picWidth * 9) / 16);

            pictureBox1.Location = new Point(panel2.Width / 2 - pictureBox1.Size.Width / 2, label1.Location.Y + label1.Size.Height + 5);//20
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;


        } //layout da picturebox
        public Point[] schrink(Size s_, int dFactor, int schrink_f = 10, string mode = "all")
        {

            Size s = new Size();
            s = s_;
            Point a = new Point(0, (s.Height * dFactor) / 100);
            Point b = new Point(s.Width, s.Height);


            Point[] array = new Point[2] { new Point(0, 0), new Point(0, 0) };
            array[0] = a;
            array[1] = b;
            int Dx = array[1].X - array[0].X;
            int Dy = array[1].Y - array[0].Y;
            int dx = (Dx * schrink_f) / 100;
            int dy = (Dy * schrink_f) / 100;

            switch (mode)
            {
                case "all":
                    array[0] = new Point((Dx - dx) / 2, (Dy - dy) / 2);
                    array[1] = new Point(array[0].X + dx, array[1].Y + dy);
                    break;
                case "widthOnly":
                    array[0] = new Point((Dx - dx) / 2, a.Y);
                    array[1] = new Point(array[0].X + dx, b.Y);
                    break;
                case "heighOnly":
                    array[0] = new Point(a.X, (Dy - dy) / 2);
                    array[1] = new Point(b.X, array[1].Y + dy);
                    break;
            }
            return array;
        } //função para o layout

        private void filesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //verifica se existe planilha
        }

        public static void time(string text)
        {
            int a = 0;
            time2 = time1;
            time1 = DateTime.Now;
            TimeSpan ts = time1 - time2;
            a = Convert.ToInt32(ts.TotalMilliseconds);
            string howlong = a.ToString();
            Console.WriteLine("Time: " + howlong + " at " + text);

        }

        private void Form1_Load(object sender, EventArgs e) //chama o carregamento do layout
        {
            time("start");
            validade_License();
            if (!licenceFound)
            {


                MessageBox.Show("No license found!");
                this.Close();
                return;
            }
            time("validar");
            load_panel_layout();
            time("loadpanellayout");
            textBox1.Focus();
            List<string> itens = CNList;
            /*
            List<string> models = new List<string>();
            
            foreach (string l in CNList)
            {
                models.Add(l.Split(VarDash)[1]);
       
            }
            foreach (string l in models)
            {
           
            }
            models = models.Distinct().ToList();
            ModelList = models;
            int a = 0;
            foreach (string l in models.ToList())
            {
                foreach(string cn in CNList.ToList())
                {
                    if(cn.Split(VarDash)[1] == l)
                    {
                        ModelList[a] += VarDash.ToString() + cn.Split(VarDash)[0];  
                    }
                }
                a++;
            }
            foreach(string l in ModelList)
            {
            
            }
            */
            textBox2.ScrollBars = ScrollBars.Both;
            time("start call load cns");
            loadAllCNs += () =>
            {
                time("start load");
                Folders f = new Folders();
                time("folder instance");
                //f.listaGeral("reduzido");
                f.listaGeral("get");
                time("get list");
                CNList = f.listaGeral1;

                loadAllFGroups();
                time("load all groups");

            };

            if (loadAllCNs != null) // takes forever
            {
                loadAllCNs();
            }
            time("load ALL CNs");
            ListClass list = new ListClass();
            modelsFixturePatterns = list.Open("Modelos");
            if (modelsFixturePatterns.Count() == 0)
                list.mainList.Add("Modelo de Fixture;Prefixo;Sulfixo;Digitos");
            list.Close();

            time("prefixos fixtures");
            ListClass list1 = new ListClass();
            idPatterns = list1.Open("IDpatterns");
            if (idPatterns.Count() == 0)
            {
                list1.mainList.Add("Modelo de Fixture;Prefixo;Sulfixo;Digitos");
                list1.mainList.Add("Default;B;-FT;5");
            }
            list1.Close();

            time("id pattens");


            ListClass list2 = new ListClass();
            settingsList = list2.Open("Settings", "settings");
            string stl = config("showChartLabels");
            if (stl == "")
            {
                config("showChartLabels", "false");
                chartLabelVisible = false;
            }
            else
            {
                if (stl == "true")
                {
                    chartLabelVisible = true;
                }
                else
                {
                    chartLabelVisible = false;
                }
            }
            list2.Close();
            time("open chart labels setting");

            try { proporcao_do_primeiro_painel_em_porcentagem = Convert.ToInt32(config("proporcao_do_primeiro_painel_em_porcentagem")); }
            catch { config("proporcao_do_primeiro_painel_em_porcentagem", "60"); proporcao_do_primeiro_painel_em_porcentagem = 60; }

            try { proporcao_do_segundo_painel_em_porcentagem = Convert.ToInt32(config("proporcao_do_segundo_painel_em_porcentagem")); }
            catch { config("proporcao_do_segundo_painel_em_porcentagem", "65"); proporcao_do_segundo_painel_em_porcentagem = 65; }

            try { proporcao_do_terceiro_painel_em_porcentagem = Convert.ToInt32(config("proporcao_do_terceiro_painel_em_porcentagem")); }
            catch { config("proporcao_do_terceiro_painel_em_porcentagem", "60"); proporcao_do_terceiro_painel_em_porcentagem = 60; }

            try { description_font_size = Convert.ToInt32(config("description_font_size")); }
            catch { config("description_font_size", "18"); description_font_size = 18; }

            try { textbox1_offset = Convert.ToInt32(config("textbox1_offset")); }
            catch { config("textbox1_offset", "90"); textbox1_offset = 90; }

            try { chartLabel_offset = Convert.ToInt32(config("chartLabel_offset")); }
            catch { config("chartLabel_offset", "50"); chartLabel_offset = 50; }

            try { COM_port = config("COM_port"); }
            catch { config("COM_port", "COM6"); COM_port = "COM6"; }

            string path = "";
            path = config("HIDE_STATUS");
            if (path == "")
            {
                path = config("HIDE_STATUS", "FALSE");
            }
            if (path == "TRUE")
                PI = true;

            time("set settings");



            var list3 = new ListClass();
            list3.Open("locationPrefix", "listFolder");
            foreach (string l in list3.mainList.ToList())
            {
                if (l != "")
                {
                    prefixs.Add(l);
                }

            }
            list3.Close();
            time("locations prefix");
            load_panel_layout();
            time("load panel layout");
            time("finish");

            int addHour = 3;
            try
            {
                addHour = Convert.ToInt32(config("backUP_hour"));
            }
            catch
            {
                config("backUP_hour", "3");
                addHour = 3;
            }
            
            Action backUpAction = new Action(dotheBackUp);

            //ScheduleAction(backUpAction, DateTime.Today.AddHours(addHour));



        }

        public void loadPlacasProduzidas()
        {
            ListClass lc = new ListClass();
            Folders f = new Folders();
            string path = "";
            path = config("Placas_Produzidas_LPath");
            if (!Directory.Exists(path))
            {
                path = config("Placas_Produzidas_LPath",f.ListaGeralFolderPath);
            }
            lc.Open("Placas Produzidas", path);
            PlascasProduzidas.Clear();
            foreach (string l in lc.mainList)
            {
                List<string> list = new List<string>();

                string date1 = "";
                string date2 = "";
                string placas = "";
                try
                {
                    list = l.Split(VarDash).ToList();
                    date1 = list[0];
                    date2 = list[1];
                    placas = list[2];
                }
                catch
                {
                    continue;
                }

                PlascasProduzidas.Add(l);

                Console.WriteLine(l);

            }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            load_panel_layout();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string t = textBox1.Text;
            char[] array;
            int n = t.Count();
            if (n != 0)
            {
                array = t.ToCharArray();
                if (array[n - 1] == ' ' && buttonMode() != "EDIT" && buttonMode() != "SEARCH")
                {
                    textBox1.Text = "";
                    buttonMode("next");
                }
            }


        }

        private void addRemoveArows(string comand, string upDown)
        {
            string newText = "";

            switch (upDown)
            {
                case "up":
                    if (buttonMode() == "CHECK")
                    {
                        buttonMode("ADD");
                        textBox1.Text = currentCN;
                        return;
                    }
                    if (buttonMode() == "REMOVE" || buttonMode() == "REMOVE Stock")
                    {
                        if (comand == "")
                        {
                            if (buttonMode() == "REMOVE Stock")
                                buttonMode("ADD Stock");
                            else
                                buttonMode("ADD");
                            textBox1.Text = currentCN;
                            return;
                        }
                        if (comand.Contains("@"))
                        {
                            if (comand.Split('@')[0] != "1")
                            {
                                newText = (Convert.ToInt32(comand.Split('@')[0]) - 1).ToString() + "@" + comand.Split('@')[1];
                                textBox1.Text = newText;
                            }
                            else
                            {
                                if (buttonMode() == "REMOVE Stock")
                                    buttonMode("ADD Stock");
                                else
                                    buttonMode("ADD");
                            }
                        }
                        else
                        {

                            newText = "@" + comand;
                            if (buttonMode() == "REMOVE Stock")
                                buttonMode("ADD Stock");
                            else
                                buttonMode("ADD");
                        }
                        return;
                    }
                    if (buttonMode() == "ADD" || buttonMode() == "ADD Stock")
                    {
                        if (comand == "")
                        {
                            textBox1.Text = currentCN;
                            return;
                        }
                        if (comand.Contains("@"))
                        {
                            newText = (Convert.ToInt32(comand.Split('@')[0]) + 1).ToString() + "@" + comand.Split('@')[1];
                            textBox1.Text = newText;
                        }
                        else
                        {
                            newText = "2@" + comand;
                            textBox1.Text = newText;

                        }
                        return;
                    }
                    break;
                case "down":
                    if (buttonMode() == "CHECK")
                    {
                        buttonMode("REMOVE");
                        textBox1.Text = currentCN;
                        return;
                    }
                    if (buttonMode() == "ADD" || buttonMode() == "ADD Stock")
                    {
                        if (comand == "")
                        {
                            if (buttonMode() == "ADD Stock")
                                buttonMode("REMOVE Stock");
                            else
                                buttonMode("REMOVE");
                            textBox1.Text = currentCN;
                            return;
                        }
                        if (comand.Contains("@"))
                        {
                            if (comand.Split('@')[0] != "1")
                            {
                                newText = (Convert.ToInt32(comand.Split('@')[0]) - 1).ToString() + "@" + comand.Split('@')[1];
                                textBox1.Text = newText;
                            }
                            else
                            {
                                if (buttonMode() == "ADD Stock")
                                    buttonMode("REMOVE Stock");
                                else
                                    buttonMode("REMOVE");
                            }
                        }
                        else
                        {

                            newText = comand;
                            if (buttonMode() == "ADD Stock")
                                buttonMode("REMOVE Stock");
                            else
                                buttonMode("REMOVE");
                        }
                        return;
                    }
                    if (buttonMode() == "REMOVE" || buttonMode() == "REMOVE Stock")
                    {
                        if (comand == "")
                        {
                            textBox1.Text = currentCN;
                            return;
                        }
                        if (comand.Contains("@"))
                        {
                            newText = (Convert.ToInt32(comand.Split('@')[0]) + 1).ToString() + "@" + comand.Split('@')[1];
                            textBox1.Text = newText;
                        }
                        else
                        {
                            newText = "2@" + comand;
                            textBox1.Text = newText;

                        }
                        return;
                    }
                    break;

            }
        }
        private void imagensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Folders folder = new Folders();
            try
            {
                Process.Start(folder.picFolder());
            }
            catch
            {

            }
        } //acessa pasta de imagens

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {



            if ((Keys)e.KeyChar == Keys.Enter)
            {
                string comand = textBox1.Text;
                textBox1.Text = "";
                computeCode(comand);
            }
            /*if ((Keys)e.KeyChar == Keys.Space)
            {
                buttonMode("next");
            }*/

        } //comanda botoes da textbox
        private void clearChartPanel()
        {
            if (disposeChartLabels != null)
            {
                disposeChartLabels();
            }
            if (deleteChartPanel != null)
            {
                deleteChartPanel();
            }
            deleteChartPanel = null;
            reapearChartpanel = null;
        }
        private string replaceChars(string text)
        {
            string output = "";
            output = text;
            ListClass lc = new ListClass();
            List<string> replaceList = new List<string>();
            lc.Open("replace", "listFolder");
            if (lc.mainList.Count() == 0)
            {
                lc.mainList.Add("////Lista de caracteres para substituição");
                lc.mainList.Add("////<caracter original><Espaço><caracter novo>");
                lc.mainList.Add("////Exemplo:");
                lc.mainList.Add("////a b");
                lc.mainList.Add("////Nesse exemplo o comando a23at se torna b23bt");
                lc.mainList.Add("////////////////////////////////////////////");
                lc.Close();
                return text;
            }
            bool start = false;
            foreach (string line in lc.mainList)
            {
                if (line.First() == '/' && start == false)
                {
                    start = true;
                    continue;
                }
                replaceList.Add(line);
            }
            foreach (string l in replaceList)
            {

                if (output.Contains(l.First().ToString()))
                {
                    try
                    {
                        string second = l.Split(' ')[1];
                        output = output.Replace(l.First(), second.First());
                    }
                    catch { }
                }
            }

            return output;
        }
        public async void ScheduleAction(Action action, DateTime ExecutionTime)
        {
            await Task.Delay((int)ExecutionTime.Subtract(DateTime.Now).TotalMilliseconds);
            action();
        }
        public void dotheBackUp()
            {
            string backupPath = "";
            Folders f = new Folders();
            backupPath = config("BackUpPath");
            if (!Directory.Exists(backupPath))
            {
                backupPath = config("BackUpPath", f.ReturnBackUpFolder());
            }
            string backFolderPath = DateTime.Now.ToString().Replace(':', '.');
            backFolderPath = backFolderPath.Replace('/', '_');
            if (!Directory.Exists(backFolderPath))
            {
                try { Directory.CreateDirectory(backFolderPath); }
                catch { }
            }
            backFolderPath = backupPath + @"\" + backFolderPath;

            f.backup(backFolderPath);

        }

        public void computeCode(string imput)
        {

            string comand = "";

            comand = replaceChars(imput); ////////////////erro
            validade_License();
            if (!licenceFound)
            {
                MessageBox.Show("No license found!");
                return;
            }



            if (comand == "")
            {
                return;
            }

            textBox1.Focus();
            if (reapearChartpanel != null)
            {
                reapearChartpanel();
            }
            searchList.Clear();

            cycleLifeLayouy("hide");
            //COMANDO ENTRADA
            if (comand.Contains('='))
            {
                string comandValue = "";
                comandValue = comand.Split('=')[1];
                switch (comand.Split('=')[0])
                {
                    case "SetIO_COM":
                        try
                        {
                            if (comandValue == "")
                            {
                                MessageBox.Show("O valor da porta deve ser diferente de nulo");
                                return;
                            }
                            try
                            {
                                Convert.ToInt32(comandValue);
                            }
                            catch
                            {
                                MessageBox.Show("Este valor não é valido para a COM Port");
                                return;
                            }
                            config("COM_port", "COM" + comandValue);
                            mainPort = new SerialPort("COM" + comandValue, 9600);
                        }
                        catch { };
                        break;
                }
            }

            if (comand == "getthejobdone")
            {
                string masterLine = "";
                ListClass lc2 = new ListClass();
                lc2.Open("Mestra", "ListaGeral");
                lc2.mainList.Clear();
                Console.WriteLine("======================");
                Folders folder = new Folders();
                int counter = 0;
                string[] CNarray = Directory.GetDirectories(folder.TPFolder);
                foreach (string a in CNarray)
                {

                    ListClass lc = new ListClass();
                    lc.Open("tpInfo", a);
     
                    if (lc.mainList.ToList().Count() < 2)
                    {
                        lc.Close();
                        continue;
                    }
                    string last = lc.mainList.Last();
                    foreach (string b in lc.mainList.ToList())
                    {

                        try
                        {
                            string varName = b.Split(':')[0];
                            string varValue = b.Split(':')[1];
                            masterLine += varName + VarDash + varValue;
                            if (!b.Equals(last))
                            {
                                masterLine += VarDashPlus.ToString();
                            }
                            

                        }
                        catch
                        {
                            masterLine = "";
                            continue;
                        }
                        
                    
                    }
                    lc2.mainList.Add(masterLine);
                    masterLine = "";




                }
                lc2.Close();
            }
            if (comand.Split(':')[0] == "code")
            {
                code();
                return;
            }
            if (comand.Split(':')[0] == "chartW")
            {
                chartW = Convert.ToInt32(comand.Split(':')[1]);
                load_panel_layout();
                return;
            }
            if (comand.Split(':')[0] == "labelOFFset")
            {
                chartLabel_offset = Convert.ToInt32(comand.Split(':')[1]) * (-1);
                load_panel_layout();
                return;
            }
            if (comand.Split(':')[0] == "chartH")
            {
                chartH = Convert.ToInt32(comand.Split(':')[1]);
                load_panel_layout();
                return;
            }
            if (comand == "loadplacasproduzidas")
            {
                loadPlacasProduzidas();
                return;
            }
            if (comand == "excelKill")
            {
                foreach (var process in Process.GetProcessesByName("excel"))
                {
                    process.Kill();
                }
                return;
            }
            if (comand == "print")
            {
                print(textBox1.Text, textBox7.Text);
                textBox1.Text = "";
                return;
            }
            //loadPlacasProduzidas()
            if (comand == "dothebackup")
            {
                string backupPath = "";
                Folders f = new Folders();
                backupPath = config("BackUpPath");
                if (!Directory.Exists(backupPath))
                {
                    backupPath = config("BackUpPath",f.ReturnBackUpFolder());
                }
                 string backFolderPath = DateTime.Now.ToString().Replace(':','.');
                backFolderPath = backFolderPath.Replace('/', '_');
                if (!Directory.Exists(backFolderPath))
                {
                    try { Directory.CreateDirectory(backFolderPath); }
                    catch { }
                }
                backFolderPath = backupPath + @"\" + backFolderPath;

                f.backup(backFolderPath);
            }

            foreach (string l in prefixs)
            {
                if (currentCN == "")
                    break;

                if (comand.StartsWith(l))
                {
                    Item item = new Item();
                    item.Open(currentCN);
                    item.stream("location", comand);
                    item.Close();
                    label45.Text = comand;
                    load_panel_layout();
                    return;
                }
            }
            if (comand.Split(':')[0] == "location")
            {
                Item item = new Item();
                item.Open(currentCN);
                item.stream("location", comand.Split(':')[1]);
                item.Close();
                label45.Text = comand.Split(':')[1];
                //label11.Text = comand; ATUALIZAR LABEL DE LOCATION
                load_panel_layout();
                return;
            }
            if (comand.Split(':')[0] == "location2")
            {
                Item item = new Item();
                item.Open(currentCN);
                item.stream("location2", comand.Split(':')[1]);
                item.Close();
                label44.Text = comand.Split(':')[1];
                //label11.Text = comand; ATUALIZAR LABEL DE LOCATION
                load_panel_layout();
                return;
            }
            if (comand.Split(':')[0] == "location3")
            {
                Item item = new Item();
                item.Open(currentCN);
                item.stream("location3", comand.Split(':')[1]);
                item.Close();
                label43.Text = comand.Split(':')[1];
                //label11.Text = comand; ATUALIZAR LABEL DE LOCATION
                load_panel_layout();
                return;
            }

            if (comand.Split(vd())[0] == "ADDSTOCK")
            {
                buttonMode("ADD Stock");
            }
            if (comand.Split(vd())[0] == "REMOVESTOCK")
            {
                buttonMode("REMOVE Stock");
            }
            if (comand.Count() > 1)
            {
                if (comand.ToArray().First() == 'N' && comand.ToArray()[1] == '#')
                {
                    renameItem("name", comand.Split('#')[1]);
                    return;
                }
                if (comand.ToArray().First() == 'A' && comand.ToArray()[1] == '#')
                {
                    renameItem("alerta", comand.Split('#')[1]);
                    return;
                }
                if (comand.ToArray().First() == 'D' && comand.ToArray()[1] == '#')
                {
                    renameItem("desc", comand.Split('#')[1]);
                    return;
                }

            }

            textBox1.Text = "";
            if (comand.Contains('#'))
            {
                int ab = 0;
                if (comand.Split('#')[0] == "EM" || comand.Split('#')[0] == "em")
                {

                    try
                    {

                        ab = Convert.ToInt32(comand.Split('#')[1]);

                    }
                    catch { return; }
                    Item item1 = new Item();
                    item1.Open(currentCN);
                    if (!item1.itemExists)
                    {
                        item1.Close();
                        return;
                    }
                    item1.set_EstoqueMinimo(ab);
                    updateLogpanels(item1.getLogList());
                    item1.Close();
                    //item1.setStatus();

                    label12.Text = item1.Status;
                    if (PI)
                        label12.Text = "";

                    label8.Text = ab.ToString();
                    load_panel_layout();
                                        return;
                }
                if (comand.Split('#')[0] == "SET" || comand.Split('#')[0] == "set")
                {

                    try
                    {

                        ab = Convert.ToInt32(comand.Split('#')[1]);

                    }
                    catch { return; }
                    Item item2 = new Item();
                    item2.Open(currentCN);
                    if (!item2.itemExists)
                    {
                        item2.Close();
                        return;
                    }
                    item2.QTD("set", ab);
                    updateLogpanels(item2.getLogList());
                    labelQTD_value.Text = ab.ToString();
                    item2.Close();
                    //item2.setStatus();
                    label12.Text = item2.Status;
                    if (PI)
                        label12.Text = "";
                    load_panel_layout();
                    display(currentCN, 0, "CHECK");
                    
                    return;
                }
                if (comand.Split('#')[0] == "QI" || comand.Split('#')[0] == "qi")
                {

                    try { ab = Convert.ToInt32(comand.Split('#')[1]); }

                    catch { return; }
                    Item item1 = new Item();
                    item1.Open(currentCN);
                    if (!item1.itemExists)
                    {
                        item1.Close();
                        return;
                    }
                    label14.Text = item1.setQTDINI(ab).ToString();
                    load_panel_layout();
                    item1.Close();
                    return;
                }
            }
            char[] ch = comand.ToCharArray();
            try
            {
                if (ch[0].ToString() == "#")
                {
                    Console.WriteLine("ID");
                    Item ite_ = new Item();
                    ite_.Open(currentCN);
                    string ID_OP = comand.Split('#')[1];
                    ID_OP = pattern("ID", ID_OP);
                    ite_.addID_inLog(ID_OP, "ID", currentSelectedLogPanel);
                    updateLogpanels(ite_.getLogList());
                    ite_.Close();
                    return;

                }
            }
            catch { }
            try
            {
                if (ch[0].ToString() == "$")
                {
                    Item ite_ = new Item();
                    ite_.Open(currentCN);
                    string ID_OP = comand.Split('$')[1];
                    ID_OP = pattern("Fixture", ID_OP);
                    ite_.addID_inLog(ID_OP, "Fixture", currentSelectedLogPanel);
                    updateLogpanels(ite_.getLogList());
                    ite_.Close();
                    return;

                }
            }
            catch { }
            currentSelectedLogPanel = 1;
            clearChartPanel();

            relocateBut = null;

            switch (comand)
            {
                case "ADD":
                    buttonMode("ADD");
                    return;
                case "add":
                    buttonMode("ADD");
                    return;
                case "REMOVE":
                    buttonMode("REMOVE");
                    return;
                case "remove":
                    buttonMode("REMOVE");
                    return;
                case "CHECK":
                    buttonMode("CHECK");
                    return;
                case "check":
                    buttonMode("CHECK");
                    return;
                case "EDIT":
                    buttonMode("EDIT");
                    return;
                case "edit":
                    buttonMode("EDIT");
                    return;
                default:
                    break;
            }



            int qtd = 1;
            string CN = "";
            if (comand.Contains("@"))
            {
                try
                {
                    qtd = Convert.ToInt32(comand.Split('@')[0]);
                }
                catch
                {
                    qtd = 1;
                }
                CN = comand.Split('@')[1];
            }
            else
            {
                CN = comand;
            }

            display(CN, qtd);
        }
        string qtdLabelCurrentValue = "";
        public async void QTD_label_Animation(int increment, string qtdLcurrentValue)
        {

            await Task.Delay(500);
            int currentValue = 0;
            if (qtdLcurrentValue == "")
            {
                currentValue = 0;
            }
            else
            {
                currentValue = Convert.ToInt32(qtdLcurrentValue);
            }
            if (buttonMode() == "ADD")
            {
                labelQTD_value.Text = qtdLcurrentValue + "+" + Convert.ToString(increment);
            }
            else
            {
                labelQTD_value.Text = qtdLcurrentValue + "-" + Convert.ToString(increment);
            }

            qtdLabelLocation();
            await Task.Delay(500);
            int newValue = currentValue;
            if (buttonMode() == "ADD")
            {
                newValue += increment;
            }
            else
            {
                newValue -= increment;
            }
            if (newValue >= 0)
            {
                labelQTD_value.Text = newValue.ToString();
            }
            else
            {
                labelQTD_value.Text = 0.ToString();
            }
            qtdLabelLocation();


        }
        public void display(string CN, int qtd = 1, string ck = "")
        {
            cycleLifeLayouy("hide");
            but10Mode("Edit");
            zoom = false;
            button13.Text = "Edit";
            textBox2.Enabled = false;
            textBox1.Focus();
            currentSelectedLogPanel = 1;
            switchtem += () =>
             {
                 return;
             };
            chart1.Series["Trocas"].Points.Clear();
            chart2.Series["Trocas"].Points.Clear();
            chart3.Series["Trocas"].Points.Clear();

            bool gerarLog = true;
            clearChartPanel();
            if (ck != "")
            {
                buttonMode(ck);
                gerarLog = false;
            }
            Item item = new Item();
            if (revertPanelColor != null)
            {
                revertPanelColor();
            }
            if (shutBlinkQTD != null)
            {
                shutBlinkQTD();
            }
            if (shutBlinkStatus != null)
            {
                shutBlinkStatus();
            }
            if (shutBlinkVersao != null)
            {
                shutBlinkVersao();
            }
            if (buttonMode() == "EDIT")
                return;
            if (buttonMode() == "SEARCH")
            {
                searchText = CN;
                loadNewSearch = true;
                loadForm(new Form6());



               

                return;
            }
                
   

            item.Open(CN);
            /////////////////////////////////////////////////////////////////////
            bool foundCN = false; //Motivo CNfound = false
            if (!item.itemExists)
            {
                foreach (string l4 in CNList.ToList())
                {
                    if (l4.Split(vd())[3] == CN)
                    {
                        currentCN = l4.Split(vd())[0];
                        foundCN = true;
                        break;
                    }
                }
                if (!foundCN)
                {
                    group_layout = false;
                    loadNoItemLayout();
                    return;
                }
                else
                {
                    item.Open(currentCN);
                }

            }
            if (!foundCN)
            {
                if(CN != currentCN)
                {
                    textBox7.Text = 1.ToString();
                }
                currentCN = CN;
            }
                
            /////////////////////////////////////////////////////////////////////

            if (gerarLog)
            {
                if (item.itemLocked == true && lastCN != currentCN)
                {
                    buttonMode("CHECK");
                }
                switch (buttonMode())
                {
                    case "ADD":
                        if (closeSearchForm != null)
                        {
                            closeSearchForm();
                        }
                        //labelQTD_value.Text = item.QTD("add", qtd).ToString();
                        qtdLabelCurrentValue = item.QTD("get").ToString();
             
                        item.QTD("add", qtd).ToString();
                        if (item.grupo == "")
                            QTD_label_Animation(qtd, qtdLabelCurrentValue);
                        break;

                    case "REMOVE":
                        if (closeSearchForm != null)
                        {
                            closeSearchForm();
                        }
                        //labelQTD_value.Text = item.QTD("add", qtd * (-1)).ToString();
                        qtdLabelCurrentValue = item.QTD("get").ToString();
                        item.QTD("add", qtd * (-1)).ToString();
                        if (item.grupo == "")
                            QTD_label_Animation(qtd, qtdLabelCurrentValue);
                        break;

                    case "CHECK":
                        labelQTD_value.Text = item.QTD("get").ToString();
                        if(qtd != -1)
                        {
                            if (closeSearchForm != null)
                            {
                                closeSearchForm();
                            }
                        }
                        break;

                }
            }
            else
            {
                labelQTD_value.Text = item.QTD("get").ToString();
            }

            if (item.QTD("get") < 0)
            {
                item.QTD("set", 0);
                labelQTD_value.Text = "0";
            }
            if (item.grupo != "")
            {
                button7.Visible = false;
                button6.Text = "Extrair";
                int ab = 0;
                ab = item.total_group - item.IN_group;

                label2.Text = "IN:" + item.IN_group.ToString() + "   OUT:" + ab.ToString() + "   TOTAL:" + item.total_group.ToString();
                if (item.itemPresentinGroup)
                {
                    labelQTD_value.Text = "IN";
                }
                else
                {
                    labelQTD_value.Text = "OUT";
                }
            }
            else
            {
                button6.Visible = true;
                button7.Visible = true;
                button6.Text = "ID";
                button7.Text = "FIXTURE";
            }

            Folders folder = new Folders();
            if (item.picture != "")
            {
                pictureBox1.Image = folder.image(item.picture);
            }
            else
            {
                pictureBox1.Image = folder.image(item.ItemCN);
            }


            label1.Text = item.ItemName;
            label10.Text = item.ItemModelo;
            label3.Text = "CN";
            label4.Text = item.ItemCN;
            label5.Text = "P/N";
            label6.Text = item.ItemPN;
            label11.Text = "ENDEREÇO: ";
            
            label43.Text = item.location3;
            label44.Text = item.location2;
            label45.Text = item.location;
            if (label44.Text != "" || label45.Text != "")
                label43.Text += ",";
            if (label6.Text == "")
            {
                label6.Text = "unk";
            }
            label7.Text = "Estoque Mínimo";
            if (label7.Text == "")
            {
                label7.Text = "unk";
            }

            label13.Text = "Quantidade Inicial";
            label14.Text = item.qtdInicial;
            string versaoModel = "";
            if (item.grupo != "")
            {
                label57.Text = "Versão do Fixture";
                label35.Text = "Data da Manutenção";
               // label37.Text = "Próxima manutenção";
                label39.Text = "Responsável";
              //  label41.Text = "Intervalo";
                label37.Text = "";
                label41.Text = "";
                //cowboy

                Form7 t = new Form7();
                string vs = t.getVersao(item.grupo);
                // f.Dock = DockStyle.Fill;


                

                
                if(vs == "")
                {
                    vs = "NONE";
                }
                versaoModel = vs;
                label59.Text = "VERSÃO: " + vs;
            }
            else
            {
                label59.Text = "";
                label57.Text = "";
                label35.Text = "Forecast";
                label37.Text = "Placas Produzidas";
                label39.Text = "Última Atualização";
                label41.Text = "Turns na linha";
            }


            if (label14.Text == "")
            {
                label14.Text = "unk";
            }
            label15.Text = "Data NPI";
            if (label15.Text == "")
            {
                label15.Text = "unk";
            }
            label16.Text = item.npiDate;
            label8.Text = item.Eminimo.ToString();
            string descrip = item.Item_description;
            char[] array = descrip.ToCharArray();
            int a1 = 0;
            foreach (char a in array)
            {
                if (a == (char)886)
                {
                    try { array[a1] = '\r'; } catch { }
                    try { array[a1 + 1] = '\n'; } catch { }
                }
                a1++;
            }
            descrip = new string(array);
            textBox2.Text = descrip;
            label27.Text = "Cycle Life";
            foreach(string l in PlascasProduzidas)
            {
                if (l.Split(VarDash)[2] == item.ItemModelo)
                {
                    item.PPdateStart = l.Split(VarDash)[0];
                    item.PPdateEnd = l.Split(VarDash)[1];
                    item.PPinRange = l.Split(VarDash)[3];
                    break;
                }
            }
            
            label28.Text = item.stream("CycleLife");
            //label28.Text = item.cycleLife.ToString("#.##");





            label29.Text = "Quantidade para Compra";
            
            //item.stream("DATA_MANUT", date);
            string dateU = "";
            dateU = item.stream("DateUsed");
            try { dateU = dateU.Split('/')[1] + "/" + dateU.Split('/')[0] + "/" + dateU.Split('/')[2]; }
            catch { dateU = ""; }



            if (item.grupo != "")
            {
                string vs2 = "";
                vs2 = item.stream("versao");
                if(vs2 == "")
                {
                    vs2 = "unk";
                }
                if(versaoModel != vs2)
                {
                    IO_mode("ALARME");
                    BlinkVersao();
                }
                label58.Text = vs2;
                label36.Text = item.stream("DATA_MANUT");
                label38.Text = item.stream("DATA_PROX");
                label40.Text = item.stream("Responsavel");
                //label42.Text = item.stream("Days_INTERVALO") + " dias"; ;
            }
            else
            {
                label58.Text = "";
                label36.Text = item.stream("Forcast");
                label38.Text = item.stream("PlacasProd");
                label40.Text = item.stream("dataDoCalculo"); ;
                label42.Text = item.stream("ItensINLine");
            }





            string rightDate = item.stream("DateUsed");
            try { rightDate = rightDate.Split('/')[1] + "/" + rightDate.Split('/')[0] + "/" + rightDate.Split('/')[2]; }
            catch { rightDate = ""; }

           // label31.Text = "Alerta(" + item.stream("Frequencia_Aviso") + ")";
            label33.Text = "Valor Unitário";
            string vUni = item.stream("valorUnitario");
            bool showdolar = false;

            if (label36.Text == "")
            {
                label36.Text = "unk";
            }
            if (label38.Text == "" && item.grupo != "")//esse
            {
                label38.Text = "";
            }
            else
            {
              //  label38.Text = "unk";
            }
            if (label40.Text == "")
            {
                label40.Text = "unk";
            }
            if (label42.Text == "" && item.grupo != "")//e esse
            {
                label42.Text = "";
            }
            else
            {
                label42.Text = "unk";
            }

            // d("Dolar:5,26");

            //list2.mainList.Add("");


            if (config("DolarEnable") == "ON")
            {
                showdolar = true;
            }
            else
            {
                showdolar = false;
            }
           
            if (vUni == "")
            {
                label34.Text = "0";
            }
            else
            {
                if (showdolar)
                    label34.Text = "R$ " + vUni;
                else
                    label34.Text = "R$ " + vUni;
            }

            string aviso_f = "";
            string mode1 = item.stream("Frequencia_Aviso");
            switch (mode1)
            {
                case "Diário":
                    aviso_f = "day";
                    break;
                case "Semanal":
                    aviso_f = "week";
                    break;
                case "Mensal":
                    aviso_f = "month";
                    break;
            }
            if (item.stream("alerta") == "")
            {
                
            }
            else
            {
                //label32.Text = item.numberScraps(aviso_f) + "/-" + item.stream("alerta");
            }
            int Nscraps = 0;
            int MaxScraps = 0;
            try
            {
                Nscraps = item.numberScraps(aviso_f);
                MaxScraps = Convert.ToInt32(item.stream("alerta")) * (-1);
                if (Nscraps > MaxScraps)
                {
                    panel6.BackColor = Color.FromArgb(51, 90, 59);
                
                }
                else
                {
                    if (Nscraps < (MaxScraps * 2))
                    {
                        panel6.BackColor = Color.Red;
                     
                    }
                    else
                    {
                        panel6.BackColor = Color.Salmon;
                       
                    }

                }
            }
            catch
            {
                panel6.BackColor = Color.FromArgb(51, 90, 59);
              
            }

            //int Quant_para_compra = item.getVar("QTD_Finalizar") - Convert.ToInt32(item.QTD("get"));
          


            int qtdF = item.QTD_Finalizar_P;
            int qtd1 = (int)item.QTD("get");
            if (label28.Text != "")
                label30.Text = item.stream("QTDcompra"); //qtd para compra
            else
            {
                label30.Text = "";
            }
            if (qtdF > qtd1)
            {
                label30.ForeColor = Color.LightSalmon;
                label29.ForeColor = Color.LightSalmon;
            }


            else
            {
                label29.ForeColor = Color.White;
                label30.ForeColor = Color.White;
            }


            updateLogpanels(item.getLogList());
            LastChart = item.chartList(lastCharsScreem);
            loadChart();

            LabInfoLoca();
            int Emin = item.Eminimo;
            if (Emin > item.QTD("get") && item.grupo == "")
            {
                labelQTD_value.ForeColor = Color.Red;
                BlinkQTD();
            }
            else
            {
                BlinkQTD("OFF");
                labelQTD_value.ForeColor = Color.White;

            }
            if (item.MissingDesatualizado)//<<< esta sempre desatualizado???
            {

                //itensMissing(getMissingPage(folder.missingList(), missingConfig[0] * missingConfig[1], missing_page.ToString()), missingConfig);
            }
            int barvalue = item.barValue();

            switch (barvalue)
            {
                case -1:
                    progressBar1.BackColor = Color.Gray;
                    progressBar1.Value = 0;
                    progressBar1.Enabled = false;
                    break;
                case -2:
                    progressBar1.BackColor = Color.Blue;
                    progressBar1.Enabled = true;
                    progressBar1.Value = 255;
                    break;
                default:
                    progressBar1.BackColor = Color.FromArgb(barvalue, 255 - barvalue, 0);
                    progressBar1.Value = barvalue;
                    progressBar1.Enabled = true;
                    break;

            }
            turnPartToolStripMenuItem.Enabled = true;

            List<string> chartList = item.scrapsPerTurno("semanal");
            int scraps1 = Convert.ToInt32(chartList[0].Split('@')[1]);
            int scraps2 = Convert.ToInt32(chartList[1].Split('@')[1]);
            int scraps3 = Convert.ToInt32(chartList[2].Split('@')[1]);
            
            scraps1 *= (-1);
            scraps2 *= (-1);
            scraps3 *= (-1);
            if (scraps1 < 0)
                scraps1 = 0;
            if (scraps2 < 0)
                scraps2 = 0;
            if (scraps3 < 0)
                scraps3 = 0;
            chart2.Series["Trocas"].Points.AddXY(scraps1.ToString(), scraps1);
            chart2.Series["Trocas"].Points.AddXY(scraps2.ToString(), scraps2);
            chart2.Series["Trocas"].Points.AddXY(scraps3.ToString(), scraps3);
            label48.Text = "Total da Semana: " + (scraps1 + scraps2 + scraps3).ToString();
            float sum = 0;
            sum = scraps1 + scraps2 + scraps3;
            if(sum != 0)
            {
                double life = (7 / sum) * item.QTD("get");
                int lifeInt = Convert.ToInt32(life);
                if (lifeInt == 1)
                    label46.Text = "Tempo de vida: " + lifeInt.ToString() + " dia";
                else
                    label46.Text = "Tempo de vida: " + lifeInt.ToString() + " dias";
            }
            else
            {
                label46.Text = "Tempo de vida: " + "unk" + " dias";
            }
            



            chartList = item.scrapsPerTurno("mensal");
            scraps1 = Convert.ToInt32(chartList[0].Split('@')[1]);
            scraps2 = Convert.ToInt32(chartList[1].Split('@')[1]);
            scraps3 = Convert.ToInt32(chartList[2].Split('@')[1]);
            scraps1 *= (-1);
            scraps2 *= (-1);
            scraps3 *= (-1);
            if (scraps1 < 0)
                scraps1 = 0;
            if (scraps2 < 0)
                scraps2 = 0;
            if (scraps3 < 0)
                scraps3 = 0;
            chart3.Series["Trocas"].Points.AddXY(scraps1.ToString(), scraps1);
            chart3.Series["Trocas"].Points.AddXY(scraps2.ToString(), scraps2);
            chart3.Series["Trocas"].Points.AddXY(scraps3.ToString(), scraps3);
            label49.Text = "Total do Mês: " + (scraps1 + scraps2 + scraps3).ToString();
            sum = scraps1 + scraps2 + scraps3;
            if (sum != 0)
            {
                double life = (30 / sum) * item.QTD("get");
                int lifeInt = Convert.ToInt32(life);
                if (lifeInt == 1)
                    label47.Text = "Tempo de vida: " + lifeInt.ToString() + " dia";
                else
                    label47.Text = "Tempo de vida: " + lifeInt.ToString() + " dias";
            }
            else
            {
                label47.Text = "Tempo de vida: " + "unk" + " dias";
            }

            chartList = item.scrapsPerTurno();
            scraps1 = Convert.ToInt32(chartList[0].Split('@')[1]);
            scraps2 = Convert.ToInt32(chartList[1].Split('@')[1]);
            scraps3 = Convert.ToInt32(chartList[2].Split('@')[1]);
            scraps1 *= (-1);
            scraps2 *= (-1);
            scraps3 *= (-1);
            if (scraps1 < 0)
                scraps1 = 0;
            if (scraps2 < 0)
                scraps2 = 0;
            if (scraps3 < 0)
                scraps3 = 0;
            chart1.Series["Trocas"].Points.AddXY(scraps1.ToString(), scraps1);
            chart1.Series["Trocas"].Points.AddXY(scraps2.ToString(), scraps2);
            chart1.Series["Trocas"].Points.AddXY(scraps3.ToString(), scraps3);
            label50.Text = "Total do dia: " + (scraps1 + scraps2 + scraps3).ToString();

            /////// 3 meses //////////////////////////////////////////////////////////////////////////////////////////////
            chartList = item.scrapsPerTurno("3 meses");
            scraps1 = Convert.ToInt32(chartList[0].Split('@')[1]);
            scraps2 = Convert.ToInt32(chartList[1].Split('@')[1]);
            scraps3 = Convert.ToInt32(chartList[2].Split('@')[1]);
            scraps1 *= (-1);
            scraps2 *= (-1);
            scraps3 *= (-1);
            if (scraps1 < 0)
                scraps1 = 0;
            if (scraps2 < 0)
                scraps2 = 0;
            if (scraps3 < 0)
                scraps3 = 0;



            sum = scraps1 + scraps2 + scraps3;
            if (sum != 0)
            {
                double life = (90 / sum) * item.QTD("get");
                int lifeInt = Convert.ToInt32(life);
                if (lifeInt == 1)
                    label52.Text = "Tempo de vida: " + lifeInt.ToString() + " dia";
                else
                    label52.Text = "Tempo de vida: " + lifeInt.ToString() + " dias";
            }
            else
            {
                label52.Text = "Tempo de vida: " + "unk" + " dias";
            }
            label51.Text = "Total do trimestre: " + (scraps1 + scraps2 + scraps3).ToString();
            ///////////////////////////////////////////////////////////////////////////////////////////



            bool getScrapList = false;
            string sCrapSemanalListPath = "";
            string h = "";
            if (scrapSemanalListPath != "")
            {
                sCrapSemanalListPath = scrapSemanalListPath;
                getScrapList = true;
            }
            else
            {
                h = config("scrapSemanalListPath");
                if(h != "" && h != "noADRESS")
                {
                    scrapSemanalListPath = h;
                    sCrapSemanalListPath = scrapSemanalListPath;
                    getScrapList = true;
                }
                else
                {
                    config("scrapSemanalListPath", "noADRESS");
                }
       
                
            }
            if (getScrapList)
            {
                string line3 = "";
                line3 = string.Join(VarDashPlus.ToString(), item.itemInfoList.ToArray());
                line3 += VarDashPlus.ToString() +"scrapSemanal" + VarDash.ToString()+ item.getScrapSemanal().ToString();
                ListClass list2 = new ListClass();
                list2.Open(DateTime.Now.ToString().Replace(':', ' ').Replace('/', '_') + " " + RandomString(8), sCrapSemanalListPath);
                list2.mainList.Add(line3);
                list2.Close();
            }






            item.Close();
            if (item.grupo == "")
            {
                label12.Text = item.Status;
                if (PI)
                    label12.Text = "";
            }
                
            else
            {
                label12.Text = item.grupo;
                BlinkStatus("OFF");
                label12.ForeColor = Color.White;
            }
            if (label12.Text == "Sem Controle")
            {
                BlinkStatus();
            }
            else
            {
                BlinkStatus("OFF");
                label12.ForeColor = Color.White;
            }
            //code("");
            if (item.grupo == "")
                group_layout = false;
            else
                group_layout = true;
            load_panel_layout();
            lastCN = currentCN;
            stopBlink();
            return;

        }
        private void loadChart()
        {

            int biggestBut = (panel8.Height * 7) / 10;
            int s1 = 0;
            int d = 0;
            int biggest = 0;
            int value = 0;
            if (LastChart != null)
                d = LastChart.Count();
            while (s1 < d)
            {
                value = Convert.ToInt32(LastChart[s1].Split('@')[1]);
                if (value < 0)
                {
                    value *= (-1);
                }
                if (value > biggest)
                {
                    biggest = value;
                }
                s1++;
            }
            if (biggest == 0)
            {
                return;
            }
            int minButSize = 5;
            int margemInferior = 30;
            int a = 0;
            int b = 0;
            int margem_Primeiro_botao = 30;
            int gap = 10;
            if (LastChart != null)
                b = LastChart.Count();
            while (a < b)
            {
                Button but = new Button();
                Label l = new Label();
                Label l2 = new Label();
                but.BackColor = Color.DarkGreen;
                int n = Convert.ToInt32(LastChart[a].Split('@')[1]);
                l2.Text = LastChart[a].Split('@')[0];
                l.Text = n.ToString();
                if (n < 0)
                    n *= (-1);
                else
                {
                    but.BackColor = Color.CadetBlue;
                }
                panel8.Controls.Add(l);
                panel8.Controls.Add(l2);
                l.Visible = false;
                if (chartLabelVisible)
                    l.Visible = true;
                l.ForeColor = Color.White;
                l.AutoSize = true;
                l.Font = new Font("Times New Roman", 13);
                l2.Visible = false;
                l2.ForeColor = Color.White;
                l2.AutoSize = true;
                l2.Font = new Font("Times New Roman", 13);
                panel8.Controls.Add(but);
                deleteChartPanel += () =>
                {
                    panel8.Controls.Remove(but);
                };
                reapearChartpanel += () =>
                {
                    panel8.Controls.Add(but);
                };
                makeitVisible += () =>
                {
                    l.Visible = true;
                };
                invisibleLabels += () =>
                {
                    l.Visible = false;
                };
                // ListClass list2 = new ListClass();
                //  settingsList = list2.Open("Settings", "settings");
                but.MouseMove += (s, args) =>
                {
                    if (chartLabelVisible)
                        return;
                    l.Visible = true;
                    l2.Visible = true;
                };
                but.MouseLeave += (s, args) =>
                {
                    if (chartLabelVisible)
                        return;
                    l.Visible = false;
                    l2.Visible = false;
                };
                but.GotFocus += (s, args) =>
                {

                    textBox1.Focus();
                };
                deleteChartPanel += () =>
                {
                    panel8.Controls.Remove(but);
                };
                disposeChartLabels += () =>
                 {
                     l.Dispose();
                     l2.Dispose();
                 };

                but.FlatStyle = FlatStyle.Flat;
                int butHei = 0;
                butHei = (biggestBut * n) / biggest;
                if (butHei < minButSize)
                {
                    butHei = minButSize;
                }
                but.Size = new Size(30, butHei);
                int gap_ = gap;
                if (a == 0)
                    gap_ = 0;
                else { gap_ = gap; }

                but.Location = new Point(margem_Primeiro_botao + (but.Width + gap_) * a, panel8.Height - but.Height - margemInferior);

                relocateBut += () =>
                {

                };
                if (relocateBut != null)
                {
                    relocateBut();
                }
                l.Size = new Size(but.Width, 50);
                l.Location = new Point(but.Location.X + but.Width / 2 - l.Width / 2, but.Location.Y - l.Height);
                l2.Size = new Size(but.Width, 50);
                l2.Location = new Point(but.Location.X + but.Width / 2 - l2.Width / 2, but.Location.Y + but.Height);
                a++;
            }
        }
        List<string> logList_;
        List<string> currentMissingList = null;
        List<string> lastWorkingMissing = null;
        private void loadChart_backup()
        {

            int biggestBut = (panel8.Height * 7) / 10;
            int s1 = 0;
            int d = 0;
            int biggest = 0;
            int value = 0;
            if (LastChart != null)
                d = LastChart.Count();
            while (s1 < d)
            {
                value = Convert.ToInt32(LastChart[s1].Split('@')[1]);
                if (value < 0)
                {
                    value *= (-1);
                }
                if (value > biggest)
                {
                    biggest = value;
                }
                s1++;
            }
            if (biggest == 0)
            {
                return;
            }
            int minButSize = 5;
            int margemInferior = 30;
            int a = 0;
            int b = 0;
            int margem_Primeiro_botao = 30;
            int gap = 10;
            if (LastChart != null)
                b = LastChart.Count();
            while (a < b)
            {
                Button but = new Button();
                Label l = new Label();
                Label l2 = new Label();
                but.BackColor = Color.DarkGreen;
                int n = Convert.ToInt32(LastChart[a].Split('@')[1]);
                l2.Text = LastChart[a].Split('@')[0];
                l.Text = n.ToString();
                if (n < 0)
                    n *= (-1);
                else
                {
                    but.BackColor = Color.CadetBlue;
                }
                panel8.Controls.Add(l);
                panel8.Controls.Add(l2);
                l.Visible = false;
                if (chartLabelVisible)
                    l.Visible = true;
                l.ForeColor = Color.White;
                l.AutoSize = true;
                l.Font = new Font("Times New Roman", 13);
                l2.Visible = false;
                l2.ForeColor = Color.White;
                l2.AutoSize = true;
                l2.Font = new Font("Times New Roman", 13);
                panel8.Controls.Add(but);
                deleteChartPanel += () =>
                {
                    panel8.Controls.Remove(but);
                };
                reapearChartpanel += () =>
                {
                    panel8.Controls.Add(but);
                };
                makeitVisible += () =>
                {
                    l.Visible = true;
                };
                invisibleLabels += () =>
                {
                    l.Visible = false;
                };
                // ListClass list2 = new ListClass();
                //  settingsList = list2.Open("Settings", "settings");
                but.MouseMove += (s, args) =>
                {
                    if (chartLabelVisible)
                        return;
                    l.Visible = true;
                    l2.Visible = true;
                };
                but.MouseLeave += (s, args) =>
                {
                    if (chartLabelVisible)
                        return;
                    l.Visible = false;
                    l2.Visible = false;
                };
                but.GotFocus += (s, args) =>
                {

                    textBox1.Focus();
                };
                deleteChartPanel += () =>
                {
                    panel8.Controls.Remove(but);
                };
                disposeChartLabels += () =>
                {
                    l.Dispose();
                    l2.Dispose();
                };

                but.FlatStyle = FlatStyle.Flat;
                int butHei = 0;
                butHei = (biggestBut * n) / biggest;
                if (butHei < minButSize)
                {
                    butHei = minButSize;
                }
                but.Size = new Size(30, butHei);
                int gap_ = gap;
                if (a == 0)
                    gap_ = 0;
                else { gap_ = gap; }

                but.Location = new Point(margem_Primeiro_botao + (but.Width + gap_) * a, panel8.Height - but.Height - margemInferior);

                relocateBut += () =>
                {

                };
                if (relocateBut != null)
                {
                    relocateBut();
                }
                l.Size = new Size(but.Width, 50);
                l.Location = new Point(but.Location.X + but.Width / 2 - l.Width / 2, but.Location.Y - l.Height);
                l2.Size = new Size(but.Width, 50);
                l2.Location = new Point(but.Location.X + but.Width / 2 - l2.Width / 2, but.Location.Y + but.Height);
                a++;
            }
        }
        private List<string> getMissingPage(List<string> fullList, int ItensPerPage, string pageNumber)
        {
            return null;
            List<string> list2 = fullList;
            int totalNumberOfItens;
            if (fullList != null)
            {
                currentMissingList = fullList;
                totalNumberOfItens = list2.Count();
            }
            else
            {
                totalNumberOfItens = currentMissingList.Count();
            }
            label9.Text = "Itens em falta Total:" + totalNumberOfItens.ToString();
            label9.AutoSize = true;
            label9.Location = new Point(panel9.Width / 2 - label9.Width / 2, 0);
            int howManyInEachPage = ItensPerPage; //Falta atribuir essas informações

            int totalNumberPages;


            if (howManyInEachPage != 0)
            {
                totalNumberPages = (totalNumberOfItens / howManyInEachPage) + 1;//2=1p + 5%3(1)
            }
            else
                totalNumberPages = 1;



            List<string> Page = new List<string>();
            if (pageNumber == "next")
                missing_page++;
            if (pageNumber == "previous")
                missing_page--;

            if (missing_page > totalNumberPages)
                missing_page = 1;

            if (missing_page < 1)
                missing_page = totalNumberPages;

            try { missing_page = Convert.ToInt32(pageNumber); }
            catch { }

            //ja tem o numero da pagina
            if (fullList != null)
            {
                Page = list2.Skip((missing_page - 1) * howManyInEachPage).Take(howManyInEachPage).ToList();
            }
            else
            {
                Page = currentMissingList.Skip((missing_page - 1) * howManyInEachPage).Take(howManyInEachPage).ToList();
            }


            return Page;

        }
        private void itensMissing(List<string> missing = null, int[] lineColumns = null)
        {

            return;
            List<string> missingList = missing;
            int offset = 0;
            Size total = new Size(panel6.Size.Width, panel7.Size.Height);
            int linhas = lineColumns[0]; //7
            int colunas = lineColumns[1]; //2


            ////////////////////////

            if (butDispose != null)
            {
                butDispose();
            }

            int count = 0;

            int breakAll = 0;


            listbuttons += () =>
            {

                foreach (string l in missingList)
                {

                }
                for (int a = 0; a < linhas; a++)
                {
                    if (breakAll == 1)
                    {
                        break;
                    }
                    for (int b = 0; b < colunas; b++)
                    {

                        try
                        {

                            if (count == missingList.Count())
                            {
                                breakAll = 1;
                                break;
                            }

                        }
                        catch
                        {
                            break;
                        }
                        int smaller = 0;
                        Button but = new Button();

                        // but.Size = new Size(total.Width / colunas - smaller, total.Height / linhas - smaller);
                        int butHeight = (panel9.Height - button8.Height) / (linhas);
                        butHeight = (panel9.Height) / (linhas);
                        total = new Size(panel6.Size.Width, panel9.Size.Height);
                        but.Size = new Size(total.Width / colunas - smaller, butHeight);
                        but.BackColor = Color.FromArgb(62, 51, 161);
                        but.Location = new Point((total.Width / colunas + smaller) * b, but.Height * a + offset);
                        Size S1 = but.Size;
                        Point p1 = but.Location;
                        int Column = b;
                        int Line = a;
                        resizeMissin += () =>
                        {
                            butHeight = (panel9.Height) / (linhas);
                            //butHeight = (panel9.Height - button8.Height) / (linhas);
                            total = new Size(panel6.Size.Width, panel9.Size.Height);
                            but.Size = new Size(total.Width / colunas - smaller, butHeight);
                            but.Location = new Point((total.Width / colunas + smaller) * Column, but.Height * Line + offset);

                            //but.Size = S1;
                            //but.Location = p1;

                        };

                        but.FlatStyle = FlatStyle.Flat;
                        but.Font = new Font("Times New Roman", 14);
                        but.ForeColor = Color.White;
                        but.TextAlign = ContentAlignment.MiddleCenter;
                        butDispose += () =>
                        {
                            but.Dispose();
                        };
                        but.Text = missingList[count].ToString().Split(vd())[0];
                        string codg = missingList[count].ToString().Split(vd())[1];
                        but.Click += (s, args) =>
                        {
                            textBox1.Focus();
                            display(codg, 1, "CHECK");
                        };
                        panel9.Controls.Add(but);

                        count++;
                    }

                }
            };

            if (listbuttons != null)
                listbuttons();





        }

        private void updateLogpanels(List<string> list)
        {
            logList_ = list;
            if (updatePanels != null)
            {
                updatePanels();
            }
        }

        private void ganarateLogPanels() //paineis
        {
            int num_of_panels = 10;
            int a = 1;
            bool Stripes = true;
            Color c1 = Color.DarkGreen;
            Color c2 = Color.LightGreen;
            Point p = new Point(0, label34.Location.Y + label34.Height + 5);
            while (a <= num_of_panels)
            {
                Panel panel = new Panel();
                panel4.Controls.Add(panel);
                panel.Size = new Size(panel4.Width, 30);

                Label l1 = new Label(); //log
                //Label l2 = new Label();//ID
                panel.Controls.Add(l1);
                //panel.Controls.Add(l2);




                //panel.Controls.Add(l1);

                resizePanel += () =>
                {
                    panel.Size = new Size(panel4.Width, panel.Size.Height);
                };
                int previewsLoc = p.Y;
                panel.Location = new Point(p.X, p.Y + panel.Height * (a));

                hideLogPanelLabels += () =>
                {
                    l1.Text = "";
                };

                updatePanels += () =>

                {
                    if (logList_ == null)
                        return;
                    int panelNumb = (panel.Location.Y - previewsLoc) / panel.Height;

                    try
                    {
                        l1.Text = logList_[panelNumb];
                        l1.AutoSize = false;
                        l1.Font = new Font("Times New Roman", 13);
                        if (panel.BackColor != Color.DarkBlue)
                        {
                            if (panelNumb % 2 == 0)
                            {
                                l1.ForeColor = Color.Black;
                            }
                            else
                            {
                                l1.ForeColor = Color.White;
                            }
                        }


                        l1.Size = new Size(panel.Width, l1.Height);
                        //l2.Text = logList_[panelNumb].Split('@')[1];
                        l1.Location = new Point(0, 0);


                        Point[] array = new Point[2] { new Point(0, 0), new Point(0, 0) };
                        array = schrink(panel.Size, 90, 90, "heightOnly");
                        //textBox2.Location = array[0];
                        l1.Location = new Point(0, panel.Height / 2 - l1.Height / 2);


                        //l2.Location = new Point(panel.Size.Width - l2.Size.Width, 0);


                    }
                    catch
                    {
                        l1.Text = "";
                        //l2.Text = "";
                    }
                };
                if (Stripes)
                {
                    Stripes = false;
                    panel.BackColor = c1;
                }
                else
                {
                    Stripes = true;
                    panel.BackColor = c2;
                }
                Color cl = panel.BackColor;
                Color cl2 = l1.ForeColor;
                revertPanelColor += () =>
                {
                    int panelNumb = (panel.Location.Y - previewsLoc) / panel.Height;
                    panel.BackColor = cl;
                    if (panelNumb % 2 == 0)
                    {
                        l1.ForeColor = Color.Black;
                    }
                    else
                    {
                        l1.ForeColor = Color.White;
                    }


                };
                panel.MouseDoubleClick += (s, args) =>
                {

                    if (panel.BackColor == Color.DarkBlue)
                    {
                        if (revertPanelColor != null)
                        {
                            revertPanelColor();
                        }
                    }
                    else
                    {
                        if (revertPanelColor != null)
                        {
                            revertPanelColor();
                        }
                        panel.BackColor = Color.DarkBlue;
                        l1.ForeColor = Color.White;
                        currentSelectedLogPanel = (panel.Location.Y - previewsLoc) / panel.Height;
                        /// criar butão aqui
                        ////
                        //
                        /*
                        Button but = new Button();
                        panel4.Controls.Add(but);
                        int c = panel.Height;
                        but.Size = new Size(c * 3, c);
                        but.Text = "ENTRADA";
                        but.Location = new Point(0,0);
                        */

                    }

                };

                l1.MouseDoubleClick += (s, args) =>
                {

                    if (panel.BackColor == Color.DarkBlue)
                    {
                        if (revertPanelColor != null)
                        {
                            revertPanelColor();
                        }
                    }
                    else
                    {
                        if (revertPanelColor != null)
                        {
                            revertPanelColor();
                        }
                        panel.BackColor = Color.DarkBlue;
                        l1.ForeColor = Color.White;
                        currentSelectedLogPanel = (panel.Location.Y - previewsLoc) / panel.Height;
                    }
                };

                a++;
            }
        }
        private string buttonMode(string mode = "")
        {

            if (mode == "next")
            {
                switch (button1.Text)
                {
                    case "ADD":
                        button1.Text = "REMOVE";
                        button1.BackColor = Color.DarkRed;
                        IO_mode("REMOVE");
                        return button1.Text;
                        break;

                    case "REMOVE":
                        button1.Text = "CHECK";
                        button1.BackColor = Color.DarkBlue;
                        IO_mode("CHECK");
                        return button1.Text;
                        break;

                    case "CHECK":
                        button1.Text = "ADD";
                        button1.BackColor = Color.DarkGreen;
                        IO_mode("ADD");
                        return button1.Text;
                        break;
                    case "ADD Stock":
                        button1.Text = "REMOVE Stock";
                        button1.BackColor = Color.LightPink;
                        IO_mode("IDLE");
                        return button1.Text;
                        break;
                    case "REMOVE Stock":
                        button1.Text = "ADD Stock";
                        button1.BackColor = Color.LightGreen;
                        IO_mode("IDLE");
                        return button1.Text;
                        break;
                    case "SEARCH":
                        button1.Text = "REMOVE";
                        button1.BackColor = Color.DarkRed;
                        IO_mode("REMOVE");
                        return button1.Text;
                        break;
                    case "EDIT":
                        button1.Text = "REMOVE";
                        button1.BackColor = Color.DarkRed;
                        IO_mode("REMOVE");
                        return button1.Text;
                        break;
                    default:
                        button1.Text = "REMOVE";
                        button1.BackColor = Color.DarkRed;
                        IO_mode("REMOVE");
                        return button1.Text;
                        break;

                }
                return button1.Text;
            }
            if (mode != "")
            {
                switch (mode)
                {
                    case "REMOVE":
                        button1.Text = "REMOVE";
                        button1.BackColor = Color.DarkRed;
                        IO_mode("REMOVE");
                        return button1.Text;
                        break;

                    case "CHECK":
                        button1.Text = "CHECK";
                        button1.BackColor = Color.DarkBlue;
                        IO_mode("CHECK");
                        return button1.Text;
                        break;

                    case "ADD":
                        button1.Text = "ADD";
                        button1.BackColor = Color.DarkGreen;
                        IO_mode("ADD");
                        return button1.Text;
                        break;
                    case "SEARCH":
                        button1.Text = "SEARCH";
                        button1.BackColor = Color.FromArgb(135, 125, 54);
                        IO_mode("IDLE");
                        textBox1.Focus();
                        return button1.Text;
                        break;
                    case "ADD Stock":
                        button1.Text = "ADD Stock";
                        button1.BackColor = Color.LightGreen;
                        IO_mode("IDLE");
                        return button1.Text;
                        break;
                    case "REMOVE Stock":
                        button1.Text = "REMOVE Stock";
                        button1.BackColor = Color.LightPink;
                        IO_mode("IDLE");
                        return button1.Text;
                    case "EDIT":

                        if (group_layout)
                        {
                            button6.Visible = true;
                            button7.Visible = true;
                            button7.Text = "MOVE OUT";
                            button6.Text = "MOVE IN";
                        }

                        button1.Text = "EDIT";
                        button1.BackColor = Color.DarkCyan;
                        IO_mode("IDLE");
                        return button1.Text;

                }

            }
            else
            {
                switch (button1.Text)
                {
                    case "REMOVE":
                        IO_mode("REMOVE");
                        return button1.Text;

                    case "CHECK":
                        IO_mode("CHECK");
                        return button1.Text;

                    case "ADD":
                        IO_mode("ADD");
                        return button1.Text;
                    case "SEARCH":
                        IO_mode("IDLE");
                        return button1.Text;
                    case "EDIT":
                        IO_mode("IDLE");
                        return button1.Text;
                    default:
                        IO_mode("IDLE");
                        return button1.Text;

                }
            }
            return button1.Text;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            buttonMode("next");
            textBox1.Focus();
        }

        private void labelQTD_text_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            // textBox1.Focus();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            // textBox1.Focus();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            //textBox1.Focus();
        }

        private void labelQTD_value_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            // textBox1.Focus();
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            // textBox1.Focus();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            //textBox1.Focus();
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearChartPanel();
            Item item = new Item();
            item.Open(currentCN);
            if (button2.Text == "CHART")
            {
                if (invisibleLabels != null)
                {
                    invisibleLabels();
                }
                button2.Text = "ID";
                lastCharsScreem = "ID";
                LastChart = item.chartList(lastCharsScreem);
                loadChart();
            }
            else
            {
                if (makeitVisible != null)
                    makeitVisible();
                button2.Text = "CHART";
                lastCharsScreem = "";
                LastChart = item.chartList(lastCharsScreem);
                loadChart();
            }
            item.Close();
            textBox1.Focus();
            if (reapearChartpanel != null)
            {
                reapearChartpanel();
            }
            cycleLifeLayouy("hide");
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            groupBox2.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
            if (deleteChartPanel != null)
            {
                deleteChartPanel();
            }
            if (invisibleLabels != null)
                invisibleLabels();
            cycleLifeLayouy();
            load_panel_layout();
            dateTimePicker1.Visible = true;
            dateTimePicker2.Visible = true;
            groupBox2.Visible = true;
           // button4.Visible = true;
           // button5.Visible = true;
        }

        private void button1_Enter(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
        private void KillSpecificExcelFileProcess(string excelFileName)
        {
            var processes = from p in Process.GetProcessesByName("EXCEL")
                            select p;

            foreach (var process in processes)
            {
                if (process.MainWindowTitle.Contains(excelFileName))
                    process.Kill();
            }
        }
        private void carregarPlanilhaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (carregarPlanilhaToolStripMenuItem.Text == "Adicionar Itens")
            {
                Folders folder1 = new Folders();
                folder1.buildStructure();
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                string message = "Tem certeza que deseja prosseguir?\r\n Todos os dados da lista de cadastro serão apagados.";
                DialogResult result = MessageBox.Show(message, "Cadastro de Itens", buttons, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                switch (result)
                {
                    case DialogResult.Yes:
                        bool planilhaCriada = folder1.createPlanilha();
                        try
                        {
                            if (planilhaCriada)
                            {
                                Process.Start(folder1.createItensPlanilhaPath());
                            }
                        }
                        catch
                        { }
                        return;
                        break;
                    case DialogResult.No:
                        return;
                        break;
                }



            }


            //deletar lista
        }

        private void adicionarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Folders folder = new Folders();
            Item item = new Item();
            bool operationSucesssfull = false;
            operationSucesssfull = item.createItemFromList();
            string[] files = Directory.GetFiles(folder.planilhaPath);
            if (operationSucesssfull)
            {
                foreach (string file in files)
                {
                    if (file.Split('\\').Last().Split('.')[0] == "AddItens")
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        try { File.Delete(file); }
                        catch { }
                    }


                }
                if (loadAllCNs != null)
                {
                    loadAllCNs();
                }
            }

        }

        private void listaGeralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Folders folder1 = new Folders();
            folder1.buildStructure();
            string listaGeralPath = folder1.listaGeralPath();
            bool value = folder1.listaGeral();

            if (value)
                Process.Start(folder1.listaGeralPath());

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {

        }
        public char vd()
        {
            return VarDash;
        }

        public List<string> updateListaGeral(bool overwriteEverything = false) // true = atualiza lista independente dela já ter sido atualizada
        {
            Folders f = new Folders();
            List<string> itens = CNList;
            if (itens.Count() == 0 || overwriteEverything)
            {
                f.listaGeral("get");
                itens = f.listaGeral1;
                CNList = itens;
            }
            return itens;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool baseResult = base.ProcessCmdKey(ref msg, keyData);

            if (keyData == Keys.Tab && textBox1.Focused)
            {
                buttonMode("SEARCH");
                textBox1.Focus();
                // MessageBox.Show("")"Tab pressed");
                return true;
            }
            if (keyData == Keys.Escape && textBox1.Focused)
            {
                buttonMode("REMOVE");
                textBox1.Text = "";
                textBox1.Focus();
                // MessageBox.Show("Tab pressed");
                return true;
            }

            if (keyData == (Keys.Control | Keys.E) && textBox1.Focused)
            {
                buttonMode("EDIT");
                return true;
            }
            if (keyData == (Keys.Control | Keys.P) && textBox1.Focused)
            {
                print(textBox1.Text, textBox7.Text) ;
                textBox1.Text = "";
                return true;
            }
            if (keyData == (Keys.Control | Keys.Q) && textBox1.Focused)
            {
                if (textBox1.Text == "")
                    return true; 
                Item item = new Item();
                string text = textBox1.Text;
                int value = 0;
                try
                {
                    value = Convert.ToInt32(text);
                }
                catch { }
                item.Open(currentCN);
                item.QTD("set", value);
                labelQTD_value.Text = text;
                textBox1.Text = "";
                item.Close();
                load_panel_layout();
                return true;
            }
            return baseResult;


        }
            private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keys)e.KeyValue == Keys.Up)
            {
                addRemoveArows(textBox1.Text, "up");

            }
            if ((Keys)e.KeyValue == Keys.Down)
            {
                addRemoveArows(textBox1.Text, "down");
            }
            

            //buttonMode
            if ((Keys)e.KeyValue == Keys.Left)
            {
                if (closeSearchForm != null)
                {
                    closeSearchForm();
                }
                textBox1.Text = "";

                

                List<string> itens = updateListaGeral();
                /*if (searchList.Count > 0)
                {
                    itens = searchList;
                }*/

                int a = 0;
                foreach (string l in itens)
                {
                    if (currentCN == "")
                    {
                        display(itens.Last().Split(vd())[0], 0, "check");
                        return;
                    }
                    string cn = l.Split(vd())[0];
                    if (cn == currentCN && currentCN != "")
                    {
                        if (a == 0)
                        {
                            display(itens.Last().Split(vd())[0], 0, "check");
                            return;
                        }
                        else
                        {
                            display(itens[a - 1].Split(vd())[0], 0, "check");
                            return;
                        }

                    }
                    a++;
                }
            }

            if ((Keys)e.KeyValue == Keys.Right)
            {
                if (closeSearchForm != null)
                {
                    closeSearchForm();
                }
                textBox1.Text = "";
                List<string> itens = updateListaGeral();
               /* if (searchList.Count > 0)
                {
                    itens = searchList;
                }*/
                int a = 0;
                foreach (string l in itens)
                {
                    if (currentCN == "")
                    {
                        display(itens[0].Split(vd())[0], 0, "check");
                        return;
                    }
                    string cn = l.Split(vd())[0];
                    if (cn == currentCN && currentCN != "")
                    {
                        if (itens.Count() == a + 1)
                        {
                            display(itens[0].Split(vd())[0], 0, "check");
                            return;
                        }
                        else
                        {
                            display(itens[a + 1].Split(vd())[0], 0, "check");
                            return;
                        }

                    }
                    a++;
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            button11.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
            closeExcel();
            ExcelClass ex = new ExcelClass();
            Folders folder = new Folders();
            string path = folder.planilhaLogsPath();
            Item item = new Item();
            item.Open(currentCN);
            ex.GerarPlanilhaLogs(item.getLogList(0, dateTimePicker1.Value.ToString().Split(' ')[0], dateTimePicker2.Value.ToString().Split(' ')[0]), path);
            item.Close();
            Process.Start(path);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ////
            textBox1.Focus();
            closeExcel();
            List<string> list = new List<string>();
            List<string> cnList = new List<string>();
            List<string> temp = new List<string>();

            ExcelClass ex = new ExcelClass();
            Folders folder = new Folders();
            string path = folder.planilhaLogsPath();
            Item item = new Item();
            cnList = item.cnList();

            foreach (string l in cnList)
            {
                item.Open(l);
                temp = item.getLogList(0, dateTimePicker1.Value.ToString().Split(' ')[0], dateTimePicker2.Value.ToString().Split(' ')[0]);
                foreach (string l2 in temp)
                {

                    list.Add(l2);
                }
                item.Close();

            }

            ex.GerarPlanilhaLogs(list, path);
            Process.Start(path);

        }

        public void closeExcel()
        {
            bool kill = false;
            string message = "Esta ação fechará todas as planilhas abertas em Excel, deseja continuar?";
            string title = "Planilha aberta";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            if (Process.GetProcessesByName("excel").Count() > 0)
            {
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                switch (result)
                {
                    case DialogResult.Yes:
                        kill = true;
                        break;
                    case DialogResult.No:
                        return;
                        break;
                }
            }
            if (kill)
            {
                foreach (var process in Process.GetProcessesByName("excel"))
                {
                    process.Kill();
                }
            }

        }
        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
            if (button6.Text == "MOVE IN")
            {
                string message = "Tem certeza de que quer mover todos os itens para dentro do inventário?";
                string title = "Movimentação de Material";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;

                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                switch (result)
                {
                    case DialogResult.Yes:
                        Item item = new Item();
                        item.Open(currentCN);
                        item.moveAllonGroup("IN");
                        label2.Text = "IN:" + item.IN_group.ToString() + "   OUT:" + (item.total_group - item.IN_group).ToString() + "   TOTAL:" + item.total_group.ToString();
                        item.Close();

                        break;
                    case DialogResult.No:
                        return;
                        break;
                }


                return;
            }
            if (button6.Text == "Extrair")
            {
                closeExcel();
                loadAllFGroups();
                ExcelClass excel = new ExcelClass();
                Item item = new Item();
                List<string> listShow = new List<string>();
                item.Open(currentCN);
                listShow = item.groupList;
                string line = "";
                line += "CN" + vd().ToString();
                line += "Posição" + vd().ToString();
                line += "P/N" + vd().ToString();
                line += "Descrição" + vd().ToString();
                line += "Responsável" + vd().ToString();
                line += "Data vistoria" + vd().ToString();
                line += "Próxima vistoria" + vd().ToString();
                line += "Rua" + vd().ToString();
                line += "Coluna" + vd().ToString();
                line += "Linha" + vd().ToString();
                listShow.Insert(0, line);
                bool a = excel.gerarPlanilhaGeral(listShow, item.grupo);
                if (a)
                {
                    Process.Start(excel.listAdress);
                }
                item.Close();
                return;
            }
            if (textBox1.Text == "")
                return;
            Item ite_ = new Item();
            ite_.Open(currentCN);
            string ID_OP = textBox1.Text;
            ID_OP = pattern("ID", ID_OP);
            ite_.addID_inLog(ID_OP, "ID", currentSelectedLogPanel);
            updateLogpanels(ite_.getLogList());
            ite_.Close();
            textBox1.Text = "";
            textBox1.Focus();

        }
        private string pattern(string mode, string name)
        {
            Item ite_ = new Item();
            ite_.Open(currentCN);
            int number = 0;
            int digitos = 0;
            int diferença = 0;
            string prefix = "";
            string sulfix = "";
            switch (mode)
            {
                case "ID":





                    try
                    {
                        number = Convert.ToInt32(name);

                        foreach (string l in idPatterns)
                        {
                            if (l.Split(';')[0] == "Default")
                            {
                                name = number.ToString();
                                prefix = l.Split(';')[1];
                                sulfix = l.Split(';')[2];
                                try { digitos = Convert.ToInt32(l.Split(';')[3]); }
                                catch { }
                                diferença = digitos - name.Count();
                                int a = 0;
                                while (a < diferença)
                                {
                                    name = "0" + name;
                                    a++;
                                }
                                break;
                            }
                        }
                    }
                    catch
                    {

                    }
                    name = prefix + name + sulfix;

                    ite_.Close();
                    return name;
                case "Fixture":

                    try
                    {
                        number = Convert.ToInt32(name);

                        foreach (string l in modelsFixturePatterns)
                        {

                            if (l.Split(';')[0] == ite_.ItemModelo)
                            {
                                name = number.ToString();
                                prefix = l.Split(';')[1];
                                sulfix = l.Split(';')[2];
                                try { digitos = Convert.ToInt32(l.Split(';')[3]); }
                                catch { }
                                diferença = digitos - name.Count();
                                int a = 0;

                                while (a < diferença)
                                {
                                    name = "0" + name;
                                    a++;
                                }

                                break;
                            }
                        }
                    }
                    catch
                    {

                    }
                    name = prefix + name + sulfix;
                    ite_.Close();
                    return name;
            }
            ite_.Close();
            return "";
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (button7.Text == "MOVE OUT")
            {
                string message = "Tem certeza de que quer mover todos os itens para fora do inventário?";
                string title = "Movimentação de Material";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;

                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                switch (result)
                {
                    case DialogResult.Yes:
                        Item item = new Item();
                        item.Open(currentCN);
                        item.moveAllonGroup("OUT");
                        label2.Text = "IN:" + item.IN_group.ToString() + "   OUT:" + (item.total_group - item.IN_group).ToString() + "   TOTAL:" + item.total_group.ToString();
                        item.Close();
                        break;
                    case DialogResult.No:
                        return;
                        break;
                }


                return;
            }
            if (textBox1.Text == "")
                return;
            Item ite_ = new Item();
            ite_.Open(currentCN);
            string ID_OP = textBox1.Text;
            ID_OP = pattern("Fixture", ID_OP);
            ite_.addID_inLog(ID_OP, "Fixture", currentSelectedLogPanel);
            updateLogpanels(ite_.getLogList());
            ite_.Close();
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            itensMissing(getMissingPage(null, missingConfig[0] * missingConfig[1], "next"), missingConfig);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            itensMissing(getMissingPage(null, missingConfig[0] * missingConfig[1], "previous"), missingConfig);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
           
        }

        private void panel9_Resize(object sender, EventArgs e)
        {

        }

        private void panel9_SizeChanged(object sender, EventArgs e)
        {
            if (resizeMissin != null)
            {
                resizeMissin();
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {


        }

        private void button10_Click(object sender, EventArgs e)
        {
            

        }
        private void but10Mode(string action = "GO")
        {
            
        }
        private void label1_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                string text = textBox1.Text;
                item.Open(currentCN);
                item.edit("nome", textBox1.Text);
                label1.Text = text;
                textBox1.Text = "";
                item.Close();
                load_panel_layout();

            }
        }
        private void renameItem(string Item, string Name)
        {
            string name = Name;
            Item item = new Item();
            switch (Item)
            {
                case "name":
                    // = textBox1.Text.Split('#')[1];

                    item.Open(currentCN);
                    name = name.Replace('_', ' ');
                    item.stream("Name", name);
                    //item.setName(name);
                    label1.Text = name;
                    item.Close();
                    return;
                case "desc":

                    item.Open(currentCN);
                    name = name.Replace('_', ' ');
                    item.stream("Descrição", name);
                    //item.setDesc(name);
                    textBox2.Text = name;
                    item.Close();
                    return;
                case "alerta":
                    int a = 0;
                    try
                    {
                        a = Convert.ToInt32(name);
                    }
                    catch { return; }
                    item.Open(currentCN);
                    item.stream("alerta", a.ToString());
                    string aviso_f = "";
                    string mode1 = item.stream("Frequencia_Aviso");
                    switch (mode1)
                    {
                        case "Diário":
                            aviso_f = "day";
                            break;
                        case "Semanal":
                            aviso_f = "week";
                            break;
                        case "Mensal":
                            aviso_f = "month";
                            break;
                    }
                    if (item.stream("alerta") == "")
                    {
                      
                    }
                    else
                    {
                      
                    }
                    item.Close();
                    load_panel_layout();
                    return;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            
        }

        private void creatSheet(List<string> itensList)
        {


        }
        private void textBox1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void textBox2_DoubleClick(object sender, EventArgs e)
        {
            if (currentCN == "")
                return;
            textBox2.Enabled = true;
        }
        // char 760 :
        private void button13_Click(object sender, EventArgs e)
        {
            if (currentCN == "")
            {
                textBox1.Focus();
                return;
            }

            if (button13.Text == "Edit")
            {
                button13.Text = "Save";
                textBox2.Focus();
                textBox2.Enabled = true;
                return;
            }
            else
            {
                Item item = new Item();
                item.Open(currentCN);
                string descrip = textBox2.Text;
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
                item.setDesc(descrip);
                item.Close();
                button13.Text = "Edit";
                textBox2.Enabled = false;
                textBox1.Focus();
            }


        }

        private void listaComprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Folders folder1 = new Folders();
            folder1.buildStructure();
            string listaGeralPath = folder1.listaGeralPath();
            bool value = folder1.listaCompras();

            if (value)
                Process.Start(folder1.listaComprasPath());
        }

        private void forcastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // fixturesA

            textBox1.Focus();
            if (currentCN == "")
            {
                return;
            }
            closeExcel();
            loadAllFGroups();
            ExcelClass excel = new ExcelClass();
            Item item = new Item();
            List<string> listShow = new List<string>();
            item.Open(currentCN);
            string model = item.ItemModelo;
            string line2 = "";
            string Position = "";
            string adress1 = "";
            string adress2 = "";
            string adress3 = "";
            int counter = 0;
            string adress = "";
            foreach (string l in CNList)
            {
                if (l.Split(vd())[2] == "Fixture")
                {
                    Item item2 = new Item();
                    item2.Open(l.Split(vd())[0]);
                    if (item2.itemPresentinGroup)
                    {
                        Position = "IN";
                    }
                    else
                    {
                        Position = "OUT";
                    }
                    if (item2.grupo == "")
                        Position = "";


                    /////////////////////////////////////////////////

                    if (adress1 != "")
                        counter++;
                    if (adress2 != "")
                        counter++;
                    if (adress3 != "")
                        counter++;

                    switch (counter)
                    {
                        case 1:
                            if (adress1 != "")
                                adress = adress1;
                            if (adress2 != "")
                                adress = adress2;
                            if (adress3 != "")
                                adress = adress3;
                            break;
                        case 2:
                            if (adress1 != "")
                            {
                                if (adress2 != "")
                                {
                                    adress = adress1 + ", " + adress2;
                                }
                                else
                                {
                                    adress = adress1 + ", " + adress3;
                                }

                            }
                            else
                            {

                            }
                            break;
                        case 3:
                            adress = adress1 + ", " + adress2 + " " + adress3;
                            break;
                    }


                    line2 += item2.ItemCN + vd().ToString(); // cn
                    line2 += item2.ItemModelo + vd().ToString(); // modelo
                    line2 += item2.ItemName + vd().ToString(); // nome
                    line2 += item2.ItemPN + vd().ToString(); // pn
                    line2 += item2.Item_description + vd().ToString(); // descrição
                    line2 += item2.dataVistoria.ToString() + vd().ToString(); // qtd
                    line2 += item2.proxVistoria + vd().ToString(); // em
                    line2 += item2.respValidação + vd().ToString(); // status
                    line2 += Position + vd().ToString(); // Posição
                    line2 += adress + vd().ToString(); // endereço
                    listShow.Add(line2);
                    line2 = "";
                }

            }

            string line = "";
            line += "CN" + vd().ToString();
            line += "Modelo" + vd().ToString();
            line += "Nome" + vd().ToString();
            line += "P/N" + vd().ToString();
            line += "Descrição" + vd().ToString();
            line += "Data Vistoria" + vd().ToString();
            line += "Próxima Vistoria" + vd().ToString();
            line += "Responsável" + vd().ToString();
            line += "Posição" + vd().ToString();
            line += "Endereço" + vd().ToString();
            listShow.Insert(0, line);
            bool a = excel.gerarPlanilhaGeral(listShow, model);
            if (a)
            {
                Process.Start(excel.listAdress);
            }
            item.Close();




        }

        private void label31_DoubleClick(object sender, EventArgs e)
        {
            string text = "";
            StringBuilder sb = new StringBuilder(text);
            sb.Remove(text.Count() - 1, 1);
            text = sb.ToString();

            Item item = new Item();
            item.Open(currentCN);


            switch (text.Split('(')[1])
            {
                case "Diário":
                   // label31.Text = text.Split('(')[0] + "(" + "Semanal" + ")";
                    item.stream("Frequencia_Aviso", "Semanal");
                    break;
                case "Semanal":
                   // label31.Text = text.Split('(')[0] + "(" + "Mensal" + ")";
                    item.stream("Frequencia_Aviso", "Mensal");
                    break;
                case "Mensal":
                   // label31.Text = text.Split('(')[0] + "(" + "Diário" + ")";
                    item.stream("Frequencia_Aviso", "Diário");
                    break;
                default:
                  //  label31.Text = text.Split('(')[0] + "(" + "Diário" + ")";
                    item.stream("Frequencia_Aviso", "Diário");
                    break;

            }
            item.Close();
        }

        private void label10_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                string text = textBox1.Text;
                item.Open(currentCN);
                item.edit("modelo", textBox1.Text);
                label10.Text = text;
                textBox1.Text = "";
                item.Close();
                load_panel_layout();
            }
            else
            {
                Form3 t = new Form3();
                t.model = label10.Text;
                t.getData();
                t.ganarateItensPanels();
                t.Show();

            }

        }

        private void label6_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                string text = textBox1.Text;
                item.Open(currentCN);
                item.edit("pn", textBox1.Text);
                label6.Text = text;
                textBox1.Text = "";
                item.Close();
                load_panel_layout();
            }
        }

        private void label8_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                string text = textBox1.Text;
                item.Open(currentCN);
                item.edit("em", textBox1.Text);
                label8.Text = text;
                textBox1.Text = "";
                item.Close();
                load_panel_layout();
            }
        }

        private void label14_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                string text = textBox1.Text;
                item.Open(currentCN);
                item.edit("qi", textBox1.Text);
                label14.Text = text;
                textBox1.Text = "";
                item.loadCycleLife();
                label28.Text = item.cycleLife.ToString("#.##");
                item.Close();
                load_panel_layout();
            }
        }

        private void label16_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                string text = textBox1.Text;
                item.Open(currentCN);
                item.edit("npiDate", textBox1.Text);
                label16.Text = text;
                textBox1.Text = "";
                item.Close();
                load_panel_layout();
            }
        }

        private void label32_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                string text = textBox1.Text;
                renameItem("alerta", text);
                textBox1.Text = "";
                load_panel_layout();
            }
        }

        private void label34_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                item.Open(currentCN);
                string text = textBox1.Text;
                double value = 0;
                try { value = Convert.ToDouble(text); }
                catch { return; }
                item.stream("valorUnitario", text);
                textBox1.Text = "";
                item.Close();
                label34.Text = "R$ " + value.ToString();
                load_panel_layout();
            }
            else
            {
                bool showdolar = true;
                ListClass list2 = new ListClass();
                settingsList = list2.Open("Settings", "settings");
                int a = 0;
                foreach (string l in settingsList.ToList())
                {
                    if (l.Split(VarDash)[0] == "DolarEnable")
                    {
                        if (l.Split(VarDash)[1] == "ON")
                        {
                            showdolar = false;
                            settingsList[a] = "DolarEnable:OFF";
                        }
                        else
                        {
                            showdolar = true;
                            settingsList[a] = "DolarEnable:ON";
                        }
                    }
                    a++;
                }
                list2.Close();
                load_panel_layout();
            }
        }

        private void scrapsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonMode("EDIT");
        }

        private void labelQTD_value_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                string text = textBox1.Text;
                int value = 0;
                try
                {
                    value = Convert.ToInt32(text);
                }
                catch { }
                item.Open(currentCN);
                item.QTD("set", value);
                labelQTD_value.Text = text;
                textBox1.Text = "";
                item.Close();
                load_panel_layout();

            }
        }

        private void atividadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 t = new Form2();
            t.Show();

        }

        private void abrirPastaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Folders folder = new Folders();
            try
            {
                Process.Start(folder.AddItensPath());
            }
            catch
            {

            }
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Item item = new Item();
            item.Open(currentCN);
            if (!item.itemExists)
                return;

            if (noneToolStripMenuItem.Text == "Desbloquear")
            {
                item.stream("itemLocked", "false");
            }
            else
            {
                item.stream("itemLocked", "true");
                buttonMode("CHECK");
                noneToolStripMenuItem.Text = "Desbloquear";
            }
            item.Close();
            load_panel_layout();
        }

        private void turnPartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Item item = new Item();
            item.Open(currentCN);
            if (!item.itemExists)
                return;
            if (item.stream("itemLocked") == "true")
            {
                noneToolStripMenuItem.Text = "Desbloquear";
            }
            else
            {
                noneToolStripMenuItem.Text = "Bloquear";
            }
        }
        string descSave = "";
        private void labelQTD_value_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Text = "Quantidade de Itens disponíveis em estoque";
        }

        private void labelQTD_value_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
        }

        private void label12_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Text = "Status atual do item";
        }

        private void label12_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
        }

        private void label14_MouseEnter(object sender, EventArgs e)
        {
        }

        private void label14_MouseLeave(object sender, EventArgs e)
        {

        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Text = "Quando a quantidade de itens no inventário estiver abaixo deste valor, uma análise de necessidade de compra deve ser feita.";
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
        }

        private void label17_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Font = new Font("Times New Roman", 12);
            textBox2.Text = "Quantidade de Itens em uso fora do inventário. Esta variável é necessária para o calculo mais preciso da quantidade de compra. Em caso da quantidade de itens em uso ser bem menor que a quantidade de itens no inventário, esta variável pode ser desprezada.";
        }

        private void label17_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
            textBox2.Font = new Font("Times New Roman", description_font_size);
        }

        private void label13_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Font = new Font("Times New Roman", 16);
            textBox2.Text = "Quantidade inicial de TurnParts. No caso de mais de um recebimento, este valor deve ser a somatória de todos os recebimentos até o presente momento.";

        }

        private void label13_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
            textBox2.Font = new Font("Times New Roman", description_font_size);
        }

        private void label21_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Font = new Font("Times New Roman", 14);
            textBox2.Text = "Média geral de ciclos de trabalho por turnpart. No caso do ciclo de trabalho resultar em uma produto PASS ou FAIL, o cycle life será a quantidade de ciclos que resultam em PASS por turnpart. ";
        }

        private void label21_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
            textBox2.Font = new Font("Times New Roman", description_font_size);
        }

        private void label27_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Font = new Font("Times New Roman", 14);
            textBox2.Text = "Média geral de ciclos de trabalho por turnpart. No caso do ciclo de trabalho resultar em uma produto PASS ou FAIL, o cycle life será a quantidade de ciclos que resultam em PASS por turnpart. ";

        }

        private void label27_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
            textBox2.Font = new Font("Times New Roman", description_font_size);
        }

        private void label19_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Font = new Font("Times New Roman", 14);
            textBox2.Text = "Quantidade total de placas produzidas que resultaram em PASS, placas FAIL devem ser desconsideradas dessa contagem pois não possuem valor útil ao Forcast.";
        }

        private void label19_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
            textBox2.Font = new Font("Times New Roman", description_font_size);
        }

        private void label20_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Font = new Font("Times New Roman", 14);
            textBox2.Text = ("Quantidade restante de placas PASS que devem ser produzidas e testadas até o final do projeto. Atualizado no término do periodo.");
        }

        private void label20_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
            textBox2.Font = new Font("Times New Roman", description_font_size);
        }

        private void label22_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Text = ("Quantidade de turnparts necessária para finalizar o projeto calculado na data do término do período.");
        }

        private void label22_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
            textBox2.Font = new Font("Times New Roman", description_font_size);
        }

        private void label23_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Text = ("Quantidade necessária para compra calculada no término do peíodo. Valores negativos representam sobra de material");
        }

        private void label23_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
            textBox2.Font = new Font("Times New Roman", description_font_size);
        }

        private void label18_MouseEnter(object sender, EventArgs e)
        {
            descSave = textBox2.Text;
            textBox2.Font = new Font("Times New Roman", 14);
            textBox2.Text = ("Data da última atualização do calculo de compra, o período é o numero de dias desde a primeira placa produzida até a última atualização do calculo de compras.");
        }

        private void label18_MouseLeave(object sender, EventArgs e)
        {
            textBox2.Text = descSave;
            textBox2.Font = new Font("Times New Roman", description_font_size);
        }

        private void quantidadeDiariaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListClass list2 = new ListClass();
            settingsList = list2.Open("Settings", "settings");
            if (chartLabelVisible)
            {
                list2.stream("showChartLabels", "false");
                chartLabelVisible = false;
            }
            else
            {
                list2.stream("showChartLabels", "true");
                chartLabelVisible = true;
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (!zoom)
            {
                picdefaultSize[0] = pictureBox1.Size.Width;
                picdefaultSize[1] = pictureBox1.Size.Height;
                panel1.Controls.Add(pictureBox1);
                pictureBox1.BringToFront();
                int[] t = { pictureBox1.Size.Width, pictureBox1.Size.Height };
                pictureBox1.Size = new Size(t[0] * 2, t[1] * 2);
                zoom = true;
            }
            else
            {
                pictureBox1.Size = new Size(picdefaultSize[0], picdefaultSize[1]);
                zoom = false;
            }

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void modelosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form9 f = new Form9();
            Item item = new Item();
            item.Open(currentCN);
            if(!item.itemExists)
            {
                return;
            }
            f.cn = currentCN;
            f.qtd = item.QTD("get").ToString();
            f.qtdCompra = item.stream("QTDcompra");
            f.qtdFIM = item.stream("QTDFinalizar");
            f.placasProduzidas = item.stream("PlacasProd");
            f.forecast = item.stream("Forcast");
            f.data = item.stream("dataDoCalculo");
            f.turnsLinha = item.stream("ItensINLine");
            f.scraps = item.getScrap().ToString();
            f.inicioIntervalo = item.stream("inicioIntervalo");
            f.fimIntervalo = item.stream("fimIntervalo");
            f.cycleLife = item.stream("CycleLife");
            f.Show();
            /*
      
        public string scraps = "5";
        public string inicioIntervalo = "";
        public string fimIntervalo = "";
        public string cycleLife = "";
             * 
             */

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button14_EXTRAIR_LISTA_DO_MODELO_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
            if (currentCN == "")
            {
                return;
            }
            closeExcel();
            loadAllFGroups();
            ExcelClass excel = new ExcelClass();
            Item item = new Item();
            List<string> listShow = new List<string>();
            item.Open(currentCN);
            string model = item.ItemModelo;
            string line2 = "";
            string Position = "";
            string adress1 = "";
            string adress2 = "";
            string adress3 = "";
            int counter = 0;
            string adress = "";
            foreach (string l in CNList)
            {
                if (l.Split(vd())[1] == model)
                {
                    counter = 0;
                    Item item2 = new Item();
                    item2.Open(l.Split(vd())[0]);
                    
                    if (item2.itemPresentinGroup)
                    {
                        Position = "IN";
                    }
                    else
                    {
                        Position = "OUT";
                    }
                    if (item2.grupo == "")
                        Position = "";


                    /////////////////////////////////////////////////
                    adress1 = item2.location3;
                    adress2 = item2.location2;
                    adress3 = item2.location;

                    if (adress1 != "")
                        counter++;
                    if (adress2 != "")
                        counter++;
                    if (adress3 != "")
                        counter++;
                  ;
                    switch (counter)
                    {
                        case 0:
                            adress = "";
                            break;
                        case 1:
                            if (adress1 != "")
                                adress = adress1;
                            if (adress2 != "")
                                adress = adress2;
                            if (adress3 != "")
                                adress = adress3;
                            break;
                        case 2:
                            if(adress1!= "")
                            {
                                if(adress2 != "")
                                {
                                    adress = adress1 + ", " + adress2;
                                }
                                else
                                {
                                    adress = adress1 + ", " + adress3;
                                }
                                
                            }
                            else
                            {
                                adress = adress2 + " " + adress3;
                            }
                            break;
                        case 3: adress = adress1 + ", " + adress2 + " " + adress3;
                            break;
                    }
            
                    line2 += item2.ItemCN + vd().ToString(); // cn
                    line2 += item2.ItemModelo + vd().ToString(); // modelo
                    line2 += item2.ItemName + vd().ToString(); // nome
                    line2 += item2.ItemPN + vd().ToString(); // pn
                    line2 += item2.Item_description + vd().ToString(); // descrição
                    line2 += item2.QTD("get").ToString() + vd().ToString(); // qtd
                    line2 += item2.Eminimo + vd().ToString(); // em
                    line2 += item2.Status + vd().ToString(); // status
                    line2 += item2.qtdInicial + vd().ToString(); // qtd in
                    line2 += item2.QTD_Finalizar_P + vd().ToString(); // fim proje
                    line2 += Position + vd().ToString(); // Posição
                    line2 += adress + vd().ToString(); // endereço
                    listShow.Add(line2);
                    line2 = "";
                }

            }

            string line = "";
            line += "CN" + vd().ToString();
            line += "Modelo" + vd().ToString();
            line += "Nome" + vd().ToString();
            line += "P/N" + vd().ToString();
            line += "Descrição" + vd().ToString();
            line += "Quantidade" + vd().ToString();
            line += "Estoque mínimo" + vd().ToString();
            line += "Status" + vd().ToString();
            line += "Quantidade Inicial" + vd().ToString();
            line += "Quantidade Fim de Projeto" + vd().ToString();
            line += "Posição" + vd().ToString();
            line += "Endereço" + vd().ToString();
            listShow.Insert(0, line);
            bool a = excel.gerarPlanilhaGeral(listShow, model);
            if (a)
            {
                Process.Start(excel.listAdress);
            }
            item.Close();
        }

        private void button14_EXTRAIR_TODOS_LOGS_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
            if (currentCN == "")
            {
                return;
            }
            closeExcel();
            ExcelClass ex = new ExcelClass();
            Folders folder = new Folders();
            string path = folder.planilhaLogsPath();
            Item item = new Item();
            item.Open(currentCN);
            List<string> logs = item.returnAllLogs(true);
        
            item.Close();
            ex.GerarPlanilhaLogs(logs, path);
            
            Process.Start(path);
        }

        private void label36_DoubleClick(object sender, EventArgs e)
        {



            if (buttonMode() == "EDIT")
            {
                return;
                if (textBox1.Text == "" && label41.Text != "Intervalo")
                    return;
                Item item = new Item();
                item.Open(currentCN);
                if (item.grupo != "")
                {
                    string date = DateTime.Today.ToString().Split(' ')[0];
                    item.stream("DATA_MANUT", date);
                    label36.Text = date;
                    int daysToNext = 0;
                    int daysIntervalo = 0;

                    try
                    {
                        daysIntervalo = Convert.ToInt32(item.stream("Days_INTERVALO"));
                    }
                    catch
                    {
                        daysIntervalo = -1; //não existe
                    }
                    if (daysIntervalo == -1)
                        daysIntervalo = 0;
                    DateTime dat = DateTime.Today.AddDays(daysIntervalo);

                    string date2 = dat.ToString().Split(' ')[0];
                    item.stream("DATA_PROX", date2);
                    label38.Text = date2;
                    item.Close();
                    load_panel_layout();

                    return;

                }


                string text = textBox1.Text;
                int value = 0;
                try
                {
                    value = Convert.ToInt32(text);
                    if (value < 0)
                        return;
                }
                catch { }
                string NewCurrent = "";

                string model = "";
                model = item.ItemModelo;
                string dateU = "";

                foreach (string l in CNList)
                {
                    if (l.Split(vd())[1] == model)
                    {
                        NewCurrent = l.Split(vd())[0]; // cn
                        Item item2 = new Item();
                        item2.Open(NewCurrent);
                        item2.stream("ForcastUsed", value.ToString());
                        string date = DateTime.Today.ToString().Split(' ')[0];
                        date = date.Split('/')[1] + "/" + date.Split('/')[0] + "/" + date.Split('/')[2];
                        item2.stream("DateUsed", date);
                        item2.loadCycleLife();
                        item2.Close();
                        if (currentCN == NewCurrent)
                            label28.Text = item2.cycleLife.ToString("#.##");
                        dateU = item2.stream("DateUsed");

                    }
                }
                textBox1.Text = "";
                label36.Text = value.ToString();
                dateU = dateU.Split('/')[1] + "/" + dateU.Split('/')[0] + "/" + dateU.Split('/')[2];
                label40.Text = dateU;
                label28.Text = item.cycleLife.ToString("#.##");
                load_panel_layout();

            }
        }

        private void label38_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                string text = textBox1.Text;
                int value = 0;
                try
                {
                    value = Convert.ToInt32(text);
                    if (value < 0)
                        return;
                }
                catch { }
                string NewCurrent = "";
                item.Open(currentCN);
                string model = "";
                model = item.ItemModelo;
                string dateU = "";
                foreach (string l in CNList)
                {
                    if (l.Split(vd())[1] == model)
                    {
                        int PP = 0;
                        NewCurrent = l.Split(vd())[0]; // cn
                        Item item2 = new Item();
                        item2.Open(NewCurrent);
                        item2.stream("PlacasProd", value.ToString());
                        PP = value;
                        string date = DateTime.Today.ToString().Split(' ')[0];
                        date = date.Split('/')[1] + "/" + date.Split('/')[0] + "/" + date.Split('/')[2];
                        item2.stream("DateUsed", date);

                        item2.loadCycleLife();
                        if (currentCN == NewCurrent)
                            label28.Text = item2.cycleLife.ToString("#.##");
                        item2.Close();
                        dateU = item2.stream("DateUsed");
                    }
                }
                textBox1.Text = "";
                label38.Text = value.ToString();
                try { dateU = dateU.Split('/')[1] + "/" + dateU.Split('/')[0] + "/" + dateU.Split('/')[2]; }
                catch { dateU = ""; }
                label40.Text = dateU;



                load_panel_layout();

            }
        }

        private void label42_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                item.Open(currentCN);


                if (item.grupo != "")
                {


                    int daysIntervalo = 0;
                    try
                    {
                        daysIntervalo = Convert.ToInt32(textBox1.Text);
                        
                        label42.Text = daysIntervalo.ToString() + " dias";
                        item.stream("Days_INTERVALO", daysIntervalo.ToString());

                        foreach (string l in item.groupList.ToList())
                        {
                            string CNnow = "";
                            CNnow = l.Split(VarDash)[0];


                            if (CNnow == currentCN)
                                continue;

                            Item item3 = new Item();

                            item3.Open(CNnow);
                            item3.stream("Days_INTERVALO", daysIntervalo.ToString());
                            item3.Close();

                        }



                        // daysIntervalo = Convert.ToInt32(item.stream("Days_INTERVALO"));
                    }
                    catch
                    {
                        daysIntervalo = -1; //não existe
                        label42.Text = "unk";
                    }

                    textBox1.Text = "";
                    string dataM = item.stream("DATA_MANUT");
                    if (dataM == "")
                    {
                        label36.Text = "unk";
                        label38.Text = "unk";
                        item.Close();
                        load_panel_layout();
                        return;
                    }

                    // pegar essa data transformar em dateTime acresentar o intervalo e salvar dnv
                    int year = 0;
                    int month = 0;
                    int day = 0;
                    DateTime date01 = new DateTime();
                    day = Convert.ToInt32(dataM.Split('/')[0]);
                    month = Convert.ToInt32(dataM.Split('/')[1]);
                    year = Convert.ToInt32(dataM.Split('/')[2]);
                    date01 = new DateTime(year, month, day, 0, 0, 0);
                    date01 = date01.AddDays(daysIntervalo);
                    string date02 = date01.ToString().Split(' ')[0];

                    item.stream("DATA_PROX", date02);
                    label38.Text = date02;


                    item.Close();
                    load_panel_layout();

                    return;

                }


                string text = textBox1.Text;
                int value = 0;
                try
                {
                    value = Convert.ToInt32(text);
                    if (value < 0)
                        return;
                }
                catch { }

                item.stream("ItensINLine", value.ToString());
                item.loadCycleLife();
                label28.Text = item.cycleLife.ToString("#.##");

                item.Close();

                string dateU = "";
                textBox1.Text = "";
                label42.Text = value.ToString();
                string date = DateTime.Today.ToString().Split(' ')[0];
                dateU = item.stream("DateUsed");
                try { dateU = dateU.Split('/')[1] + "/" + dateU.Split('/')[0] + "/" + dateU.Split('/')[2]; }
                catch { dateU = ""; }
                label40.Text = dateU;
                load_panel_layout();

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //go abobora
            if (comboBox1.Text == "TODOS")
            {
                List<string> list = new List<string>();

                textBox1.Focus();
                closeExcel();
                ExcelClass ex = new ExcelClass();
                Folders folder = new Folders();
                string path = folder.planilhaLogsPath();

                foreach(string cnL in CNList)
                {
                    List<string> list1 = new List<string>();
                    Item item = new Item();
                    item.Open(cnL.Split(VarDash)[0]);
                    list1 = item.getLogList(0, dateTimePicker1.Value.ToString().Split(' ')[0], dateTimePicker2.Value.ToString().Split(' ')[0]);
                    item.Close();
                    list.AddRange(list1);
                }


                ex.GerarPlanilhaLogs(list, path);
                
                Process.Start(path);
                return;
            }
            if (comboBox1.Text == "ITEM")
            {
                textBox1.Focus();
                closeExcel();
                ExcelClass ex = new ExcelClass();
                Folders folder = new Folders();
                string path = folder.planilhaLogsPath();
                Item item = new Item();
                item.Open(currentCN);
                ex.GerarPlanilhaLogs(item.getLogList(0, dateTimePicker1.Value.ToString().Split(' ')[0], dateTimePicker2.Value.ToString().Split(' ')[0]), path);
                item.Close();
                Process.Start(path);
                return;
            }
            if (comboBox1.Text == "MODELO")
            {
                List<string> list = new List<string>();

                textBox1.Focus();
                closeExcel();
                ExcelClass ex = new ExcelClass();
                Folders folder = new Folders();
                string path = folder.planilhaLogsPath();
                Item itemM = new Item();
                itemM.Open(currentCN);
                string Cmodel = itemM.ItemModelo;

                foreach (string cnL in CNList)
                {
                    List<string> list1 = new List<string>();
                    Item item = new Item();
                    item.Open(cnL.Split(VarDash)[0]);
                    if (Cmodel == item.ItemModelo)
                    {
                        list1 = item.getLogList(0, dateTimePicker1.Value.ToString().Split(' ')[0], dateTimePicker2.Value.ToString().Split(' ')[0]);
                        item.Close();
                        list.AddRange(list1);
                    }
                    else
                    {
                        item.Close();
                    }
                    
                }


                ex.GerarPlanilhaLogs(list, path);

                Process.Start(path);
                return;
            }
        }

        private void label40_DoubleClick(object sender, EventArgs e)
        {
            //item.stream("Responsavel");

            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                item.Open(currentCN);
                string Name = textBox1.Text;
                textBox1.Text = "";

                if (item.grupo != "")
                {
                    item.stream("Responsavel", Name);
                    label40.Text = Name;
                    item.Close();
                    load_panel_layout();
                }
            }

            /*
             * 
             * ItensINLine
             * label17.Text = "Itens na linha";
                label18.Text = "Termino do Periodo";
                label19.Text = "Placas produzidas";
                label20.Text = "Forcast";
                label21.Text = "Cycle Life";
                label22.Text = "Quantidade necessaria"; //ID PANEL
                label23.Text = "Quantidade para Compra";
             * 
             */
        }

        private void relatórioToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        





    private void button14_Click(object sender, EventArgs e)
        {

            textBox1.Focus();
            buttonMode("SEARCH");

            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            

            ////////// SOMENTE DISPLAY LIST!!!!!/////////////////////////
            if (!callDisplayBoll)                           /////////////
                return;                                     ////////////
            callDisplayBoll = false;

            string searchButtonReturnTO = "CHECK";
            string a = "";
            if(searchButtonReturnTo != "")
            {
                searchButtonReturnTO = searchButtonReturnTo;
            }
            else
            {
                searchButtonReturnTo = config("searchButtonReturnTO");
                searchButtonReturnTO = searchButtonReturnTo;
            }
            
            a = searchButtonReturnTO;
            if (a != "REMOVE" && a != "CHECK" && a != "ADD" )
            {
                Console.WriteLine("a "+ a);
                config("searchButtonReturnTO","CHECK");
                searchButtonReturnTO = "CHECK";
            }
            display(callDisplayCN, -1, searchButtonReturnTO);
            callDisplayCN = "";                             ////////////

            Console.WriteLine("Tick 1");

            /////////////////////////////////////////////////////////////
        }

        private void setIOSerialPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonMode("REMOVE");
            textBox1.Text = "SetIO_COM=";
        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadNoItemLayout();
            textBox2.Text += mainPort.PortName.ToString() + "\r\n";
            textBox2.Text += "IsOpen = " + mainPort.IsOpen.ToString()+"\r\n"; ;


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (ActiveForm != this)
            {
                IO_mode("IDLE");
            }
            else { buttonMode(); }
                if (!mainPort.IsOpen)
            {
                string com = "";
                try
                {
                    com = config("COM_port");
                    lastCOM = com;
                }
                catch
                {
                    if (lastCOM != "")
                    {
                        com = lastCOM;
                    }
                    else
                    {
                        return;
                    }
                }

               
                if (com == "")
                    return;
                mainPort = new SerialPort(com, 9600);
                try { mainPort.Open(); } catch { return; }
            }
            string VOID_String = "hashfjhbgfhjgvycgmwkkjiof74rrrfwerfwev"; //RANDOM
            string myLine = VOID_String;
            if (mainPort.IsOpen)
            {
                while (mainPort.BytesToRead > 0)
                {
                    myLine = mainPort.ReadLine();
                    myLine = myLine.Substring(0, myLine.Length - 1);
                }
                if (myLine != VOID_String)
                {
             
                }
                    //label1.Text = myLine;
            }

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            IO_mode("IDLE");
            //abacaxi
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            buttonMode();
        }

        private void configuraçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sortListaGeralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelClass ec = new ExcelClass();
            ListClass lc = new ListClass();
            lc.Open("Mestra", "ListaGeral");
            List<string> excelList = new List<string>();
            excelList = lc.mainList.ToList();
            excelList = ec.sortGeralList(excelList);
            lc.mainList = excelList;
            lc.Close();

        }

        private void gerarBackUPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dotheBackUp();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            print(textBox1.Text, textBox7.Text);
            textBox1.Text = "";
            textBox1.Focus();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                print(textBox1.Text, textBox7.Text);
                textBox1.Text = "";
                textBox1.Focus();
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            button11.Focus();
        }

        private void comboBox1_ValueMemberChanged(object sender, EventArgs e)
        {
            //button11.Focus();
        }

        private void label58_DoubleClick(object sender, EventArgs e)
        {
            if (buttonMode() == "EDIT")
            {
                if (textBox1.Text == "")
                    return;
                Item item = new Item();
                string text = textBox1.Text;
                item.Open(currentCN);
                item.stream("versao", textBox1.Text);
                label58.Text = text;
                textBox1.Text = "";
                item.Close();
                load_panel_layout();

            }
        }

        private void label59_DoubleClick(object sender, EventArgs e)
        {
            //OPEN VERSAO FORM

            Form7 t = new Form7();
            Item item = new Item();
            item.Open(currentCN);
            t.grupo = item.grupo;
            t.cn = item.ItemCN;
            t.TopLevel = true;
           // f.Dock = DockStyle.Fill;
            

            t.Show();

        }

        private void label58_Click(object sender, EventArgs e)
        {

        }

        private void label58_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {

                case MouseButtons.Left:
                    // Left click
                    break;

                case MouseButtons.Right:

                    Form7 t = new Form7();
                    Item item = new Item();
                    item.Open(currentCN);
                    string versao = t.getVersao(item.grupo);
                    if(versao == "")
                    {
                        MessageBox.Show("Nenhuma versão cadastrada");
                        return;
                    }

                    string text = "Atualizar para versão atual?";
                   // MessageBox.Show("Atualizar para versão atual?");
                    DialogResult dialogResult = MessageBox.Show(text, "UPGRADE para " + versao, MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Item item3 = new Item();

                        
                        item.stream("versao", versao);
                        Console.WriteLine("salvo " + versao + " em " + item.ItemCN);
                        
                        label58.Text = versao;
                        
                        string time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToString();
                        time = time.Split(' ')[0];
                        label36.Text = item.stream("DATA_MANUT",time);
                        item.Close();
                        load_panel_layout();



                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                    }
                    break;
                    
}
        }

        private void pOWERBIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Folders folder1 = new Folders();
            folder1.buildStructure();
            string listaGeralPath = folder1.listaGeralPath();
            bool value = folder1.listaPowerBI();

            if (value)
                Process.Start(folder1.listaGeralPath());
        }
    }



    // The example displays the following output:
    //       Does 'This is a string.' contain 'this'?
    //          Ordinal: False
    //          OrdinalIgnoreCase: True




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