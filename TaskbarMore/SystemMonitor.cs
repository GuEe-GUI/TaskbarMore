using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace TaskbarMore
{
    // Sampling is owned by the control's UI timer; no worker can outlive a closed deskband.
    internal sealed class SystemMonitor : IDisposable
    {
        private PerformanceCounter cpu;
        private readonly Stopwatch clock = Stopwatch.StartNew();
        private readonly Dictionary<string, long[]> previous = new Dictionary<string, long[]>();
        private double lastSample;
        internal double UploadBytesPerSecond { get; private set; }
        internal double DownloadBytesPerSecond { get; private set; }
        internal string CpuText { get; private set; } = "--";
        internal string MemoryText { get; private set; } = "--";

        internal void Sample()
        {
            try
            {
                if (cpu == null) cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                CpuText = Math.Min(100, Math.Max(0, cpu.NextValue())).ToString("0") + "%";
            }
            catch (Exception e) when (e is InvalidOperationException || e is Win32Exception || e is UnauthorizedAccessException)
            {
                cpu?.Dispose();
                cpu = null;
                CpuText = "--";
            }

            var memory = new MemoryStatus { Length = (uint)Marshal.SizeOf(typeof(MemoryStatus)) };
            MemoryText = GlobalMemoryStatusEx(ref memory) ? memory.MemoryLoad + "%" : "--";
            SampleNetwork();
        }

        private void SampleNetwork()
        {
            double now = clock.Elapsed.TotalSeconds;
            double elapsed = now - lastSample;
            long received = 0, sent = 0;
            var current = new Dictionary<string, long[]>();
            try
            {
                foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (adapter.OperationalStatus != OperationalStatus.Up ||
                        adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                        adapter.NetworkInterfaceType == NetworkInterfaceType.Tunnel) continue;
                    try
                    {
                        var stats = adapter.GetIPStatistics();
                        var values = new[] { stats.BytesReceived, stats.BytesSent };
                        current[adapter.Id] = values;
                        if (previous.TryGetValue(adapter.Id, out var old))
                        {
                            received += Math.Max(0, values[0] - old[0]);
                            sent += Math.Max(0, values[1] - old[1]);
                        }
                    }
                    catch (NetworkInformationException) { }
                }
            }
            catch (NetworkInformationException) { }
            previous.Clear();
            foreach (var pair in current) previous.Add(pair.Key, pair.Value);
            DownloadBytesPerSecond = elapsed > 0 ? received / elapsed : 0;
            UploadBytesPerSecond = elapsed > 0 ? sent / elapsed : 0;
            lastSample = now;
        }

        public void Dispose() { cpu?.Dispose(); cpu = null; }

        [StructLayout(LayoutKind.Sequential)]
        private struct MemoryStatus
        {
            internal uint Length, MemoryLoad;
            internal ulong TotalPhysical, AvailablePhysical, TotalPageFile, AvailablePageFile,
                TotalVirtual, AvailableVirtual, AvailableExtendedVirtual;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx(ref MemoryStatus status);
    }
}
