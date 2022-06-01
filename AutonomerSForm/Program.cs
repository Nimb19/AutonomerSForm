using System;
using System.Windows.Forms;

namespace AutonomerSForm
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Config config = ReadConfig();
            Application.Run(new TableForm(config.SqlServerSettings));
        }

        private static Config ReadConfig()
        {
            try
            {
                return Extensions.DeserializeFile<Config>(Constants.PathToConfig);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка во время чтения конфига", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
                return null; // Сюда не дойдём никогда
            }
        }
    }
}
