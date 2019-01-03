using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MikuMikuWMP
{
    public partial class Form3 : Form
    {

        private Point mouse_offset;

        public Form3()
        {
            InitializeComponent();
            label1.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            label2.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            label3.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            label4.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            label5.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            label6.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            label7.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            label8.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            label9.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            label10.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            label11.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            pictureBox1.BackgroundImage = Form1.skins.getImage("Close");
            BackgroundImage = Form1.skins.getImage("MusicLsWindow");
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Form1.skins.getImage("CloseH");
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Form1.skins.getImage("Close");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                Location = mousePos;
            }
        }
    }
}
