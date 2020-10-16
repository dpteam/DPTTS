using DPTTS.Config;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using VkNet;
using VkNet.Model;

namespace DPTTS
{
    class Program
    {
        public static VkApi api = new VkApi();

        [STAThread]
        static void Main(string[] args)
        {
            Marshal.PrelinkAll(typeof(Program));
            Trace.AutoFlush = true;
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.Listeners.Add(new TextWriterTraceListener(System.IO.Path.GetFileNameWithoutExtension(typeof(Program).Assembly.GetName().Name) + ".log"));
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.PriorityClass = ProcessPriorityClass.High;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                // Work in Progress...
                //ConfigReader.Config_Read();
                IniFile INI = new IniFile("DPTTS.INI");
                api.Authorize(new ApiAuthParams() { AccessToken = (string)INI.ReadINI("DPTTS", "Token")});
                // Debug Show Token
                //Trace.Write((string)INI.ReadINI("DPTTS", "Token"));
                Console.ReadKey();
            }
            catch (Exception value)
            {
                try
                {
                    MessageBox.Show(value.ToString(), "Error");
                }
                catch
                {
                }
            }
        }
    }
}