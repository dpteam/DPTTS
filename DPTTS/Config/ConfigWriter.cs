using System.Windows.Forms;

namespace DPTTS.Config
{
    class ConfigWriter
    {
        public static void Config_Write()
        {
            IniFile INI = new IniFile("config.ini");
            INI.WriteINI("SettingForm1", "Height", numericUpDown2.Value.ToString());
            INI.WriteINI("SettingForm1", "Width", numericUpDown1.Value.ToString());
            this.Height = int.Parse(numericUpDown1.Value.ToString());
            this.Width = int.Parse(numericUpDown2.Value.ToString());
            MessageBox.Show("Настройки сохранены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); // Говорим пользователю, что сохранили конфиг.
        }
    }
}
