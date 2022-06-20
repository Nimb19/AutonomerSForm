using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace AutonomerSForm
{
    public partial class RunPyScriptForm : Form
    {
        public RunPyScriptForm()
        {
            InitializeComponent();
        }

        private void ButtonExecuteScript_Click(object sender, EventArgs e)
        {
            TryCatch(() =>
            {
                var videoPath = new FileInfo(textBoxServicePath.Text.Trim());
                if (!videoPath.Exists)
                {
                    ShowWarningBox($"Не существует видео по указанному пути: '{videoPath.FullName}'");
                    return;
                }

                TryCatch(() =>
                {
                    ExecuteScript(videoPath);
                }, $"Ошибка во время выполнения скрипта", true);
            }, $"Ошибка во время взятия значений для выполнения скрипта");
        }

        private void ExecuteScript(FileInfo video)
        {
            var pyScriptFile = new FileInfo(Constants.PathToPythonScript);
            if (!pyScriptFile.Exists)
                throw new FileNotFoundException($"Не найден скрипт по пути: {pyScriptFile.FullName}");

            var textBoxLogger = new TextBoxLogger(textBoxLog, SynchronizationContext.Current);
            textBoxLog.Clear();

            var args = $"{pyScriptFile.FullName} /video:{video.FullName}";
            var exitCode = Extensions.StartProcess(textBoxLogger, args, pyScriptFile.Directory.FullName, "python ");
            
            var exitCodeString = $"Код завершения: {exitCode}";
            if (exitCode == 0)
                ShowInfoBox($"Скрипт '{pyScriptFile.Name}' успешно исполнен, окно можно закрывать. {exitCodeString}");
            else
                ShowWarningBox($"Скрипт '{pyScriptFile.Name}' отдал не успешный код завершения, окно можно закрывать. {exitCodeString}");
        }

        private void ButtonSelectVideoPath_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog()
            {
                Title = $"Выберите видео, которое хотите обработать",
                CheckFileExists = true,
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxServicePath.Text = fileDialog.FileName;
            }
        }

        protected void ShowWarningBox(string text, string header = "Путь был введён не верно или он не был указан"
            , bool isError = false)
        {
            MessageBox.Show(text, header, MessageBoxButtons.OK
                , isError ? MessageBoxIcon.Error : MessageBoxIcon.Warning);
        }

        protected void ShowInfoBox(string text, string header = "Информация")
        {
            MessageBox.Show(text, header, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected bool TryCatch(Action action, string errHeader = null, bool isErr = false)
        {
            try
            {
                action.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                if (errHeader != null)
                    ShowWarningBox(ex.ToString(), errHeader, isErr);
                else
                    ShowWarningBox(ex.ToString(), isError: isErr);
                return false;
            }
        }
    }
}
