using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace DPTTS
{
    class Program
    {
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