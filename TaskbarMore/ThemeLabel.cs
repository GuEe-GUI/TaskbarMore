using System.Drawing;
using System.Windows.Forms;

namespace TaskbarMore
{
    /// <summary>
    /// 仅保存文字与布局；实际绘制由 TaskbarMore 窗体一次性合成，避免透明控件分层重绘残影。
    /// </summary>
    internal class ThemeLabel : Control
    {
        public Color ThemeForeColor { get; set; } = Color.White;

        public ThemeLabel()
        {
            TabStop = false;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
        }

        internal static Color GetContrastColor(Color background)
        {
            if (background.A == 0)
            {
                background = Color.FromArgb(243, 243, 243);
            }

            int lum = (int)(0.299 * background.R + 0.587 * background.G + 0.114 * background.B);
            return lum >= 128 ? Color.Black : Color.White;
        }
    }
}
