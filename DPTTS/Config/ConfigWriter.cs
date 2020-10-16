using System.Windows.Forms;

namespace DPTTS.Config
{
    class ConfigWriter
    {
        public static void Config_Write()
        {
            IniFile INI = new IniFile("config.ini");
            INI.Write("DPTTS", "Token", ConfigManager.Token);
            MessageBox.Show("Настройки сохранены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); // Говорим пользователю, что сохранили конфиг.
        }
    }
}
