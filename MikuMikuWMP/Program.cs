using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MikuMikuWMP
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        public static string[] filePath;

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                filePath = new string[args.Length];
                for (int i = 0; i < args.Length; i++)
                    filePath[i] = args[i];
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
