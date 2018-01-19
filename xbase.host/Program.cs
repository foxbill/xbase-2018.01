using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using Newtonsoft.Json;

namespace xbase.host
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            int cmd = 0;
            if (args == null || args.Length < 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
                return;
            }

            if (!int.TryParse(args[0].Trim(), out cmd))
                return;

            switch (cmd)
            {
                case 1:
                    Console.Write(RegMachine.getAccCode());
                    break;
                case 2:
                    Console.Write(RegMachine.getRegistorCode());
                    break;
                case 3:
                    UserInfo ui = RegMachine.writeAccredit(args[1]);
                    Console.Write(JsonConvert.SerializeObject(ui));
                    break;
            }

        }
    }
}
