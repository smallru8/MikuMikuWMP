using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;
using System.Web;
using Ionic.Zip;

namespace MikuMikuWMP
{
    /// <summary>
    /// 設定檔管理
    /// %appdata%\MikuMikuWMP\Config.yml
    /// </summary>
    public partial class Form5 : Form
    {
        private Point mouse_offset;
        private InstalledFontCollection installedFontCollection;

        private Form1 f1_link;
        public static FontFamily[] fontFamilies;//Font集
        public static int fontIndex = 0;//使用的Font

        public Form5()
        {
            InitializeComponent();
            checkCfg();//確定設定檔存在

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            installedFontCollection = new InstalledFontCollection();//讀取所有字型

            fontFamilies = new FontFamily[installedFontCollection.Families.Length + 1];

            for(int i=0;i< installedFontCollection.Families.Length; i++)
            {
                fontFamilies[i + 1] = installedFontCollection.Families[i];
            }

            PrivateFontCollection privateFonts = new PrivateFontCollection();
            Stream fontStream = GetType().Assembly.GetManifestResourceStream("MikuMikuWMP.Resources.setofont.ttf");///讀出Resource字型
            byte[] fontdata = new byte[fontStream.Length];
            fontStream.Read(fontdata, 0, (int)fontStream.Length);
            fontStream.Close();
            unsafe
            {
                fixed (byte* pFontData = fontdata)
                {
                    privateFonts.AddMemoryFont((System.IntPtr)pFontData, fontdata.Length);
                }
            }
            fontFamilies[0] = privateFonts.Families[0];
            for (int i = 0; i < fontFamilies.Length; i++)
            {
                comboBox1.Items.Add(fontFamilies[i].Name);
            }

            string setFont = getCfg("Font");
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.Items[i].Equals(setFont))
                {
                    if (i != 0)
                    {
                        fontIndex = i;
                        Console.WriteLine(fontFamilies[i].Name);
                    }
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }

            comboBox1.Font = new Font(fontFamilies[fontIndex], 12);
            label1.Font = new Font(fontFamilies[fontIndex], 14);
            label2.Font = new Font(fontFamilies[fontIndex], 12);
            label4.Font = new Font(fontFamilies[fontIndex], 12);
            label5.Font = new Font(fontFamilies[fontIndex], 12);
            label6.Font = new Font(fontFamilies[fontIndex], 12);
            label7.Font = new Font(fontFamilies[fontIndex], 12);
            label8.Font = new Font(fontFamilies[fontIndex], 12);
            label9.Font = new Font(fontFamilies[fontIndex], 12);
            button1.Font = new Font(fontFamilies[fontIndex], 10);
            button2.Font = new Font(fontFamilies[fontIndex], 10);
            button4.Font = new Font(fontFamilies[fontIndex], 10);
            button5.Font = new Font(fontFamilies[fontIndex], 10);
            button6.Font = new Font(fontFamilies[fontIndex], 10);
            label4.Text = "目前套用:" + fontFamilies[fontIndex].Name;
            checkBox1.Font = new Font(fontFamilies[fontIndex], 12);
            comboBox2.Font = new Font(fontFamilies[fontIndex], 12);
        }
        private bool checkCfg()//搜尋config.yml
        {
            if (System.IO.File.Exists(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\config.yml"))
            {
                return true;
            }
            else
            {
                if (!Directory.Exists(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP"))
                {
                    Directory.CreateDirectory(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP");
                }
                FileStream fs = File.Create(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\config.yml");
                fs.Close();
                createCfg();
                return false;
            }  
        }
        private string getCfg(string name)///取得Cfg值
        {
            StreamReader sr = new StreamReader(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\config.yml");
            string tmp = "";

            if (name.Equals("Font"))
            {
                do
                {
                    tmp = sr.ReadLine();
                } while (!tmp.StartsWith(name));
                tmp = (tmp.Split(' '))[1];
                sr.Close();
                return tmp;
            }
            return null;
        }
        public void getCfg()///Component位置
        {
            KeyValuePair<int, int>[] p = new KeyValuePair<int, int>[13];
            StreamReader sr = new StreamReader(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\config.yml");
            string tmp = "";
            int i = 0;
            do
            {
                tmp = sr.ReadLine();
            }
            while (!tmp.StartsWith("    Button: L T"));
            for(; i < 11; i++)
            {
                tmp = sr.ReadLine();
                if ((tmp.Split(' ')).Length > 10)
                    p[i] = new KeyValuePair<int, int>(Int32.Parse((tmp.Split(' '))[(tmp.Split(' ')).Length - 2]), Int32.Parse((tmp.Split(' '))[(tmp.Split(' ')).Length - 1]));
                else
                    p[i] = new KeyValuePair<int, int>(876,0);
            }
            do
            {
                tmp = sr.ReadLine();
            }
            while (!tmp.StartsWith("    Lable: L T"));
            for (; i < 13; i++)
            {
                tmp = sr.ReadLine();
                if ((tmp.Split(' ')).Length > 10)
                    p[i] = new KeyValuePair<int, int>(Int32.Parse((tmp.Split(' '))[(tmp.Split(' ')).Length - 2]), Int32.Parse((tmp.Split(' '))[(tmp.Split(' ')).Length - 1]));
                else
                    p[i] = new KeyValuePair<int, int>(876, 0);
            }
            sr.Close();
            f1_link.pictureBox14.Location = p[0].Key != 876 ? new Point(p[0].Key, p[0].Value) : f1_link.pictureBox14.Location;
            f1_link.pictureBox2.Location = p[1].Key != 876 ? new Point(p[1].Key, p[1].Value) : f1_link.pictureBox2.Location;
            f1_link.pictureBox3.Location = p[2].Key != 876 ? new Point(p[2].Key, p[2].Value) : f1_link.pictureBox3.Location;
            f1_link.pictureBox1.Location = p[3].Key != 876 ? new Point(p[3].Key, p[3].Value) : f1_link.pictureBox1.Location;
            f1_link.pictureBox5.Location = p[4].Key != 876 ? new Point(p[4].Key, p[4].Value) : f1_link.pictureBox5.Location;
            f1_link.pictureBox4.Location = p[5].Key != 876 ? new Point(p[5].Key, p[5].Value) : f1_link.pictureBox4.Location;
            f1_link.pictureBox11.Location = p[6].Key != 876 ? new Point(p[6].Key, p[6].Value) : f1_link.pictureBox11.Location;
            f1_link.pictureBox9.Location = p[7].Key != 876 ? new Point(p[7].Key, p[7].Value) : f1_link.pictureBox9.Location;
            f1_link.pictureBox8.Location = p[8].Key != 876 ? new Point(p[8].Key, p[8].Value) : f1_link.pictureBox8.Location;
            f1_link.pictureBox7.Location = p[9].Key != 876 ? new Point(p[9].Key, p[9].Value) : f1_link.pictureBox7.Location;
            f1_link.pictureBox10.Location = p[10].Key != 876 ? new Point(p[10].Key, p[10].Value) : f1_link.pictureBox10.Location;
            f1_link.label1.Location = p[11].Key != 876 ? new Point(p[11].Key, p[11].Value) : f1_link.label1.Location;
            f1_link.pictureBox6.Location = p[12].Key != 876 ? new Point(p[12].Key, p[12].Value) : f1_link.pictureBox6.Location;
            
        }

        private void setCfg()///儲存Cfg
        {
            StreamWriter sw = new StreamWriter(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\config.yml");
            sw.WriteLine("[MikuMikuWMP-" + Form1.version + "]");
            sw.WriteLine("Font: " + comboBox1.SelectedItem);
            sw.WriteLine("Components:");
            sw.WriteLine("    Button: L T");
            sw.WriteLine("        setting: " + f1_link.pictureBox14.Location.X + " " + f1_link.pictureBox14.Location.Y);
            sw.WriteLine("        play: " + f1_link.pictureBox2.Location.X + " " + f1_link.pictureBox2.Location.Y);
            sw.WriteLine("        stop: " + f1_link.pictureBox3.Location.X + " " + f1_link.pictureBox3.Location.Y);
            sw.WriteLine("        close: " + f1_link.pictureBox1.Location.X + " " + f1_link.pictureBox1.Location.Y);
            sw.WriteLine("        next: " + f1_link.pictureBox5.Location.X + " " + f1_link.pictureBox5.Location.Y);
            sw.WriteLine("        previous: " + f1_link.pictureBox4.Location.X + " " + f1_link.pictureBox4.Location.Y);
            sw.WriteLine("        video: " + f1_link.pictureBox11.Location.X + " " + f1_link.pictureBox11.Location.Y);
            sw.WriteLine("        shuffle: " + f1_link.pictureBox9.Location.X + " " + f1_link.pictureBox9.Location.Y);
            sw.WriteLine("        cycle: " + f1_link.pictureBox8.Location.X + " " + f1_link.pictureBox8.Location.Y);
            sw.WriteLine("        musicLs: " + f1_link.pictureBox7.Location.X + " " + f1_link.pictureBox7.Location.Y);
            sw.WriteLine("        qA: " + f1_link.pictureBox10.Location.X + " " + f1_link.pictureBox10.Location.Y);
            sw.WriteLine("    Lable: L T");
            sw.WriteLine("        marquee: " + f1_link.label1.Location.X + " " + f1_link.label1.Location.Y);
            sw.WriteLine("        vol: " + f1_link.pictureBox6.Location.X + " " + f1_link.pictureBox6.Location.Y);
            sw.Close();

            StreamWriter sw2 = new StreamWriter(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\skin.yml");
            sw2.WriteLine("[MikuMikuWMP-" + Form1.version + "]");
            sw2.WriteLine("Skin: " + comboBox2.SelectedItem);
            sw2.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            checkBox1.Checked = true;
            //getCfg();
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

        private void button2_Click(object sender, EventArgs e)//套用
        {
            checkBox1.Checked = true;
            setCfg();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label3.Font = new Font(fontFamilies[comboBox1.SelectedIndex], 12);
        }

        private void createCfg()
        {
            checkBox1.Checked = true;
            StreamWriter sw = new StreamWriter(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\config.yml");
            sw.WriteLine("[MikuMikuWMP-" + Form1.version + "]");
            sw.WriteLine("Font: SetoFont");
            sw.WriteLine("Components:");
            sw.WriteLine("    Button: L T");
            sw.WriteLine("        setting: 106 49" );
            sw.WriteLine("        play: 9 233" );
            sw.WriteLine("        stop: 51 233");
            sw.WriteLine("        close: 165 34" );
            sw.WriteLine("        next: 135 233" );
            sw.WriteLine("        previous: 93 233" );
            sw.WriteLine("        video: 122 277" );
            sw.WriteLine("        shuffle: 64 278" );
            sw.WriteLine("        cycle: 37 278" );
            sw.WriteLine("        musicLs: 189 210" );
            sw.WriteLine("        qA: 135 49" );
            sw.WriteLine("    Lable: L T");
            sw.WriteLine("        marquee: 120 191" );
            sw.WriteLine("        vol: 177 240" );
            sw.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            createCfg();
        }

        public void setParentWindow(Form1 f1)
        {
            f1_link = f1;
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                label6.Text = "編輯模式";
                f1_link.editable = true;
            }
            else
            {
                label6.Text = "已鎖定";
                f1_link.editable = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            f1_link.pictureBox14.Location = new Point(106,49);
            f1_link.pictureBox2.Location = new Point(9,233);
            f1_link.pictureBox3.Location = new Point(51,233);
            f1_link.pictureBox1.Location = new Point(165,34);
            f1_link.pictureBox5.Location = new Point(135,233);
            f1_link.pictureBox4.Location = new Point(93,233);
            f1_link.pictureBox11.Location = new Point(122,277);
            f1_link.pictureBox9.Location = new Point(64,278);
            f1_link.pictureBox8.Location = new Point(37,278);
            f1_link.pictureBox7.Location = new Point(189,210);
            f1_link.pictureBox10.Location = new Point(135,49);
            f1_link.label1.Location = new Point(120,191);
            f1_link.pictureBox6.Location = new Point(177,240);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string myPath = Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\Skins\\";
            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            prc.StartInfo.FileName = myPath;
            prc.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (var zip = new ZipFile(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\Skins\\" + (label8.Text.Split(' '))[1]))
            {
               
                foreach(var zipEntry in zip)
                {
                    if (zipEntry.FileName.Equals("config.yml"))
                    {
                        zip.RemoveEntry(zipEntry.FileName);
                        break;
                    }
                }
                zip.AddFile(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\config.yml", "");
                zip.Save();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        public void getCfgFromSkinPack()
        {
            Form1.skins.setImageFromCfg(comboBox2.SelectedItem + "");
        }
    }
}
