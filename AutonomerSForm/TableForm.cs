﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            _sqlShell = new SqlServerShell(sqlServerSettings, Constants.ModuleInfo, null);
            if (_sqlShell.IsDbExist(Constants.DbName))
            {
                _sqlShell.DbName = Constants.DbName;
            }
            else
            {
                var genScript = Extensions.ReadFile(Constants.PathToGenerateDbScript);
                _sqlShell.CreateDb(genScript, Constants.DbName);
            }

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
                const string orderBy = "ORDER BY " + nameof(Record.Date) + " DESC";
                var records = _sqlShell.GetArrayOf<Record>(Record.TableName, orderBy: orderBy);
                AddRecordsControls(records);
            }, "Не удалось получить или добавить записи в таблице в БД", true);
        }

        private void ExecuteScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var runScroptForm = new RunPyScriptForm();
            runScroptForm.ShowDialog();

            TryUpdateTable();
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
                    newRecordControl.Location = new Point(0, (i * height));
                    newRecordControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

                    _recordControls.Insert(0, newRecordControl);
                    panelTable.Controls.Add(newRecordControl);
                }

                labelRecordsCount.Text = _recordControls.Count.ToString();
                labelEstimatedCarsCount.Text = sortedRecords.GroupBy(x => x.CarNumber)
                    .Where(x => x.Count() % 2 == 1)
                    .Count()
                    .ToString();
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
    }
}
