using System;
using System.Timers;
using System.Collections;
using System.Diagnostics;

namespace NetWorkSpeedMonitor
{
    /// <summary>
    /// The NetworkMonitor class monitors network speed for each network adapter on the computer,
    /// using classes for Performance counter in .NET library.
    /// </summary>
    public class NetworkMonitor
    {
        private Timer timer;				// The timer event executes every second to refresh the values in adapters.
        private ArrayList adapters;			// The list of adapters on the computer.
        private ArrayList monitoredAdapters;// The list of currently monitored adapters.

        public NetworkMonitor()
        {
            this.adapters = new ArrayList();
            this.monitoredAdapters = new ArrayList();
            EnumerateNetworkAdapters();

            timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }
        /// <summary>
        /// Enumerates network adapters installed on the computer.
        /// </summary>
        private void EnumerateNetworkAdapters()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");

            foreach (string name in category.GetInstanceNames())
            {
                // This one exists on every computer.
                if (name == "MS TCP Loopback interface")
                    continue;
                // Create an instance of NetworkAdapter class, and create performance counters for it.
                NetworkAdapter adapter = new NetworkAdapter(name);
                adapter.dlCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name);
                adapter.ulCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name);
                this.adapters.Add(adapter);	// Add it to ArrayList adapter
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (NetworkAdapter adapter in this.monitoredAdapters)
                adapter.refresh();
        }
        /// <summary>
        /// Get instances of NetworkAdapter for installed adapters on this computer.
        /// </summary>
        public NetworkAdapter[] Adapters
        {
            get { return (NetworkAdapter[])this.adapters.ToArray(typeof(NetworkAdapter)); }
        }
        /// <summary>
        /// Enable the timer and add all adapters to the monitoredAdapters list, 
        /// unless the adapters list is empty.
        /// </summary>
        public void StartMonitoring()
        {
            if (this.adapters.Count > 0)
            {
                foreach (NetworkAdapter adapter in this.adapters)
                    if (!this.monitoredAdapters.Contains(adapter))
                    {
                        this.monitoredAdapters.Add(adapter);
                        adapter.init();
                    }

                timer.Enabled = true;
            }
        }
        /// <summary>
        /// Enable the timer, and add the specified adapter to the monitoredAdapters list
        /// </summary>
        public void StartMonitoring(NetworkAdapter adapter)
        {
            if (!this.monitoredAdapters.Contains(adapter))
            {
                this.monitoredAdapters.Add(adapter);
                adapter.init();
            }
            timer.Enabled = true;
        }
        /// <summary>
        /// Disable the timer, and clear the monitoredAdapters list.
        /// </summary>
        public void StopMonitoring()
        {
            this.monitoredAdapters.Clear();
            timer.Enabled = false;
        }
        /// <summary>
        /// Remove the specified adapter from the monitoredAdapters list, and 
        /// disable the timer if the monitoredAdapters list is empty.
        /// </summary>
        public void StopMonitoring(NetworkAdapter adapter)
        {
            if (this.monitoredAdapters.Contains(adapter))
                this.monitoredAdapters.Remove(adapter);
            if (this.monitoredAdapters.Count == 0)
                timer.Enabled = false;
        }
    }
}