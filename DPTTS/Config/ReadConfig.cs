using System.Windows.Forms;

namespace DPTTS.Config
{
    public static class ConfigReader
    {
        public static void Config_Read()
        {
            IniFile INI = new IniFile("config.ini");
            if (INI.KeyExists("DPTTS", "Token"))
                numericUpDown2.Value = int.Parse(INI.ReadINI("SettingForm1", "Height"));
            else
                numericUpDown1.Value = this.MinimumSize.Height;

            if (INI.KeyExists("SettingForm1", "Width"))
                textBox1.Text = INI.ReadINI("Other", "Text");

            this.Height = int.Parse(numericUpDown1.Value.ToString());
            this.Width = int.Parse(numericUpDown2.Value.ToString());

            MessageBox.Show("Настройки загружены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); // Говорим пользователю, что загрузили конфиг.
        }
    }
}
