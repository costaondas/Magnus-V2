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
    public partial class Form20 : Form
    {
        public List<string> list= new List<string>();
        public List<string> head = new List<string>();
        public string title = "";

        public class item
        {
            public string cn = "";
            public int qtd = 0;
            public int PossibleKits = 0;
        }
        public List<item> SKUlist = new System.Collections.Generic.List<item>();
        public Form20()
        {
            InitializeComponent();
        }
        public void callDisplay(string cn)
        {
            Item item = new Item();
            item.Open(cn);
            label8.Text = cn;
            label7.Text = item.ItemName;
            label9.Text = item.QTD("get").ToString();
            Folders folder = new Folders();
            if (item.picture != "")
            {
                pictureBox2.Image = folder.image(item.picture);
            }
            else
            {
                pictureBox2.Image = folder.image(item.ItemCN);
            }
        }

        
        private void Form20_Load(object sender, EventArgs e)
        {
            ListClass lc = new ListClass();
            char vd = lc.VarDash;
            char vdP = lc.VarDashPlus;
            label4.Text = title;
            Form12 form = new Form12();
            form.call20 = true;
            form.displayList = list;
            form.headList = head;
            form.dontFocus = true ;
            form.TopLevel = false;
            form.FormBorderStyle= FormBorderStyle.None;
            splitContainer1.Panel1.Controls.Add(form);
            form.Show();
            foreach(string l in list)
            {
                string cn = l.Split(vdP)[0].Split(vd)[1];
                lc.mainList = l.Split(vdP).ToList();
                string qtd = lc.stream("QTD");
                int qtd_ = 0;
                try
                {
                    qtd_ = Convert.ToInt32(qtd);
                }
                catch { }
                item i = new item();
                i.cn = cn;
                i.qtd = qtd_;
                SKUlist.Add(i);
            }
            ListClass lc2 = new ListClass();
            lc2.Open("Mestra");
            int total = lc2.mainList.Count;
            int count = 1;
            foreach(string l in lc2.mainList.ToList())
            {
                
                string cn = l.Split(vdP)[0].Split(vd)[1];
                this.Text = "Analisando " + cn + " (" + count.ToString() +  "//" + total.ToString() + ")"; 
                string qtd = "";
                
                foreach(item i in SKUlist.ToList())
                {
                    if(i.cn == cn)
                    {
                        lc.mainList = l.Split(vdP).ToList();
                        qtd = lc.stream("QTD");
                        int qtd_ = 0;
                        try
                        {
                            qtd_ = Convert.ToInt32(qtd);
                        }
                        catch { }
                        if(i.qtd == 0)
                        {
                            i.PossibleKits = 0;
                        }
                        else
                        {
                            i.PossibleKits = qtd_ / i.qtd;
                        }
                        
                    }
                }
                Application.DoEvents();
                count++;
            }
            int kits = 0;
            if(SKUlist!= null)
            {
                kits = SKUlist[0].PossibleKits;
            }
            foreach(item i in SKUlist)
            {
                if(i.PossibleKits < kits)
                {
                    kits = i.PossibleKits;
                }
            }
            this.Text = "Calculo de Kits";
            label2.Text = kits.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
