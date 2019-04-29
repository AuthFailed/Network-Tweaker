using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Upgrade_Your_Network.Properties;

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
            comboBox1.SelectedIndexChanged += (s, a) => { button1.Enabled = true; };
            comboBox2.SelectedIndexChanged += (s, a) =>
            {
                pictureBox1.Enabled = true;
                label12.Enabled = true;
                label13.Enabled = true;
            };
            button6.Enabled = false;
            FuncLoad();
            tabControl1.SelectedIndexChanged += (s, a) => { Width = tabControl1.SelectedTab != tabPage5 ? 597 : 680; };
            OptionsLoad();
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

            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Upgrade Your Network", false))
            {
                autoupdatebox.Checked = key?.GetValue("AutoUpdate")?.ToString() == "1";
                protocolbox.Checked = key?.GetValue("Protocols")?.ToString() == "1";
                checkBox2.Checked = key?.GetValue("Shell")?.ToString() == "1";
                checkBox3.Checked = key?.GetValue("Optimize DnsCache")?.ToString() == "1";
            }
        }

        private void FuncLoad()
        {
            button1.MouseEnter += (s, e) => { button1.BackColor = Color.LightGray; };
            button2.MouseEnter += (s, e) => { button2.BackColor = Color.LightGray; };
            button3.MouseEnter += (s, e) => { button3.BackColor = Color.LightGray; };
            button4.MouseEnter += (s, e) => { button4.BackColor = Color.LightGray; };
            button6.MouseEnter += (s, e) => { button6.BackColor = Color.LightGray; };
            button1.MouseLeave += (s, e) => { button1.BackColor = Color.Gainsboro; };
            button2.MouseLeave += (s, e) => { button2.BackColor = Color.Gainsboro; };
            button3.MouseLeave += (s, e) => { button3.BackColor = Color.Gainsboro; };
            button4.MouseLeave += (s, e) => { button4.BackColor = Color.Gainsboro; };
            button6.MouseLeave += (s, e) => { button6.BackColor = Color.Gainsboro; };
            pictureBox1.MouseEnter += (s, e) => { pictureBox1.Image = Resources.Refresh_Gray; };
            pictureBox1.MouseLeave += (s, e) => { pictureBox1.Image = Resources.Refresh_Lite; };
            pictureBox2.MouseEnter += (s, e) => { pictureBox2.Image = Resources.Folder_Gray; };
            pictureBox2.MouseLeave += (s, e) => { pictureBox2.Image = Resources.Folder_Lite; };
            pictureBox6.MouseEnter += (s, e) => { pictureBox6.Image = Resources.Connections_Gray; };
            pictureBox6.MouseLeave += (s, e) => { pictureBox6.Image = Resources.Connections_Lite; };
            pictureBox6.MouseClick += (s, e) => { Process.Start("ncpa.cpl"); };
            pictureBox7.MouseEnter += (s, e) => { pictureBox7.Image = Resources.TestPing_Gray; };
            pictureBox7.MouseLeave += (s, e) => { pictureBox7.Image = Resources.TestPing_Lite; };
            pictureBox7.MouseClick += (s, e) =>
            {
                var form = new Form3();
                form.ShowDialog();
            };
            autoupdatebox.MouseEnter += (s, e) => { label5.Text = @"Включает автоматическое обновление фильтра при каждой загрузке системы"; };
            proxybox.MouseEnter += (s, e) => { label5.Text = @"Добавляет прокси Антизапрета, который разблокирует сайты, блокируемые РКН"; };
            protocolbox.MouseEnter += (s, e) => { label5.Text = @"Этот твик отключает ненужные для большинства протоколы Teredo, ISATAP и IPV6"; };
            checkBox2.MouseEnter += (s, e) => { label5.Text = @"Добавляет в контекстное меню пункт для блокировки интернета"; };
            checkBox3.MouseEnter += (s, e) => { label5.Text = @"Увеличение скорости загрузки веб-ресурсов"; };
            button3.MouseEnter += (sender, e) =>
            {
                label5.Text = @"Устанавливает стандартные значения для фильтра, которые поставляются вместе с Windows";
            };
            button4.MouseEnter += (sender, e) => { label5.Text = @"Восстанавливает прошлую версию фильтра, если таковая существует"; };
            autoupdatebox.MouseLeave += (s, e) => { label5.Text = @""; };
            proxybox.MouseLeave += (s, e) => { label5.Text = ""; };
            autoupdatebox.MouseLeave += (s, e) => { label5.Text = ""; };
            checkBox2.MouseLeave += (s, e) => { label5.Text = ""; };
            checkBox3.MouseLeave += (s, e) => { label5.Text = ""; };
            button3.MouseLeave += (sender, e) => { label5.Text = ""; };
            button4.MouseLeave += (sender, e) => { label5.Text = ""; };
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
            var audio = new SoundPlayer(Resources.s);
            audio.Play();
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
                    audio.Play();
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
                    textBox1.Text = @"192.168.1.1";
                    textBox2.Text = @"192.168.1.1";
                    button6.Enabled = true;
                    break;
                case 1:
                    textBox1.ReadOnly = false;
                    textBox2.ReadOnly = false;
                    button6.Enabled = true;
                    break;
                case 2:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"77.88.8.1";
                    textBox2.Text = @"77.88.8.8";
                    button6.Enabled = true;
                    break;
                case 3:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"8.8.8.8";
                    textBox2.Text = @"8.8.4.4";
                    button6.Enabled = true;
                    break;
                case 4:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"208.67.222.222";
                    textBox2.Text = @"208.67.220.220";
                    button6.Enabled = true;
                    break;
                case 5:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"208.67.222.220";
                    textBox2.Text = @"208.67.220.222";
                    button6.Enabled = true;
                    break;
                case 6:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"176.103.130.130";
                    textBox2.Text = @"176.103.130.131";
                    button6.Enabled = true;
                    break;
                case 7:
                    checkBox1.Checked = false;
                    TxtRdNl();
                    textBox1.Text = @"1.1.1.1";
                    textBox2.Text = @"1.0.0.1";
                    button6.Enabled = true;
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
            var audio = new SoundPlayer(Resources.s);
            audio.Play();
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
            var audio = new SoundPlayer(Resources.s);
            audio.Play();
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
            var audio = new SoundPlayer(Resources.s);
            audio.Play();
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
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                comboBox2.SelectedIndex = 1;
            }
            else
            {
                TxtRdNl();
                foreach (string line in comboBox2.Items)
                    if (line == textBox1.Text)
                        comboBox2.SelectedIndex = comboBox2.FindString(textBox2.Text);
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            var audio = new SoundPlayer(Resources.s);
            audio.Play();
            progressBar2.Value = 0;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    ChangeMainDns(textBox1.Text);
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
                    ChangeMainDns(textBox1.Text);
                    progressBar2.Value = 50;
                    ChangeExtraDns(textBox2.Text);
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 3:
                    ChangeMainDns(textBox1.Text);
                    progressBar2.Value = 50;
                    ChangeExtraDns(textBox2.Text);
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 4:
                    ChangeMainDns(textBox1.Text);
                    progressBar2.Value = 50;
                    ChangeExtraDns(textBox2.Text);
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 5:
                    ChangeMainDns(textBox1.Text);
                    progressBar2.Value = 50;
                    ChangeExtraDns(textBox2.Text);
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 6:
                    ChangeMainDns(textBox1.Text);
                    progressBar2.Value = 50;
                    ChangeExtraDns(textBox2.Text);
                    progressBar2.Value = 100;
                    Notification(@"DNS был успешно изменен");
                    break;
                case 7:
                    ChangeMainDns(textBox1.Text);
                    progressBar2.Value = 50;
                    ChangeExtraDns(textBox2.Text);
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
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Upgrade Your Network", true))
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
            using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Upgrade Your Network", true))
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
            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Upgrade Your Network", true))
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

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "") & (textBox2.Text != ""))
            {
                switch (comboBox2.SelectedIndex)
                {
                    case 0:
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        Task.Run(TtlGetAsync);
                        break;
                    case 1:

                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        Task.Run(TtlGetAsync);
                        break;
                    case 2:
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        Task.Run(TtlGetAsync);
                        break;
                    case 3:
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        Task.Run(TtlGetAsync);
                        break;
                    case 4:
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        Task.Run(TtlGetAsync);
                        break;
                    case 5:
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        Task.Run(TtlGetAsync);
                        break;
                    case 6:
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        Task.Run(TtlGetAsync);
                        break;
                    case 7:
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        Task.Run(TtlGetAsync);
                        break;
                    default:
                        textBox1.Text = @"-";
                        textBox2.Text = @"-";
                        break;
                }
            }
            else if (textBox1.Text == "" && textBox2.Text != "")
            {
                label10.Text = "";
                Task.Run(PingExtraAsync);
                Task.Run(TtlGetAsync);
            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {
                label11.Text = "";
                Task.Run(PingMainAsync);
                Task.Run(TtlGetAsync);
            }
        }

        // ReSharper disable once MissingSuppressionJustification
        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private async Task PingMainAsync()
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c chcp 65001 & ping {textBox1.Text} -n 3",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            });
            label10.Text = @"*";
            await Task.Delay(700).ConfigureAwait(false);
            label10.Text = @"**";
            await Task.Delay(700).ConfigureAwait(false);
            label10.Text = @"***";
            await Task.Delay(700).ConfigureAwait(false);
            if (process?.StandardOutput == null)
                return;
            var line = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
            var match = Regex.Match(line, @"(.*?)Average = (.*?)ms");
            label10.Text = match.Groups[2].Value + @" мс";
        }

        private async Task PingExtraAsync()
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c chcp 65001 & ping {textBox2.Text} -n 3",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            });
            label11.Text = @"*";
            await Task.Delay(700).ConfigureAwait(false);
            label11.Text = @"**";
            await Task.Delay(700).ConfigureAwait(false);
            label11.Text = @"***";
            await Task.Delay(700).ConfigureAwait(false);
            if (process?.StandardOutput == null)
                return;
            var line = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
            var match = Regex.Match(line, @"(.*?)Average = (.*?)ms");

            label11.Text = match.Groups[2].Value + @" мс";
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = @"/n, /select, C:\Windows\System32\drivers\etc\hosts"
            });
        }

        private void PictureBox4_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("regedit")) process.Kill();
            var path = @"HKEY_CURRENT_USER\Software\Upgrade Your Network";
            Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Applets\Regedit")
                ?.SetValue("LastKey", path);
            Process.Start("regedit");
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

                using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Upgrade Your Network"))
                {
                    key?.SetValue("Shell", "1");
                }
            }
            else
            {
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Classes\exefile\shell\Firewall_Allow", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Classes\exefile\shell\Firewall_Block", false);
                using (var key = Registry.CurrentUser.CreateSubKey(@"Software\Upgrade Your Network"))
                {
                    key?.SetValue("Shell", "0");
                }
            }
        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            using (var key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"))
            {
                if (checkBox3.Checked)
                {
                    {
                        key?.SetValue("CacheHashTableBucketSize", "10", RegistryValueKind.DWord);
                        key?.SetValue("NegativeCacheTime", "300", RegistryValueKind.DWord);
                        key?.SetValue("CacheHashTableSize", "211", RegistryValueKind.DWord);
                        key?.SetValue("MaxCacheEntryTtlLimit", "86400", RegistryValueKind.DWord);
                        key?.SetValue("MaxSOACacheEntryTtlLimit", "120", RegistryValueKind.DWord);
                    }

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = "/c ipconfig /flushdns",
                        WindowStyle = ProcessWindowStyle.Hidden
                    });
                    Registry.CurrentUser.CreateSubKey(@"Software\Upgrade Your Network")?.SetValue("Optimize DnsCache", "1");
                }
                else
                {
                    key?.SetValue("CacheHashTableBucketSize", "1", RegistryValueKind.DWord);
                    key?.DeleteValue("NegativeCacheTime");
                    key?.SetValue("CacheHashTableSize", "384", RegistryValueKind.DWord);
                    key?.SetValue("MaxCacheEntryTtlLimit", "86400", RegistryValueKind.DWord);
                    key?.SetValue("MaxSOACacheEntryTtlLimit", "300", RegistryValueKind.DWord);

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = "/c ipconfig /flushdns",
                        WindowStyle = ProcessWindowStyle.Hidden
                    });
                    Registry.CurrentUser.CreateSubKey(@"Software\Upgrade Your Network")?.SetValue("Optimize DnsCache", "0");
                }
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox1.Text, "[^0-9-.]"))
                return;
            textBox1.Text = textBox1.Text.Remove(textBox1.TextLength - 1);
            textBox1.SelectionStart = textBox1.TextLength;
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox2.Text, "[^0-9-.]"))
                return;
            textBox2.Text = textBox2.Text.Remove(textBox2.TextLength - 1);
            textBox2.SelectionStart = textBox2.TextLength;
        }

        private async void TabControl1_SelectedIndexChanged(object sender, EventArgs e) // если переключаемся на вкладку с паролями
        {
            if (tabControl1.SelectedIndex != 4)
                return;
            Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = "/c sc start wlansvc",
                    WindowStyle = ProcessWindowStyle.Hidden
                })
                ?.WaitForExit();
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c netsh wlan show profiles",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            });
            if (process?.StandardOutput != null)
            {
                var wifiInfo = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
                var regex = new Regex(@"(.*?)All user profile(.*?): (.*?)");
                var mc = regex.Matches(wifiInfo);
                foreach (Match stringMatch in mc)
                    comboBox3.Items.Add(stringMatch);
            }
        }

        private async Task TtlGetAsync()
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c chcp65001 & ping {textBox1.Text}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            });
            if (process != null)
            {
                var line = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
                var match = Regex.Match(line, "(.*?) TTL=(.*?)");
                label15.Text = match.Groups[2].Value;
            }
        }

        private async Task GetWiFiInfoAsync(string line) // парсим информацию об нужном вай-фае
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c netsh wlan show profile \"{line}\" key=clear",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            });
            if (process?.StandardOutput != null)
            {
                var wifiInfo = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
                var match = Regex.Match(wifiInfo, @"(.*?)Key Content(.*?): (.*?) Cost");
                label14.Text = match.Groups[3].Value.Trim();
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            var wiFiInfoAsync = GetWiFiInfoAsync(comboBox3.SelectedText);
        }
    }
}