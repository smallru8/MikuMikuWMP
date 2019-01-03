using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AxWMPLib;

namespace MikuMikuWMP
{

    public partial class Form4 : Form, IMessageFilter
    {

        private Point mouse_offset;

        private bool timerFlag = true;

        public Form4()
        {
            InitializeComponent();
            wmp.uiMode = "none";
            wmp.enableContextMenu = false;
        }

        private void Form4_Load(object sender, EventArgs e)

        {

            timer1.Interval = 200;
            timer1.Start();
            pictureBox1.BackgroundImage = Form1.skins.getImage("PlayerBtnX");
            pictureBox2.BackgroundImage = Form1.skins.getImage("Max");
            pictureBox3.BackgroundImage = Form1.skins.getImage("Min");
            BackgroundImage = Form1.skins.getImage("Player");

        }
        private void Form4_FormClosing(object sender, FormClosingEventArgs e)

        {

        }


        #region IMessageFilter Members

        private const UInt32 WM_KEYDOWN = 0x0100;

        public bool PreFilterMessage(ref Message m)

        {

            if (m.Msg == WM_KEYDOWN)

            {

                Keys keyCode = (Keys)(int)m.WParam & Keys.KeyCode;

                if (keyCode == Keys.Escape)

                {

                    this.wmp.fullScreen = false;

                }

                return true;

            }

            return false;

        }

        #endregion


        private void Form4_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void Form4_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                Location = mousePos;
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Form1.skins.getImage("PlayerBtnXH");
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Form1.skins.getImage("PlayerBtnX");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.BackgroundImage = Form1.skins.getImage("MaxH");
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackgroundImage = Form1.skins.getImage("Max");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (wmp.playState == WMPLib.WMPPlayState.wmppsPlaying || wmp.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                wmp.fullScreen = true;
            }
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.BackgroundImage = Form1.skins.getImage("MinH");
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BackgroundImage = Form1.skins.getImage("Min");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (wmp.fullScreen && timerFlag)//全螢幕
            {
                
                Application.AddMessageFilter(this);
                timerFlag = false;

            }
            else if (!wmp.fullScreen && !timerFlag)//非全螢幕
            {

                Application.RemoveMessageFilter(this);
                timerFlag = true;

            }

        }
    }
}
