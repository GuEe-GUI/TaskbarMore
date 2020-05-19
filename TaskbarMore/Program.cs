using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskbarMore
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TaskbarMore());
=======
using System.Windows.Forms;

namespace TaskbarMore {
    static class Program {
        [STAThread]
        static void Main() {
            /* 确认程序是否已经启动，防止重复启动 */
            bool Running = false;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, "OnlyRunOneInstance", out Running);
            if (Running) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TaskbarMore());
                mutex.ReleaseMutex();
            }
            else {
                MessageBox.Show("TaskbarMore is running!", "TaskbarMore Running Tip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
>>>>>>> 25b12ccfb54d31de3f1f3b0774ac87a84c562c9c
        }
    }
}
