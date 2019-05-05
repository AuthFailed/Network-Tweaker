using Microsoft.Win32;
using Network_Upgrade.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network_Upgrade
{
    public partial class MainWindows
    {
        void OptionsLoad()
        {
            using (RegistryKey key =
                Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true))
            {
                if (key != null)
                    proxybox.Checked = key.GetValue("AutoConfigURL")?.ToString() ==
                                       @"https://antizapret.prostovpn.org/proxy.pac";
            }

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Upgrade Your Network", false))
            {
                autoupdatebox.Checked = key?.GetValue("AutoUpdate")?.ToString() == "1";
                protocolbox.Checked = key?.GetValue("Protocols")?.ToString() == "1";
                checkBox2.Checked = key?.GetValue("Shell")?.ToString() == "1";
                checkBox3.Checked = key?.GetValue("Optimize DnsCache")?.ToString() == "1";
            }
        }

        void FuncLoad()
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
            pictureBox1.MouseEnter += (s, e) => { pictureBox1.Image = Resources.AnimatedRefresh; };
            pictureBox1.MouseLeave += (s, e) => { pictureBox1.Image = Resources.RefreshLite; };
            pictureBox2.MouseEnter += (s, e) => { pictureBox2.Image = Resources.Folder_Gray; };
            pictureBox2.MouseLeave += (s, e) => { pictureBox2.Image = Resources.Folder_Lite; };
            pictureBox4.MouseEnter += (s, e) => { pictureBox4.Image = Resources.registry_gray; };
            pictureBox4.MouseLeave += (s, e) => { pictureBox4.Image = Resources.registry_lite; };
            pictureBox6.MouseEnter += (s, e) => { pictureBox6.Image = Resources.Connections_Gray; };
            pictureBox6.MouseLeave += (s, e) => { pictureBox6.Image = Resources.Connections_Lite; };
            pictureBox6.MouseClick += (s, e) => { Process.Start("ncpa.cpl"); };
            pictureBox7.MouseEnter += (s, e) => { pictureBox7.Image = Resources.TestPing_Gray; };
            pictureBox7.MouseLeave += (s, e) => { pictureBox7.Image = Resources.TestPing_Lite; };
            pictureBox7.MouseClick += (s, e) =>
            {
                SpeedTest form = new SpeedTest();
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
            autoupdatebox.MouseLeave += (s, e) => { label5.Text = @"Наведи мышкой на любой пункт :)"; };
            proxybox.MouseLeave += (s, e) => { label5.Text = @"Наведи мышкой на любой пункт :)"; };
            autoupdatebox.MouseLeave += (s, e) => { label5.Text = @"Наведи мышкой на любой пункт :)"; };
            checkBox2.MouseLeave += (s, e) => { label5.Text = @"Наведи мышкой на любой пункт :)"; };
            checkBox3.MouseLeave += (s, e) => { label5.Text = @"Наведи мышкой на любой пункт :)"; };
            button3.MouseLeave += (sender, e) => { label5.Text = @"Наведи мышкой на любой пункт :)"; };
            button4.MouseLeave += (sender, e) => { label5.Text = @"Наведи мышкой на любой пункт :)"; };
        }

        static void CmdExe(string line)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c {line}",
                WindowStyle = ProcessWindowStyle.Hidden
            })
                ?.WaitForExit();
        }

        void Notification(string line)
        {
            Notifications cf = new Notifications();
            {
                cf.Show();
                cf.label2.Text = line;
            }
            Activate();
        }

        void TxtRdNl()
        {
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
        }

        void UpdHosts(string line)
        {
            if (File.Exists(Backup))
                File.Delete(Backup);
            if (File.Exists(Path))
                File.Copy(Path, Backup);
            Uri filer = new Uri(line);
            try
            {
                using (WebClient wc = new WebClient())
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

        void Web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            label4.Text = @"Последнее обновление: " + File.GetLastWriteTime(Path).ToString("dd/MM/yyyy");
        }

        void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl ctlTab = (TabControl)sender;

            Graphics g = e.Graphics;

            string sText = ctlTab.TabPages[e.Index].Text;
            SizeF sizeText = g.MeasureString(sText, ctlTab.Font);
            int iX = e.Bounds.Left + 6;
            float iY = e.Bounds.Top + (e.Bounds.Height - sizeText.Height) / 2;
            g.DrawString(sText, ctlTab.Font, Brushes.Black, iX, iY);
            e.Graphics.SetClip(e.Bounds);
            string text = tabControl1.TabPages[e.Index].Text;
            SizeF sz = e.Graphics.MeasureString(text, e.Font);

            bool bSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            using (SolidBrush b = new SolidBrush(bSelected ? SystemColors.Highlight : SystemColors.Control))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }

            using (SolidBrush b = new SolidBrush(bSelected ? SystemColors.HighlightText : SystemColors.ControlText))
            {
                e.Graphics.DrawString(text, e.Font, b, e.Bounds.X + 2, e.Bounds.Y + (e.Bounds.Height - sz.Height) / 2);
            }

            if (tabControl1.SelectedIndex == e.Index)
                e.DrawFocusRectangle();

            e.Graphics.ResetClip();
        }

        void AUpd()
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

        static void AUpdTrue()
        {
            string path = Environment.GetEnvironmentVariable("SYSTEMROOT");
            Uri uri = new Uri("https://github.com/AuthFailed/Update-Your-Hosts/raw/master/hosts.exe");
            try
            {
                using (WebClient wc = new WebClient())
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

            using (RegistryKey key =
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                key?.SetValue("Hosts Update", $@"{path}\hosts.exe");
            }
        }

        static void AUpdFalse()
        {
            string path = Environment.GetEnvironmentVariable("SYSTEMROOT");
            if (File.Exists(path + @"\hosts.exe"))
                File.Delete(path + @"\hosts.exe");
            using (RegistryKey key =
                Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (key?.GetValue("Hosts Update") != null)
                    key.DeleteValue("Hosts Update");
            }
        }

        static void ChangeMainDns(string line)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c netsh interface ipv4 set dnsservers \"Ethernet\" static address={line} primary",
                WindowStyle = ProcessWindowStyle.Hidden
            })
                ?.WaitForExit();
        }

        static void ChangeExtraDns(string line)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c netsh interface ipv4 add dnsservers \"Ethernet\" address={line}",
                WindowStyle = ProcessWindowStyle.Hidden
            })
                ?.WaitForExit();
        }

        async Task PingMainAsync()
        {
            Process process = Process.Start(new ProcessStartInfo
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
            string line = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
            Match match = Regex.Match(line, @"(.*?)Average = (.*?$)");
            label10.Text = match.Groups[2].Value;
        }

        async Task PingExtraAsync()
        {
            Process process = Process.Start(new ProcessStartInfo
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
            string line = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
            Match match = Regex.Match(line, @"(.*?)Average = (.*?$)");
            label11.Text = match.Groups[2].Value;
        }

        void FillComboAsync()
        {
            if (tabControl1.SelectedIndex == 3)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = "/c sc start wlansvc",
                    WindowStyle = ProcessWindowStyle.Hidden
                });
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = "/c chcp 65001 & netsh wlan show profiles",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.UTF8
                });
                if (process?.StandardOutput != null)
                {
                    var wifiInfo = process.StandardOutput.ReadToEnd();
                    if (wifiInfo.Contains("There is no wireless interface"))
                    {
                        MessageBox.Show(@"В системе нет модуля Wi-Fi
Данная вкладка вам недоступна");
                    }
                    else
                    {
                        comboBox3.Enabled = true;
                        Regex regex = new Regex(@"(.*?)All User Profile : (.*?$)", RegexOptions.Multiline);
                        MatchCollection mc = regex.Matches(wifiInfo);
                        if (mc.Count < 1)
                            MessageBox.Show(@"Не удалось заполнить массив или нет сохраненных сетей");
                        else
                            foreach (Match match in mc)
                                comboBox3.Items.Add(match.Groups[3].Value);
                    }
                }
            }
        }

        async Task TtlGetAsync()
        {
            Process process = Process.Start(new ProcessStartInfo
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

        async Task GetWiFiInfoAsync(string line)
        {
            Process process = Process.Start(new ProcessStartInfo
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
    }
}
