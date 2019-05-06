using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network_Tweaker
{
    public partial class Notifications : Form
    {
        public Notifications()
        {
            InitializeComponent();
        }

        private async Task SmoothOnAsync()
        {
            for (; Opacity < .85; Opacity += .04) await Task.Delay(2).ConfigureAwait(false);
        }

        private async Task SmoothOffAsync()
        {
            for (; Opacity > 0; Opacity -= .04) await Task.Delay(2).ConfigureAwait(false);
            Close();
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            CloseLoad();
            var width = Screen.PrimaryScreen.Bounds.Width;
            var height = Screen.PrimaryScreen.Bounds.Height;
            Location = new Point(width - Size.Width - 3, height - Size.Height - 34);
            await SmoothOnAsync().ConfigureAwait(false);
            await Task.Delay(5000).ConfigureAwait(false);
            await SmoothOffAsync().ConfigureAwait(false);
        }

        private void CloseLoad()
        {
            pictureBox1.Click += (s, a) => { Close(); };
            pictureBox2.Click += (s, a) => { Close(); };
            label1.Click += (s, a) => { Close(); };
            label2.Click += (s, a) => { Close(); };
            Click += (s, a) => { Close(); };
        }
    }
}