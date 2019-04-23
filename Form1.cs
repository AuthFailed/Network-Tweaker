using EnvDetection;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceProcess;
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

        void Cmd(string line)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c {line}",
                WindowStyle = ProcessWindowStyle.Hidden
            }).WaitForExit();
        }

        void Notification(string line)
        {
            Form2 childform = new Form2();
            childform.Show();
            childform.label2.Text = line;
            Activate();
        }

        readonly string hosts = @"C:\Windows\System32\drivers\etc\hosts";
        readonly string backup = @"C:\\Windows\System32\drivers\etc\hosts_bak";
        public string Path => hosts;

        // Определяем выбор фильтра и применяем его
        void Button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Updatehosts("http://sbc.io/hosts/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 1:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 2:
                    Updatehosts("http://sbc.io/hosts/alternates/gambling/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 3:
                    Updatehosts("http://sbc.io/hosts/alternates/porn/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 4:
                    Updatehosts("http://sbc.io/hosts/alternates/social/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 5:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-gambling/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 6:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-porn/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 7:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-social/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 8:
                    Updatehosts("http://sbc.io/hosts/alternates/gambling-porn/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 9:
                    Updatehosts("http://sbc.io/hosts/alternates/gambling-social/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 10:
                    Updatehosts("http://sbc.io/hosts/alternates/porn-social/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 11:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-gambling-porn/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 12:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-gambling-social/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 13:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-porn-social/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 14:
                    Updatehosts("http://sbc.io/hosts/alternates/gambling-porn-social/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                case 15:
                    Updatehosts("http://sbc.io/hosts/alternates/fakenews-gambling-porn-social/hosts");
                    Notification("Фильтр успешно обновлен.");
                    break;
                default:
                    MessageBox.Show("Выберите фильтр!");
                    break;
            }
        }

        void Textreadonly()
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
        }

        void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    checkBox1.Checked = false;
                    Textreadonly();
                    textBox1.Text = "192 . 168 . 1 . 1";
                    textBox2.Text = " . . . ";
                    break;
                case 1:
                    textBox1.ReadOnly = false;
                    textBox2.ReadOnly = false;
                    break;
                case 2:
                    checkBox1.Checked = false;
                    Textreadonly();
                    textBox1.Text = "77 . 88 . 8 . 1";
                    textBox2.Text = "77 . 88 . 8 . 8";
                    break;
                case 3:
                    checkBox1.Checked = false;
                    Textreadonly();
                    textBox1.Text = "8 . 8 . 8 . 8";
                    textBox2.Text = "8 . 8 . 4 . 4";
                    break;
                case 4:
                    checkBox1.Checked = false;
                    Textreadonly();
                    textBox1.Text = "208 . 67 . 222 . 222";
                    textBox2.Text = "208 . 67 . 220 . 220";
                    break;
                case 5:
                    checkBox1.Checked = false;
                    Textreadonly();
                    textBox1.Text = "208 . 67 . 222 . 220";
                    textBox2.Text = "208 . 67 . 220 . 222";
                    break;
                case 6:
                    checkBox1.Checked = false;
                    Textreadonly();
                    textBox1.Text = "176 . 103 . 130 . 130";
                    textBox2.Text = "176 . 103 . 130 . 131";
                    break;
                case 7:
                    checkBox1.Checked = false;
                    Textreadonly();
                    textBox1.Text = "1 . 1 . 1 . 1";
                    textBox2.Text = "1 . 0 . 0 . 1";
                    break;
            }
        }

        // Загрузка и применение фильтра
        void Updatehosts(string line)
        {
            try
            {
                if (File.Exists(backup))
                    File.Delete(backup);
                if (File.Exists(hosts))
                    File.Copy(hosts, backup);
                Uri fileuri = new Uri(line);
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFileCompleted += Web_DownloadFileCompleted;
                    wc.DownloadFileAsync(fileuri, hosts);
                }
                Cmd("ipconfig / flushdns");
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
                        if (ar.Equals(richTextBox1.Text)) // А вдруг такой домен уже есть в фильтре
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
                            Notification("Домен " + richTextBox1.Text + " добавлен в черный список");
                        }
                        Cmd("ipconfig / flushdns");
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

        // При успешной загрузке файла выводим уведомление об успехе и обновляем дату изменения фильтра
        void Web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
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
            iY = e.Bounds.Top + ((e.Bounds.Height - sizeText.Height) / 2);
            g.DrawString(sText, ctlTab.Font, Brushes.Black, iX, iY);
            e.Graphics.SetClip(e.Bounds);
            string text = tabControl1.TabPages[e.Index].Text;
            SizeF sz = e.Graphics.MeasureString(text, e.Font);

            bool bSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            using (SolidBrush b = new SolidBrush(bSelected ? SystemColors.Highlight : SystemColors.Control))
                e.Graphics.FillRectangle(b, e.Bounds);

            using (SolidBrush b = new SolidBrush(bSelected ? SystemColors.HighlightText : SystemColors.ControlText))
                e.Graphics.DrawString(text, e.Font, b, e.Bounds.X + 2, e.Bounds.Y + ((e.Bounds.Height - sz.Height) / 2));

            if (tabControl1.SelectedIndex == e.Index)
                e.DrawFocusRectangle();

            e.Graphics.ResetClip();
        }

        // Сбрасываем фильтр и восстанавливаем стандартное значение
        void Button3_Click(object sender, EventArgs e)
        {
            Uri uri = new Uri("https://raw.githubusercontent.com/AuthFailed/Update-Your-Hosts/master/hosts");
            using (WebClient wc = new WebClient())
                wc.DownloadFileAsync(uri, hosts);
            Notification("Восстановлен стандартный файл hosts");
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = @"/c ipconfig /flushdns",
                WindowStyle = ProcessWindowStyle.Hidden
            }).WaitForExit();
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
                    Notification("Бэкап восстановлен");
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

        void EditHosts()
        {
            using (StreamReader sr = new StreamReader(hosts))
                richTextBox2.Text = sr.ReadToEnd();
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
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.Text = textBox1.Text.Replace(" ", "");
                textBox2.Text = textBox2.Text.Replace(" ", "");
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                comboBox2.SelectedIndex = 1;
            }
            else
            {
                textBox1.Text = textBox1.Text.Replace(".", " . ");
                textBox2.Text = textBox2.Text.Replace(".", " . ");
                Textreadonly();
                foreach (string line in comboBox2.Items)
                {
                    if (line == textBox1.Text)
                        comboBox2.SelectedIndex = comboBox2.FindString(textBox2.Text);
                }
            }
        }

        void Button6_Click(object sender, EventArgs e)
        {
            progressBar2.Value = 0;
            ServiceController sc = new ServiceController("Dnscache");
            if (sc.Status != ServiceControllerStatus.Running)
                Cmd("sc start Dnscache");
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    ChangeMainDns("192.168.1.1");
                    progressBar2.Value = 50;
                    ChangeExtraDns("");
                    progressBar2.Value = 100;
                    Notification("DNS был успешно изменен");
                    break;
                case 1:
                    ChangeMainDns(textBox1.Text.Trim());
                    progressBar2.Value = 50;
                    ChangeExtraDns(textBox2.Text.Trim());
                    progressBar2.Value = 100;
                    Notification("DNS был успешно изменен");
                    break;
                case 2:
                    ChangeMainDns("77.88.8.1");
                    progressBar2.Value = 50;
                    ChangeExtraDns("77.88.8.8");
                    progressBar2.Value = 100;
                    Notification("DNS был успешно изменен");
                    break;
                case 3:
                    ChangeMainDns("8.8.8.8");
                    progressBar2.Value = 50;
                    ChangeExtraDns("8.8.4.4");
                    progressBar2.Value = 100;
                    Notification("DNS был успешно изменен");
                    break;
                case 4:
                    ChangeMainDns("208.67.222.222");
                    progressBar2.Value = 50;
                    ChangeExtraDns("208.67.220.220");
                    progressBar2.Value = 100;
                    Notification("DNS был успешно изменен");
                    break;
                case 5:
                    ChangeMainDns("208.67.222.220");
                    progressBar2.Value = 50;
                    ChangeExtraDns("208.67.220.222");
                    progressBar2.Value = 100;
                    Notification("DNS был успешно изменен");
                    break;
                case 6:
                    ChangeMainDns("176.103.130.130");
                    progressBar2.Value = 50;
                    ChangeExtraDns("176.103.130.131");
                    progressBar2.Value = 100;
                    Notification("DNS был успешно изменен");
                    break;
                case 7:
                    ChangeMainDns("1.1.1.1");
                    progressBar2.Value = 50;
                    ChangeExtraDns("1.0.0.1");
                    progressBar2.Value = 100;
                    Notification("DNS был успешно изменен");
                    break;
            }
        }

        void ChangeMainDns(string line)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c netsh interface ipv4 set dnsservers \"Ethernet\" static address={line} primary",
                WindowStyle = ProcessWindowStyle.Hidden
            }).WaitForExit();
        }
        void ChangeExtraDns(string line)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c netsh interface ipv4 add dnsservers \"Ethernet\" address={line}",
                WindowStyle = ProcessWindowStyle.Hidden
            }).WaitForExit();
        }

        // Проверяем статус чекбоксов
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
        void Protocolbox_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Update Your Hosts", true))
                if (protocolbox.Checked)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = "/c netsh interface teredo set state disabled & " +
                            "netsh interface isatap set state disabled & " +
                            "netsh int ipv6 isatap set state disabled &" +
                            "netsh int ipv6 6to4 set state disabled &" +
                            "netsh interface IPV6 set global randomizeidentifier=disabled &" +
                            "netsh interface IPV6 set privacy state=disable",
                        WindowStyle = ProcessWindowStyle.Hidden
                    }).WaitForExit();
                    key.SetValue("Protocols", "1");
                }
                else
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = "/c netsh interface teredo set state type=default servername=default refreshinterval=default clientport=default & " +
                                                "netsh interface isatap set state enabled & " +
                                                "netsh int ipv6 isatap set state enabled &" +
                                                "netsh int ipv6 6to4 set state enabled &" +
                                                "netsh interface IPV6 set global randomizeidentifier=enabled &" +
                                                "netsh interface IPV6 set privacy state=enabled",
                        WindowStyle = ProcessWindowStyle.Hidden
                    }).WaitForExit();
                    key.SetValue("Protocols", "0");
                }
        }

        void Proxybox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Update Your Hosts", true))
                    if (proxybox.Checked)
                    {
                        using (RegistryKey settingskey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings"))
                            settingskey.SetValue("AutoConfigURL", "https://antizapret.prostovpn.org/proxy.pac");
                        key.SetValue("Proxy", "1");
                    }
                    else
                    {
                        using (RegistryKey settingskey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings"))
                            settingskey.DeleteValue("AutoConfigURL");
                        key.SetValue("Proxy", "0");
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
            Notification("Фильтр успешно изменен");
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

        void CheckBox1_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = "Добавляет прокси Антизапрета, который разблокирует сайты, блокируемые РКН";
        }
        void CheckBox1_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        void Protocolbox_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = "Этот твик отключает ненужные для большинства протоколы Teredo, ISATAP и IPV6";
        }
        void Protocolbox_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        void PictureBox1_Click(object sender, EventArgs e)
        {
            label10.Text = "Ожидание";
            label11.Text = "Ожидание";
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    Ping(textBox1.Text.Replace(" ", ""), textBox2.Text.Replace(" ", ""));
                    break;
                case 2:
                    Ping("77.88.8.1", "77.88.8.8");
                    break;
                case 3:
                    Ping("8.8.8.8", "8.8.4.4");
                    break;
                case 4:
                    Ping("208.67.222.222", "208.67.220.220");
                    break;
                case 5:
                    Ping("208.67.222.220", "208.67.220.222");
                    break;
                case 6:
                    Ping("176.103.130.130", "176.103.130.131");
                    break;
                case 7:
                    Ping("1.1.1.1", "1.0.0.1");
                    break;
            }
        }

        void Ping(string mainaddress, string extraadress)
        {
            Ping ping = new Ping();
            PingReply pingReply = null;
            pingReply = ping.Send($"{mainaddress}");
            label10.Text = pingReply.RoundtripTime.ToString() + " мс";
            pingReply = ping.Send($"{extraadress}");
            label11.Text = pingReply.RoundtripTime.ToString() + " мс";
        }

        void PictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = $@"/n, /select, C:\Windows\System32\drivers\etc\hosts"
            });
        }

        void PictureBox3_Click(object sender, EventArgs e)
        {
            foreach (Process process in Process.GetProcessesByName("regedit")) process.Kill();
            string path = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings";
            Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Applets\Regedit")
                .SetValue("LastKey", path);
            Process.Start("regedit");
        }

        void PictureBox4_Click(object sender, EventArgs e)
        {
            foreach (Process process in Process.GetProcessesByName("regedit")) process.Kill();
            string path = @"HKEY_CURRENT_USER\Software\Update Your Hosts";
            Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Applets\Regedit")
                .SetValue("LastKey", path);
            Process.Start("regedit");
        }
    }
}
