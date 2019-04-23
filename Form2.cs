using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvDetection
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        async void ПлавноВкл()
        {
            for (; Opacity < .93; Opacity += .02)
            {
                await Task.Delay(2).ConfigureAwait(false);
            }
        }

        async void ПлавноВыкл()
        {
            for (; Opacity > 0; Opacity -= .02)
            {
                await Task.Delay(2).ConfigureAwait(false);
            }
            Close();
        }

        async void Form2_Load(object sender, EventArgs e)
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            Location = new System.Drawing.Point(width - Size.Width-3, height - Size.Height-34);
            ПлавноВкл();
            await Task.Delay(2000).ConfigureAwait(false);
            ПлавноВыкл();
        }

        void PictureBox1_Click_1(object sender, EventArgs e) => Close();
    }
}
