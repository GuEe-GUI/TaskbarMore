using System;
using System.Windows.Forms;
namespace TaskbarMore{
    static class Program{
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(){
            bool Running;//确认程序是否已经启动，防止重复启动
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, "OnlyRunOneInstance", out Running);
            if (Running){
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TaskbarMore());
                mutex.ReleaseMutex();
            }
            else{
                MessageBox.Show("TaskbarMore is running!", "TaskbarMore Running Tip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}