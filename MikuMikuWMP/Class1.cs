using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.IO;
using Ionic.Zip;

namespace MikuMikuWMP
{
    public class TransParentListBox : ListBox
    {
        public TransParentListBox()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Focused && this.SelectedItem != null)
            {
                Rectangle itemRect = this.GetItemRectangle(this.SelectedIndex);
                e.Graphics.FillRectangle(Brushes.LightBlue, itemRect);
            }
            for (int i = 0; i < Items.Count; i++)
            {
                StringFormat strFmt = new System.Drawing.StringFormat();
                strFmt.Alignment = StringAlignment.Center;
                strFmt.LineAlignment = StringAlignment.Center; 
                e.Graphics.DrawString(this.GetItemText(this.Items[i]), this.Font, new SolidBrush(this.ForeColor), this.GetItemRectangle(i), strFmt);
            }
            base.OnPaint(e);
        }

    }

    public class Skins///這個class要在cfg讀取完成後建立
    {

        private Dictionary<string, Image> components;

        public Skins()//53
        {
            components = new Dictionary<string, Image>();

            ResourceSet resourceSet = Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)///先預載內建Resources
            {
                string resourceKey = entry.Key.ToString();
                object resource = entry.Value;
                components.Add(resourceKey, (Image)resource);       
            }
            checkSkinCfg();
            Form1.settingW.comboBox2.Items.Add("Default");
            foreach (string path in Directory.GetFiles(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\Skins"))
            {       
                Form1.settingW.comboBox2.Items.Add(path.Split('\\')[(path.Split('\\')).Length - 1]);
            }

            StreamReader sr = new StreamReader(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\skin.yml");
            string skinName;
            do
            {
                skinName = sr.ReadLine();
            } while (!skinName.StartsWith("Skin:"));
            sr.Close();
            skinName = (skinName.Split(' '))[1];
            Form1.settingW.comboBox2.SelectedItem = skinName;
            setImageFromCfg("");///確定材質包
        }
        private bool checkSkinCfg()
        {
            if (File.Exists(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\skin.yml"))
            {
                return true;
            }
            else
            {
                if (!Directory.Exists(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP"))
                {
                    Directory.CreateDirectory(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP");
                }
                if (!Directory.Exists(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\Skins"))
                {
                    Directory.CreateDirectory(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\Skins");
                }
                FileStream fs = File.Create(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\skin.yml");
                fs.Close();

                StreamWriter sw = new StreamWriter(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\skin.yml");
                sw.WriteLine("[MikuMikuWMP-" + Form1.version + "]");
                sw.WriteLine("Skin: Default");
                sw.Close();
                return false;
            }
        }
        public Image getImage(string imageName)
        {
            if (components.ContainsKey(imageName))
            {
                return components[imageName];
            }
            return null;
        }

        public void skinSetup(Form1 f1)//套用skin
        {
            f1.BackgroundImage = getImage("004");
            f1.pictureBox1.BackgroundImage = getImage("Close");
            f1.pictureBox2.BackgroundImage = getImage("Play");
            f1.pictureBox3.BackgroundImage = getImage("Stop");
            f1.pictureBox4.BackgroundImage = getImage("Before");
            f1.pictureBox5.BackgroundImage = getImage("Next");
            f1.pictureBox6.BackgroundImage = getImage("vol1");
            f1.pictureBox7.BackgroundImage = getImage("OpenMusicLs");
            f1.pictureBox8.BackgroundImage = getImage("Re");
            f1.pictureBox9.BackgroundImage = getImage("Random");
            f1.pictureBox10.BackgroundImage = getImage("Question");
            f1.pictureBox11.BackgroundImage = getImage("OpenPlayerWindow");
            f1.pictureBox14.BackgroundImage = getImage("setting");
        }

        public void setImageFromCfg(string skN)
        {
            string skinName = skN;
            if (skN.Equals(""))
            {
                StreamReader sr = new StreamReader(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\skin.yml");
                do
                {
                    skinName = sr.ReadLine();
                } while (!skinName.StartsWith("Skin:"));
                sr.Close();
                skinName = (skinName.Split(' '))[1];
            }
            Form1.settingW.label8.Text = "目前套用: " + skinName;
            if (!skinName.Equals("Default") && File.Exists(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\Skins\\" + skinName))
            {
                using (var zip = ZipFile.Read(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP\\Skins\\" + skinName))
                {
                    foreach (var zipEntry in zip)
                    {
                        MemoryStream tempS = new MemoryStream();
                        zipEntry.Extract(tempS);
                        if (components.ContainsKey((zipEntry.FileName.Split('.'))[0]))
                        {
                            components[(zipEntry.FileName.Split('.'))[0]] = Image.FromStream(tempS);
                        }
                        else if (zipEntry.FileName.Equals("config.yml"))
                        {
                            zipEntry.Extract(Environment.ExpandEnvironmentVariables("%AppData%") + "\\MikuMikuWMP", ExtractExistingFileAction.OverwriteSilently);
                        }
                        tempS.Close();
                    }
                }
            }
        }
    }
}
