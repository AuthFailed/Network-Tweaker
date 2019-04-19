﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Update_Your_Hosts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        readonly string path = @"C:\Windows\System32\drivers\etc\hosts";
        public string Path => path;

        void Button1_Click(object sender, EventArgs e)
        {
            label2.Text = "";
            progressBar1.Value = 0;
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    Updatehosts("http://sbc.io/hosts/hosts");
                    break;
                case 1:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews/hosts");
                    break;
                case 2:
                    Updatehosts("http://sbc.io/hosts/alternates/gambling/hosts");
                    break;
                case 3:
                    Updatehosts("http://sbc.io/hosts/alternates/porn/hosts");
                    break;
                case 4:
                    Updatehosts("http://sbc.io/hosts/alternates/social/hosts");
                    break;
                case 5:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-gambling/hosts");
                    break;
                case 6:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-porn/hosts");
                    break;
                case 7:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-social/hosts");
                    break;
                case 8:
                    Updatehosts("http://sbc.io/hosts/alternates/gambling-porn/hosts");
                    break;
                case 9:
                    Updatehosts("http://sbc.io/hosts/alternates/gambling-social/hosts");
                    break;
                case 10:
                    Updatehosts("http://sbc.io/hosts/alternates/porn-social/hosts");
                    break;
                case 11:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-gambling-porn/hosts");
                    break;
                case 12:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-gambling-social/hosts");
                    break;
                case 13:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-porn-social/hosts");
                    break;
                case 14:
                    Updatehosts("http://sbc.io/hosts/alternates/gambling-porn-social/hosts");
                    break;
                case 15:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-gambling-porn-social/hosts");
                    break;
                default:
                    MessageBox.Show("Выберите фильтр!");
                    break;
            }
        }

        void Updatehosts(string line)
        {
            ProcessStartInfo cmd;
            cmd = new ProcessStartInfo("cmd", @"/c ipconfig /flushdns");
            try
            {
                if (File.Exists(@"C:\\Windows\System32\drivers\etc\hosts_bak"))
                    File.Delete(@"C:\\Windows\System32\drivers\etc\hosts_bak");
                if (File.Exists(@"C:\Windows\System32\drivers\\etc\hosts"))
                File.Copy(@"C:\Windows\System32\drivers\\etc\hosts", @"C:\Windows\System32\drivers\etc\hosts_bak");
                Uri fileuri = new Uri(line);
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += Web_DownloadProgressChanged;
                    wc.DownloadFileCompleted += Web_DownloadFileCompleted;
                    wc.DownloadFileAsync(fileuri, @"C:\Windows\System32\drivers\etc\hosts");
                }
                Process.Start(cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла какая-то неведомая херня.\nКод ошибки записан в буфер обмена", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Clipboard.Clear();
                Clipboard.SetText(ex.ToString());
            }
        }
        void Button2_Click(object sender, EventArgs e)
        {
            ProcessStartInfo cmd;
            cmd = new ProcessStartInfo("cmd", @"/c ipconfig /flushdns");
            try
            {
                if (string.IsNullOrWhiteSpace(richTextBox1.Text))
                {
                    MessageBox.Show("Введите домен!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!richTextBox1.Text.Contains("."))
                {
                    MessageBox.Show("Домен должен быть вида сайт.зона", "Внимание!");
                }
                else
                {
                    int t = 0;
                    string[] array = File.ReadAllLines(Path);
                    foreach (string ar in array)
                    {
                        if (ar.Contains(richTextBox1.Text))
                        {
                            MessageBox.Show("Данный домен уже есть в фильтре", "Внимание!");
                            t++;
                            break;
                        }
                    }
                    if(t<1)
                    {
                        using (StreamWriter sw = new StreamWriter(Path, append: true))
                        {
                            sw.WriteLine("0.0.0.0 " + richTextBox1.Text);
                            MessageBox.Show("Домен " + richTextBox1.Text + " добавлен в черный список");
                        }
                        Process.Start(cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Данный домен уже присутствует в фильтре.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Clipboard.Clear();
                Clipboard.SetText(ex.ToString());
            }
        }

        // Показываем прогресс загрузки файла
        void Web_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            label2.Text = e.ProgressPercentage.ToString() + "%";
            progressBar1.Value = e.ProgressPercentage;
        }

        void Web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Обновление завершено!\nИнтернет появится через 10-15 секунд!", "Внимание!");
            label4.Text = "Последнее обновление: " + File.GetLastWriteTime(@"C:\Windows\System32\drivers\\etc\hosts").ToString("dd/MM/yyyy");
        }

        async void Form1_Load(object sender, EventArgs e)
        {
            for (; Opacity < .93; Opacity += .04)
                await Task.Delay(30);
            label4.Text += File.GetLastWriteTime(@"C:\Windows\System32\drivers\\etc\hosts").ToString("dd/MM/yyyy");
        }

        void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g;
            string sText;
            int iX;
            float iY;

            SizeF sizeText;
            TabControl ctlTab;

            ctlTab = (TabControl)sender;

            g = e.Graphics;

            sText = ctlTab.TabPages[e.Index].Text;
            sizeText = g.MeasureString(sText, ctlTab.Font);
            iX = e.Bounds.Left + 6;
            iY = e.Bounds.Top + (e.Bounds.Height - sizeText.Height) / 2;
            g.DrawString(sText, ctlTab.Font, Brushes.Black, iX, iY);
            e.Graphics.SetClip(e.Bounds);
            string text = tabControl1.TabPages[e.Index].Text;
            SizeF sz = e.Graphics.MeasureString(text, e.Font);

            bool bSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            using (SolidBrush b = new SolidBrush(bSelected ? SystemColors.Highlight : SystemColors.Control))
                e.Graphics.FillRectangle(b, e.Bounds);

            using (SolidBrush b = new SolidBrush(bSelected ? SystemColors.HighlightText : SystemColors.ControlText))
                e.Graphics.DrawString(text, e.Font, b, e.Bounds.X + 2, e.Bounds.Y + (e.Bounds.Height - sz.Height) / 2);

            if (tabControl1.SelectedIndex == e.Index)
                e.DrawFocusRectangle();

            e.Graphics.ResetClip();
        }

        void Button3_Click(object sender, EventArgs e)
        {
            ProcessStartInfo cmd;
            cmd = new ProcessStartInfo("cmd", @"/c ipconfig /flushdns");
            Uri uri = new Uri("https://dropbox.com/s/mvjwnxi97wldzx8/hosts?dl=1");
            using (WebClient wc = new WebClient())
                wc.DownloadFileAsync(uri, @"C:\Windows\System32\drivers\\etc\hosts");
            MessageBox.Show("Восстановлен стандартный файл hosts", "Внимание!");
            Process.Start(cmd);
            label4.Text += File.GetLastWriteTime(@"C:\Windows\System32\drivers\\etc\hosts").ToString("dd/MM/yyyy");
        }

        void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(@"C:\Windows\System32\drivers\etc\hosts_bak"))
                {
                    File.Delete(@"C:\Windows\System32\drivers\etc\hosts");
                    File.Move(@"C:\Windows\System32\drivers\etc\hosts_bak", @"C:\Windows\System32\drivers\etc\hosts");
                    label4.Text += File.GetLastWriteTime(@"C:\Windows\System32\drivers\\etc\hosts").ToString("dd/MM/yyyy");
                }
                else
                {
                    MessageBox.Show("Бэкапа нет");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Произошла какая-то неведомая херня.\nКод ошибки записан в буфер обмена", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Clipboard.Clear();
                Clipboard.SetText(ex.ToString());
            }
        }
    }
}
