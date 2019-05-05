using Microsoft.Win32;
using Network_Upgrade.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network_Upgrade
{
    /// <inheritdoc />
    public partial class MainWindows : Form
    {
        public MainWindows()
        {
            InitializeComponent();
        }

        static string Backup { get; } = @"C:\\Windows\System32\drivers\etc\hosts_bak";

        string Path { get; } = @"C:\Windows\System32\drivers\etc\hosts";

        async void Form1_Load(object sender, EventArgs e)
        {
            for (; Opacity < .93; Opacity += .04)
                await Task.Delay(30).ConfigureAwait(true);
            FillComboAsync();
            comboBox1.SelectedIndexChanged += (s, a) => { button1.Enabled = true; };
            comboBox2.SelectedIndexChanged += (s, a) =>
            {
                pictureBox1.Enabled = true;
                label12.Enabled = true;
                label13.Enabled = true;
                label16.Enabled = true;
            };
            button6.Enabled = false;
            FuncLoad();
            tabControl1.SelectedIndexChanged += (s, a) => { Width = tabControl1.SelectedTab != tabPage6 ? 597 : 680; }; // 672; 237
            OptionsLoad();
            label4.Text +=
                File.GetLastWriteTime(Path).ToString("dd/MM/yyyy");
            for (; Opacity < .93; Opacity += .04)
                await Task.Delay(30).ConfigureAwait(false);
            ServiceController sc = new ServiceController(@"Dnscache");
            if (sc.Status == ServiceControllerStatus.Running)
                return;
            CmdExe(@"sc start Dnscache");
            Notification(@"В фонов режиме была запущена служба DNS");
        }

        // ReSharper disable once MethodTooLong
        void Button1_Click(object sender, EventArgs e)
        {
            SoundPlayer audio = new SoundPlayer(Resources.s);
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

        void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
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

        void Button2_Click(object sender, EventArgs e)
        {
            SoundPlayer audio = new SoundPlayer(Resources.s);
            audio.Play();
            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                MessageBox.Show(@"Введите домен!", @"Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string[] array = File.ReadAllLines(Path);
                if (!richTextBox1.Text.Contains("."))
                {
                    MessageBox.Show(@"Домен должен быть вида сайт.зона", @"Внимание!");
                }
                else
                {
                    int t = 0;
                    if (array.Any(ar => ar.Equals(richTextBox1.Text)))
                    {
                        MessageBox.Show(@"Данный домен уже есть в фильтре", @"Внимание!");
                        t++;
                    }

                    if (t >= 1) return;
                    using (StreamWriter sw = new StreamWriter(Path, true))
                    {
                        sw.WriteLine("0.0.0.0 " + richTextBox1.Text);
                        Notification(@"Домен " + richTextBox1.Text + @" добавлен в черный список");
                    }

                    CmdExe(@"ipconfig / flushdns");
                }
            }
        }

        void Button3_Click(object sender, EventArgs e)
        {
            SoundPlayer audio = new SoundPlayer(Resources.s);
            audio.Play();
            Uri uri = new Uri("https://raw.githubusercontent.com/AuthFailed/Update-Your-Hosts/master/hosts");
            using (WebClient wc = new WebClient())
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

        void Button4_Click(object sender, EventArgs e)
        {
            SoundPlayer audio = new SoundPlayer(Resources.s);
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

        void CheckBox1_CheckedChanged(object sender, EventArgs e)
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

        void Button6_Click(object sender, EventArgs e)
        {
            SoundPlayer audio = new SoundPlayer(Resources.s);
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

        void Autoupdatebox_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Upgrade Your Network", true))
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

        void Protocolbox_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Upgrade Your Network", true))
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

        void Proxybox_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Upgrade Your Network", true))
            {
                if (proxybox.Checked)
                {
                    using (RegistryKey settings =
                        Registry.CurrentUser.CreateSubKey(
                            @"Software\Microsoft\Windows\CurrentVersion\Internet Settings"))
                    {
                        settings?.SetValue("AutoConfigURL", "https://antizapret.prostovpn.org/proxy.pac");
                    }

                    key?.SetValue("Proxy", "1");
                }
                else
                {
                    using (RegistryKey settings =
                        Registry.CurrentUser.CreateSubKey(
                            @"Software\Microsoft\Windows\CurrentVersion\Internet Settings"))
                    {
                        settings?.DeleteValue("AutoConfigURL");
                    }

                    key?.SetValue("Proxy", "0");
                }
            }
        }

        void PictureBox1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "") & (textBox2.Text != ""))
            {
                switch (comboBox2.SelectedIndex)
                {
                    case 0:
                        Task.Run(TtlGetAsync);
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        break;
                    case 1:

                        Task.Run(TtlGetAsync);
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        break;
                    case 2:
                        Task.Run(TtlGetAsync);
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        break;
                    case 3:
                        Task.Run(TtlGetAsync);
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        break;
                    case 4:
                        Task.Run(TtlGetAsync);
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        break;
                    case 5:
                        Task.Run(TtlGetAsync);
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        break;
                    case 6:
                        Task.Run(TtlGetAsync);
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
                        break;
                    case 7:
                        Task.Run(TtlGetAsync);
                        Task.Run(PingMainAsync);
                        Task.Run(PingExtraAsync);
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

        void PictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = @"/n, /select, C:\Windows\System32\drivers\etc\hosts"
            });
        }

        void PictureBox4_Click(object sender, EventArgs e)
        {
            foreach (Process process in Process.GetProcessesByName("regedit")) process.Kill();
            string path = @"HKEY_CURRENT_USER\Software\Upgrade Your Network";
            Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Applets\Regedit")
                ?.SetValue("LastKey", path);
            Process.Start("regedit");
        }

        void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c netsh advfirewall set allprofiles state on",
                WindowStyle = ProcessWindowStyle.Hidden
            });
            if (checkBox2.Checked)
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\exefile\shell\Firewall_Allow"))
                {
                    key?.SetValue("", @"Разрешить доступ в интернет");
                    key?.SetValue("Extended", "");
                    key?.SetValue("Icon", @"netcenter.dll,10");
                }

                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\exefile\shell\Firewall_Allow\command"))
                {
                    key?.SetValue("", @"netsh advfirewall firewall delete rule name=" + "\"%1\"");
                }

                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\exefile\shell\Firewall_Block"))
                {
                    key?.SetValue("", @"Запретить доступ в интернет");
                    key?.SetValue("Extended", "");
                    key?.SetValue("Icon", @"netcenter.dll,5");
                }

                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Classes\exefile\shell\Firewall_Block\command"))
                {
                    key?.SetValue("",
                        @"cmd /d /c ""netsh advfirewall firewall add rule name=""%1"" dir=in action=block program=""%1"" & netsh advfirewall firewall add rule name=""%1"" dir=out action=block program=""%1""");
                }

                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Upgrade Your Network"))
                {
                    key?.SetValue("Shell", "1");
                }
            }
            else
            {
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Classes\exefile\shell\Firewall_Allow", false);
                Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Classes\exefile\shell\Firewall_Block", false);
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Upgrade Your Network"))
                {
                    key?.SetValue("Shell", "0");
                }
            }
        }

        void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"))
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

        void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox1.Text, "[^0-9-.]"))
                return;
            textBox1.Text = textBox1.Text.Remove(textBox1.TextLength - 1);
            textBox1.SelectionStart = textBox1.TextLength;
        }

        void TextBox2_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox2.Text, "[^0-9-.]"))
                return;
            textBox2.Text = textBox2.Text.Remove(textBox2.TextLength - 1);
            textBox2.SelectionStart = textBox2.TextLength;
        }

        void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != 3)
                return;
            FillComboAsync();
        }

        void Label5_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 1)
            {
                DashPattern = new float[] { 2, 2 }
            };
            e.Graphics.DrawRectangle(pen, 0, 0, label5.Width, label5.Height);
        }

        async void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3 != null)
            {
                await GetWiFiInfoAsync(comboBox3.SelectedText).ConfigureAwait(false);
                label14.Enabled = true;
                label18.Enabled = true;
                label20.Enabled = true;
                label19.Enabled = true;
                label17.Enabled = true;
                label7.Enabled = true;
                button5.Enabled = true;
                button8.Enabled = true;
            }
        }

        void Button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Вы действительно хотите удалить эту сеть?", @"Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = $"/c chcp 65001 & netsh wlan delete profile {comboBox3.SelectedText}",
                    WindowStyle = ProcessWindowStyle.Hidden
                });
                Notification(@"Сеть успешно удалена");
            }
            else
            {
                MessageBox.Show(@"Операция отменена");
            }
        }

        void Button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Вы действительно хотите удалить все соединения?\n Это удалит все сохраненные пароли Wi-Fi", @"Внимание!",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = "/c netsh wlan delete profile name=* i=*",
                    WindowStyle = ProcessWindowStyle.Hidden
                });
                Notification(@"Все сети успешно удалены");
            }
            else
            {
                MessageBox.Show(@"Операция отменена");
            }
        }

        void TextBox3_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBox3.Text, "[^0-9-.]"))
            {
                textBox3.Text = textBox3.Text.Remove(textBox3.Text.Length - 1);
                textBox3.SelectionStart = textBox3.Text.Length;
                button9.Enabled = false;
            }
            else
            {
                button9.Enabled = true;
            }
        }

        void Button9_Click(object sender, EventArgs e)
        {
            label30.Visible = true;
            if (Regex.IsMatch(textBox3.Text, @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$"))
                GetIpInfoAsync();
            else
                MessageBox.Show(@"Загугли, как выглядит IP");
        }

        private void GetIpInfoAsync()
        {
            var wc = new WebClient();
            var line = wc.DownloadString($"http://free.ipwhois.io/xml/{textBox3.Text}?lang=en");
            var link = Regex.Match(line, "<latitude>(.*?)</latitude>(.*?)<longitude>(.*?)</longitude>");
            var latitude = link.Groups[1].ToString();
            var longitude = link.Groups[3].ToString();
            linkLabel1.Click += (s, e) => { Process.Start($"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}"); };

            void Action()
            {
                var match = Regex.Match(line,
                    "<country>(.*?)</country>(.*?)<region>(.*?)</region>(.*?)<city>(.*?)</city>(.*?)<org>(.*?)</org>(.*?)<currency>(.*?)</currency>");
                label23.Text = match.Groups[7].ToString();
                label26.Text = match.Groups[1].ToString();
                label27.Text = match.Groups[5].ToString();
                label28.Text = match.Groups[3].ToString();
                label29.Text = match.Groups[9].ToString();
            }

            if (!line.Contains("invalid"))
            {
                if (InvokeRequired)
                    Invoke((Action)Action);
                else
                    Action();
            }
            else
            {
                MessageBox.Show(@"Загугли, как выглядит IP");
            }
            linkLabel1.Enabled = true;
            label30.Visible = false;
        }
    }
}