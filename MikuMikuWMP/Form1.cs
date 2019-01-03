using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using AxWMPLib;
using System.IO;
using System.Drawing.Text;
using System.Reflection;

namespace MikuMikuWMP
{
    public partial class Form1 : Form
    {

        public static string version = "Beta-1812.5";
        public static Skins skins;
        public static Form4 f4;/// WMP宣告在f4，影像也顯示在f4
        public static Form5 settingW;///設定視窗

        public bool editable = false;///編輯模式
        private int vol = 10;//音量
        private bool re = false;//是否循環單曲
        private bool random = false;//是否隨機撥放
        private bool playPause = false;//是否播放中

        private bool musicNameMove = true;

        private Point mouse_offset;

        private Form2 musicLs;///清單處理用視窗

        private Pen myPen = new Pen(Color.FromArgb(242, 68, 68), 2);
        Graphics graphics;

        public Form1()
        {
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(pictureBox6_MouseWheel);
            label1.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox1.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox2.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox3.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox4.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox5.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox6.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox7.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox8.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox9.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox10.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox11.MouseDown += new MouseEventHandler(pic_MouseDown);
            pictureBox14.MouseDown += new MouseEventHandler(pic_MouseDown);

            label1.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox1.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox2.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox3.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox4.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox5.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox6.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox7.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox8.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox9.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox10.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox11.MouseMove += new MouseEventHandler(pic_MouseMove);
            pictureBox14.MouseMove += new MouseEventHandler(pic_MouseMove);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(form1_DragEnter);
            this.DragDrop += new DragEventHandler(form1_DragDrop);

            settingW = new Form5();
            settingW.Visible = false;
            settingW.setParentWindow(this);
 
            skins = new Skins();
            settingW.BackgroundImage = skins.getImage("Player");
            skins.skinSetup(this);
            settingW.getCfgFromSkinPack();
            settingW.getCfg();///載入components位置

            f4 = new Form4();
            f4.Visible = false;

            ///初始化撥放器
            f4.wmp.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(player_PlayStateChange);
            f4.wmp.Visible = true;
            f4.wmp.settings.volume = vol;

            f4.wmp.settings.setMode("shuffle", false);
            f4.wmp.settings.setMode("loop", false);
            ///-----------
            timer1.Interval = 200;
            timer1.Start();

            //f4.wmp.currentPlaylist.appendItem(f4.wmp.newMedia(@"H:\mp3\歌單\relax\墻.mp4"));

            if(Program.filePath != null)
                for (int i = 0; i < Program.filePath.Length; i++)
                    f4.wmp.currentPlaylist.appendItem(f4.wmp.newMedia(@Program.filePath[i]));

            graphics = pictureBox2.CreateGraphics();//進度條寫在timer1 
            
            

            label1.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);
            
            
        }

        private void Form1_Close(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
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
            //((PictureBox)sender).BackgroundImage.Dispose();
            pictureBox1.BackgroundImage = skins.getImage("CloseH");
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = skins.getImage("Close");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(!editable)
                this.Close();
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {

            if (playPause)
            {
                pictureBox2.BackgroundImage = skins.getImage("PauseH");
            }
            else
            {
                pictureBox2.BackgroundImage = skins.getImage("PlayH");
            }
            //graphics.DrawArc(myPen, 0, 0, pictureBox2.Size.Width, pictureBox2.Size.Height, 0, 360);
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            if (playPause)
            {
                pictureBox2.BackgroundImage = skins.getImage("Pause");
            }
            else
            {
                pictureBox2.BackgroundImage = skins.getImage("Play");
            }
            //graphics.DrawArc(myPen, 0, 0, pictureBox2.Size.Width, pictureBox2.Size.Height, 0, 360);
        }

        private void pictureBox2_Click(object sender, EventArgs e)//Play Pause
        {
            if (f4.wmp.currentMedia != null && !editable)
            {
                if (playPause)
                {
                    pictureBox2.BackgroundImage = skins.getImage("Play");
                    playPause = false;
                    f4.wmp.Ctlcontrols.pause();//暫停

                }
                else
                {
                    pictureBox2.BackgroundImage = skins.getImage("Pause");
                    playPause = true;
                    f4.wmp.Ctlcontrols.play();//撥放
                }
            }
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.BackgroundImage = skins.getImage("StopH");
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BackgroundImage = skins.getImage("Stop");
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.BackgroundImage = skins.getImage("BeforeH");
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackgroundImage = skins.getImage("Before");
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            pictureBox5.BackgroundImage = skins.getImage("NextH");
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.BackgroundImage = skins.getImage("Next");
        }

        private void pictureBox6_MouseWheel(object sender, MouseEventArgs e)
        {
            ///滑鼠滾輪音量調整
            if (e.Delta > 0)
            {
                vol += 5;
            }
            else if(e.Delta < 0)
            {
                vol -= 5;
            }
            if (vol > 100)
                vol = 100;
            else if (vol < 0)
                vol = 0;
            if (vol <= 10)
            {
                pictureBox6.BackgroundImage = skins.getImage("vol1");
            }
            else if (vol <= 20)
            {
                pictureBox6.BackgroundImage = skins.getImage("vol2");
            }
            else if (vol <= 30)
            {;
                pictureBox6.BackgroundImage = skins.getImage("vol3");
            }
            else if (vol <= 40)
            {
                pictureBox6.BackgroundImage = skins.getImage("vol4");
            }
            else if (vol <= 50)
            {
                pictureBox6.BackgroundImage = skins.getImage("vol5");
            }
            else if (vol <= 60)
            {
                pictureBox6.BackgroundImage = skins.getImage("vol6");
            }
            else if (vol <= 70)
            {
                pictureBox6.BackgroundImage = skins.getImage("vol7");
            }
            else if (vol <= 80)
            {
                pictureBox6.BackgroundImage = skins.getImage("vol8");
            }
            else if (vol <= 90)
            {
                pictureBox6.BackgroundImage = skins.getImage("vol9");
            }
            else if (vol <= 100)
            {
                pictureBox6.BackgroundImage = skins.getImage("vol10");
            }
            f4.wmp.settings.volume = vol;

            if (vol >= 50)
            {
                this.BackgroundImage = skins.getImage("005");
            }
            else if (vol < 50)
            {
                this.BackgroundImage = skins.getImage("004");
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)//Stop
        {
            if (!editable)
            {
                f4.wmp.Ctlcontrols.stop();///停止
                playPause = false;
                pictureBox2.BackgroundImage = skins.getImage("Play");
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (musicLs == null && !editable)
            {
                musicLs = new Form2();
                musicLs.Show();
            }else if (!editable && musicLs.IsDisposed)
            {
                musicLs = new Form2();
                musicLs.Show();
            }
        }

        private void pictureBox7_MouseEnter(object sender, EventArgs e)
        {
            pictureBox7.BackgroundImage = skins.getImage("OpenMusicLsH");
        }

        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {
            pictureBox7.BackgroundImage = skins.getImage("OpenMusicLs");
        }

        private void pictureBox8_MouseEnter(object sender, EventArgs e)
        {
            if (!re)
            {
                pictureBox8.BackgroundImage = skins.getImage("ReH");
            }
            else
            {
                pictureBox8.BackgroundImage = skins.getImage("ReTH");
            }
        }

        private void pictureBox8_MouseLeave(object sender, EventArgs e)
        {
            if (!re)
            {
                pictureBox8.BackgroundImage = skins.getImage("Re");
            }
            else
            {
                pictureBox8.BackgroundImage = skins.getImage("ReT");
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (!editable)
            {
                if (!re)
                {
                    re = true;
                    f4.wmp.settings.setMode("loop", true);//單曲循環
                }
                else
                {
                    re = false;
                    f4.wmp.settings.setMode("loop", false);//關閉單曲循環
                }
            }
        }

        private void pictureBox9_MouseEnter(object sender, EventArgs e)
        {
            if (!random)
            {
                pictureBox9.BackgroundImage = skins.getImage("RandomH");
            }
            else
            {
                pictureBox9.BackgroundImage = skins.getImage("RandomTH");
            }
        }

        private void pictureBox9_MouseLeave(object sender, EventArgs e)
        {
            if (!random)
            {
                pictureBox9.BackgroundImage = skins.getImage("Random");
            }
            else
            {
                pictureBox9.BackgroundImage = skins.getImage("RandomT");
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (!editable)
            {
                if (!random)
                {
                    random = true;
                    f4.wmp.settings.setMode("shuffle", true);
                }
                else
                {
                    random = false;
                    f4.wmp.settings.setMode("shuffle", false);
                }
            }
        }

        private void player_PlayStateChange(object sender,_WMPOCXEvents_PlayStateChangeEvent e)///播放狀態改變時
        {
            label1.Text = f4.wmp.currentMedia.name;
            if (e.newState == 1)//Stop
            {
                playPause = false;
                pictureBox2.BackgroundImage = skins.getImage("Play");
            }else if(e.newState == 2)//Paused
            {
                playPause = false;
                pictureBox2.BackgroundImage = skins.getImage("Play");
            }
            else if(e.newState == 3)//Playing
            {
                playPause = true;
                pictureBox2.BackgroundImage = skins.getImage("Pause");
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)//Next
        {
            if (!editable)
                f4.wmp.Ctlcontrols.next();
        }

        private void pictureBox4_Click(object sender, EventArgs e)//Previous
        {
            if (!editable)
                f4.wmp.Ctlcontrols.previous();
        }

        private void pictureBox10_MouseEnter(object sender, EventArgs e)
        {
            pictureBox10.BackgroundImage = skins.getImage("QuestionH");
        }

        private void pictureBox10_MouseLeave(object sender, EventArgs e)
        {
            pictureBox10.BackgroundImage = skins.getImage("Question");
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            if (!editable)
            {
                Form3 f3 = new Form3();
                f3.Show();
                f3.label2.Text = "在撥放器上可使用滾輪";
                f3.label3.Text = "來調整音量";
                f3.label4.Text = "右邊的";
                f3.pictureBox2.BackgroundImage = global::MikuMikuWMP.Properties.Resources.OpenMusicLs;
                f3.pictureBox2.Left = 100;
                f3.pictureBox2.Top = 100;
                f3.label5.Text = "可以開啟Music List";
                f3.label6.Text = "左下的";
                f3.pictureBox3.BackgroundImage = global::MikuMikuWMP.Properties.Resources.Re;
                f3.pictureBox3.Left = 100;
                f3.pictureBox3.Top = 139;
                f3.label7.Text = "可以選擇是否單曲循環";
                f3.label8.Text = "左下的";
                f3.pictureBox4.BackgroundImage = global::MikuMikuWMP.Properties.Resources.Random;
                f3.pictureBox4.Left = 100;
                f3.pictureBox4.Top = 179;
                f3.label9.Text = "可以選擇是否隨機撥放";
                f3.label10.Text = "右下的";
                f3.pictureBox5.BackgroundImage = global::MikuMikuWMP.Properties.Resources.OpenPlayerWindow;
                f3.pictureBox5.Left = 100;
                f3.pictureBox5.Top = 219;
                f3.label11.Text = "可以顯示影像";
            }
        }

        private void pictureBox11_MouseEnter(object sender, EventArgs e)
        {
            pictureBox11.BackgroundImage = skins.getImage("OpenPlayerWindowH");
        }

        private void pictureBox11_MouseLeave(object sender, EventArgs e)
        {
            pictureBox11.BackgroundImage = skins.getImage("OpenPlayerWindow");
        }

        private void pictureBox11_Click(object sender, EventArgs e)///打開影像視窗
        {
            if (!editable)
            {
                if (f4.Visible)
                {
                    f4.Visible = false;
                }
                else
                {
                    f4.Visible = true;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (musicNameMove)
            {
                label1.Left -= 3;
                if (label1.Left + label1.Width <= 0)
                    label1.Left = 216;
            }
            double angle = 0;
            if (f4.wmp.currentMedia != null)
                angle = 360 * f4.wmp.Ctlcontrols.currentPosition / f4.wmp.currentMedia.duration;
            //Console.WriteLine(angle);
            graphics.DrawArc(myPen, 0, 0, pictureBox2.Size.Width, pictureBox2.Size.Width, 0, (float)angle);///進度條
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            musicNameMove = false;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            musicNameMove = true;
        }

        private void form1_DragEnter(object sender, DragEventArgs e)///拖曳偵測
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void form1_DragDrop(object sender, DragEventArgs e)///拖曳檔案進入
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (s.Length == 1 && s[0].EndsWith(".SmaMu"))
            {
                Form1.f4.wmp.currentPlaylist.clear();
                FileStream fs = new FileStream(@s[0], FileMode.Open);
                StreamReader reader = new StreamReader(fs);
                while (!reader.EndOfStream)
                {
                    Form1.f4.wmp.currentPlaylist.appendItem(Form1.f4.wmp.newMedia(reader.ReadLine()));
                }
                reader.Close();
                fs.Close();
            }
            else
            {
                for (int i = 0; i < s.Length; i++)
                {
                    Form1.f4.wmp.currentPlaylist.appendItem(Form1.f4.wmp.newMedia(s[i]));             
                }
            }
            if (musicLs == null) { }
            else if (musicLs != null || !musicLs.IsDisposed)
                musicLs.Close();
        }

        private void pictureBox14_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = skins.getImage("settingH");
        }

        private void pictureBox14_MouseLeave(object sender, EventArgs e)
        {
            ((PictureBox)sender).BackgroundImage = skins.getImage("setting");
        }

        private void pictureBox14_Click(object sender, EventArgs e)///打開設定視窗
        {
            if (!editable)
            {
                settingW.Visible = true;
                settingW.Show();
            }
        }
        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            if (editable)
            {
                mouse_offset = new Point(e.X, e.Y);    
            }

        }

        private void pic_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && editable)
            {
                ((Control)sender).Left += e.X - mouse_offset.X;
                ((Control)sender).Top += e.Y - mouse_offset.Y;
            }
        }

    }
}
