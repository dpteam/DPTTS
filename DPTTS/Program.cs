using DPTTS.BotCmds;
using DPTTS.Config;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace DPTTS
{
    class Program
    {
        public static VkApi api = new VkApi();

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("[CORE] Загрузка кода");
            Marshal.PrelinkAll(typeof(Program));
            Trace.AutoFlush = true;
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ConsoleTraceListener());
            if (ConfigManager.LoggingEnabled == true)
            {
                Trace.Listeners.Add(new TextWriterTraceListener(System.IO.Path.GetFileNameWithoutExtension(typeof(Program).Assembly.GetName().Name) + ".log"));
                Trace.WriteLine("[INFO] Логирование включено и активно");
            }
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
                // Часть фич взята с тутора с лолзтим, автор Shellar
                string GroupID = INI.ReadINI("DPTTS", "GroupID");
                Trace.WriteLine("[INFO] Авторизация завершена");
                while (true) // Бесконечный цикл, получение обновлений
                {
                    var s = api.Groups.GetLongPollServer(Convert.ToUInt64(GroupID));
                    var poll = api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams(){ Server = s.Server, Ts = s.Ts, Key = s.Key, Wait = 25 });
                    if (poll?.Updates == null) continue; // Проверка на новые события
                    foreach (var a in poll.Updates)
                    {
                        if (a.Type == GroupUpdateType.MessageNew)
                        {
                            string userMessage = a.MessageNew.Message.Body.ToLower(null);
                            long ? userID = a.MessageNew.Message.UserId;
                            if (userMessage == "привет")
                            {
                                MessagesManager.SendMessage("Здарова!", userID);
                                Trace.WriteLine("[INFO] Сообщение отправлено пользователю с ID: " + userID);
                            }
                        }
                    }
                }
            }
            catch (Exception value)
            {
                try
                {
                    MessageBox.Show(value.ToString(), "Error");
                    Trace.WriteLine("[ERROR] " + value.ToString());
                    Console.ReadKey();
                }
                catch
                {
                }
            }
        }
    }
}