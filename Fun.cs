using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Network_Tweaker.Properties;

namespace Network_Tweaker
{
    public partial class MainWindows
    {
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

            LoadClrs();
        }

        private void LoadClrs()
        {
            autoupdatebox.ForeColor = autoupdatebox.Checked ? Color.FromArgb(9, 153, 246) : SystemColors.ControlDarkDark;
            protocolbox.ForeColor = protocolbox.Checked ? Color.FromArgb(9, 153, 246) : SystemColors.ControlDarkDark;
            checkBox2.ForeColor = checkBox2.Checked ? Color.FromArgb(9, 153, 246) : SystemColors.ControlDarkDark;
            checkBox3.ForeColor = checkBox3.Checked ? Color.FromArgb(9, 153, 246) : SystemColors.ControlDarkDark;
            proxybox.ForeColor = proxybox.Checked ? Color.FromArgb(9, 153, 246) : SystemColors.ControlDarkDark;
        }

        private void FuncLoad()
        {
            button1.MouseEnter += (s, e) => { button1.LghtGr(); };
            button2.MouseEnter += (s, e) => { button2.LghtGr(); };
            button3.MouseEnter += (s, e) => { button3.LghtGr(); };
            button4.MouseEnter += (s, e) => { button4.LghtGr(); };
            button6.MouseEnter += (s, e) => { button6.LghtGr(); };
            button1.MouseLeave += (s, e) => { button1.Gnsbr(); };
            button2.MouseLeave += (s, e) => { button2.Gnsbr(); };
            button3.MouseLeave += (s, e) => { button3.Gnsbr(); };
            button4.MouseLeave += (s, e) => { button4.Gnsbr(); };
            button6.MouseLeave += (s, e) => { button6.Gnsbr(); };
            label5.MouseEnter += (s, e) => { label5.Text = @"Я не пункт :)"; };
            label5.MouseLeave += (s, e) => { label5.Text = @"Наведи мышкой на любой пункт :)"; };
            label31.MouseEnter += (s, e) => { label31.BldTxt(); };
            label31.MouseLeave += (s, e) => { label31.MnTxt(); };
            label31.Click += async (s, e) =>
            {
                var httpClient = new HttpClient();
                var ip = await httpClient.GetStringAsync("https://api.ipify.org/?format=json");
                var regexIp = Regex.Match(ip, "\"ip\":\"(.*?)\"");
                textBox3.Text = regexIp.Groups[1].Value;
            };
            pictureBox1.MouseEnter += (s, e) => { pictureBox1.Image = Resources.AnimatedRefresh; };
            pictureBox1.MouseLeave += (s, e) => { pictureBox1.Image = Resources.RefreshLite; };
            pictureBox2.MouseEnter += (s, e) => { pictureBox2.Image = Resources.folder; };
            pictureBox2.MouseLeave += (s, e) => { pictureBox2.Image = Resources.folder_Lite; };
            pictureBox4.MouseEnter += (s, e) => { pictureBox4.Image = Resources.registry_gray; };
            pictureBox4.MouseLeave += (s, e) => { pictureBox4.Image = Resources.registry_lite; };
            pictureBox6.MouseEnter += (s, e) => { pictureBox6.Image = Resources.network; };
            pictureBox6.MouseLeave += (s, e) => { pictureBox6.Image = Resources.network_Lite; };
            pictureBox6.MouseClick += (s, e) => { Process.Start("ncpa.cpl"); };
            pictureBox7.MouseEnter += (s, e) => { pictureBox7.Image = Resources.graph; };
            pictureBox7.MouseLeave += (s, e) => { pictureBox7.Image = Resources.graph_Lite; };
            pictureBox7.MouseClick += (s, e) =>
            {
                var form = new SpeedTest();
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
            button3.MouseLeave += (sender, e) => { label5.Text = @"Наведи мышкой на любой пункт :)"; };
            button4.MouseEnter += (sender, e) => { label5.Text = @"Восстанавливает прошлую версию фильтра, если таковая существует"; };
            button4.MouseLeave += (sender, e) => { label5.DfLblTxt(); };
            autoupdatebox.MouseLeave += (s, e) => { label5.DfLblTxt(); };
            proxybox.MouseLeave += (s, e) => { label5.DfLblTxt(); };
            autoupdatebox.MouseLeave += (s, e) => { label5.DfLblTxt(); };
            checkBox2.MouseLeave += (s, e) => { label5.DfLblTxt(); };
            protocolbox.MouseLeave += (s, e) => { label5.DfLblTxt(); };
            checkBox3.MouseLeave += (s, e) => { label5.DfLblTxt(); };
            button3.MouseLeave += (sender, e) => { label5.DfLblTxt(); };
            button4.MouseLeave += (sender, e) => { label5.DfLblTxt(); };
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
            var cf = new Notifications();
            {
                cf.Show();
                cf.label2.Text = line;
            }
            Activate();
        }

        private void TxtRdNl()
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
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

        private static async void AUpdTrue()
        {
            var path = Environment.GetEnvironmentVariable("SYSTEMROOT");
            var uri = new Uri("https://github.com/AuthFailed/Update-Your-Hosts/raw/master/hosts.exe");
            try
            {
                var wc = new WebClient();
                await wc.DownloadFileTaskAsync(uri, path + @"\hosts.exe").ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Что-то случилось и не удалось скачать файл");
                Clipboard.Clear();
                Clipboard.SetText(ex.ToString());
            }

            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            key?.SetValue("Hosts Update", $@"{path}\hosts.exe");
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

        private static void ChangeMainDns(string line)
        {
            CmdExe($"netsh interface ipv4 set dnsservers \"Ethernet\" static address={line} primary");
        }

        private static void ChangeExtraDns(string line)
        {
            CmdExe($"netsh interface ipv4 add dnsservers \"Ethernet\" address={line}");
        }

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
            var match = Regex.Match(line, @"(.*?)Average = (.*?$)");
            label10.Text = match.Groups[2].Value;
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
            var match = Regex.Match(line, @"(.*?)Average = (.*?$)");
            label11.Text = match.Groups[2].Value;
        }

        private void FillComboAsync()
        {
            if (tabControl1.SelectedIndex != 3)
                return;
            ChckWlansvc();
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c chcp 65001 & netsh wlan show profiles",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8
            });
            if (process?.StandardOutput == null)
                return;
            var wifiInfo = process.StandardOutput.ReadToEnd();
            if (wifiInfo.Contains("There is no wireless interface"))
            {
                MessageBox.Show(@"В системе нет модуля Wi-Fi
Данная вкладка вам недоступна", @"Внимание!");
            }
            else
            {
                comboBox3.Enabled = true;
                var regex = new Regex(@"(.*?)All User Profile : (.*?$)", RegexOptions.Multiline);
                var mc = regex.Matches(wifiInfo);
                if (mc.Count < 1)
                    MessageBox.Show(@"Не удалось заполнить массив или нет сохраненных сетей");
                else
                    foreach (Match match in mc)
                        comboBox3.Items.Add(match.Groups[3].Value);
            }
        }

        private void ChckWlansvc()
        {
            var sc = new ServiceController(@"wlansvc");
            if (sc.Status == ServiceControllerStatus.Running)
                return;
            CmdExe(@"sc start wlansvc");
            Notification(@"В фонов режиме была запущена служба DNS");
        }

        private async Task TtlGetAsync()
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c chcp 65001 & ping {textBox1.Text} -n 1",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            });
            if (process == null)
                return;
            var line = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
            var match = Regex.Match(line, "(.*?) TTL=(.*?)\n");
            label15.Text = match.Groups[2].Value;
        }

        private async Task GetWiFiInfoAsync(string line)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c chcp 65001 & netsh wlan show profile \"{line}\" key=clear",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            });
            if (process?.StandardOutput != null)
            {
                var wifiInfo = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
                var match = Regex.Match(wifiInfo, @"(.*?)Type : (.*?$)");
                label7.Text = match.Groups[2].Value.Contains("Wireless") ? @"Прямое/Кабель" : @"Беспроводное";
                match = Regex.Match(wifiInfo, @"(.*?)Authentication : (.*?$)");
                label17.Text = match.Groups[2].Value;
                match = Regex.Match(wifiInfo, @"(.*?)Key Content(.*?): (.*?$)");
                label19.Text = match.Groups[3].Value;
            }
        }

        private async Task GetIpInfoAsync()
        {
            var hc = new HttpClient();
            var line = await hc.GetStringAsync($"http://api.sypexgeo.net/json/{textBox3.Text}");
            var link = Regex.Match(line, "\"lat\":(.*?),\"lon\":(.*?),", RegexOptions.Multiline);
            var latitude = link.Groups[1].ToString();
            var longitude = link.Groups[2].ToString();

            void Action()
            {
                var match = Regex.Match(line,
                    "\"city\":(.*?)\"name_ru\":\"(.*?)\",(.*?)\"region\"(.*?)\"name_ru\":\"(.*?)\",(.*?)\"country\"(.*?)\"name_ru\":\"(.*?)\",",
                    RegexOptions.Multiline);
                label26.Text = match.Groups[8].ToString();
                label27.Text = match.Groups[5].ToString();
                label28.Text = match.Groups[2].ToString();
            }

            if (!line.Contains("invalid"))
            {
                if (InvokeRequired)
                    Invoke((Action) Action);
                else
                    Action();
            }
            else
            {
                MessageBox.Show(@"Загугли, как выглядит IP");
            }

            linkLabel1.Enabled = true;
            label30.Visible = false;
            linkLabel1.Click += (s, e) => { Process.Start($"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}"); };
        }
    }
}