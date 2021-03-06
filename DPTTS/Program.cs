using DPTTS.BotCmds;
using DPTTS.Config;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using VkNet;
using VkNet.Model;
using VkNet.Model.GroupUpdate;
using VkNet.Model.RequestParams;

namespace DPTTS
{
    class Program
    {
        public static VkApi api = new VkApi();
        public static MessageNew MessageNew = new MessageNew();
        public static Message Message = new Message();

        [STAThread]
        static void Main(string[] args)
        {
            // Смена цвета консоли
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            // Ядро (WIP)
            Console.ForegroundColor = ConsoleColor.Magenta;
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
                Console.ForegroundColor = ConsoleColor.White;
                Trace.WriteLine("[INFO] Логирование включено и активно");
            }
            // Задаем высокий приоритет процесса
            Process currentProcess = Process.GetCurrentProcess();
            currentProcess.PriorityClass = ProcessPriorityClass.High;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Trace.WriteLine("[DEBUG] Инициализация завершена, запуск сервера");
            try
            {
                // Инициализация конфига
                //Trace.WriteLine("[INFO] Идет загрузка конфига");
                // Work in Progress...
                //ConfigReader.Config_Read();
                if (System.IO.File.Exists("DPTTS.INI"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Trace.WriteLine("[DEBUG] Конфигурционный файл найден");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Trace.WriteLine("[ERROR] Конфигурционный файл не найден, следуйте документации");
                    Console.ReadKey();
                }
                IniFile INI = new IniFile("DPTTS.INI");

                // Меняем версию API
                api.VkApiVersion.SetVersion(5, 103);
				
                // Авторизация
                Console.ForegroundColor = ConsoleColor.White;
                Trace.WriteLine("[INFO] Идет авторизация");
                api.Authorize(new ApiAuthParams() { AccessToken = (string)INI.ReadINI("DPTTS", "Token")});
                // Debug Show Token
                //Trace.Write((string)INI.ReadINI("DPTTS", "Token"));
                string GroupID = INI.ReadINI("DPTTS", "GroupID");
                string SecretKey = INI.ReadINI("DPTTS", "SecretKey");
                Console.ForegroundColor = ConsoleColor.White;
                Trace.WriteLine("[INFO] Авторизация завершена");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Trace.WriteLine("[DEBUG] Начинаем взаимодействие с ВК");

                // Ограничитель времени отправки запроса для цикла (временно)
                //Thread.Sleep(100);

                // Рандом для значения Ts (временно)
                Random random = new Random();
                int randomTs = random.Next();
                // Long Poll
                while (true) // Бесконечный цикл, получение обновлений
                {
                    try
                    {
                        var s = new LongPollServerResponse();
                        s = api.Groups.GetLongPollServer(Convert.ToUInt64(GroupID));
                        var poll = api.Groups.GetBotsLongPollHistory(new BotsLongPollHistoryParams
                        {
                            Key = s.Key,
                            Server = s.Server,
                            Ts = s.Ts,
                            Wait = 25
                        });
                        string userMessage = new MessageNew().ToString().ToLower();
                        long? peerId = Message.PeerId - Convert.ToInt32(2000000000.0);
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Trace.WriteLine("[DEBUG] Сообщение получено от ID: " + peerId + "\nСообщение: " + Message.Text);
                        var payload = Message.Payload;
                        if (userMessage == "привет")
                        {
                            MessagesManager.SendMessage("Здарова!", peerId);
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Trace.WriteLine("[DEBUG] Сообщение отправлено пользователю с ID: " + peerId);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Trace.WriteLine("[DEBUG] Пришло неизвестное сообщение от пользователя: " + peerId + "\nСообщение: " + userMessage);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Trace.WriteLine("[FATAL] " + e.Message);
                    }
                }
            }
            catch (Exception value)
            {
                try
                {
                    //MessageBox.Show(value.ToString(), "Error");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Trace.WriteLine("[FATAL] " + value.ToString());
                    Console.ReadKey();
                }
                catch
                {
                }
            }
        }
    }
}
