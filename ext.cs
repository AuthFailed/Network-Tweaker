using System.Drawing;
using System.Windows.Forms;

namespace Network_Tweaker
{
    public static class Ext
    {
        public static void BldTxt(this Label label)
        {
            label.Font = new Font(@"Gilroy", 12F, FontStyle.Bold);
        }

        public static void MnTxt(this Label label)
        {
            label.Font = new Font(@"Gilroy", 12F, FontStyle.Regular);
        }

        public static void Gnsbr(this Button button)
        {
            button.BackColor = Color.Gainsboro;
        }

        public static void LghtGr(this Button button)
        {
            button.BackColor = Color.LightGray;
        }

        public static void DfLblTxt(this Label label)
        {
            label.Text = @"Наведи мышкой на любой пункт :)";
        }
    }
}