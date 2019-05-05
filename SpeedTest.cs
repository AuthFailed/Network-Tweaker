using Network_Upgrade.Properties;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network_Upgrade
{
    public partial class SpeedTest : Form
    {
        public SpeedTest()
        {
            InitializeComponent();
            button1.MouseEnter += (s, a) => { button1.BackColor = Color.LightGray; };
            button1.MouseLeave += (s, a) => { button1.BackColor = Color.LightGray; };
        }

        private void Fulling()
        {
            foreach (ListViewItem item in listView1.CheckedItems)
                switch (item.Index)
                {
                    case 0:
                        item.SubItems.Add("77.88.8.1");
                        item.SubItems.Add("77.88.8.8");
                        break;
                    case 1:
                        item.SubItems.Add("8.8.8.8");
                        item.SubItems.Add("8.8.4.4");
                        break;
                    case 2:
                        item.SubItems.Add("208.67.222.222");
                        item.SubItems.Add("208.67.220.220");
                        break;
                    case 3:
                        item.SubItems.Add("208.67.222.220");
                        item.SubItems.Add("208.67.222.222");
                        break;
                    case 4:
                        item.SubItems.Add("176.103.130.130");
                        item.SubItems.Add("176.103.130.131");
                        break;
                    case 5:
                        item.SubItems.Add("1.1.1.1");
                        item.SubItems.Add("1.0.0.1");
                        break;
                }
        }

        private static async Task<string> MainPingAsync(string main)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c chcp 65001 & ping {main} -n 2",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            });
            Debug.Assert(process != null, nameof(process) + " != null");
            var line = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
            var match = Regex.Match(line, @"(.*?)Average = (.*?$)");
            var mainPing = match.Groups[2].Value;
            return mainPing;
        }

        private static async Task<string> ExtraPingAsync(string extra)
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c chcp 65001 & ping {extra} -n 2",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            });
            Debug.Assert(process != null, nameof(process) + " != null");
            var line = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
            var match = Regex.Match(line, @"(.*?)Average = (.*?$)");
            var extraPing = match.Groups[2].Value;
            return extraPing;
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            var audio = new SoundPlayer(Resources.s);
            audio.Play();
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                await Task.Delay(20).ConfigureAwait(false);
                switch (item.Index)
                {
                    case 0:
                        item.SubItems.Add(await MainPingAsync("77.88.8.1").ConfigureAwait(false));
                        await Task.Delay(10).ConfigureAwait(false);
                        item.SubItems.Add(await ExtraPingAsync("77.88.8.8").ConfigureAwait(false));
                        item.BackColor = Color.GreenYellow;
                        break;
                    case 1:
                        item.SubItems.Add(await MainPingAsync("8.8.8.8").ConfigureAwait(false));
                        await Task.Delay(10).ConfigureAwait(false);
                        item.SubItems.Add(await ExtraPingAsync("8.8.4.4").ConfigureAwait(false));
                        item.BackColor = Color.GreenYellow;
                        break;
                    case 2:
                        item.SubItems.Add(await MainPingAsync("208.67.222.222").ConfigureAwait(false));
                        await Task.Delay(10).ConfigureAwait(false);
                        item.SubItems.Add(await ExtraPingAsync("208.67.220.220").ConfigureAwait(false));
                        item.BackColor = Color.GreenYellow;
                        break;
                    case 3:
                        item.SubItems.Add(await MainPingAsync("208.67.222.220").ConfigureAwait(false));
                        await Task.Delay(10).ConfigureAwait(false);
                        item.SubItems.Add(await ExtraPingAsync("208.67.222.222").ConfigureAwait(false));
                        item.BackColor = Color.GreenYellow;
                        break;
                    case 4:
                        item.SubItems.Add(await MainPingAsync("176.103.130.130").ConfigureAwait(false));
                        await Task.Delay(10).ConfigureAwait(false);
                        item.SubItems.Add(await ExtraPingAsync("176.103.130.131").ConfigureAwait(false));
                        item.BackColor = Color.GreenYellow;
                        break;
                    case 5:
                        item.SubItems.Add(await MainPingAsync("1.1.1.1").ConfigureAwait(false));
                        await Task.Delay(10).ConfigureAwait(false);
                        item.SubItems.Add(await ExtraPingAsync("1.0.0.1").ConfigureAwait(false));
                        item.BackColor = Color.GreenYellow;
                        break;
                }
            }
            ColumnClickEventArgs eArgs = new ColumnClickEventArgs(4);
            ListView1_ColumnClick(listView1, eArgs);
            listView1.FocusedItem = listView1.Items[0];
            listView1.SelectedIndexChanged += (s, a) => { button2.Enabled = true; };
        }

        private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewColumnComparer();
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
                listView1.Select();
            }
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

        private void Button2_Click(object sender, EventArgs e)
        {
            switch (listView1.SelectedItems[0].Text)
            {
                case @"RU - Yandex":
                    ChangeMainDns("77.88.8.1");
                    ChangeExtraDns("77.88.8.8");
                    Notification(@"DNS был успешно изменен");
                    break;
                case @"RU - AdGuard":
                    ChangeMainDns("176.103.130.130");
                    ChangeExtraDns("176.103.130.131");
                    Notification(@"DNS был успешно изменен");
                    break;
                case @"AU - Cloudflare":
                    ChangeMainDns("1.1.1.1");
                    ChangeExtraDns("1.0.0.1");
                    Notification(@"DNS был успешно изменен");
                    break;
                case "US - OpenDNS":
                    ChangeMainDns("208.67.222.222");
                    ChangeExtraDns("208.67.220.220");
                    Notification(@"DNS был успешно изменен");
                    break;
                case "US - OpenDNS - 2":
                    ChangeMainDns("208.67.222.220");
                    ChangeExtraDns("208.67.220.222");
                    Notification(@"DNS был успешно изменен");
                    break;
                case "US - Google Public DNS":
                    ChangeMainDns("8.8.8.8");
                    ChangeExtraDns("8.8.4.4");
                    Notification(@"DNS был успешно изменен");
                    break;
                default:
                    Notification(@"Выберите сервер");
                    break;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Fulling();
        }


        private class ListViewColumnComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                try
                {
                    return string.CompareOrdinal(
                        ((ListViewItem)x)?.SubItems[4].Text.Replace("ms", ""),
                        ((ListViewItem)y)?.SubItems[4].Text.Replace("ms", ""));
                }
                catch
                {
                    return 0;
                }
            }
        }

        private void ListView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }
    }
}