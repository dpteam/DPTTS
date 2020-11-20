using System;

namespace DPTTS.Config
{
    public static class ConfigReader
    {
        public static void Config_Read()
        {
            IniFile INI = new IniFile("DPTTS.INI");
            if (INI.KeyExists("DPTTS", "Token"))
                ConfigManager.Token = (string)INI.ReadINI("DPTTS", "Token");
            else
                ConfigManager.Token = "";

            Console.WriteLine("[INFO] Настройки загружены"); // Говорим пользователю, что загрузили конфиг.
        }
    }
}
