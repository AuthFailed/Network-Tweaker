using System.Drawing;
using System.Windows.Forms;

namespace Network_Upgrade
{
    public static class Ext
    {
        public static void BoldText(this Label label)
        {
            label.Font = new Font(@"Gilroy", 12F, FontStyle.Bold);
        }
        public static void MainText(this Label label)
        {
            label.Font = new Font(@"Gilroy", 12F, FontStyle.Regular);
        }
    }
}
