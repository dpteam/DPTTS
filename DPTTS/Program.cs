using DPTTS.BotCmds;
using DPTTS.Config;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
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
            // Смена цвета консоли
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            // Ядро (WIP)
            Console.WriteLine("[CORE] Инициализация ядра");
            Console.WriteLine("[CORE] Загрузка кода");
            // Загрузка всего кода в ОЗУ (оптимизация процесса)
            Marshal.PrelinkAll(typeof(Program));
            // Инициализация системы логирования
            Trace.AutoFlush = true;
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ConsoleTraceListener());
            if (ConfigManager.LoggingEnabled == true)
            {
                Trace.Listeners.Add(new TextWriterTraceListener(System.IO.Path.GetFileNameWithoutExtension(typeof(Program).Assembly.GetName().Name) + ".log"));
                Trace.WriteLine("[INFO] Логирование включено и активно");
            }
            // Задаем высокий приоритет процесса
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.PriorityClass = ProcessPriorityClass.High;
            Trace.WriteLine("[DEBUG] Инициализация завершена, запуск сервера");
            try
            {
                // Инициализация конфига
                //Trace.WriteLine("[INFO] Идет загрузка конфига");
                // Work in Progress...
                //ConfigReader.Config_Read();
                IniFile INI = new IniFile("DPTTS.INI");

                // Авторизация
                Trace.WriteLine("[INFO] Идет авторизация");
                api.Authorize(new ApiAuthParams() { AccessToken = (string)INI.ReadINI("DPTTS", "Token")});
                // Debug Show Token
                //Trace.Write((string)INI.ReadINI("DPTTS", "Token"));
                string GroupID = INI.ReadINI("DPTTS", "GroupID");
                string SecretKey = INI.ReadINI("DPTTS", "SecretKey");
                Trace.WriteLine("[INFO] Авторизация завершена");
                Trace.WriteLine("[DEBUG] Начинаем взаимодействие с ВК");
                // Меняем версию API
                //api.VkApiVersion.SetVersion(5, 124);

                // Ограничитель времени отправки запроса для цикла (временно)
                //Thread.Sleep(100);

                // Рандом для значения Ts (временно)
                Random random = new Random();
                int randomTs = random.Next();

                // Long Poll
                var s = new LongPollServerResponse();
                s = api.Groups.GetLongPollServer(Convert.ToUInt64(GroupID));
                var poll = api.Groups.GetBotsLongPollHistory(
                new BotsLongPollHistoryParams()
                { Server = s.Server, Key = s.Key, Ts = s.Ts, Wait = 25 });
                var update = poll.Updates.First();
                var messageNew = update.MessageNew;
                var message = messageNew?.Message;
                var clientInfo = messageNew?.ClientInfo;
                while (true) // Бесконечный цикл, получение обновлений
                {
                    try
                    {
                        if (poll?.Updates == null) continue; //если обновлений нет, ждём
                        foreach (var a in poll.Updates) //если есть, ищем среди них сообщение
                        {
                            if (a.Type == GroupUpdateType.MessageNew)
                            {
                                string userMessage = a.Message.Text.ToLower(); // ERROR: NullReferenceException
                                long? peerId = a.Message.PeerId - Convert.ToInt32(2000000000.0);
                                Trace.WriteLine("[DEBUG] Сообщение получено от ID: " + peerId + "\nСообщение: " + a.Message.Body);
                                var payload = a.Message.Payload; // Что это блять
                                if (userMessage == "привет")
                                {
                                    MessagesManager.SendMessage("Здарова!", peerId);
                                    Trace.WriteLine("[DEBUG] Сообщение отправлено пользователю с ID: " + peerId);
                                }
                                else
                                {
                                    Trace.WriteLine("[DEBUG] Пришло неизвестное сообщение от пользователя: " + peerId + "\nСообщение: " + userMessage);
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