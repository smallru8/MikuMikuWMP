using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using WMPLib;


namespace MikuMikuWMP
{
    public partial class Form2 : Form
    {
        private TransParentListBox transParentListBox;
        private Point mouse_offset;

        public Form2()
        {
            InitializeComponent();
            transParentListBox = new TransParentListBox();
            transParentListBox.Width = 125;
            transParentListBox.Height = 163;
            transParentListBox.Left = 24;
            transParentListBox.Top = 24;
            transParentListBox.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(transParentListBox);
            transParentListBox.DragEnter+= new DragEventHandler(listBox1_DragEnter);
            transParentListBox.DragDrop += new DragEventHandler(listBox1_DragDrop);
            transParentListBox.KeyDown += new KeyEventHandler(listBox1_KeyDown);
            transParentListBox.MouseDoubleClick += new MouseEventHandler(listBox1_MouseDoubleClick);
            transParentListBox.AllowDrop = true;
            transParentListBox.Font = new Font(Form5.fontFamilies[Form5.fontIndex], 12);

            for (int i=0;i<Form1.f4.wmp.currentPlaylist.count;i++)//初始化
                transParentListBox.Items.Add(Form1.f4.wmp.currentPlaylist.Item[i].name);

            pictureBox4.BackgroundImage = Form1.skins.getImage("Question");
            pictureBox1.BackgroundImage = Form1.skins.getImage("Close");
            pictureBox2.BackgroundImage = Form1.skins.getImage("LoadList");
            pictureBox3.BackgroundImage = Form1.skins.getImage("SaveList");
            BackgroundImage = Form1.skins.getImage("MusicLsWindow");
        }

        public void Form2_closed(object sender,FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
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
            pictureBox1.BackgroundImage = Form1.skins.getImage("CloseH");
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = Form1.skins.getImage("Close");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)///拖曳偵測
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)///拖曳檔案進入
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
                for (int i = 0; i < Form1.f4.wmp.currentPlaylist.count; i++)//初始化
                    transParentListBox.Items.Add(Form1.f4.wmp.currentPlaylist.Item[i].name);
            }
            else
            {
                for (int i = 0; i < s.Length; i++)
                {
                    Form1.f4.wmp.currentPlaylist.appendItem(Form1.f4.wmp.newMedia(s[i]));
                    transParentListBox.Items.Add(Form1.f4.wmp.currentPlaylist.Item[Form1.f4.wmp.currentPlaylist.count - 1].name);
                }
            }
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.BackgroundImage = Form1.skins.getImage("LoadListH");
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackgroundImage = Form1.skins.getImage("LoadList");
        }

        private void pictureBox2_Click(object sender, EventArgs e)///Load List
        {

            Form1.f4.wmp.currentPlaylist.clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Small MusicLs |*.SmaMu|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                var fileStream = openFileDialog.OpenFile();
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    while (!reader.EndOfStream)
                    {
                        Form1.f4.wmp.currentPlaylist.appendItem(Form1.f4.wmp.newMedia(reader.ReadLine()));
                    }
                    reader.Close();
                }
            }
            for (int i = 0; i < Form1.f4.wmp.currentPlaylist.count; i++)//初始化
                transParentListBox.Items.Add(Form1.f4.wmp.currentPlaylist.Item[i].name);

        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("Delete");
            if (e.KeyCode == Keys.Delete && transParentListBox.Items.Count > 1)///從List上刪除選取的歌
            {
                Form1.f4.wmp.currentPlaylist.removeItem(Form1.f4.wmp.currentPlaylist.Item[transParentListBox.SelectedIndex]);
                transParentListBox.Items.Remove(transParentListBox.SelectedItem);
            }
        }
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            Form1.f4.wmp.Ctlcontrols.playItem(Form1.f4.wmp.currentPlaylist.get_Item(transParentListBox.SelectedIndex));

        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.BackgroundImage = Form1.skins.getImage("SaveListH");
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BackgroundImage = Form1.skins.getImage("SaveList");
        }

        private void pictureBox3_Click(object sender, EventArgs e)///Save List
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Small MusicLs |*.SmaMu";
            saveFileDialog1.Title = "Save an music list";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                FileStream fs =(FileStream)saveFileDialog1.OpenFile();
                for(int i = 0; i < Form1.f4.wmp.currentPlaylist.count; i++)
                {
                    fs.Write(Encoding.UTF8.GetBytes(Form1.f4.wmp.currentPlaylist.Item[i].sourceURL + "\n"),0, Encoding.UTF8.GetBytes(Form1.f4.wmp.currentPlaylist.Item[i].sourceURL + "\n").Length);
                }
                fs.Close();
            }
            
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.BackgroundImage = Form1.skins.getImage("QuestionH");
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackgroundImage = Form1.skins.getImage("Question");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }
    }
}
