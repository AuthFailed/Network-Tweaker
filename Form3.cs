using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using Upgrade_Your_Network.Properties;

namespace Upgrade_Your_Network
{
    public partial class Form3 : Form
    {
        public Form3()
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

#pragma warning disable 1998
        private static async Task<string> MainPingAsync(string main)
#pragma warning restore 1998
        {
            var ping = new Ping();
            var m1 = ping.Send($"{main}")?.RoundtripTime;
            var m2 = ping.Send($"{main}")?.RoundtripTime;
            var m3 = ping.Send($"{main}")?.RoundtripTime;
            var average = (m1 + m2 + m3) / 3;
            var value = average + " мс";
            return value;
        }

        private string ExtraPingAsync(string extra)
        {
            var ping = new Ping();
            var m1 = ping.Send($"{extra}")?.RoundtripTime;
            var m2 = ping.Send($"{extra}")?.RoundtripTime;
            var m3 = ping.Send($"{extra}")?.RoundtripTime;
            var average = (m1 + m2 + m3) / 3;
            var value = average + " мс";
            return value;
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
                        item.SubItems.Add(ExtraPingAsync("77.88.8.8"));
                        item.BackColor = Color.GreenYellow;
                        break;
                    case 1:
                        item.SubItems.Add(await MainPingAsync("8.8.8.8").ConfigureAwait(false));
                        await Task.Delay(10).ConfigureAwait(false);
                        item.SubItems.Add(ExtraPingAsync("8.8.4.4"));
                        item.BackColor = Color.GreenYellow;
                        break;
                    case 2:
                        item.SubItems.Add(await MainPingAsync("208.67.222.222").ConfigureAwait(false));
                        await Task.Delay(10).ConfigureAwait(false);
                        item.SubItems.Add(ExtraPingAsync("208.67.220.220"));
                        item.BackColor = Color.GreenYellow;
                        break;
                    case 3:
                        item.SubItems.Add(await MainPingAsync("208.67.222.220").ConfigureAwait(false));
                        await Task.Delay(10).ConfigureAwait(false);
                        item.SubItems.Add(ExtraPingAsync("208.67.222.222"));
                        item.BackColor = Color.GreenYellow;
                        break;
                    case 4:
                        item.SubItems.Add(await MainPingAsync("176.103.130.130").ConfigureAwait(false));
                        await Task.Delay(10).ConfigureAwait(false);
                        item.SubItems.Add(ExtraPingAsync("176.103.130.131"));
                        item.BackColor = Color.GreenYellow;
                        break;
                    case 5:
                        item.SubItems.Add(await MainPingAsync("1.1.1.1").ConfigureAwait(false));
                        await Task.Delay(10).ConfigureAwait(false);
                        item.SubItems.Add(ExtraPingAsync("1.0.0.1"));
                        item.BackColor = Color.GreenYellow;
                        break;
                }
            }
            listView1.SelectedIndexChanged += (s, a) => { button2.Enabled = true; };
        }

        private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewColumnComparer(e.Column);
            if (listView1.Items.Count > 0)
            {
                listView1.Items[0].Selected = true;
                listView1.Select();
            }
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
            public ListViewColumnComparer(int columnIndex)
            {
                ColumnIndex = columnIndex;
            }

            private int ColumnIndex { get; }

            public int Compare(object x, object y)
            {
                try
                {
                    return string.CompareOrdinal(
                        ((ListViewItem) x)?.SubItems[ColumnIndex].Text.Replace(" мс", ""),
                        ((ListViewItem) y)?.SubItems[ColumnIndex].Text.Replace(" мс", ""));
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