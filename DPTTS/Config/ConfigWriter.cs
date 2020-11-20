using System;

namespace DPTTS.Config
{
    class ConfigWriter
    {
        public static void Config_Write()
        {
            IniFile INI = new IniFile("config.ini");
            INI.Write("DPTTS", "Token", ConfigManager.Token);
            Console.WriteLine("[INFO] Настройки сохранены");
        }
    }
}
