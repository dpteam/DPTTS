using System.Windows.Forms;

namespace DPTTS.Config
{
    public static class ConfigReader
    {
        public static void Config_Read()
        {
            IniFile INI = new IniFile("config.ini");
            if (INI.KeyExists("DPTTS", "Token"))
                ConfigManager.Token = (string)INI.ReadINI("DPTTS", "Token");
            else
                ConfigManager.Token = "";

            MessageBox.Show("Настройки загружены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); // Говорим пользователю, что загрузили конфиг.
        }
    }
}
