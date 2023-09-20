using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections;
using System.Diagnostics;

namespace Bozteke_Alarm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string surum = "1.0.0";
        int islem, saat, dakika, saniye, hour, minute, second;
        string animsatilacakNot, secilenIslem, veri, aIslem;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Bozteke Alarm V" + surum;
            comboBox1.Items.Add(new DictionaryEntry("Bilgisayarı kapat", "-s -f -t 0"));
            comboBox1.Items.Add(new DictionaryEntry("Bilgisayarı yeniden başlat", "-r -f -t 0"));
                        comboBox1.Items.Add(new DictionaryEntry("Anımsatılacak bir not oluştur", "Anımsatılacak bir not oluştur"));
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";
            comboBox1.DataSource = comboBox1.Items;


            button1.Text = "Ayarla";
            textBox1.Enabled = false;
            textBox1.Visible = false;
            listBox1.Visible = false;
            listBox1.Enabled = false;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (button1.Text == "Durdur")
                {
                    timer1.Stop();
                    hour = 0;
                    minute = 0;
                    second = 0;
                    saat = 0;
                    dakika = 0;
                    saniye = 0;
                    button1.Text = "Ayarla";
                    listBox1.Items.Clear();
                    listBox1.Enabled = false;
                    listBox1.Visible = false;
                    comboBox1.SelectedIndex = 0;
                    comboBox2.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                    comboBox4.SelectedIndex = 0;
                    textBox1.Text = "";
                    comboBox1.Focus();
                    return;
                }
                else
                {
                    if (textBox1.Text == "")
                    {
                        animsatilacakNot = "Tanımlı mesaj yok.";
                    }
                    else
                    {
                        animsatilacakNot = textBox1.Text;
                    }
                    islem = comboBox1.SelectedIndex;
                    saat = comboBox2.SelectedIndex;
                    dakika = comboBox3.SelectedIndex;
                    saniye = comboBox4.SelectedIndex;
                    if (saat == 0 && dakika == 0 && saniye == 0)
                    {
                        secilenIslem = ((DictionaryEntry)comboBox1.SelectedItem).Key.ToString();
                        MessageBox.Show(secilenIslem + " işlemi için herhangi bir zaman ayarı yapmadınız. Bu işlemi sihirli bir şekilde mi gerçekleştirmek istiyorsunuz?", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        comboBox2.Focus();
                        return;
                    }
                    else
                    {
                        secilenIslem = ((DictionaryEntry)comboBox1.SelectedItem).Value.ToString();
                        listBox1.Visible = true;
                        listBox1.Enabled = true;
                        listBox1.Items.Clear();
                        if (islem == 2)
                        {
                            listBox1.Items.Add("İşlem: " + ((DictionaryEntry)comboBox1.SelectedItem).Key.ToString());
                            listBox1.Items.Add("Ayarlanan zaman: " + saat.ToString("00") + ":" + dakika.ToString("00") + ":" + saniye.ToString("00"));
                            listBox1.Items.Add("Anımsatılacak not: " + animsatilacakNot);
                        }
                        else
                        {
                            listBox1.Items.Add("İşlem: " + ((DictionaryEntry)comboBox1.SelectedItem).Key.ToString());
                            listBox1.Items.Add("Ayarlanan zaman: " + saat.ToString("00") + ":" + dakika.ToString("00") + ":" + saniye.ToString("00"));
                        }
                        listBox1.Focus();
                        listBox1.SelectedIndex = 0;
                    }
                    button1.Text = "Durdur";
                    timer1.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Kritik hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            second = second + 1;
            if (second == 60)
            {
                second = 0;
                minute = minute + 1;
            }
            if (minute == 60)
            {
                minute = 0;
                hour = hour + 1;
            }
            if (hour == saat && minute == dakika && second == saniye)
            {
                listBox1.Items.Clear();
                listBox1.Visible = false;
                listBox1.Enabled = false;
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
                comboBox4.SelectedIndex = 0;
                textBox1.Text = "";
                textBox1.Visible = false;
                textBox1.Enabled = false;
                button1.Text = "Ayarla";
                hour = 0;
                minute = 0;
                second = 0;
                saat = 0;
                dakika = 0;
                saniye = 0;
                if (islem == 2)
                {
                    MessageBox.Show("Hatırlatılacak not: " + animsatilacakNot, "Hatırlatıcı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    timer1.Stop();
                    timer1.Enabled = false;
                    return;
                }
                else
                {
                    timer1.Stop();
                    timer1.Enabled = false;
                    KomutYurut(secilenIslem);
                                    }
            }
                    }//timer nesnesinin sonu

        public void KomutYurut(string calistirilacakKomut)
        {
            Process komut = new Process();
            ProcessStartInfo baslangicBilgisi = new ProcessStartInfo();
            baslangicBilgisi.FileName = "shutdown";
            baslangicBilgisi.Arguments = calistirilacakKomut;
            baslangicBilgisi.UseShellExecute = false;
            baslangicBilgisi.RedirectStandardOutput = true;
            baslangicBilgisi.RedirectStandardError = true;
            baslangicBilgisi.CreateNoWindow = true;
            komut.StartInfo = baslangicBilgisi;
            komut.Start();

        }//fonksiyon sonu

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(3000);
                if (timer1.Enabled == false)
                {
                    notifyIcon1.Text = "Bozteke Alarm V" + surum;
                    return;
                }
                else
                {
                    timer2.Enabled = true;
                    timer2.Start();
                }
            }
        }//form resize olayının sonu

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            timer2.Stop();
            timer2.Enabled = false;
            notifyIcon1.Visible = false;
        }//notifyIcon1 double click olayı sonu

         private void timer2_Tick(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)
            {
                notifyIcon1.Text = "Bozteke Alarm V" + surum;
                return;
            }
            if (islem == 0)
            {
                notifyIcon1.Text = "Bilgisayar kapatma için geçen zaman " + hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");
                return;
            }
            if (islem == 1)
            {
                notifyIcon1.Text = "Yeniden başlatma için geçen zaman: " + hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");
                return;
            }
            if (islem == 2)
            {
                notifyIcon1.Text = "Anımsatıcı için geçen zaman: " + hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");
                return;
            }

        }//timer2 tick olayının sonu

                private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 2)
            {
                textBox1.Visible = true;
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Visible = false;
                textBox1.Enabled = false;
            }
        }//combobox1 changed olayının sonu

                    }//Proje kod kapsamı name space sonu
}//program sonu
