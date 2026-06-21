using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Management;
using NetWorkSpeedMonitor;
using KeyboardHookNS;
using Microsoft.Win32;
using System.IO;

namespace TaskbarMore
{
    public partial class TaskbarMore : Form
    {
        private const int WM_DISPLAYCHANGE = 0x007E;
        private const int WM_SETTINGCHANGE = 0x001A;
        private const int WM_DPICHANGED = 0x02E0;
        private const int GWL_STYLE = -16;
        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;
        private const int WS_POPUP = unchecked((int)0x80000000);
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private const uint BI_RGB = 0;
        private const uint DIB_RGB_COLORS = 0;
        private const uint CLR_INVALID = 0xFFFFFFFF;
        private const double TaskbarLightOnThreshold = 145.0;
        private const double TaskbarDarkOnThreshold = 115.0;
        private const string ThemePersonalizeKey = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x00080000;
        private const int ULW_ALPHA = 0x00000002;
        private const byte AC_SRC_OVER = 0x00;
        private const byte AC_SRC_ALPHA = 0x01;
        private const int WidgetWidth = 200;
        private const int WidgetHeight = 49;
        private const int BaseDpi = 96;
        private const int MaxTaskbarSpan = 8192;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_MOUSELEAVE = 0x02A3;
        private const uint TME_LEAVE = 0x00000002;
        private const int HoverOverlayAlphaLight = 165;
        private const int HoverOverlayAlphaDark = 145;
        private const int DisplayChangeDebounceMs = 400;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "SetParent")]
        static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndNewParent);
        [DllImport("user32.dll", EntryPoint = "GetParent")]
        static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        static extern bool IsWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        static extern bool InvalidateRect(IntPtr hWnd, ref RECT lpRect, bool bErase);
        [DllImport("user32.dll")]
        static extern bool UpdateWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);
        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState);
        [DllImport("shell32.dll")]
        public static extern IntPtr SHAppBarMessage(uint dwMessage, ref AppBarData pData);
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
        [DllImport("user32.dll")]
        static extern uint GetDpiForWindow(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern uint GetDpiForSystem();
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("user32.dll")]
        static extern bool UpdateLayeredWindow(
            IntPtr hwnd,
            IntPtr hdcDst,
            ref POINT pptDst,
            ref SIZE psize,
            IntPtr hdcSrc,
            ref POINT pptSrc,
            int crKey,
            ref BLENDFUNCTION pblend,
            int dwFlags);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        static extern bool DeleteDC(IntPtr hdc);
        [DllImport("gdi32.dll", SetLastError = true)]
        static extern IntPtr CreateDIBSection(
            IntPtr hdc,
            [In] ref BITMAPINFO pbmi,
            uint usage,
            out IntPtr ppvBits,
            IntPtr hSection,
            uint dwOffset);
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr dest, IntPtr src, int length);
        [DllImport("user32.dll")]
        static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

        [StructLayout(LayoutKind.Sequential)]
        private struct TRACKMOUSEEVENT
        {
            public uint cbSize;
            public uint dwFlags;
            public IntPtr hwndTrack;
            public uint dwHoverTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public uint bmiColors;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SIZE
        {
            public int cx;
            public int cy;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        public enum AppBarMessages
        {
            New = 0x00000000,
            Remove = 0x00000001,
            QueryPos = 0x00000002,
            SetPos = 0x00000003,
            GetState = 0x00000004,
            GetTaskBarPos = 0x00000005,
            Activate = 0x00000006,
            GetAutoHideBar = 0x00000007,
            SetAutoHideBar = 0x00000008,
            WindowPosChanged = 0x00000009,
            SetState = 0x0000000a
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AppBarData
        {
            public int cbSize;
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public RECT rc;
            public int lParam;
        }

        public enum AppBarEdge
        {
            ABE_LEFT = 0,
            ABE_TOP = 1,
            ABE_RIGHT = 2,
            ABE_BOTTOM = 3
        }

        public enum AppBarAxis
        {
            AXIS_H = 0,
            AXIS_V = 1,
        }

        private readonly KeyboardHook KeyHook;
        private readonly NetworkAdapter[] Adapters;
        private readonly NetworkMonitor Monitor;
        private readonly PerformanceCounter CPU_Counter;
        private readonly PerformanceCounter RAM_Counter;
        private readonly Timer TaskbarMoreTimer;
        private readonly Timer EmbedRetryTimer;
        private readonly Timer DisplayChangeDebounceTimer;
        private readonly long RAM_ALL;

        private IntPtr Shell_TrayWnd;
        private IntPtr ReBarWindow32;
        private IntPtr MSTaskSwWClass;
        private IntPtr MSTaskListWClass;

        private AppBarAxis _lastAxis;
        private int _embedRetryCount;
        private int _lastSwWidth;
        private int _lastSwHeight;
        private uint _lastAppliedDpi;
        private float _lastLayoutScale;
        private bool _displayChangePending;
        private bool _inCompositePaint;
        private bool _embeddedBackgroundCaptureScheduled;
        private bool _embeddedBackgroundCaptureDeferredByHover;
        private Font[] _labelFonts;

        private const float DesignFontSize = 8.25f;
        private const string DesignFontName = "Microsoft YaHei UI";
        // Designer: Key 在 165，宽 21 → 右缘 186，再加 4px 右内边距
        private const int DesignContentRight = 186;
        private const int DesignRightPadding = 4;

        private bool _isLightTaskbar;
        private bool _isHovering;
        private bool _applyingTheme;
        private bool _embeddedVisualThemeDisabled;
        private bool _layeredSurfaceEnabled;
        private bool _layeredSurfaceFailed;
        private bool _trackingMouseLeave;
        private Bitmap _embeddedBackgroundCache;
        private int _cachedBackgroundWidth;
        private int _cachedBackgroundHeight;
        private Bitmap _iconContentCache;
        private int _iconCacheWidth;
        private int _iconCacheHeight;
        private Color _textColor;
        private Color _idleBackgroundColor;
        private Color _normalBackColor;
        private Color _hoverBackColor;

        internal bool IsLightTaskbar => _isLightTaskbar;

        internal Color CurrentThemeBackColor => _isHovering ? _hoverBackColor : _normalBackColor;

        private ThemeLabel[] GetTextLabels()
        {
            return new ThemeLabel[]
            {
                Upload_Text,
                Download_Text,
                CPU_Title,
                RAM_Title,
                CPU_Text,
                RAM_Text
            };
        }

        /// <summary>采样任务栏背景色，结果写入 _isLightTaskbar。</summary>
        private void DetectTaskbarTheme()
        {
            _isLightTaskbar = IsTaskbarLightColor();

            Color? sampled = TrySampleTaskbarBackgroundColor();
            if (sampled.HasValue)
            {
                _idleBackgroundColor = sampled.Value;
                _normalBackColor = sampled.Value;
                _isLightTaskbar = ThemeLabel.GetContrastColor(sampled.Value) == Color.Black;
            }
            else
            {
                _idleBackgroundColor = _isLightTaskbar
                    ? Color.FromArgb(243, 243, 243)
                    : Color.Black;
                _normalBackColor = _idleBackgroundColor;
            }

            _hoverBackColor = _isLightTaskbar
                ? Color.FromArgb(255, 255, 255)
                : ControlPaint.Light(_idleBackgroundColor, 0.20f);
            _textColor = ThemeLabel.GetContrastColor(_idleBackgroundColor);
        }

        private void ApplyTextColors()
        {
            foreach (ThemeLabel label in GetTextLabels())
            {
                label.ThemeForeColor = _textColor;
            }
        }

        private void ApplyTheme(bool invalidateBackgroundCache = false)
        {
            if (_applyingTheme || IsDisposed)
            {
                return;
            }

            _applyingTheme = true;
            try
            {
                bool wasLight = _isLightTaskbar;
                DetectTaskbarTheme();
                bool lightDarkChanged = wasLight != _isLightTaskbar;

                ApplyTextColors();

                AllowTransparency = false;
                TransparencyKey = Color.Empty;

                if (IsEmbeddedInTaskbar() && (invalidateBackgroundCache || lightDarkChanged))
                {
                    EnsureEmbeddedVisualThemeDisabled();
                    EnsureLayeredSurface();
                    InvalidateBackgroundCache();
                }
                else if (IsEmbeddedInTaskbar())
                {
                    EnsureEmbeddedVisualThemeDisabled();
                    EnsureLayeredSurface();
                }

                BackColor = IsEmbeddedInTaskbar() ? Color.Black : _idleBackgroundColor;

                foreach (ThemeLabel label in GetTextLabels())
                {
                    label.BackColor = Color.Transparent;
                }

                Upload_Img.BackColor = _idleBackgroundColor;
                Download_Img.BackColor = _idleBackgroundColor;
                Key.BackColor = _idleBackgroundColor;

                ApplyCapsLockImage();
                ApplyCompositeDisplay();
                Invalidate();
            }
            finally
            {
                _applyingTheme = false;
            }
        }

        private void EnsureEmbeddedVisualThemeDisabled()
        {
            if (_embeddedVisualThemeDisabled || !IsHandleCreated)
            {
                return;
            }

            SetWindowTheme(Handle, string.Empty, string.Empty);
            _embeddedVisualThemeDisabled = true;
        }

        private void ApplyCompositeDisplay()
        {
            Upload_Img.Visible = false;
            Download_Img.Visible = false;
            Key.Visible = false;

            foreach (ThemeLabel label in GetTextLabels())
            {
                label.Visible = false;
            }
        }

        private void EnsureLayeredSurface()
        {
            if (_layeredSurfaceFailed || _layeredSurfaceEnabled || !IsEmbeddedInTaskbar() || !IsHandleCreated)
            {
                return;
            }

            int exStyle = GetWindowLong(Handle, GWL_EXSTYLE);
            SetWindowLong(Handle, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);
            _layeredSurfaceEnabled = true;
        }

        private static TextFormatFlags LabelTextFlags =>
            TextFormatFlags.Left
                | TextFormatFlags.VerticalCenter
                | TextFormatFlags.NoPrefix
                | TextFormatFlags.EndEllipsis
                | TextFormatFlags.SingleLine
                | TextFormatFlags.NoPadding;

        private void PaintCompositeContent(PaintEventArgs e)
        {
            int width = ClientSize.Width;
            int height = ClientSize.Height;
            if (width <= 0 || height <= 0)
            {
                return;
            }

            bool embedded = IsEmbeddedInTaskbar();
            if (embedded)
            {
                EnsureLayeredSurface();
            }

            Color fore = ThemeLabel.GetContrastColor(_idleBackgroundColor);
            bool tryLayered = embedded && _layeredSurfaceEnabled && !_layeredSurfaceFailed;

            using (Bitmap buffer = new Bitmap(width, height, PixelFormat.Format32bppPArgb))
            {
                buffer.SetResolution(e.Graphics.DpiX, e.Graphics.DpiY);
                using (Graphics g = Graphics.FromImage(buffer))
                {
                    if (tryLayered)
                    {
                        g.Clear(Color.Transparent);
                        DrawHoverOverlay(g, width, height);
                        DrawForegroundLayer(g, buffer, fore, width, height);
                        PremultiplyAlphaInPlace(buffer);
                        if (TryPresentLayeredSurface(buffer))
                        {
                            return;
                        }

                        g.Clear(Color.Transparent);
                    }

                    if (embedded)
                    {
                        DrawEmbeddedBaseBackground(g, width, height);
                    }
                    else
                    {
                        g.Clear(_idleBackgroundColor);
                    }

                    DrawHoverOverlay(g, width, height);
                    DrawForegroundLayer(g, buffer, fore, width, height);
                }

                e.Graphics.DrawImageUnscaled(buffer, 0, 0);
            }
        }

        private void DrawForegroundLayer(Graphics g, Bitmap buffer, Color fore, int width, int height)
        {
            Bitmap labelBackground = null;
            try
            {
                labelBackground = buffer.Clone() as Bitmap;
                DrawCachedIcons(g, width, height);
                DrawLabelsOnComposite(g, fore, labelBackground ?? buffer);
            }
            finally
            {
                labelBackground?.Dispose();
            }
        }

        private void DrawCachedIcons(Graphics g, int width, int height)
        {
            EnsureIconContentCache(width, height);
            if (_iconContentCache != null)
            {
                g.DrawImage(_iconContentCache, 0, 0);
                return;
            }

            DrawIcons(g);
        }

        private void EnsureIconContentCache(int width, int height)
        {
            if (_iconContentCache != null
                && _iconCacheWidth == width
                && _iconCacheHeight == height)
            {
                return;
            }

            InvalidateIconContentCache();
            _iconContentCache = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            using (Graphics cacheGraphics = Graphics.FromImage(_iconContentCache))
            {
                cacheGraphics.Clear(Color.Transparent);
                DrawIcons(cacheGraphics);
            }

            _iconCacheWidth = width;
            _iconCacheHeight = height;
        }

        private void InvalidateIconContentCache()
        {
            if (_iconContentCache != null)
            {
                _iconContentCache.Dispose();
                _iconContentCache = null;
            }

            _iconCacheWidth = 0;
            _iconCacheHeight = 0;
        }

        private void DrawIcons(Graphics g)
        {
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            DrawIcon(g, Upload_Img);
            DrawIcon(g, Download_Img);
            DrawKeyIcon(g, Key);
        }

        private void DrawLabelsOnComposite(Graphics g, Color fore, Bitmap backgroundSource)
        {
            foreach (ThemeLabel label in GetTextLabels())
            {
                Color textBack = SampleCompositeBackground(backgroundSource, label);
                DrawLabelText(g, label, fore, textBack);
            }
        }

        private static Color SampleCompositeBackground(Bitmap composite, ThemeLabel label)
        {
            if (composite == null)
            {
                return Color.Transparent;
            }

            Rectangle bounds = new Rectangle(label.Left, label.Top, label.Width, label.Height);
            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                return Color.Transparent;
            }

            int x = bounds.X + bounds.Width / 2;
            int y = bounds.Y + bounds.Height / 2;
            x = Math.Max(0, Math.Min(composite.Width - 1, x));
            y = Math.Max(0, Math.Min(composite.Height - 1, y));
            Color sampled = composite.GetPixel(x, y);
            if (sampled.A == 0)
            {
                return Color.Transparent;
            }

            return Color.FromArgb(255, sampled.R, sampled.G, sampled.B);
        }

        private void DrawEmbeddedBaseBackground(Graphics g, int width, int height)
        {
            EnsureEmbeddedBackgroundCache();
            if (_embeddedBackgroundCache != null)
            {
                g.DrawImage(_embeddedBackgroundCache, 0, 0, width, height);
            }
            else
            {
                g.Clear(ResolveEmbeddedBackgroundFillColor());
            }
        }


        private void InvalidateEmbeddedBackgroundCacheOnly()
        {
            if (_embeddedBackgroundCache != null)
            {
                _embeddedBackgroundCache.Dispose();
                _embeddedBackgroundCache = null;
            }

            _cachedBackgroundWidth = 0;
            _cachedBackgroundHeight = 0;
        }

        private void InvalidateBackgroundCache()
        {
            InvalidateEmbeddedBackgroundCacheOnly();
            InvalidateIconContentCache();
        }

        private void EnsureEmbeddedBackgroundCache()
        {
            if (!IsEmbeddedInTaskbar() || !IsHandleCreated || _displayChangePending)
            {
                return;
            }

            if (_layeredSurfaceEnabled && !_layeredSurfaceFailed)
            {
                return;
            }

            int width = ClientSize.Width;
            int height = ClientSize.Height;
            if (width <= 0 || height <= 0)
            {
                return;
            }

            if (_embeddedBackgroundCache != null
                && _cachedBackgroundWidth == width
                && _cachedBackgroundHeight == height)
            {
                return;
            }

            if (_isHovering)
            {
                _embeddedBackgroundCaptureDeferredByHover = true;
                return;
            }

            if (_inCompositePaint)
            {
                ScheduleEmbeddedBackgroundCapture();
                return;
            }

            CaptureEmbeddedBackgroundCache(width, height);
        }

        private void ScheduleEmbeddedBackgroundCapture()
        {
            if (_embeddedBackgroundCaptureScheduled || IsDisposed || !IsHandleCreated)
            {
                return;
            }

            if (_isHovering)
            {
                _embeddedBackgroundCaptureDeferredByHover = true;
                return;
            }

            _embeddedBackgroundCaptureScheduled = true;
            BeginInvoke(new Action(() =>
            {
                _embeddedBackgroundCaptureScheduled = false;
                if (IsDisposed || !IsHandleCreated || !IsEmbeddedInTaskbar() || _displayChangePending)
                {
                    return;
                }

                int width = ClientSize.Width;
                int height = ClientSize.Height;
                if (width <= 0 || height <= 0)
                {
                    return;
                }

                if (_embeddedBackgroundCache != null
                    && _cachedBackgroundWidth == width
                    && _cachedBackgroundHeight == height)
                {
                    return;
                }

                CaptureEmbeddedBackgroundCache(width, height);
                Invalidate();
            }));
        }

        private void CaptureEmbeddedBackgroundCache(int width, int height)
        {
            if (_isHovering)
            {
                _embeddedBackgroundCaptureDeferredByHover = true;
                return;
            }

            RECT windowRect = new RECT();
            if (!GetWindowRect(Handle, ref windowRect))
            {
                return;
            }

            int captureWidth = windowRect.Right - windowRect.Left;
            int captureHeight = windowRect.Bottom - windowRect.Top;
            if (captureWidth <= 0 || captureHeight <= 0
                || captureWidth != width || captureHeight != height)
            {
                return;
            }

            Rectangle virtualScreen = SystemInformation.VirtualScreen;
            if (windowRect.Left >= virtualScreen.Right
                || windowRect.Top >= virtualScreen.Bottom
                || windowRect.Right <= virtualScreen.Left
                || windowRect.Bottom <= virtualScreen.Top)
            {
                FillEmbeddedBackgroundFallback(width, height);
                return;
            }

            ShowWindow(Handle, SW_HIDE);
            try
            {
                RefreshExposedTaskbarArea(windowRect, width, height);

                InvalidateEmbeddedBackgroundCacheOnly();
                _embeddedBackgroundCache = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
                if (!TryCopyScreenRegion(
                        _embeddedBackgroundCache,
                        windowRect.Left,
                        windowRect.Top,
                        width,
                        height))
                {
                    FillEmbeddedBackgroundFallbackBitmap(_embeddedBackgroundCache, width, height);
                }

                _cachedBackgroundWidth = width;
                _cachedBackgroundHeight = height;
            }
            catch
            {
                FillEmbeddedBackgroundFallback(width, height);
            }
            finally
            {
                ShowWindow(Handle, SW_SHOW);
            }
        }

        private static bool TryCopyScreenRegion(Bitmap bitmap, int sourceX, int sourceY, int width, int height)
        {
            try
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(
                        sourceX,
                        sourceY,
                        0,
                        0,
                        new Size(width, height),
                        CopyPixelOperation.SourceCopy);
                }

                return true;
            }
            catch (Win32Exception)
            {
                return false;
            }
        }

        private void FillEmbeddedBackgroundFallback(int width, int height)
        {
            InvalidateEmbeddedBackgroundCacheOnly();
            _embeddedBackgroundCache = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            FillEmbeddedBackgroundFallbackBitmap(_embeddedBackgroundCache, width, height);
            _cachedBackgroundWidth = width;
            _cachedBackgroundHeight = height;
        }

        private void FillEmbeddedBackgroundFallbackBitmap(Bitmap bitmap, int width, int height)
        {
            Color fill = ResolveEmbeddedBackgroundFillColor();
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(fill);
            }
        }

        private Color ResolveEmbeddedBackgroundFillColor()
        {
            Color? sampled = TrySampleTaskbarBackgroundColor();
            return sampled ?? _idleBackgroundColor;
        }

        private void RefreshExposedTaskbarArea(RECT windowRect, int width, int height)
        {
            if (MSTaskSwWClass != IntPtr.Zero && IsWindow(MSTaskSwWClass))
            {
                POINT topLeft = new POINT { x = windowRect.Left, y = windowRect.Top };
                if (ScreenToClient(MSTaskSwWClass, ref topLeft))
                {
                    RECT clientRect = new RECT
                    {
                        Left = topLeft.x,
                        Top = topLeft.y,
                        Right = topLeft.x + width,
                        Bottom = topLeft.y + height
                    };
                    InvalidateRect(MSTaskSwWClass, ref clientRect, true);
                    UpdateWindow(MSTaskSwWClass);
                }
            }
        }

        private static void PremultiplyAlphaInPlace(Bitmap bitmap)
        {
            Rectangle bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData data = bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
            try
            {
                int byteCount = Math.Abs(data.Stride) * bitmap.Height;
                byte[] pixels = new byte[byteCount];
                Marshal.Copy(data.Scan0, pixels, 0, byteCount);
                for (int i = 0; i < byteCount; i += 4)
                {
                    byte alpha = pixels[i + 3];
                    if (alpha == 0)
                    {
                        pixels[i] = 0;
                        pixels[i + 1] = 0;
                        pixels[i + 2] = 0;
                    }
                    else if (alpha < 255)
                    {
                        pixels[i] = (byte)(pixels[i] * alpha / 255);
                        pixels[i + 1] = (byte)(pixels[i + 1] * alpha / 255);
                        pixels[i + 2] = (byte)(pixels[i + 2] * alpha / 255);
                    }
                }

                Marshal.Copy(pixels, 0, data.Scan0, byteCount);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
        }

        private static IntPtr CreateArgbDibSection(Bitmap bitmap, IntPtr hdc, out IntPtr bits)
        {
            bits = IntPtr.Zero;
            BITMAPINFO info = new BITMAPINFO
            {
                bmiHeader = new BITMAPINFOHEADER
                {
                    biSize = Marshal.SizeOf(typeof(BITMAPINFOHEADER)),
                    biWidth = bitmap.Width,
                    biHeight = -bitmap.Height,
                    biPlanes = 1,
                    biBitCount = 32,
                    biCompression = (int)BI_RGB
                },
                bmiColors = 0
            };

            IntPtr hBitmap = CreateDIBSection(hdc, ref info, DIB_RGB_COLORS, out bits, IntPtr.Zero, 0);
            if (hBitmap == IntPtr.Zero || bits == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            Rectangle bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData data = bitmap.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            try
            {
                int byteCount = Math.Abs(data.Stride) * bitmap.Height;
                CopyMemory(bits, data.Scan0, byteCount);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }

            return hBitmap;
        }

        private void DrawLabelText(Graphics g, ThemeLabel label, Color fore, Color textBackForFallback)
        {
            if (label.Font == null || string.IsNullOrEmpty(label.Text))
            {
                return;
            }

            Rectangle bounds = new Rectangle(label.Left, label.Top, label.Width, label.Height);
            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                return;
            }

            Color textBack = textBackForFallback.A == 0
                ? ResolveEmbeddedBackgroundFillColor()
                : textBackForFallback;

            if (_isLightTaskbar)
            {
                TextRenderer.DrawText(
                    g,
                    label.Text,
                    label.Font,
                    bounds,
                    fore,
                    textBack,
                    LabelTextFlags);
                return;
            }

            TextRenderingHint oldHint = g.TextRenderingHint;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            try
            {
                using (StringFormat format = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip))
                {
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = StringAlignment.Near;
                    format.Trimming = StringTrimming.EllipsisCharacter;

                    using (SolidBrush brush = new SolidBrush(fore))
                    {
                        g.DrawString(label.Text, label.Font, brush, bounds, format);
                    }

                    using (SolidBrush shade = new SolidBrush(Color.FromArgb(72, fore)))
                    {
                        Rectangle shifted = bounds;
                        shifted.Offset(1, 0);
                        g.DrawString(label.Text, label.Font, shade, shifted, format);
                    }
                }
            }
            catch (ArgumentException)
            {
                TextRenderer.DrawText(
                    g,
                    label.Text,
                    label.Font,
                    bounds,
                    fore,
                    textBack,
                    LabelTextFlags);
            }
            finally
            {
                g.TextRenderingHint = oldHint;
            }
        }

        private int GetHoverOverlayAlpha()
        {
            return _isLightTaskbar ? HoverOverlayAlphaLight : HoverOverlayAlphaDark;
        }

        private void DrawHoverOverlay(Graphics g, int width, int height)
        {
            if (!_isHovering)
            {
                return;
            }

            using (SolidBrush hoverBrush = new SolidBrush(Color.FromArgb(GetHoverOverlayAlpha(), _hoverBackColor)))
            {
                g.FillRectangle(hoverBrush, 0, 0, width, height);
            }
        }

        private bool TryPresentLayeredSurface(Bitmap bitmap)
        {
            IntPtr screenDc = IntPtr.Zero;
            IntPtr memDc = IntPtr.Zero;
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr dibBits = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                screenDc = GetDC(IntPtr.Zero);
                if (screenDc == IntPtr.Zero)
                {
                    return false;
                }

                memDc = CreateCompatibleDC(screenDc);
                if (memDc == IntPtr.Zero)
                {
                    return false;
                }

                hBitmap = CreateArgbDibSection(bitmap, screenDc, out dibBits);
                if (hBitmap == IntPtr.Zero)
                {
                    return false;
                }

                oldBitmap = SelectObject(memDc, hBitmap);

                RECT windowRect = new RECT();
                if (!GetWindowRect(Handle, ref windowRect))
                {
                    return false;
                }

                POINT dst = new POINT { x = windowRect.Left, y = windowRect.Top };
                SIZE size = new SIZE { cx = bitmap.Width, cy = bitmap.Height };
                POINT src = new POINT { x = 0, y = 0 };
                BLENDFUNCTION blend = new BLENDFUNCTION
                {
                    BlendOp = AC_SRC_OVER,
                    BlendFlags = 0,
                    SourceConstantAlpha = 255,
                    AlphaFormat = AC_SRC_ALPHA
                };

                if (!UpdateLayeredWindow(Handle, IntPtr.Zero, ref dst, ref size, memDc, ref src, 0, ref blend, ULW_ALPHA))
                {
                    _layeredSurfaceFailed = true;
                    _layeredSurfaceEnabled = false;
                    return false;
                }

                return true;
            }
            catch
            {
                _layeredSurfaceFailed = true;
                _layeredSurfaceEnabled = false;
                return false;
            }
            finally
            {
                if (oldBitmap != IntPtr.Zero && memDc != IntPtr.Zero)
                {
                    SelectObject(memDc, oldBitmap);
                }

                if (hBitmap != IntPtr.Zero)
                {
                    DeleteObject(hBitmap);
                }

                if (memDc != IntPtr.Zero)
                {
                    DeleteDC(memDc);
                }

                if (screenDc != IntPtr.Zero)
                {
                    ReleaseDC(IntPtr.Zero, screenDc);
                }
            }
        }

        private static Color? SampleScreenColor(IntPtr hdc, int x, int y)
        {
            uint pixel = GetPixel(hdc, x, y);
            if (pixel == CLR_INVALID)
            {
                return null;
            }

            int b = (int)(pixel & 0xFF);
            int g = (int)((pixel >> 8) & 0xFF);
            int r = (int)((pixel >> 16) & 0xFF);
            return Color.FromArgb(r, g, b);
        }

        private Color? TrySampleTaskbarBackgroundColor()
        {
            if (!ResolveTaskbarHandles() || Shell_TrayWnd == IntPtr.Zero)
            {
                return null;
            }

            IntPtr hdc = GetDC(IntPtr.Zero);
            if (hdc == IntPtr.Zero)
            {
                return null;
            }

            List<Color> samples = new List<Color>();
            try
            {
                IntPtr trayNotify = FindDescendantByClass(Shell_TrayWnd, "TrayNotifyWnd");
                if (trayNotify != IntPtr.Zero)
                {
                    RECT trayRect = new RECT();
                    if (GetWindowRect(trayNotify, ref trayRect))
                    {
                        int trayWidth = trayRect.Right - trayRect.Left;
                        int trayHeight = trayRect.Bottom - trayRect.Top;
                        if (trayWidth > 8 && trayHeight > 8)
                        {
                            int midY = trayRect.Top + trayHeight / 2;
                            AddColorSample(hdc, samples, trayRect.Left + trayWidth / 5, midY);
                            AddColorSample(hdc, samples, trayRect.Left + trayWidth / 2, midY);
                            AddColorSample(hdc, samples, trayRect.Left + trayWidth * 4 / 5, midY);
                        }
                    }
                }

                RECT shellRect = new RECT();
                if (GetWindowRect(Shell_TrayWnd, ref shellRect))
                {
                    AppBarAxis axis = TaskbarAxis(Shell_TrayWnd);
                    int shellWidth = shellRect.Right - shellRect.Left;
                    int shellHeight = shellRect.Bottom - shellRect.Top;

                    if (axis == AppBarAxis.AXIS_H && shellWidth > 8 && shellHeight > 8)
                    {
                        int midY = shellRect.Top + shellHeight / 2;
                        AddColorSample(hdc, samples, shellRect.Left + shellWidth / 16, midY);
                        AddColorSample(hdc, samples, shellRect.Right - shellWidth / 8, midY);
                    }
                }
            }
            finally
            {
                ReleaseDC(IntPtr.Zero, hdc);
            }

            if (samples.Count == 0)
            {
                return null;
            }

            int r = 0;
            int g = 0;
            int b = 0;
            foreach (Color color in samples)
            {
                r += color.R;
                g += color.G;
                b += color.B;
            }

            int count = samples.Count;
            return Color.FromArgb(r / count, g / count, b / count);
        }

        private static void AddColorSample(IntPtr hdc, List<Color> samples, int x, int y)
        {
            Color? color = SampleScreenColor(hdc, x, y);
            if (color.HasValue)
            {
                samples.Add(color.Value);
            }
        }

        private const int KeyIconNativeSize = 16;

        private static void DrawIcon(Graphics g, PictureBox pictureBox)
        {
            if (pictureBox.Image == null || pictureBox.Width <= 0 || pictureBox.Height <= 0)
            {
                return;
            }

            g.DrawImage(
                pictureBox.Image,
                new Rectangle(pictureBox.Left, pictureBox.Top, pictureBox.Width, pictureBox.Height));
        }

        private static void DrawKeyIcon(Graphics g, PictureBox keyBox)
        {
            Image image = keyBox.Image;
            if (image == null || image.Width <= 0 || image.Height <= 0)
            {
                return;
            }

            int x = keyBox.Left + Math.Max(0, (keyBox.Width - image.Width) / 2);
            int y = keyBox.Top + Math.Max(0, (keyBox.Height - image.Height) / 2);
            InterpolationMode oldInterpolation = g.InterpolationMode;
            PixelOffsetMode oldPixelOffset = g.PixelOffsetMode;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.DrawImage(image, x, y, image.Width, image.Height);
            g.InterpolationMode = oldInterpolation;
            g.PixelOffsetMode = oldPixelOffset;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _inCompositePaint = true;
            try
            {
                PaintCompositeContent(e);
            }
            finally
            {
                _inCompositePaint = false;
            }
        }

        private static bool SetTextIfChanged(ThemeLabel label, string text)
        {
            if (label.Text == text)
            {
                return false;
            }

            label.Text = text;
            return true;
        }

        private static int SampleScreenLuminance(IntPtr hdc, int x, int y)
        {
            uint pixel = GetPixel(hdc, x, y);
            if (pixel == CLR_INVALID)
            {
                return -1;
            }

            int b = (int)(pixel & 0xFF);
            int g = (int)((pixel >> 8) & 0xFF);
            int r = (int)((pixel >> 16) & 0xFF);
            return (int)(0.299 * r + 0.587 * g + 0.114 * b);
        }

        private static void AddLuminanceSample(IntPtr hdc, List<int> samples, int x, int y)
        {
            int lum = SampleScreenLuminance(hdc, x, y);
            if (lum >= 0)
            {
                samples.Add(lum);
            }
        }

        private void CollectTaskbarColorSamples(IntPtr hdc, List<int> samples)
        {
            IntPtr trayNotify = FindDescendantByClass(Shell_TrayWnd, "TrayNotifyWnd");
            if (trayNotify != IntPtr.Zero)
            {
                RECT trayRect = new RECT();
                if (GetWindowRect(trayNotify, ref trayRect))
                {
                    int trayWidth = trayRect.Right - trayRect.Left;
                    int trayHeight = trayRect.Bottom - trayRect.Top;
                    if (trayWidth > 8 && trayHeight > 8)
                    {
                        int midY = trayRect.Top + trayHeight / 2;
                        AddLuminanceSample(hdc, samples, trayRect.Left + trayWidth / 5, midY);
                        AddLuminanceSample(hdc, samples, trayRect.Left + trayWidth / 2, midY);
                        AddLuminanceSample(hdc, samples, trayRect.Left + trayWidth * 4 / 5, midY);
                    }
                }
            }

            RECT shellRect = new RECT();
            if (GetWindowRect(Shell_TrayWnd, ref shellRect))
            {
                AppBarAxis axis = TaskbarAxis(Shell_TrayWnd);
                int shellWidth = shellRect.Right - shellRect.Left;
                int shellHeight = shellRect.Bottom - shellRect.Top;

                if (axis == AppBarAxis.AXIS_H && shellWidth > 8 && shellHeight > 8)
                {
                    int midY = shellRect.Top + shellHeight / 2;
                    AddLuminanceSample(hdc, samples, shellRect.Left + shellWidth / 16, midY);
                    AddLuminanceSample(hdc, samples, shellRect.Right - shellWidth / 8, midY);
                }
                else if (shellWidth > 8 && shellHeight > 8)
                {
                    int midX = shellRect.Left + shellWidth / 2;
                    AddLuminanceSample(hdc, samples, midX, shellRect.Bottom - shellHeight / 8);
                }
            }
        }

        private static bool? ReadSystemUsesLightTheme()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(ThemePersonalizeKey))
                {
                    if (key == null)
                    {
                        return null;
                    }

                    object value = key.GetValue("SystemUsesLightTheme");
                    if (value is int intValue)
                    {
                        return intValue != 0;
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        private bool IsTaskbarLightColorFromPixels()
        {
            if (!ResolveTaskbarHandles() || Shell_TrayWnd == IntPtr.Zero)
            {
                return _isLightTaskbar;
            }

            IntPtr hdc = GetDC(IntPtr.Zero);
            if (hdc == IntPtr.Zero)
            {
                return _isLightTaskbar;
            }

            List<int> samples = new List<int>();
            CollectTaskbarColorSamples(hdc, samples);
            ReleaseDC(IntPtr.Zero, hdc);

            if (samples.Count == 0)
            {
                return ReadSystemUsesLightTheme() ?? _isLightTaskbar;
            }

            samples.Sort();
            double median = samples[samples.Count / 2];

            if (median >= TaskbarLightOnThreshold)
            {
                return true;
            }

            if (median <= TaskbarDarkOnThreshold)
            {
                return false;
            }

            return _isLightTaskbar;
        }

        private bool IsTaskbarLightColor()
        {
            bool? systemTheme = ReadSystemUsesLightTheme();
            if (systemTheme.HasValue)
            {
                return systemTheme.Value;
            }

            return IsTaskbarLightColorFromPixels();
        }

        private void ApplyCapsLockImage()
        {
            if (Console.CapsLock)
            {
                Key.Image = _isLightTaskbar ? Properties.Resources.Capital_light : Properties.Resources.Capital_dark;
            }
            else
            {
                Key.Image = _isLightTaskbar ? Properties.Resources.Lower_case_light : Properties.Resources.Lower_case_dark;
            }
        }

        private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (IsDisposed)
            {
                return;
            }

            if (e.Category == UserPreferenceCategory.General || e.Category == UserPreferenceCategory.Color)
            {
                BeginInvoke(new Action(() => ApplyTheme(invalidateBackgroundCache: true)));
            }
        }

        private AppBarEdge GetTaskbarEdge()
        {
            AppBarData taskbarData = new AppBarData
            {
                cbSize = Marshal.SizeOf(typeof(AppBarData))
            };
            SHAppBarMessage((uint)AppBarMessages.GetTaskBarPos, ref taskbarData);
            return (AppBarEdge)taskbarData.uEdge;
        }

        public AppBarAxis TaskbarAxis(IntPtr shellTrayWnd)
        {
            AppBarEdge taskbarEdge = GetTaskbarEdge();
            if (taskbarEdge == AppBarEdge.ABE_LEFT || taskbarEdge == AppBarEdge.ABE_RIGHT)
            {
                return AppBarAxis.AXIS_V;
            }
            return AppBarAxis.AXIS_H;
        }

        private static IntPtr FindDescendantByClass(IntPtr parent, string className)
        {
            if (parent == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            IntPtr direct = FindWindowEx(parent, IntPtr.Zero, className, null);
            if (direct != IntPtr.Zero)
            {
                return direct;
            }

            IntPtr child = IntPtr.Zero;
            while (true)
            {
                child = FindWindowEx(parent, child, null, null);
                if (child == IntPtr.Zero)
                {
                    break;
                }

                IntPtr found = FindDescendantByClass(child, className);
                if (found != IntPtr.Zero)
                {
                    return found;
                }
            }

            return IntPtr.Zero;
        }

        private bool ResolveTaskbarHandles()
        {
            IntPtr shellTrayWnd = FindWindow("Shell_TrayWnd", null);
            if (shellTrayWnd == IntPtr.Zero || !IsWindow(shellTrayWnd))
            {
                return false;
            }

            IntPtr reBarWindow32 = FindDescendantByClass(shellTrayWnd, "ReBarWindow32");
            if (reBarWindow32 == IntPtr.Zero || !IsWindow(reBarWindow32))
            {
                return false;
            }

            IntPtr msTaskSwWClass = FindDescendantByClass(reBarWindow32, "MSTaskSwWClass");
            if (msTaskSwWClass == IntPtr.Zero || !IsWindow(msTaskSwWClass))
            {
                return false;
            }

            Shell_TrayWnd = shellTrayWnd;
            ReBarWindow32 = reBarWindow32;
            MSTaskSwWClass = msTaskSwWClass;
            MSTaskListWClass = FindDescendantByClass(MSTaskSwWClass, "MSTaskListWClass");
            return true;
        }

        private bool IsEmbeddedInTaskbar()
        {
            return MSTaskSwWClass != IntPtr.Zero
                && IsWindow(MSTaskSwWClass)
                && GetParent(this.Handle) == MSTaskSwWClass;
        }

        private static bool IsValidTaskbarSpan(int span)
        {
            return span > 0 && span <= MaxTaskbarSpan;
        }

        private bool TryGetTaskSwClientSize(out int swWidth, out int swHeight)
        {
            swWidth = 0;
            swHeight = 0;

            if (MSTaskSwWClass == IntPtr.Zero || !IsWindow(MSTaskSwWClass))
            {
                return false;
            }

            RECT taskSwClient;
            if (!GetClientRect(MSTaskSwWClass, out taskSwClient))
            {
                return false;
            }

            swWidth = taskSwClient.Right - taskSwClient.Left;
            swHeight = taskSwClient.Bottom - taskSwClient.Top;
            return IsValidTaskbarSpan(swWidth) && IsValidTaskbarSpan(swHeight);
        }

        private bool HasLayoutContextChanged(int swWidth, int swHeight, uint dpi, float scale)
        {
            return swWidth != _lastSwWidth
                || swHeight != _lastSwHeight
                || dpi != _lastAppliedDpi
                || Math.Abs(scale - _lastLayoutScale) > 0.001f;
        }

        private static bool IsReasonableDpi(uint dpi)
        {
            return dpi >= 72 && dpi <= 480;
        }

        /// <summary>多源检测任务栏所在显示器的 DPI，不依赖外部 manifest。</summary>
        private uint DetectTaskbarDpi()
        {
            IntPtr[] dpiSources = { Shell_TrayWnd, MSTaskSwWClass, Handle };
            foreach (IntPtr source in dpiSources)
            {
                if (source == IntPtr.Zero)
                {
                    continue;
                }

                uint dpi = GetDpiForWindow(source);
                if (IsReasonableDpi(dpi))
                {
                    return dpi;
                }
            }

            if (Shell_TrayWnd != IntPtr.Zero)
            {
                try
                {
                    using (Graphics graphics = Graphics.FromHwnd(Shell_TrayWnd))
                    {
                        uint dpi = (uint)Math.Round(graphics.DpiX);
                        if (IsReasonableDpi(dpi))
                        {
                            return dpi;
                        }
                    }
                }
                catch
                {
                }
            }

            try
            {
                uint dpi = GetDpiForSystem();
                if (IsReasonableDpi(dpi))
                {
                    return dpi;
                }
            }
            catch
            {
            }

            try
            {
                object appliedDpi = Registry.GetValue(
                    @"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics",
                    "AppliedDPI",
                    null);
                if (appliedDpi is int applied && IsReasonableDpi((uint)applied))
                {
                    return (uint)applied;
                }

                object logPixels = Registry.GetValue(
                    @"HKEY_CURRENT_USER\Control Panel\Desktop",
                    "LogPixels",
                    null);
                if (logPixels is int pixels && IsReasonableDpi((uint)pixels))
                {
                    return (uint)pixels;
                }
            }
            catch
            {
            }

            using (Graphics graphics = CreateGraphics())
            {
                return (uint)Math.Max(BaseDpi, Math.Round(graphics.DpiX));
            }
        }

        private static int ScaleDesign(int designValue, float scale)
        {
            return Math.Max(1, (int)Math.Round(designValue * scale));
        }

        private void DisposeLabelFonts()
        {
            if (_labelFonts == null)
            {
                return;
            }

            foreach (Font font in _labelFonts)
            {
                if (font != null)
                {
                    font.Dispose();
                }
            }

            _labelFonts = null;
        }

        /// <summary>
        /// 严格按 Designer 坐标等比缩放；窗体宽 = 内容右缘 + 少量 padding。
        /// </summary>
        private int ApplyWidgetLayout(float layoutScale, float fontScale, int clientHeight)
        {
            layoutScale = Math.Max(0.75f, layoutScale);
            fontScale = Math.Max(layoutScale, fontScale);
            int S(int value) => ScaleDesign(value, layoutScale);
            int labelH = S(20);
            float fontSize = Math.Max(7f, (float)Math.Round(DesignFontSize * fontScale) - 2.5f);

            ThemeLabel[] labels = GetTextLabels();
            Font[] newFonts = new Font[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                newFonts[i] = new Font(DesignFontName, fontSize, FontStyle.Regular, GraphicsUnit.Point);
            }

            SuspendLayout();

            Upload_Img.Image = Properties.Resources.Upload;
            Upload_Img.SizeMode = PictureBoxSizeMode.CenterImage;
            Upload_Img.Margin = Padding.Empty;
            Upload_Img.SetBounds(S(7), S(8), S(9), S(10));

            Download_Img.Image = Properties.Resources.Download;
            Download_Img.SizeMode = PictureBoxSizeMode.CenterImage;
            Download_Img.Margin = Padding.Empty;
            Download_Img.SetBounds(S(7), S(31), S(9), S(10));

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].AutoSize = false;
                labels[i].Margin = Padding.Empty;
                labels[i].Font = newFonts[i];
            }

            Upload_Text.SetBounds(S(19), S(2), S(75), labelH);
            Download_Text.SetBounds(S(19), S(26), S(75), labelH);
            CPU_Title.SetBounds(S(89), S(2), S(37), labelH);
            CPU_Text.SetBounds(S(126), S(2), S(42), labelH);
            RAM_Title.SetBounds(S(89), S(26), S(42), labelH);
            RAM_Text.SetBounds(S(126), S(26), S(42), labelH);

            ApplyCapsLockImage();
            ApplyCompositeDisplay();
            Key.SizeMode = PictureBoxSizeMode.CenterImage;
            Key.Margin = Padding.Empty;
            int keySlotW = S(21);
            int keyX = S(165) + Math.Max(0, (keySlotW - KeyIconNativeSize) / 2);
            int keyY = Math.Max(0, (clientHeight - KeyIconNativeSize) / 2);
            Key.SetBounds(keyX, keyY, KeyIconNativeSize, KeyIconNativeSize);

            int clientWidth = S(DesignContentRight + DesignRightPadding);
            contextMenu.ImageScalingSize = new Size(S(20), S(20));
            ClientSize = new Size(clientWidth, clientHeight);
            _lastLayoutScale = layoutScale;

            Font[] oldFonts = _labelFonts;
            _labelFonts = newFonts;

            ResumeLayout(false);
            PerformLayout();

            if (oldFonts != null)
            {
                foreach (Font font in oldFonts)
                {
                    if (font != null)
                    {
                        font.Dispose();
                    }
                }
            }

            return clientWidth;
        }

        private void EnsureEmbedRetry()
        {
            if (IsEmbeddedInTaskbar())
            {
                return;
            }

            _embedRetryCount = 0;
            EmbedRetryTimer.Interval = 500;
            if (!EmbedRetryTimer.Enabled)
            {
                EmbedRetryTimer.Start();
            }
        }

        private void HideFromDesktop()
        {
            if (!IsEmbeddedInTaskbar() && Visible)
            {
                Hide();
            }
        }

        private void ScheduleDisplayChangeLayoutUpdate()
        {
            if (IsDisposed)
            {
                return;
            }

            _displayChangePending = true;
            DisplayChangeDebounceTimer.Stop();
            DisplayChangeDebounceTimer.Start();
        }

        private void OnDisplayChangeDebounced(object sender, EventArgs e)
        {
            DisplayChangeDebounceTimer.Stop();
            if (IsDisposed)
            {
                return;
            }

            _displayChangePending = false;
            Hide();
            RestoreTaskbarLayout();
            _lastSwWidth = 0;
            _lastSwHeight = 0;
            _lastAppliedDpi = 0;
            _lastLayoutScale = 0;
            ResolveTaskbarHandles();
            UpdateTaskbarLayout(force: true);
            EnsureEmbedRetry();
        }

        private void UpdateTaskbarLayout(bool force = false)
        {
            if (!ResolveTaskbarHandles())
            {
                HideFromDesktop();
                EnsureEmbedRetry();
                return;
            }

            if (!IsWindow(MSTaskSwWClass))
            {
                HideFromDesktop();
                EnsureEmbedRetry();
                return;
            }

            _lastAxis = TaskbarAxis(Shell_TrayWnd);

            int swWidth;
            int swHeight;
            if (!TryGetTaskSwClientSize(out swWidth, out swHeight))
            {
                HideFromDesktop();
                EnsureEmbedRetry();
                return;
            }

            if (!force && IsEmbeddedInTaskbar())
            {
                uint currentDpi = DetectTaskbarDpi();
                float currentScale = swHeight / (float)WidgetHeight;
                if (!HasLayoutContextChanged(swWidth, swHeight, currentDpi, currentScale))
                {
                    return;
                }
            }

            uint dpi = DetectTaskbarDpi();
            float layoutScale = swHeight / (float)WidgetHeight;
            float fontScale = Math.Max(layoutScale, dpi / (float)BaseDpi);
            int widgetH = swHeight;

            if (_lastAxis == AppBarAxis.AXIS_V)
            {
                widgetH = Math.Min(ScaleDesign(WidgetHeight, layoutScale), swHeight);
            }

            int widgetW = ApplyWidgetLayout(layoutScale, fontScale, widgetH);
            widgetW = Math.Min(widgetW, swWidth);
            if (ClientSize.Width != widgetW)
            {
                ClientSize = new Size(widgetW, widgetH);
            }
            _lastAppliedDpi = dpi;

            int widgetX;
            int widgetY;

            if (_lastAxis == AppBarAxis.AXIS_H)
            {
                widgetX = Math.Max(0, swWidth - widgetW);
                widgetY = Math.Max(0, (swHeight - widgetH) / 2);

                if (MSTaskListWClass != IntPtr.Zero && IsWindow(MSTaskListWClass))
                {
                    int listWidth = Math.Max(0, swWidth - widgetW);
                    MoveWindow(MSTaskListWClass, 0, 0, listWidth, swHeight, true);
                }
            }
            else
            {
                widgetX = Math.Max(0, (swWidth - widgetW) / 2);
                widgetY = Math.Max(0, swHeight - widgetH);

                if (MSTaskListWClass != IntPtr.Zero && IsWindow(MSTaskListWClass))
                {
                    int listHeight = Math.Max(0, swHeight - widgetH);
                    MoveWindow(MSTaskListWClass, 0, 0, swWidth, listHeight, true);
                }
            }

            if (GetParent(this.Handle) != MSTaskSwWClass)
            {
                SetParent(this.Handle, MSTaskSwWClass);
                int style = GetWindowLong(Handle, GWL_STYLE);
                SetWindowLong(Handle, GWL_STYLE, (style | WS_CHILD | WS_VISIBLE) & ~WS_POPUP);
            }

            MoveWindow(this.Handle, widgetX, widgetY, widgetW, widgetH, false);

            if (!IsEmbeddedInTaskbar())
            {
                HideFromDesktop();
                EnsureEmbedRetry();
            }
            else
            {
                bool sizeChanged = widgetW != _cachedBackgroundWidth || widgetH != _cachedBackgroundHeight;
                if (sizeChanged)
                {
                    InvalidateBackgroundCache();
                }

                ApplyTheme(invalidateBackgroundCache: sizeChanged);
                EnsureLayeredSurface();
            }

            MoveWindow(this.Handle, widgetX, widgetY, widgetW, widgetH, true);
            ShowWindow(Handle, SW_SHOW);

            if (!Visible)
            {
                Show();
            }

            _lastSwWidth = swWidth;
            _lastSwHeight = swHeight;
        }

        private void RestoreTaskbarLayout()
        {
            if (!ResolveTaskbarHandles())
            {
                return;
            }

            int swWidth;
            int swHeight;
            if (!TryGetTaskSwClientSize(out swWidth, out swHeight))
            {
                return;
            }

            if (MSTaskListWClass != IntPtr.Zero && IsWindow(MSTaskListWClass))
            {
                MoveWindow(MSTaskListWClass, 0, 0, swWidth, swHeight, true);
            }

            if (GetParent(this.Handle) != IntPtr.Zero)
            {
                SetParent(this.Handle, IntPtr.Zero);
            }

            _embeddedVisualThemeDisabled = false;
            _layeredSurfaceEnabled = false;
            _layeredSurfaceFailed = false;
            InvalidateBackgroundCache();
        }

        private void OnDisplaySettingsChanged(object sender, EventArgs e)
        {
            if (IsDisposed)
            {
                return;
            }

            BeginInvoke(new Action(ScheduleDisplayChangeLayoutUpdate));
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DISPLAYCHANGE || m.Msg == WM_DPICHANGED)
            {
                BeginInvoke(new Action(ScheduleDisplayChangeLayoutUpdate));
            }
            else if (m.Msg == WM_SETTINGCHANGE && m.LParam != IntPtr.Zero)
            {
                string setting = Marshal.PtrToStringAuto(m.LParam);
                if (setting == "ImmersiveColorSet" || setting == "WindowsThemeElement")
                {
                    ApplyTheme(invalidateBackgroundCache: true);
                }
            }
            else if (m.Msg == WM_MOUSEMOVE)
            {
                int lp = m.LParam.ToInt32();
                int x = (short)(lp & 0xFFFF);
                int y = (short)((lp >> 16) & 0xFFFF);
                bool inside = ClientRectangle.Contains(x, y);
                if (inside)
                {
                    EnsureMouseLeaveTracking();
                }
                else
                {
                    _trackingMouseLeave = false;
                }

                SetHoverState(inside);
            }
            else if (m.Msg == WM_MOUSELEAVE)
            {
                _trackingMouseLeave = false;
                Point client = PointToClient(Cursor.Position);
                if (!ClientRectangle.Contains(client))
                {
                    SetHoverState(false);
                }
            }

            base.WndProc(ref m);
        }

        public TaskbarMore()
        {
            InitializeComponent();
            ApplyCompositeDisplay();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);
            UpdateStyles();

            _isLightTaskbar = ReadSystemUsesLightTheme() ?? false;
            ResolveTaskbarHandles();
            ApplyTheme();

            KeyHook = new KeyboardHook();
            KeyHook.KeyUpEvent += new KeyEventHandler(TaskbarMore_KeyUp);
            KeyHook.Start();

            Monitor = new NetworkMonitor();
            Adapters = Monitor.Adapters;
            if (Adapters.Length == 0)
            {
                MessageBox.Show("No network adapters found on this computer.");
            }
            else
            {
                Monitor.StopMonitoring();
                Monitor.StartMonitoring(Adapters[0]);
            }

            ManagementClass MC = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection MOC = MC.GetInstances();
            foreach (ManagementObject MO in MOC)
            {
                if (MO["TotalPhysicalMemory"] != null)
                {
                    RAM_ALL = long.Parse(MO["TotalPhysicalMemory"].ToString()) >> 20;
                }
            }

            CPU_Counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            CPU_Counter.NextValue();
            RAM_Counter = new PerformanceCounter("Memory", "Available MBytes");

            TaskbarMoreTimer = new Timer();
            TaskbarMoreTimer.Tick += TaskbarMore_ShowInfo;
            TaskbarMoreTimer.Interval = 1000;
            TaskbarMoreTimer.Enabled = true;

            EmbedRetryTimer = new Timer();
            EmbedRetryTimer.Tick += EmbedRetryTimer_Tick;
            EmbedRetryTimer.Interval = 500;

            DisplayChangeDebounceTimer = new Timer();
            DisplayChangeDebounceTimer.Tick += OnDisplayChangeDebounced;
            DisplayChangeDebounceTimer.Interval = DisplayChangeDebounceMs;

            SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
            SystemEvents.UserPreferenceChanged += OnUserPreferenceChanged;

            _lastAxis = TaskbarAxis(Shell_TrayWnd);
        }

        private void EmbedRetryTimer_Tick(object sender, EventArgs e)
        {
            UpdateTaskbarLayout(force: true);

            if (IsEmbeddedInTaskbar())
            {
                EmbedRetryTimer.Stop();
            }
            else if (_embedRetryCount++ >= 60)
            {
                EmbedRetryTimer.Interval = 2000;
                _embedRetryCount = 0;
            }
        }

        private void TaskbarMore_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            UpdateTaskbarLayout(force: true);
            _embedRetryCount = 0;
            EmbedRetryTimer.Start();
        }

        private void TaskbarMore_FormClosing(object sender, FormClosingEventArgs e)
        {
            SystemEvents.DisplaySettingsChanged -= OnDisplaySettingsChanged;
            SystemEvents.UserPreferenceChanged -= OnUserPreferenceChanged;
            DisplayChangeDebounceTimer.Stop();
            DisplayChangeDebounceTimer.Dispose();
            EmbedRetryTimer.Stop();
            EmbedRetryTimer.Dispose();
            RestoreTaskbarLayout();
            DisposeLabelFonts();
            InvalidateBackgroundCache();
            KeyHook.Stop();
            TaskbarMoreTimer.Stop();
            TaskbarMoreTimer.Dispose();
            Monitor.StopMonitoring();
            CPU_Counter.Close();
            CPU_Counter.Dispose();
            RAM_Counter.Close();
            RAM_Counter.Dispose();
        }

        private void TaskbarMore_ShowInfo(object sender, EventArgs e)
        {
            if (!IsEmbeddedInTaskbar() || _displayChangePending)
            {
                UpdateTaskbarLayout(force: !IsEmbeddedInTaskbar());
            }
            else if (ResolveTaskbarHandles())
            {
                int swWidth;
                int swHeight;
                if (TryGetTaskSwClientSize(out swWidth, out swHeight))
                {
                    uint dpi = DetectTaskbarDpi();
                    float scale = swHeight / (float)WidgetHeight;
                    if (HasLayoutContextChanged(swWidth, swHeight, dpi, scale))
                    {
                        UpdateTaskbarLayout(force: true);
                    }
                }
            }

            bool light = IsTaskbarLightColor();
            if (light != _isLightTaskbar)
            {
                ApplyTheme(invalidateBackgroundCache: true);
            }

            if (Adapters.Length == 0)
            {
                return;
            }

            double UploadSpeedKbps = Adapters[0].UploadSpeedKbps;
            double DownloadSpeedKbps = Adapters[0].DownloadSpeedKbps;
            bool dirty = false;

            if (UploadSpeedKbps < 100)
            {
                dirty |= SetTextIfChanged(Upload_Text, string.Format("{0:0.00} K/s", UploadSpeedKbps));
            }
            else
            {
                dirty |= SetTextIfChanged(Upload_Text, string.Format("{0:0.0} M/s", UploadSpeedKbps / 1024));
            }

            if (DownloadSpeedKbps < 100)
            {
                dirty |= SetTextIfChanged(Download_Text, string.Format("{0:0.00} K/s", DownloadSpeedKbps));
            }
            else
            {
                dirty |= SetTextIfChanged(Download_Text, string.Format("{0:0.0} M/s", DownloadSpeedKbps / 1024));
            }

            dirty |= SetTextIfChanged(CPU_Text, string.Format("{0}%", (int)CPU_Counter.NextValue()));
            dirty |= SetTextIfChanged(RAM_Text, string.Format("{0}%", (int)((1 - RAM_Counter.NextValue() / RAM_ALL) * 100)));

            if (dirty)
            {
                Invalidate();
            }
        }

        private void SetHoverState(bool hovering)
        {
            if (_isHovering == hovering)
            {
                return;
            }

            _isHovering = hovering;
            BackColor = IsEmbeddedInTaskbar() ? Color.Black : CurrentThemeBackColor;

            if (IsEmbeddedInTaskbar() && !hovering && _embeddedBackgroundCaptureDeferredByHover)
            {
                _embeddedBackgroundCaptureDeferredByHover = false;
                ScheduleEmbeddedBackgroundCapture();
            }

            RepaintComposite();
        }

        private void RepaintComposite()
        {
            if (!IsHandleCreated || IsDisposed)
            {
                return;
            }

            _inCompositePaint = true;
            try
            {
                using (Graphics g = CreateGraphics())
                {
                    PaintCompositeContent(new PaintEventArgs(g, ClientRectangle));
                }
            }
            finally
            {
                _inCompositePaint = false;
            }
        }

        private void EnsureMouseLeaveTracking()
        {
            if (_trackingMouseLeave || !IsHandleCreated)
            {
                return;
            }

            TRACKMOUSEEVENT track = new TRACKMOUSEEVENT
            {
                cbSize = (uint)Marshal.SizeOf(typeof(TRACKMOUSEEVENT)),
                dwFlags = TME_LEAVE,
                hwndTrack = Handle,
                dwHoverTime = 0
            };

            if (TrackMouseEvent(ref track))
            {
                _trackingMouseLeave = true;
            }
        }

        private void TaskbarMore_MouseHover(object sender, EventArgs e)
        {
            Point client = PointToClient(Cursor.Position);
            SetHoverState(ClientRectangle.Contains(client));
        }

        private void TaskbarMore_MouseMove(object sender, MouseEventArgs e)
        {
            Point client = sender == this ? e.Location : PointToClient(Cursor.Position);
            SetHoverState(ClientRectangle.Contains(client));
        }

        private void TaskbarMore_MouseLeave(object sender, EventArgs e)
        {
            Point client = PointToClient(Cursor.Position);
            if (!ClientRectangle.Contains(client))
            {
                SetHoverState(false);
            }
        }

        private void TaskbarMore_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Capital)
            {
                ApplyCapsLockImage();
                InvalidateIconContentCache();
                Invalidate();
            }
        }

        private void StartupMenuItem_Click(object sender, EventArgs e)
        {
            string KeyName = "TaskbarMore";
            try
            {
                string strName = Application.ExecutablePath;
                if (!File.Exists(strName)) return;
                Microsoft.Win32.RegistryKey Rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (Rkey == null)
                {
                    Rkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                Rkey.SetValue(KeyName, strName + " -s");
                MessageBox.Show("The program will boot starting!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch
            {
                MessageBox.Show("Please use the administrator's permission to open the application!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UnStartupMenuItem_Click(object sender, EventArgs e)
        {
            string KeyName = "TaskbarMore";
            try
            {
                string strName = Application.ExecutablePath;
                RegistryKey Rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (Rkey == null)
                {
                    Rkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                }
                Rkey.DeleteValue(KeyName, false);
                MessageBox.Show("The program will not boot starting!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch
            {
                MessageBox.Show("Please use the administrator's permission to open the application!", "TaskbarMore Boot Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It is powered by GUI.\n\rIt can display network, CPU, RAM, caps lock status.", "TaskbarMore About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            Application.Exit();
        }
    }
}
