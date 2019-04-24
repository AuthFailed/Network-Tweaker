using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Update_Your_Hosts;

namespace Upgrade_Your_Network
{
    /// <inheritdoc />
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string Backup { get; } = @"C:\\Windows\System32\drivers\etc\hosts_bak";

        private string Path { get; } = @"C:\Windows\System32\drivers\etc\hosts";

        private async void Form1_Load(object sender, EventArgs e)
        {
            OptionsLoad();
            EditHosts();
            label4.Text +=
                File.GetLastWriteTime(Path).ToString("dd/MM/yyyy");
            for (; Opacity < .93; Opacity += .04)
                await Task.Delay(30).ConfigureAwait(false);
            var sc = new ServiceController(@"Dnscache");
            if (sc.Status == ServiceControllerStatus.Running)
                return;
            CmdExe(@"sc start Dnscache");
            Notification(@"В фонов режиме была запущена служба DNS");
        }

        private void OptionsLoad()
        {
            using (var key =
                Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true))
            {
                if (key != null)
                    proxybox.Checked = key.GetValue("AutoConfigURL")?.ToString() ==
                                       @"https://antizapret.prostovpn.org/proxy.pac";
            }

            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Update Your Hosts", false))
            {
                autoupdatebox.Checked = key?.GetValue("AutoUpdate")?.ToString() == "1";
                protocolbox.Checked = key?.GetValue("Protocols")?.ToString() == "1";
                checkBox2.Checked = key?.GetValue("Shell")?.ToString() == "1";
            }
        }

        private static void CmdExe(string line)
        {
            Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = $"/c {line}",
                    WindowStyle = ProcessWindowStyle.Hidden
                })
                ?.WaitForExit();
        }

        private void Notification(string line)
        {
            var cf = new Form2();
            {
                cf.Show();
                cf.label2.Text = line;
            }
            Activate();
        }

        // ReSharper disable once MethodTooLong
        private void Button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    UpdHosts("http://sbc.io/hosts/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 1:
                    UpdHosts("http://sbc.io/hosts/alternates/fakenews/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 2:
                    UpdHosts("http://sbc.io/hosts/alternates/gambling/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 3:
                    UpdHosts("http://sbc.io/hosts/alternates/porn/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 4:
                    UpdHosts("http://sbc.io/hosts/alternates/social/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 5:
                    UpdHosts("http://sbc.io/hosts/alternates/fakenews-gambling/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 6:
                    UpdHosts("http://sbc.io/hosts/alternates/fakenews-porn/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 7:
                    UpdHosts("http://sbc.io/hosts/alternates/fakenews-social/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 8:
                    UpdHosts("http://sbc.io/hosts/alternates/gambling-porn/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 9:
                    UpdHosts("http://sbc.io/hosts/alternates/gambling-social/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 10:
                    UpdHosts("http://sbc.io/hosts/alternates/porn-social/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 11:
                    UpdHosts("http://sbc.io/hosts/alternates/fakenews-gambling-porn/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 12:
                    UpdHosts("http://sbc.io/hosts/alternates/fakenews-gambling-social/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 13:
                    UpdHosts("http://sbc.io/hosts/alternates/fakenews-porn-social/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 14:
                    UpdHosts("http://sbc.io/hosts/alternates/gambling-porn-social/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                case 15:
                    UpdHosts("http://sbc.io/hosts/alternates/fakenews-gambling-porn-social/hosts");
                    Notification(@"Фильтр успешно обновлен.");
                    break;
                default:
                    MessageBox.Show(@"Выберите фильтр!");
                    break;
            }
        }

        private void TxtRdNl()
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"192 . 168 . 1 . 1";
                    textBox2.Text = @"192 . 168 . 1 . 1";
                    break;
                case 1:
                    textBox1.ReadOnly = false;
                    textBox2.ReadOnly = false;
                    break;
                case 2:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"77 . 88 . 8 . 1";
                    textBox2.Text = @"77 . 88 . 8 . 8";
                    break;
                case 3:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"8 . 8 . 8 . 8";
                    textBox2.Text = @"8 . 8 . 4 . 4";
                    break;
                case 4:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"208 . 67 . 222 . 222";
                    textBox2.Text = @"208 . 67 . 220 . 220";
                    break;
                case 5:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"208 . 67 . 222 . 220";
                    textBox2.Text = @"208 . 67 . 220 . 222";
                    break;
                case 6:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"176 . 103 . 130 . 130";
                    textBox2.Text = @"176 . 103 . 130 . 131";
                    break;
                case 7:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"1 . 1 . 1 . 1";
                    textBox2.Text = @"1 . 0 . 0 . 1";
                    break;
                default:
                    textBox1.Text = "";
                    textBox2.Text = "";
                    break;
            }
        }

        private void UpdHosts(string line)
        {
            if (File.Exists(Backup))
                File.Delete(Backup);
            if (File.Exists(Path))
                File.Copy(Path, Backup);
            var filer = new Uri(line);
            try
            {
                using (var wc = new WebClient())
                {
                    wc.DownloadFileCompleted += Web_DownloadFileCompleted;
                    wc.DownloadFileAsync(filer, Path);
                }

                CmdExe(@"ipconfig / flushdns");
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Произошла какая-то неведомая херня.\nКод ошибки записан в буфер обмена", @"Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Clipboard.Clear();
                Clipboard.SetText(ex.Message);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                MessageBox.Show(@"Введите домен!", @"Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var array = File.ReadAllLines(Path);
                if (!richTextBox1.Text.Contains("."))
                {
                    MessageBox.Show(@"Домен должен быть вида сайт.зона", @"Внимание!");
                }
                else
                {
                    var t = 0;
                    if (array.Any(ar => ar.Equals(richTextBox1.Text)))
                    {
                        MessageBox.Show(@"Данный домен уже есть в фильтре", @"Внимание!");
                        t++;
                    }

                    if (t >= 1) return;
                    using (var sw = new StreamWriter(Path, true))
                    {
                        sw.WriteLine("0.0.0.0 " + richTextBox1.Text);
                        Notification(@"Домен " + richTextBox1.Text + @" добавлен в черный список");
                    }

                    CmdExe(@"ipconfig / flushdns");
                }
            }
        }

        private void Web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            label4.Text = @"Последнее обновление: " + File.GetLastWriteTime(Path).ToString("dd/MM/yyyy");
        }

        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            var ctlTab = (TabControl) sender;

            var g = e.Graphics;

            var sText = ctlTab.TabPages[e.Index].Text;
            var sizeText = g.MeasureString(sText, ctlTab.Font);
            var iX = e.Bounds.Left + 6;
            var iY = e.Bounds.Top + (e.Bounds.Height - sizeText.Height) / 2;
            g.DrawString(sText, ctlTab.Font, Brushes.Black, iX, iY);
            e.Graphics.SetClip(e.Bounds);
            var text = tabControl1.TabPages[e.Index].Text;
            var sz = e.Graphics.MeasureString(text, e.Font);

            var bSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            using (var b = new SolidBrush(bSelected ? SystemColors.Highlight : SystemColors.Control))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }

            using (var b = new SolidBrush(bSelected ? SystemColors.HighlightText : SystemColors.ControlText))
            {
                e.Graphics.DrawString(text, e.Font, b, e.Bounds.X + 2, e.Bounds.Y + (e.Bounds.Height - sz.Height) / 2);
            }

            if (tabControl1.SelectedIndex == e.Index)
                e.DrawFocusRectangle();

            e.Graphics.ResetClip();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            var uri = new Uri("https://raw.githubusercontent.com/AuthFailed/Update-Your-Hosts/master/hosts");
            using (var wc = new WebClient())
            {
                wc.DownloadFileAsync(uri, Path);
            }

            Notification(@"Восстановлен стандартный файл hosts");
            Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = @"/c ipconfig /flushdns",
                    WindowStyle = ProcessWindowStyle.Hidden
                })
                ?.WaitForExit();
            label4.Text += File.GetLastWriteTime(Path).ToString("dd/MM/yyyy");
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (File.Exists(Backup))
            {
                File.Delete(Path);
                File.Move(Backup, Path);
                label4.Text += File.GetLastWriteTime(Path).ToString("dd/MM/yyyy");
                Notification(@"Бэкап восстановлен");
            }
            else
            {
                MessageBox.Show(@"Бэкапа нет");
            }
        }

        private void EditHosts()
        {
            using (var sr = new StreamReader(Path))
            {
                richTextBox2.Text = sr.ReadToEnd();
            }
        }

        private void AUpd()
        {
            if (autoupdatebox.Checked && !File.Exists(@"C:\Windows\hosts.exe"))
                try
                {
                    AUpdTrue();
                }
                catch
                {
                    MessageBox.Show(@"Не нажимайте так часто");
                }
            else
                try
                {
                    AUpdFalse();
                }
                catch
                {
                    MessageBox.Show(@"Не нажимайте так часто");
                }
                finally
                {
                    AUpdFalse();
                }
        }

        private static void AUpdTrue()
        {
            var path = Environment.GetEnvironmentVariable("SYSTEMROOT");
            var uri = new Uri("https://github.com/AuthFailed/Update-Your-Hosts/raw/master/hosts.exe");
            try
            {
                using (var wc = new WebClient())
                {
                    wc.DownloadFileAsync(uri, path + @"\hosts.exe");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Что-то случилось и не удалось скачать файл");
                Clipboard.Clear();
                Clipboard.SetText(ex.ToString());
            }

            using (var key =
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                key?.SetValue("Hosts Update", $@"{path}\hosts.exe");
            }
        }

        private static void AUpdFalse()
        {
            var path = Environment.GetEnvironmentVariable("SYSTEMROOT");
            if (File.Exists(path + @"\hosts.exe"))
                File.Delete(path + @"\hosts.exe");
            using (var key =
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (key?.GetValue("Hosts Update") != null)
                    key.DeleteValue("Hosts Update");
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
                TxtRdNl();
                foreach (string line in comboBox2.Items)
                    if (line == textBox1.Text)
                        comboBox2.SelectedIndex = comboBox2.FindString(textBox2.Text);
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            progressBar2.Value = 0;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    ChangeMainDns("192.168.1.1");
                    progressBar2.Value = 50;
                    ChangeExtraDns("");
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 1:
                    ChangeMainDns(textBox1.Text.Trim());
                    progressBar2.Value = 50;
                    ChangeExtraDns(textBox2.Text.Trim());
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 2:
                    ChangeMainDns("77.88.8.1");
                    progressBar2.Value = 50;
                    ChangeExtraDns("77.88.8.8");
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 3:
                    ChangeMainDns("8.8.8.8");
                    progressBar2.Value = 50;
                    ChangeExtraDns("8.8.4.4");
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 4:
                    ChangeMainDns("208.67.222.222");
                    progressBar2.Value = 50;
                    ChangeExtraDns("208.67.220.220");
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 5:
                    ChangeMainDns("208.67.222.220");
                    progressBar2.Value = 50;
                    ChangeExtraDns("208.67.220.222");
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 6:
                    ChangeMainDns("176.103.130.130");
                    progressBar2.Value = 50;
                    ChangeExtraDns("176.103.130.131");
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 7:
                    ChangeMainDns("1.1.1.1");
                    progressBar2.Value = 50;
                    ChangeExtraDns("1.0.0.1");
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
            }
        }

        private static void ChangeMainDns(string line)
        {
            Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = $"/c netsh interface ipv4 set dnsservers \"Ethernet\" static address={line} primary",
                    WindowStyle = ProcessWindowStyle.Hidden
                })
                ?.WaitForExit();
        }

        private static void ChangeExtraDns(string line)
        {
            Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = $"/c netsh interface ipv4 add dnsservers \"Ethernet\" address={line}",
                    WindowStyle = ProcessWindowStyle.Hidden
                })
                ?.WaitForExit();
        }

        private void Autoupdatebox_CheckedChanged(object sender, EventArgs e)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Update Your Hosts", true))
            {
                if (autoupdatebox.Checked)
                {
                    AUpd();
                    key.SetValue("AutoUpdate", "1");
                }
                else
                {
                    AUpd();
                    key.SetValue("AutoUpdate", "0");
                }
            }
        }

        private void Protocolbox_CheckedChanged(object sender, EventArgs e)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Update Your Hosts", true))
            {
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
                        })
                        ?.WaitForExit();
                    key.SetValue("Protocols", "1");
                }
                else
                {
                    Process.Start(new ProcessStartInfo
                        {
                            FileName = "cmd",
                            Arguments =
                                "/c netsh interface teredo set state type=default servername=default refreshinterval=default clientport=default & " +
                                "netsh interface isatap set state enabled & " +
                                "netsh int ipv6 isatap set state enabled &" +
                                "netsh int ipv6 6to4 set state enabled &" +
                                "netsh interface IPV6 set global randomizeidentifier=enabled &" +
                                "netsh interface IPV6 set privacy state=enabled",
                            WindowStyle = ProcessWindowStyle.Hidden
                        })
                        ?.WaitForExit();
                    key.SetValue("Protocols", "0");
                }
            }
        }

        private void Proxybox_CheckedChanged(object sender, EventArgs e)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Update Your Hosts", true))
            {
                if (proxybox.Checked)
                {
                    using (var settings =
                        Registry.CurrentUser.CreateSubKey(
                            @"Software\Microsoft\Windows\CurrentVersion\Internet Settings"))
                    {
                        settings?.SetValue("AutoConfigURL", "https://antizapret.prostovpn.org/proxy.pac");
                    }

                    key?.SetValue("Proxy", "1");
                }
                else
                {
                    using (var settings =
                        Registry.CurrentUser.CreateSubKey(
                            @"Software\Microsoft\Windows\CurrentVersion\Internet Settings"))
                    {
                        settings?.DeleteValue("AutoConfigURL");
                    }

                    key?.SetValue("Proxy", "0");
                }
            }
        }

        private async void Button5_Click(object sender, EventArgs e)
        {
            using (var fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var sw = new StreamWriter(fs))
            {
                var a = File.ReadAllLines(Path);
                if (richTextBox2.Text != a.ToString()) await sw.WriteLineAsync(Convert.ToChar(a)).ConfigureAwait(false);
            }

            Notification(@"Фильтр успешно изменен");
        }


        private void PictureBox1_Click(object sender, EventArgs e)
        {
            label10.Text = @"Ожидание";
            label11.Text = @"Ожидание";
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    Ping("192.168.1.1", "192.168.1.1");
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
                default:
                    textBox1.Text = @"-";
                    textBox2.Text = @"-";
                    break;
            }
        }

        // ReSharper disable once MissingSuppressionJustification
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private void Ping(string main, string extra)
        {
            var ping = new Ping();
            label10.Text = ping.Send($"{main}").RoundtripTime + @" мс";
            label11.Text = ping.Send($"{extra}").RoundtripTime + @" мс";
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = @"/n, /select, C:\Windows\System32\drivers\etc\hosts"
            });
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("regedit")) process.Kill();
            var path = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings";
            Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Applets\Regedit")
                ?.SetValue("LastKey", path);
            Process.Start("regedit");
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("regedit")) process.Kill();
            var path = @"HKEY_CURRENT_USER\Software\Update Your Hosts";
            Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Applets\Regedit")
                ?.SetValue("LastKey", path);
            Process.Start("regedit");
        }

        private void Autoupdatebox_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = @"Включает автоматическое обновление фильтра при каждой загрузке системы";
        }

        private void Autoupdatebox_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        private void Button4_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = @"Восстанавливает прошлую версию фильтра, если таковая существует";
        }

        private void Button4_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        private void Button3_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = @"Устанавливает стандартные значения для фильтра, которые поставляются вместе с Windows";
        }

        private void Button3_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        private void CheckBox1_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = @"Добавляет прокси Антизапрета, который разблокирует сайты, блокируемые РКН";
        }

        private void CheckBox1_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        private void Protocolbox_MouseEnter(object sender, EventArgs e)
        {
            label5.Text = @"Этот твик отключает ненужные для большинства протоколы Teredo, ISATAP и IPV6";
        }

        private void Protocolbox_MouseLeave(object sender, EventArgs e)
        {
            label5.Text = "";
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c netsh advfirewall set allprofiles state on",
                WindowStyle = ProcessWindowStyle.Hidden
            });
            if (checkBox2.Checked)
            {
                using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\exefile\shell\Firewall_Allow"))
                {
                    key?.SetValue("", @"Разрешить доступ в интернет");
                    key?.SetValue("Extended", "");
                    key?.SetValue("Icon", @"netcenter.dll,10");
                }

                using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\exefile\shell\Firewall_Allow\command"))
                {
                    key?.SetValue("", @"netsh advfirewall firewall delete rule name=" + "\"%1\"");
                }

                using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\exefile\shell\Firewall_Block"))
                {
                    key?.SetValue("", @"Запретить доступ в интернет");
                    key?.SetValue("Extended", "");
                    key?.SetValue("Icon", @"netcenter.dll,5");
                }

                using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\exefile\shell\Firewall_Block\command"))
                {
                    key?.SetValue("",
                        @"cmd /d /c ""netsh advfirewall firewall add rule name=""%1"" dir=in action=block program=""%1"" & netsh advfirewall firewall add rule name=""%1"" dir=out action=block program=""%1""");
                }

                using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Update Your Hosts"))
                {
                    key?.SetValue("Shell", "1");
                }
            }
            else
            {
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Classes\exefile\shell\Firewall_Allow", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Classes\exefile\shell\Firewall_Block", false);
                using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Update Your Hosts"))
                {
                    key?.SetValue("Shell", "0");
                }
            }
        }

        private void PictureBox5_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("regedit")) process.Kill();
            const string path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\exefile\shell";
            Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Applets\Regedit")
                ?.SetValue("LastKey", path);
            Process.Start("regedit");
        }
    }
}