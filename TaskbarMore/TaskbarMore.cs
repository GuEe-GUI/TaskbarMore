using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CSDeskBand;
using CSDeskBand.ContextMenu;
using CSDeskBand.Win;
using Microsoft.Win32;

namespace TaskbarMore
{
    [ComVisible(true)]
    [Guid("A96DCE38-8F92-4B9D-B780-024E43CFB865")]
    [ClassInterface(ClassInterfaceType.None)]
    [CSDeskBandRegistration(Name = "TaskbarMore", ShowDeskBand = true)]
    public sealed class TaskbarMore : CSDeskBandWin
    {
        private readonly SystemMonitor monitor;
        private readonly Timer timer;
        private readonly Stopwatch sampleClock = Stopwatch.StartNew();
        private readonly ContextMenuStrip menu;
        private Font displayFont;
        private double lastSample = -1;
        private bool capsLock, lightTheme, closed, configuring;
        private int configuredDpi;
        private const int HorizontalWidth = 164;
        private bool Vertical => TaskbarInfo != null && TaskbarInfo.Orientation == TaskbarOrientation.Vertical;

        public TaskbarMore()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            AutoScaleMode = AutoScaleMode.None;
            TabStop = false;
            Text = "TaskbarMore 2.0";
            AccessibleName = "TaskbarMore";
            monitor = new SystemMonitor();
            menu = new ContextMenuStrip();
            menu.Items.Add("About TaskbarMore", null, ShowAbout);
            menu.Items.Add("Close Toolbar", null, (sender, args) => CloseDeskBand());
            ContextMenuStrip = menu;
            var about = new DeskBandMenuAction("About TaskbarMore");
            about.Clicked += ShowAbout;
            var close = new DeskBandMenuAction("Close TaskbarMore");
            close.Clicked += (sender, args) => CloseDeskBand();
            Options.ContextMenuItems.Add(about);
            Options.ContextMenuItems.Add(close);
            Options.ShowTitle = false;
            Options.HeightCanChange = true;
            ConfigureSize();
            timer = new Timer { Interval = 200 };
            timer.Tick += RefreshStatus;
            if (!IsDesignMode) timer.Start();
        }

        private bool IsDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime;

        private void ConfigureSize()
        {
            if (configuring || IsDisposed) return;
            configuring = true;
            try
            {
                int dpi = IsHandleCreated ? (int)GetDpiForWindow(Handle) : 96;
                if (dpi < 72) dpi = 96;
                if (dpi == configuredDpi) return;
                configuredDpi = dpi;
                float scale = dpi / 96f;
                Options.MinHorizontalSize = new CSDeskBand.Size((int)(HorizontalWidth * scale), (int)(30 * scale));
                Options.HorizontalSize = new CSDeskBand.Size((int)(HorizontalWidth * scale), (int)(40 * scale));
                Options.MinVerticalSize = new CSDeskBand.Size((int)(62 * scale), (int)(110 * scale));
                Options.VerticalSize = new CSDeskBand.Size((int)(90 * scale), (int)(110 * scale));
                var oldFont = displayFont;
                displayFont = new Font("Segoe UI", 10f * scale, FontStyle.Regular, GraphicsUnit.Pixel);
                oldFont?.Dispose();
                Invalidate();
            }
            finally { configuring = false; }
        }

        private void RefreshStatus(object sender, EventArgs args)
        {
            if (closed || IsDisposed) return;
            try
            {
                capsLock = (GetKeyState((int)Keys.CapsLock) & 1) != 0;
                if (sampleClock.Elapsed.TotalSeconds - lastSample >= 1)
                {
                    monitor.Sample();
                    lastSample = sampleClock.Elapsed.TotalSeconds;
                    Text = "TaskbarMore 2.0 | CPU " + monitor.CpuText + " | RAM " + monitor.MemoryText +
                        " | Up " + Speed(monitor.UploadBytesPerSecond) + " | Down " + Speed(monitor.DownloadBytesPerSecond);
                    AccessibleDescription = Text;
                    using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                        lightTheme = key?.GetValue("SystemUsesLightTheme") is int value && value != 0;
                }
                ConfigureSize();
                Invalidate();
            }
            // A failed sample must never escape a timer callback hosted by Explorer.
            catch (Exception e) when (e is Win32Exception || e is InvalidOperationException ||
                e is UnauthorizedAccessException || e is System.Security.SecurityException || e is System.IO.IOException)
            {
                Debug.WriteLine(e);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // WinForms transparency alone cannot paint an unmanaged Explorer parent.
            // Ask the native host to paint its themed background into our buffer.
            if (Parent == null && IsHandleCreated && GetParent(Handle) != IntPtr.Zero)
            {
                var rect = new NativeRect { Right = ClientSize.Width, Bottom = ClientSize.Height };
                IntPtr dc = e.Graphics.GetHdc();
                int result;
                try { result = DrawThemeParentBackground(Handle, dc, ref rect); }
                finally { e.Graphics.ReleaseHdc(dc); }
                if (result == 0) return;
            }
            base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (displayFont == null || monitor == null) return;
            Color color = lightTheme ? Color.Black : Color.White;
            if (Vertical)
            {
                int row = Math.Max(1, Height / 5);
                DrawText(e.Graphics, "↑ " + Speed(monitor.UploadBytesPerSecond), new Rectangle(2, 0, Width - 4, row), color);
                DrawText(e.Graphics, "↓ " + Speed(monitor.DownloadBytesPerSecond), new Rectangle(2, row, Width - 4, row), color);
                DrawText(e.Graphics, "CPU " + monitor.CpuText, new Rectangle(2, row * 2, Width - 4, row), color);
                DrawText(e.Graphics, "RAM " + monitor.MemoryText, new Rectangle(2, row * 3, Width - 4, row), color);
                DrawText(e.Graphics, capsLock ? "CAPS" : "abc", new Rectangle(2, row * 4, Width - 4, row), color);
                return;
            }
            float scale = configuredDpi / 96f;
            // Keep the reference layout's columns fixed even when Explorer grants extra width.
            int S(int value) => (int)Math.Round(value * scale);
            int contentHeight = Math.Min(Height, S(40));
            int top = Math.Max(0, (Height - contentHeight) / 2);
            int half = contentHeight / 2;
            DrawImage(e.Graphics, Properties.Resources.Upload, new Rectangle(S(2), top, S(14), half));
            DrawImage(e.Graphics, Properties.Resources.Download, new Rectangle(S(2), top + half, S(14), contentHeight - half));
            DrawText(e.Graphics, Speed(monitor.UploadBytesPerSecond), new Rectangle(S(18), top, S(61), half), color);
            DrawText(e.Graphics, Speed(monitor.DownloadBytesPerSecond), new Rectangle(S(18), top + half, S(61), contentHeight - half), color);
            DrawText(e.Graphics, "CPU " + monitor.CpuText, new Rectangle(S(82), top, S(58), half), color);
            DrawText(e.Graphics, "RAM " + monitor.MemoryText, new Rectangle(S(82), top + half, S(58), contentHeight - half), color);
            Image keyImage = capsLock
                ? (lightTheme ? Properties.Resources.Capital_light : Properties.Resources.Capital_dark)
                : (lightTheme ? Properties.Resources.Lower_case_light : Properties.Resources.Lower_case_dark);
            DrawImage(e.Graphics, keyImage, new Rectangle(S(142), top, S(20), contentHeight));
        }

        private void DrawText(Graphics graphics, string text, Rectangle bounds, Color color)
        {
            if (bounds.Width <= 0 || bounds.Height <= 0) return;
            // Keep GDI's original font metrics, then explicitly restore glyph alpha
            // before compositing. GDI+ DrawString uses different glyph sizes/spacing.
            using (var glyphs = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppPArgb))
            {
                using (var canvas = Graphics.FromImage(glyphs))
                {
                    canvas.Clear(Color.Black);
                    TextRenderer.DrawText(canvas, text, displayFont,
                        new Rectangle(Point.Empty, bounds.Size), Color.White, Color.Black,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix |
                        TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
                }
                var data = glyphs.LockBits(new Rectangle(Point.Empty, bounds.Size),
                    ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
                try
                {
                    var pixels = new byte[data.Stride * data.Height];
                    Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);
                    for (int y = 0; y < data.Height; y++)
                    {
                        for (int x = 0; x < data.Width; x++)
                        {
                            int offset = y * data.Stride + x * 4;
                            // Convert ClearType coverage to grayscale without expanding
                            // every colored edge to its strongest channel (which looks bold).
                            byte alpha = (byte)((pixels[offset + 2] * 77 + pixels[offset + 1] * 150 +
                                pixels[offset] * 29 + 128) >> 8);
                            pixels[offset] = (byte)(color.B * alpha / 255);
                            pixels[offset + 1] = (byte)(color.G * alpha / 255);
                            pixels[offset + 2] = (byte)(color.R * alpha / 255);
                            pixels[offset + 3] = alpha;
                        }
                    }
                    Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
                }
                finally { glyphs.UnlockBits(data); }
                graphics.DrawImageUnscaled(glyphs, bounds.Location);
            }
        }

        private void DrawImage(Graphics graphics, Image image, Rectangle bounds)
        {
            float scale = configuredDpi / 96f;
            int width = Math.Min(bounds.Width, (int)(image.Width * scale));
            int height = Math.Min(bounds.Height, (int)(image.Height * scale));
            if (width > 0 && height > 0)
                graphics.DrawImage(image, bounds.Left + (bounds.Width - width) / 2,
                    bounds.Top + (bounds.Height - height) / 2, width, height);
        }

        private static string Speed(double bytes)
        {
            double kb = bytes / 1024;
            return kb < 1024 ? kb.ToString(kb < 100 ? "0.00" : "0.0") + " K/s" : (kb / 1024).ToString("0.0") + " M/s";
        }

        protected override void VisibilityChanged(bool visible)
        {
            base.VisibilityChanged(visible);
            if (timer == null || IsDesignMode || IsDisposed) return;
            closed = !visible;
            if (visible) { lastSample = -1; timer.Start(); }
            else { timer.Stop(); monitor.Dispose(); }
        }

        protected override void OnClose()
        {
            closed = true;
            timer?.Stop();
            monitor?.Dispose();
            base.OnClose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                closed = true;
                timer?.Dispose();
                monitor?.Dispose();
                menu?.Dispose();
                displayFont?.Dispose();
                displayFont = null;
            }
            base.Dispose(disposing);
        }

        private void ShowAbout(object sender, EventArgs args)
        {
            MessageBox.Show(this, "TaskbarMore\nNetwork speed, CPU usage, memory usage and CapsLock status\nPowered by GuEe-GUI",
                "About TaskbarMore", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        [DllImport("user32.dll")] private static extern short GetKeyState(int key);
        [DllImport("user32.dll")] private static extern uint GetDpiForWindow(IntPtr window);
        [DllImport("user32.dll")] private static extern IntPtr GetParent(IntPtr window);
        [DllImport("uxtheme.dll")] private static extern int DrawThemeParentBackground(IntPtr window, IntPtr dc, ref NativeRect rect);
        [StructLayout(LayoutKind.Sequential)]
        private struct NativeRect { internal int Left, Top, Right, Bottom; }
    }
}
