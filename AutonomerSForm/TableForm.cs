using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AutonomerSForm
{
    public partial class TableForm : Form
    {
        private readonly SqlServerShell _sqlShell;
        private readonly List<TableRecordControl> _recordControls = new List<TableRecordControl>();

        public TableForm()
        {
            InitializeComponent();
        }

        public TableForm(SqlServerSettings sqlServerSettings) : this()
        {
            _sqlShell = new SqlServerShell(sqlServerSettings, Constants.ModuleInfo, Constants.DbName);
            TryUpdateTable(); 
        }

        private void UpdateTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TryUpdateTable();
        }

        private void TryUpdateTable()
        {
            TryCatch(() =>
            {
                var records = _sqlShell.GetArrayOf<Record>(Record.TableName);
                AddRecordsControls(records);
            }, "Не удалось получить или добавить записи в таблице в БД", true);
        }

        private void ExecuteScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TryCatch(() =>
            {
                ExecuteScript();
            }, $"Ошибка во время выполнения скрипта", true);
        }

        private void ExecuteScript()
        {
            var pyScriptFile = new FileInfo(Constants.PathToPythonScript);
            if (!pyScriptFile.Exists)
                throw new FileNotFoundException($"Не найден скрипт по пути: {pyScriptFile.FullName}");

            var exitCode = Extensions.StartProcess(pyScriptFile.FullName, pyScriptFile.Directory.FullName, "python ");
            var exitCodeString = $"Код завершения: {exitCode}";
            if (exitCode == 0)
                ShowInfoBox($"Скрипт '{pyScriptFile.Name}' успешно исполнен. {exitCodeString}");
            else
                ShowWarningBox($"Скрипт '{pyScriptFile.Name}' отдал не успешный код завершения. {exitCodeString}");
        }

        public void AddRecordsControls(Record[] sortedRecords)
        {
            TryCatch(() =>
            {
                panelTable.Controls.Clear();
                _recordControls.Clear();

                var height = new TableRecordControl().Size.Height;
                for (int i = 0; i < sortedRecords.Length; i++)
                {
                    var newRecordControl = new TableRecordControl(sortedRecords[i]);
                    if (i == 0)
                        newRecordControl.Location = new Point(0, (i * height));
                    else
                        newRecordControl.Location = new Point(0, (i * height) + 1);
                    newRecordControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                    _recordControls.Insert(0, newRecordControl);
                    panelTable.Controls.Add(newRecordControl);
                }

                labelRecordsCount.Text = _recordControls.Count.ToString();
            }, "Ошибка во время обновления таблицы", true);
        }

        private void CloseAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        protected void ShowWarningBox(string text, string header = "Ошибка во время выполнения команды"
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

        protected bool StringEquals(string left, string right)
        {
            return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
        }
    }
}
