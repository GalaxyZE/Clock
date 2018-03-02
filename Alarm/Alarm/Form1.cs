using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//---------------------------
using System.Threading;
using System.Media;
using System.IO;
using WMPLib;
using System.Reflection;
using AllHotkey;

namespace Alarm
{
    public partial class Form1 : Form
    {
        public int TimeSecond;
        public int Hours, Minutes, Seconds;
        public int ihours, iminutes, iseconds;
        public Boolean Enable_Switch;
        public string AudioPath;
        public int AudioSecond;
        private Properties.Settings MySetting = new Properties.Settings();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            #region Interface_Setting
            this.Text = "Clock";
            button3.Visible = false;
            //--------------------
            timer1.Interval = 1000;
            timer2.Interval = 500;
            label1.Text = "Hours";
            label2.Text = "Minutes";
            label3.Text = "Seconds";
            /*
            textBox1.Text = "00";
            textBox2.Text = "00";
            textBox3.Text = "00";
            */
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            button1.Text = "Start";
            button2.Text = "Restart";
            groupBox1.Text = "Mode";
            radioButton1.Text = "Alarm";
            radioButton2.Text = "Stopwatch";
            radioButton3.Text = "Countdown";
            stopwatchlabel.Visible = false;
            stopwatchlabel.Text = "00:00:00.00";
            #endregion
            #region ReadSetting
            this.Location = MySetting.StartPosition;
            AudioPath = MySetting.MusicPath;
            if (AudioPath != null)
            {
                if (!File.Exists(AudioPath))
                {
                    AudioPath = null;
                }
            }
            textBox1.Text = MySetting.Hour.ToString();
            textBox2.Text = MySetting.Minutes.ToString();
            textBox3.Text = MySetting.Second.ToString();
            if(Convert.ToInt32(textBox1.Text)>24 || Convert.ToInt32(textBox1.Text)<0)
            { textBox1.Text = "00";}
            if(Convert.ToInt32(textBox2.Text)>=60 || Convert.ToInt32(textBox2.Text)<0)
            { textBox2.Text = "00"; }
            if(Convert.ToInt32(textBox3.Text)>=60 || Convert.ToInt32(textBox3.Text)<0)
            { textBox3.Text = "00"; }
            #endregion
            #region HotKey

            AllHotkey.Hotkey hokey1 = new AllHotkey.Hotkey(this.Handle);
            AllHotkey.Hotkey.Hotkey1 = hokey1.RegisterHotkey(System.Windows.Forms.Keys.F1, AllHotkey.Hotkey.KeyFlags.MOD_None);
            hokey1.OnHotkey += new AllHotkey.HotkeyEventHandler(OnHotkey);

            AllHotkey.Hotkey hotkey2 = new AllHotkey.Hotkey(this.Handle);
            AllHotkey.Hotkey.Hotkey2 = hokey1.RegisterHotkey(System.Windows.Forms.Keys.F2, AllHotkey.Hotkey.KeyFlags.MOD_None);

            hotkey2.OnHotkey += new AllHotkey.HotkeyEventHandler(OnHotkey);
            #endregion

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MySetting.Hour = Convert.ToInt32(textBox1.Text);
            MySetting.Minutes = Convert.ToInt32(textBox2.Text);
            MySetting.Second = Convert.ToInt32(textBox3.Text);
            MySetting.MusicPath = AudioPath;
            MySetting.StartPosition = Location;
            MySetting.Save();
        }


        System.IO.Stream str = Properties.Resources.Step;
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        public void PlayWmv()
        {
            //player.SoundLocation = @"C:\Users\user\Music\ccheer.wav";
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(str);
            player.Play();
        }
        WMPLib.WindowsMediaPlayer player1 = new WMPLib.WindowsMediaPlayer();

        public void PlayMp3()
        {
            if(AudioPath==null)
            {
                PlayWmv();  
            }
            else
            {
                player1.URL=@""+Path.GetFullPath(AudioPath);
            }
            player1.controls.play();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Enable_Switch)
            {
                if (Convert.ToInt16(textBox1.Text) >= 0 && Convert.ToInt16(textBox1.Text) < 60)
                {
                    if (Convert.ToInt16(textBox2.Text) >= 0 && Convert.ToInt16(textBox2.Text) < 60)
                    {
                        if (Convert.ToInt16(textBox3.Text) >= 0 && Convert.ToInt16(textBox3.Text) < 60)
                        {
                            Hours = Convert.ToInt16(textBox1.Text);
                            Minutes = Convert.ToInt16(textBox2.Text);
                            Seconds = Convert.ToInt16(textBox3.Text);
                            ihours = Hours;
                            iminutes = Minutes;
                            iseconds = Seconds;
                            timer1.Enabled = true;
                        }
                    }
                }
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                Enable_Switch = true;
                button1.Text = "Pause";
            }
            else
            {
                timer1.Enabled = !timer1.Enabled;
                if(!timer1.Enabled)
                {
                    button1.Text = "Pause";
                }
                else
                {
                    button1.Text = "Start";
                }
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void alarmPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RE:
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            AudioPath = file.FileName;
            try
            {
                Path.GetDirectoryName(AudioPath);
            }
            catch
            {
                string Mess = "音樂尚未設定，請問是否重新設定?";
                DialogResult result;
                result=MessageBox.Show(Mess,"Error",MessageBoxButtons.OKCancel);
                if(result==System.Windows.Forms.DialogResult.OK)
                {
                    goto RE;     
                }else if(result==System.Windows.Forms.DialogResult.Cancel)
                {
                    //MessageBox.Show("Fuck", "Fuck you!!!!", MessageBoxButtons.OK);
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (("已停止" == player1.status) && AudioPath!=null)
            {
                timer1.Enabled = true;
                timer2.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        #region radioButton
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                this.Text = "Clock_Alarm";
                label1.Visible = label2.Visible = label3.Visible = label4.Visible = label5.Visible = label6.Visible = textBox1.Visible = textBox2.Visible = textBox3.Visible = false;
                stopwatchlabel.Visible = false;
            }
            else if (radioButton2.Checked)
            {
                this.Text = "Clock_Stopwatch";
                label1.Visible = label2.Visible = label3.Visible = label4.Visible = label5.Visible = label6.Visible = textBox1.Visible = textBox2.Visible = textBox3.Visible = false;
                stopwatchlabel.Visible = true;
            }
            else if (radioButton3.Checked)
            {
                this.Text = "Clock_Countdown";
                label1.Visible = label2.Visible = label3.Visible = label4.Visible = label5.Visible = label6.Visible = textBox1.Visible = textBox2.Visible = textBox3.Visible = true;
                stopwatchlabel.Visible = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                this.Text = "Clock_Alarm";
                label1.Visible = label2.Visible = label3.Visible = label4.Visible = label5.Visible = label6.Visible = textBox1.Visible = textBox2.Visible = textBox3.Visible = false;
                stopwatchlabel.Visible = false;
            }
            else if (radioButton2.Checked)
            {
                this.Text = "Clock_Stopwatch";
                label1.Visible = label2.Visible = label3.Visible = label4.Visible = label5.Visible = label6.Visible = textBox1.Visible = textBox2.Visible = textBox3.Visible = false;
                stopwatchlabel.Visible = true;
            }
            else if (radioButton3.Checked)
            {
                this.Text = "Clock_Countdown";
                label1.Visible = label2.Visible = label3.Visible = label4.Visible = label5.Visible = label6.Visible = textBox1.Visible = textBox2.Visible = textBox3.Visible = true;
                stopwatchlabel.Visible = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                this.Text = "Clock_Alarm";
                label1.Visible = label2.Visible = label3.Visible = label4.Visible = label5.Visible = label6.Visible = textBox1.Visible = textBox2.Visible = textBox3.Visible = false;
                stopwatchlabel.Visible = false;
            }
            else if (radioButton2.Checked)
            {
                this.Text = "Clock_Stopwatch";
                label1.Visible = label2.Visible = label3.Visible = label4.Visible = label5.Visible = label6.Visible = textBox1.Visible = textBox2.Visible = textBox3.Visible = false;
                stopwatchlabel.Visible = true;
            }
            else if (radioButton3.Checked)
            {
                this.Text = "Clock_Countdown";
                label1.Visible = label2.Visible = label3.Visible = label4.Visible = label5.Visible = label6.Visible = textBox1.Visible = textBox2.Visible = textBox3.Visible = true;
                stopwatchlabel.Visible = false;
            }
        }

        #endregion
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            button1.Text = "Start";
            Enable_Switch = false;
            textBox1.Text = ihours.ToString();
            textBox2.Text = iminutes.ToString();
            textBox3.Text = iseconds.ToString();
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            button1.Enabled = true;
            player1.controls.stop();
            if(AudioPath==null)
            {
                player.Stop();
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(Seconds>0)
            {
                Seconds--;
            }
            else
            {
                if(Minutes>0)
                {
                    Minutes--;
                    Seconds += 60;
                    Seconds--;
                }
                else
                {
                    if(Hours>0)
                    {
                        Hours--;
                        Minutes += 60;
                        Minutes--;
                        Seconds += 60;
                        Seconds--;
                    }
                    else
                    {
                        if(Hours==0 && Minutes ==0 && Seconds==0)
                        {
                            PlayMp3();   
                            button1.Enabled = false;
                            timer1.Enabled = false;
                            timer2.Enabled = true;
                        }       
                    }
                }
            }
            textBox1.Text = Hours.ToString();
            textBox2.Text = Minutes.ToString();
            textBox3.Text = Seconds.ToString();
        }

        public void OnHotkey(int HotkeyID)
        {
            if (HotkeyID == Hotkey.Hotkey1)
            {
                this.button1_Click(this, null);
            }

            if (HotkeyID == Hotkey.Hotkey2)
            {
                this.button2_Click(this, null);
            }
        }
    }
}
