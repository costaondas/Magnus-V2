using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace MagnusSpace
{
    public partial class Form4 : Form
    {

        List<string> itensList = new List<string>();
        char VarDash = ((char)887);
        char VarsubDash = ((char)888); // to split vardash
        public Bitmap lastbitmap;
        public Bitmap OGbitmap;
        public Point Pfixture = new Point(0, 0);

        
        /// </MODO DE EDIÇAO DO FIXTURE >
        /// FIXTURE SENDO EDITADO EM TEMPO REAL NA PICTUREBOX
        public bool editmode = false;
        public string CNbeingEdited = "";
        public int lastItemEditedXposition = 0;
        public int lastItemEditedYposition = 0;
        int multiplier = 1;
        /////////////////////////////////////////////
        ///
        public SolidBrush solidBrush = new SolidBrush(Color.RoyalBlue);

        public string getSpec(string spec_to_SEARCH, string CN_to_SEARCH = "")
        {
            string CN = CN_to_SEARCH; ;
            if (CN_to_SEARCH == "")
                CN = CNbeingEdited;
            if (CN == "")
                return "";
            int index = 0;
            index = CNtoListIndex(CN);
            if (index < 0)
                return "";


            string name = "";
            string value = "";
            int a = 0;
            List<string> specs = itensList[index].Split(VarDash).ToList();
            foreach (string spec in specs)
            {
                name = spec.Split(VarsubDash)[0];
                value = spec.Split(VarsubDash)[1];
                if (name == spec_to_SEARCH)
                {
                    return value;
                }

            }
            return "";
        }
        private void editedFixtoreADDposition(int X = 0,int Y = 0)//EDITA A POSIÇÂO DO FIXTURE QUANDO EDITADO
        {
            if (CNbeingEdited == "")
                return;
            List<string> position = new List<string>();
            int newXvalue = 0;
            int newYvalue = 0;
            try
            {
                newXvalue = Convert.ToInt32(getSpec("X"));
                newXvalue += X;
                if (newXvalue < 0)
                    newXvalue = 0;
            }
            catch { }
            try
            {
                newYvalue = Convert.ToInt32(getSpec("Y"));
                newYvalue += Y;
                if (newYvalue < 0)
                    newYvalue = 0;
            }
            catch { }
            position.Add("CN" + VarsubDash + CNbeingEdited);
            position.Add("X" + VarsubDash + newXvalue.ToString());
            position.Add("Y" + VarsubDash + newYvalue.ToString());
            createItem(position);
            pictureBox1.Refresh();
        }
        
        //EDITAR O CN NA TEXTBOX
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                if (textBox4.Text != "")
                {
                    CNbeingEdited = textBox4.Text;
                    button4.Text = lastItemEditedXposition.ToString();
                    button12.Text = lastItemEditedYposition.ToString();
                    if (textBox2.Text == "")
                        textBox2.Text = "10";
                    if (textBox3.Text == "")
                        textBox3.Text = "10";
                    int width = 0;
                    int height = 0;
                    List<string> editItem = new List<string>();

                    try
                    {
                        width = Convert.ToInt32(textBox2.Text);
                        height = Convert.ToInt32(textBox3.Text);
                    }
                    catch { }
                    editItem.Add("CN" + VarsubDash + CNbeingEdited);
                    editItem.Add("WIDTH" + VarsubDash + width.ToString());
                    editItem.Add("HEIGHT" + VarsubDash + height.ToString());
                    createItem(editItem);
                    pictureBox1.Refresh();

                }
            }
        }
        //EDITAR O WIDTH NA TEXTBOX
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox2.Text != ""&& (Keys)e.KeyChar == Keys.Enter)
            {
                if (CNbeingEdited == "")
                    return;
                int width = 0;
                int height = 0;
                List<string> editItem = new List<string>();

                try
                {
                    width = Convert.ToInt32(textBox2.Text);
                    height = Convert.ToInt32(textBox3.Text);
                }
                catch { }
                editItem.Add("CN" + VarsubDash + CNbeingEdited);
                editItem.Add("WIDTH" + VarsubDash + width.ToString());
                editItem.Add("HEIGHT" + VarsubDash + height.ToString());
                createItem(editItem);
                pictureBox1.Refresh();
            }
        }
        //EDITAR O HEIGHT NA TEXTBOX
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox3.Text != ""&& (Keys)e.KeyChar == Keys.Enter)
            {
                if (CNbeingEdited == "")
                    return;
                int width = 0;
                int height = 0;
                List<string> editItem = new List<string>();

                try
                {
                    width = Convert.ToInt32(textBox2.Text);
                    height = Convert.ToInt32(textBox3.Text);
                }
                catch { }
                editItem.Add("CN" + VarsubDash + CNbeingEdited);
                editItem.Add("WIDTH" + VarsubDash + width.ToString());
                editItem.Add("HEIGHT" + VarsubDash + height.ToString());
                createItem(editItem);
                pictureBox1.Refresh();
            }
        }


       
        
        public Form4()
        {
            InitializeComponent();
        }
        public void loadItensListfromFile()
        {
            ListClass lc = new ListClass();
            lc.Open("itensList", "listFolder");
            itensList = lc.mainList;
            lc.Close();
            foreach(string item in itensList)
            {
                textBox1.Text += item;
            }

        }
        private void Form4_Load(object sender, EventArgs e)
        {
            loadItensListfromFile();
            Folders folder = new Folders();
            pictureBox1.Image = folder.image(groupBox1.Text);
            Console.WriteLine("LAST PATH " + folder.fulllastImagePath);
        }
        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
        {
            foreach(string item in itensList)
            {
                if (item == "")
                    continue;
                //VARS IN SPEC FILE
                ///////////////////////////////
                int red = 0;
                int green = 0;
                int blue = 0;
                int X = 0;
                int Y = 0;
                int width = 0;
                int height = 0;
                ///////////////////////////////


                //LOCAL VARS
                ///////////////////////////////
                string Vname = "";
                string value = "";
                List<string> itemSpecs = new List<string>();
                try
                {
                    itemSpecs = item.Split(VarDash).ToList();
                    foreach(string subSpec in itemSpecs)
                    {
                        Vname = subSpec.Split(VarsubDash)[0];
                        value = subSpec.Split(VarsubDash)[1];
                        switch (Vname)
                        {
                            case "RED":
                                red = Convert.ToInt32(value);
                                break;
                            case "GREEN":
                                green = Convert.ToInt32(value);
                                break;
                            case "BLUE":
                                blue = Convert.ToInt32(value);
                                break;
                            case "X":
                                X = Convert.ToInt32(value);
                                break;
                            case "Y":
                                Y = Convert.ToInt32(value);
                                break;
                            case "WIDTH":
                                width = Convert.ToInt32(value);
                                break;
                            case "HEIGHT":
                                height = Convert.ToInt32(value);
                                break;
                        }
                    }
                }
                catch
                {

                }
                //AFTER GET ALL FILE VARS, ITS TIME TO DRAW THE FIXTURE ON PICTURE BOX
                ///////////////////////////////////////////
                Color bruchColor = Color.FromArgb(red, green, blue);
                Rectangle rect = new Rectangle(X, Y, width, height);
                e.Graphics.FillRectangle(new SolidBrush(bruchColor), rect);
            }
            
        }
        public int CNtoListIndex(string CN) // retorna -1 se não existir CN
        {
            Console.WriteLine("SEARCH CN <"+CN+">");
            string name = "";
            string value = "";
            int a = 0;
            foreach(string item in itensList)
            {
                Console.WriteLine(item);
                List<string> specs = item.Split(VarDash).ToList();
                foreach(string spec in specs)
                {
                    name = spec.Split(VarsubDash)[0];
                    value = spec.Split(VarsubDash)[1];
                    Console.WriteLine("NAME " + name + " VALUE " + value);
                    if (name == "CN" && value == CN)
                    {
                        return a;
                    }
                        
                }
                a++;
                
            }
            return -1;
        }
        public void createItem(List<string> specs) //LIST OF SINGLE ITEM SPECS
        {
            //ITEM SPECS
            ////////////////////////
            string CN = "";
            string red = "";
            string green = "";
            string blue = "";
            string X = "";
            string Y = "";
            string width = "";
            string height = "";

            ////////////////////////

            string name = "";
            string value = "";
            foreach(string spec in specs)
            {
                name = spec.Split(VarsubDash)[0];
                value = spec.Split(VarsubDash)[1];
                switch (name)
                {
                    case "CN":
                        CN = value;
                        Console.WriteLine("<" + value + "> CN que será cadastrado");
                        break;
                    case "RED":
                        red = value;
                        break;
                    case "GREEN":
                        green = value;
                        break;
                    case "BLUE":
                        blue = value;
                        break;
                    case "X":
                        X =value;
                        break;
                    case "Y":
                        Y = value;
                        break;
                    case "WIDTH":
                        width = value;
                        break;
                    case "HEIGHT":
                        height = value;
                        break;
                }
            }
            ListClass lc = new ListClass();
            lc.Open("itensList", "listFolder");
            if (CN == "")
                return;
            


            string itemLine = "";

            int index = CNtoListIndex(CN);
            if (index < 0) // item não existem
            {
                Console.WriteLine("CN <"+ CN+ "> Não EXISTE");
                itemLine += "CN" + VarsubDash + CN + VarDash;
                ////////////////////////////////////////////////////
                if (red == "")
                    red = "0";
                itemLine += "RED" + VarsubDash + red + VarDash;
                ////////////////////////////////////////////////////
                if (green == "")
                    green = "0"; 
                itemLine += "GREEN" + VarsubDash + green + VarDash;
                ////////////////////////////////////////////////////
                if (blue == "")
                    blue = "0";
                itemLine += "BLUE" + VarsubDash + blue + VarDash;
                ////////////////////////////////////////////////////
                if (X == "")
                    X = "0";
                itemLine += "X" + VarsubDash + X + VarDash;
                ////////////////////////////////////////////////////
                if (Y == "")
                    Y = "0";
                itemLine += "Y" + VarsubDash + Y + VarDash;
                ////////////////////////////////////////////////////
                if (width == "")
                    width = "10";
                itemLine += "WIDTH" + VarsubDash + width + VarDash;
                ////////////////////////////////////////////////////
                if (height == "")
                    height = "0";
                itemLine += "HEIGHT" + VarsubDash + height; // LAST DONT HAVE VARDASH
                ////////////////////////////////////////////////////
                lc.mainList.Add(itemLine);
            }
            else//item existe
            {
                Console.WriteLine("CN <" + CN + "> EXISTE");
                List<string> itemSpecs = lc.mainList[index].Split(VarDash).ToList(); ///ITEM QUE PRECISA SER EDITADO
                foreach (string spec in itemSpecs)
                {
                    name = spec.Split(VarsubDash)[0];
                    value = spec.Split(VarsubDash)[1];
                    switch (name)
                    {
                        case "RED":
                            if(red == "")//NÂO ESTA SENDO EDITADO
                            {
                                red = value;
                            }
                            break;
                        case "GREEN":
                            if (green == "")//NÂO ESTA SENDO EDITADO
                            {
                                green = value;
                            }                           
                            break;
                        case "BLUE":
                            if (blue == "")//NÂO ESTA SENDO EDITADO
                            {
                                blue = value;
                            }
                            break;
                        case "X":
                            if (X == "")//NÂO ESTA SENDO EDITADO
                            {
                                X = value;
                            }                         
                            break;
                        case "Y":
                            if (Y == "")//NÂO ESTA SENDO EDITADO
                            {
                                Y = value;
                            }
                            break;
                        case "WIDTH":
                            if (width == "")//NÂO ESTA SENDO EDITADO
                            {
                                width = value;
                            }
                            break;
                        case "HEIGHT":
                            if (height == "")//NÂO ESTA SENDO EDITADO
                            {
                                height = value;
                            }
                            break;
                    }

                }
                itemLine = "";
                itemLine += "CN" + VarsubDash + CN + VarDash;
                ////////////////////////////////////////////////////
                if (red == "")
                    red = "0";
                itemLine += "RED" + VarsubDash + red + VarDash;
                ////////////////////////////////////////////////////
                if (green == "")
                    green = "0";
                itemLine += "GREEN" + VarsubDash + green + VarDash;
                ////////////////////////////////////////////////////
                if (blue == "")
                    blue = "0";
                itemLine += "BLUE" + VarsubDash + blue + VarDash;
                ////////////////////////////////////////////////////
                if (X == "")
                    X = "0";
                itemLine += "X" + VarsubDash + X + VarDash;
                ////////////////////////////////////////////////////
                if (Y == "")
                    Y = "0";
                itemLine += "Y" + VarsubDash + Y + VarDash;
                ////////////////////////////////////////////////////
                if (width == "")
                    width = "10";
                itemLine += "WIDTH" + VarsubDash + width + VarDash;
                ////////////////////////////////////////////////////
                if (height == "")
                    height = "0";
                itemLine += "HEIGHT" + VarsubDash + height; // LAST DONT HAVE VARDASH
                ////////////////////////////////////////////////////
                lc.mainList[index] = itemLine;

            }
            lc.Close();
            itensList = lc.mainList;
        }
        
        private void button6_Click(object sender, EventArgs e) //UP EDITED FIXTURE
        {
            editedFixtoreADDposition(0,-1*multiplier);
        }
        private void button7_Click(object sender, EventArgs e) //DOWN EDITED FIXTURE
        {
            editedFixtoreADDposition(0, 1 * multiplier);
        }
        private void button8_Click(object sender, EventArgs e)//LEFT EDITED FIXTURE
        {
            editedFixtoreADDposition(-1 * multiplier,0);
        }
        private void button9_Click(object sender, EventArgs e)//RIGHT EDITED FIXTURE
        {
            editedFixtoreADDposition(1 * multiplier, 0);
        }

        /// <summary>
        /// ///////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>



        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
           

        }

        private void button3_Click(object sender, EventArgs e)
        {
            return;
            Folders folder = new Folders();
            string path = folder.fxPositionPath();
            List<string> list = new List<string>();
            string fistLine = "";
            // list.Add(fistLine);
            ExcelClass excel = new ExcelClass();
            bool value = excel.closeExcel();
            if (!value)
                return;
            excel.createFile(list, folder.createFXpositionsPlanilhaPath());
            Process.Start(folder.createFXpositionsPlanilhaPath());
        }

        private void novoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        bool pinpoint = false;
        private void button4_Click(object sender, EventArgs e)
        {
            return;
            pinpoint = !pinpoint;
            button4.Text = pinpoint.ToString();
            if (pinpoint)
            {
                prebuild();
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            
        }
  
        private void pictureBox1_Move(object sender, EventArgs e)
        {
            
        }
        public void prebuild()
        {
            return;
            if (pinpoint)
            {


                

            }
        }
        int a = 0;
        
        public void addFixturePosition(int x = 0,int y = 0)
        {
            Pfixture = new Point(Pfixture.X + x*multiplier, Pfixture.Y + y*multiplier);
            if(Pfixture.X + x*multiplier < 0)
            {
                Pfixture = new Point(0, Pfixture.Y + y * multiplier);
            }
            if (Pfixture.Y + y * multiplier < 0)
            {
                Pfixture = new Point(Pfixture.X + x * multiplier, 0);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //SAVE ITEM
            string spec = "";
            List<string> item = new List<string>();
            ////////////////////////////
            spec = "CN" + VarsubDash.ToString() + textBox4.Text;
            item.Add(spec);
            ////////////////////////////
            spec = "WIDTH" + VarsubDash.ToString() + textBox2.Text;
            item.Add(spec);
            ////////////////////////////
            spec = "HEIGHT" + VarsubDash.ToString() + textBox3.Text;
            item.Add(spec);
            ////////////////////////////
            createItem(item);
            loadItensListfromFile();
            pictureBox1.Refresh();
        }

       
        public void display()
        {
            
            Color rectColor = Color.Red;
            using (Graphics graphics = Graphics.FromImage((Image)lastbitmap))
            {
                using (System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(rectColor))
                {
                    int x = Pfixture.X;
                    int y = Pfixture.Y;
                    int side = Convert.ToInt32(40);
                    graphics.FillRectangle(myBrush, new Rectangle(x, y, side, side));

                }
                pictureBox1.Image = (Image)lastbitmap;
            }

         }




        bool isMouseDown = false;

        

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            textBox1.Text = "down";
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
          
            
        }

        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            textBox1.Text = "up";
        }
    

    private void button10_Click(object sender, EventArgs e)
        {
            switch (button10.Text)
            {
                case "slow":
                    button10.Text = "medium";
                    multiplier = 10;
                    break;
                case "medium":
                    button10.Text = "fast";
                    multiplier = 30;
                    break;
                case "fast":
                    button10.Text = "slow";
                    multiplier = 1;
                    break;

            }
        }

        

        

       

        

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void imagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Folders folder = new Folders();
            try
            {
                Process.Start(folder.picFolder());
            }
            catch
            {

            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
