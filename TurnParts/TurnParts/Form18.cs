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

namespace MagnusSpace
{
    
    public partial class Form18 : Form
    {
        Color colorON = Color.FromArgb(255, 255, 192);
        Color colorOFF = Color.FromArgb(241, 255, 227);
        public Form18()
        {
            InitializeComponent();
        }

        private void Form18_Load(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            TextBox bt = sender as TextBox;
            bt.BackColor = colorON;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            TextBox bt = sender as TextBox;
            bt.BackColor = colorON;
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            TextBox bt = sender as TextBox;
            bt.BackColor = colorON;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            TextBox bt = sender as TextBox  ;
            bt.BackColor = colorOFF;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            TextBox bt = sender as TextBox;
            bt.BackColor = colorOFF;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            TextBox bt = sender as TextBox;
            bt.BackColor = colorOFF;
        }

        private void textBox2_BackColorChanged(object sender, EventArgs e)
        {
            label1.ForeColor = textBox2.BackColor;
        }

        private void textBox1_BackColorChanged(object sender, EventArgs e)
        {
            label2.ForeColor = textBox1.BackColor;
        }

        private void textBox3_BackColorChanged(object sender, EventArgs e)
        {
            label3.ForeColor = textBox3.BackColor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool isAllgood = true;
            foreach(var t in splitContainer1.Panel2.Controls)
            {
                if(t.GetType() != typeof(TextBox))
                {
                    continue;
                }
                TextBox tx = t as TextBox;
                string text = tx.Text.Trim();
                if (text == "")
                {
                    tx.Text = "";
                    tx.BackColor = Color.DarkGray;
                    isAllgood = false;
                }
                    
            }
            if (!isAllgood)
                return;
            string path = "";
            string client = textBox2.Text;
            string modelo = textBox1.Text;
            string sku = textBox3.Text;
            Folders folder = new Folders();
            path = folder.skuPath + "\\" + client;
            folder.build(path);
            path += "\\" + modelo;
            folder.build(path);
            ListClass lc = new ListClass();
            lc.Open(sku,path);
            lc.Close();
            //lc.streamPlus(sku);
            Form1 form = new Form1();
            form = System.Windows.Forms.Application.OpenForms["Form1"] as Form1;
            form.loadSKUtoolstrip();
            //form.refresh();
            this.Close();
        }
    }
}
