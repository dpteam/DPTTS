using DPTTS.BotCmds;
using DPTTS.Config;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.GroupUpdate;
using VkNet.Model.RequestParams;

namespace DPTTS
{
    class Program
    {
        public static VkApi api = new VkApi();

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("[CORE] Инициализация ядра");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
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
            Trace.WriteLine("[DEBUG] Инициализация завершена, запуск сервера");
            try
            {
                //Trace.WriteLine("[INFO] Идет загрузка конфига");
                // Work in Progress...
                //ConfigReader.Config_Read();
                IniFile INI = new IniFile("DPTTS.INI");
                Trace.WriteLine("[INFO] Идет авторизация");
                api.Authorize(new ApiAuthParams() { AccessToken = (string)INI.ReadINI("DPTTS", "Token")});
                // Debug Show Token
                //Trace.Write((string)INI.ReadINI("DPTTS", "Token"));
                string GroupID = INI.ReadINI("DPTTS", "GroupID");
                Trace.WriteLine("[INFO] Авторизация завершена");
                Trace.WriteLine("[DEBUG] Начинаем взаимодействие с ВК");
                api.VkApiVersion.SetVersion(5, 124);
                var s = new LongPollServerResponse();
                s = api.Groups.GetLongPollServer(Convert.ToUInt64(GroupID));
                while (true) // Бесконечный цикл, получение обновлений
                {
                    try
                    {
                        var poll = api.Groups.GetBotsLongPollHistory(
                        new BotsLongPollHistoryParams()
                        { Server = s.Server, Ts = s.Ts, Key = s.Key, Wait = 25 });
                        if (poll?.Updates == null) continue; //если обновлений нет, ждём
                        foreach (var a in poll.Updates) //если есть, ищем среди них сообщение
                        {
                            if (a.Type == GroupUpdateType.MessageNew)
                            {
                                Trace.WriteLine(a.Message.Body);
                                string userMessage = a.Message.Body.ToLower();
                                if (userMessage != null)
                                {
                                    long? peerId = a.Message.PeerId - Convert.ToInt32(2000000000.0);
                                    var payload = a.Message.Payload; // Что это блять
                                    if (userMessage == "привет")
                                    {
                                        MessagesManager.SendMessage("Здарова!", peerId);
                                        Trace.WriteLine("[DEBUG] Сообщение отправлено пользователю с ID: " + peerId);
                                    }
                                    else
                                    {
                                        Trace.WriteLine("[DEBUG] Пришло неизвестное сообщение от пользователя: " + peerId + "\n Сообщение: " + userMessage);
                                    }
                                }
                                else
                                {
                                    Trace.WriteLine("[DEBUG] Что-то пошло не так... Опять этот ебучий NullReferenceException... ");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.Message);
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