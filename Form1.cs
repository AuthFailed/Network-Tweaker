using Microsoft.Win32;
using System;
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

        async void Form1_Load(object sender, EventArgs e)
        {
            OptionsLoad();
            EditHosts();
            label4.Text += File.GetLastWriteTime(hosts).ToString("dd/MM/yyyy"); // Получаем дату последнего изменения фильтра
            // Плавность появление формы
            for (; Opacity < .93; Opacity += .04)
                await Task.Delay(30);
        }


        // Подгрузка настроек из реестра
        void OptionsLoad()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Update Your Hosts", false))
            {
                autoupdatebox.Checked = key?.GetValue("AutoUpdate").ToString() == "1" ? true : false;
                proxybox.Checked = key?.GetValue("Proxy").ToString() == "1" ? true : false;
                protocolbox.Checked = key?.GetValue("Protocols").ToString() == "1" ? true : false;
            }
        }

        readonly string hosts = @"C:\Windows\System32\drivers\etc\hosts";
        readonly string backup = @"C:\\Windows\System32\drivers\etc\hosts_bak";
        public string Path => hosts;

        // Определяем выбор фильтра и применяем его
        void Button1_Click(object sender, EventArgs e)
        {
            label2.Text = "";
            progressBar1.Value = 0;
            switch (comboBox1.SelectedIndex)
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

        // Загрузка и применение фильтра
        void Updatehosts(string line)
        {
            ProcessStartInfo cmd;
            cmd = new ProcessStartInfo("cmd", @"/c ipconfig /flushdns");
            try
            {
                if (File.Exists(backup))
                    File.Delete(backup);
                if (File.Exists(hosts))
                    File.Copy(hosts, backup);
                Uri fileuri = new Uri(line);
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += Web_DownloadProgressChanged;
                    wc.DownloadFileCompleted += Web_DownloadFileCompleted;
                    wc.DownloadFileAsync(fileuri, hosts);
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

        // Ручное добавления доменов в фильтр
        void Button2_Click(object sender, EventArgs e)
        {
            ProcessStartInfo cmd;
            cmd = new ProcessStartInfo("cmd", @"/c ipconfig /flushdns");
            try
            {
                if (string.IsNullOrWhiteSpace(richTextBox1.Text)) // При пустой строке
                {
                    MessageBox.Show("Введите домен!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!richTextBox1.Text.Contains(".")) // Если  не находим точки в домене
                {
                    MessageBox.Show("Домен должен быть вида сайт.зона", "Внимание!");
                }
                else
                {
                    int t = 0;
                    string[] array = File.ReadAllLines(hosts);
                    foreach (string ar in array)
                    {
                        string line = ar.Replace("0.0.0.0 ", "");
                        if (line.Equals(richTextBox1.Text)) // А вдруг такой домен уже есть в фильтре
                        {
                            MessageBox.Show("Данный домен уже есть в фильтре", "Внимание!");
                            t++;
                            break;
                        }
                    }
                    if (t < 1)
                    {
                        // Добавляем новый домен в фильтр
                        using (StreamWriter sw = new StreamWriter(hosts, append: true))
                        {
                            sw.WriteLine("0.0.0.0 " + richTextBox1.Text);
                            MessageBox.Show("Домен " + richTextBox1.Text + " добавлен в черный список");
                        }
                        Process.Start(cmd);
                    }
                }
            }
            // Отлавливаем ошибки
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

        // При успешной загрузке файла выводим уведомление об успехе и обновляем дату изменения фильтра
        void Web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Обновление завершено!\nИнтернет появится через 10-15 секунд!", "Внимание!");
            label4.Text = "Последнее обновление: " + File.GetLastWriteTime(hosts).ToString("dd/MM/yyyy");
        }


        // Отрисовываем вертикальные вкладки
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

        // Сбрасываем фильтр и восстанавливаем стандартное значение
        void Button3_Click(object sender, EventArgs e)
        {
            ProcessStartInfo cmd;
            cmd = new ProcessStartInfo("cmd", @"/c ipconfig /flushdns");
            Uri uri = new Uri("https://raw.githubusercontent.com/AuthFailed/Update-Your-Hosts/master/hosts");
            using (WebClient wc = new WebClient())
                wc.DownloadFileAsync(uri, hosts);
            MessageBox.Show("Восстановлен стандартный файл hosts", "Внимание!");
            Process.Start(cmd);
            label4.Text += File.GetLastWriteTime(hosts).ToString("dd/MM/yyyy");
        }

        // Восстанавливаем фильтр из бэкапа
        void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(backup))
                {
                    File.Delete(hosts);
                    File.Move(backup, hosts);
                    label4.Text += File.GetLastWriteTime(hosts).ToString("dd/MM/yyyy");
                }
                else
                {
                    MessageBox.Show("Бэкапа нет");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла какая-то неведомая херня.\nКод ошибки записан в буфер обмена", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Clipboard.Clear();
                Clipboard.SetText(ex.ToString());
            }
        }

        // Автоматическое обновление
        void Autoupd()
        {
            string path = Environment.GetEnvironmentVariable("SYSTEMROOT");
            // Проверяем включен ли чекбокс и отсутствие файла
            if (autoupdatebox.Checked && !File.Exists(@"C:\Windows\hosts.exe"))
            {
                // Качаем файл
                try
                {
                    Uri uri = new Uri("https://github.com/AuthFailed/Update-Your-Hosts/raw/master/hosts.exe");
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFileAsync(uri, path + @"\hosts.exe");
                    }
                    // Добавляем файл в автозагрузку с помощью реестра
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                        key.SetValue("Hosts Update", $@"{path}\hosts.exe");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла какая-то неведомая херня.\nКод ошибки записан в буфер обмена", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Clipboard.Clear();
                    Clipboard.SetText(ex.ToString());
                }
            }
            else
            {
                // Если находим либо включенный чекбокс, либо файл - удаляем из автозагрузки и удаляем файл
                try
                {
                    if (File.Exists(path + @"\hosts.exe"))
                        File.Delete(path + @"\hosts.exe");
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                        if (key.GetValue("Hosts Update") != null)
                            key.DeleteValue("Hosts Update");
                }
                catch
                {
                    MessageBox.Show("Не тыкай так часто!");
                }
            }
        }

        void EditHosts()
        {
            using (StreamReader sr = new StreamReader(hosts))
                richTextBox2.Text = sr.ReadToEnd();
        }

        // Проверяем статус чекбокса
        void Autoupdatebox_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Update Your Hosts", true))
                if (autoupdatebox.Checked)
                {
                    {
                        Autoupd();
                    }
                    key.SetValue("AutoUpdate", "1");
                }
                else
                {
                    {
                        Autoupd();
                    }
                    key.SetValue("AutoUpdate", "0");
                }
        }

        // Вывод описания каждого контрола в настройках
        void Autoupdatebox_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = "Включает автоматическое обновление фильтра при каждой загрузке системы";
        }
        void Autoupdatebox_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        void Button4_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = "Восстанавливает прошлую версию фильтра, если таковая существует";
        }
        void Button4_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        void Button3_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = "Устанавливает стандартные значения для фильтра, которые поставляются вместе с Windows";
        }
        void Button3_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        async void Button5_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream(hosts, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (StreamReader sr = new StreamReader(fs))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                string[] a = File.ReadAllLines(hosts);
                if (richTextBox2.Text != a.ToString())
                {
                    await sw.WriteLineAsync(Convert.ToChar(a));
                }
            }
        }

        void CheckBox1_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = "Добавляет прокси Антизапрета, который разблокирует сайты, блокируемые РКН";
        }

        void CheckBox1_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        void Proxybox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Update Your Hosts", true))
                    if (proxybox.Checked)
                    {
                        {
                            Proxyon();
                        }
                        key.SetValue("Proxy", "1");
                    }
                    else
                    {
                        {
                            Proxyoff();
                        }
                        key.SetValue("Proxy", "0");
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void Proxyon()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings"))
                key.SetValue("AutoConfigURL", "https://antizapret.prostovpn.org/proxy.pac");
        }

        void Proxyoff()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings"))
                key.DeleteValue("AutoConfigURL");
        }

        void Protocolbox_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = "Этот твик отключает ненужные для большинства протоколы Teredo, ISATAP и IPV6";
        }

        void Protocolbox_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        void Protocolbox_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Update Your Hosts", true))
                if (protocolbox.Checked)
                {
                    Process.Start("cmd.exe", "/C netsh interface teredo set state disabled && " +
                            "netsh interface isatap set state disabled && " +
                            "netsh int ipv6 isatap set state disabled &&" +
                            "netsh int ipv6 6to4 set state disabled &&" +
                            "netsh interface IPV6 set global randomizeidentifier=disabled &&" +
                            "netsh interface IPV6 set privacy state=disable");
                    key.SetValue("Protocols", "1");
                }
                else
                {
                    Process.Start("cmd.exe", "/C netsh interface teredo set state enabled && " +
                                                "netsh interface isatap set state enabled && " +
                                                "netsh int ipv6 isatap set state enabled &&" +
                                                "netsh int ipv6 6to4 set state enabled &&" +
                                                "netsh interface IPV6 set global randomizeidentifier=enabled &&" +
                                                "netsh interface IPV6 set privacy state=enabled");
                    key.SetValue("Protocols", "0");
                }
        }
    }
}
