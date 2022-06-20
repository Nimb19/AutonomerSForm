using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AutonomerSForm
{
    public partial class TableRecordControl : UserControl
    {
        public event EventHandler<Record> RecordChanged;

        private SqlServerShell _sqlServerShell;
        private Record _record = null;
        public Record Record
        {
            get
            {
                return _record;
            }
            private set
            {
                SetRecordInfo(value);
            }
        }


        public TableRecordControl()
        {
            InitializeComponent();
        }

        public TableRecordControl(Record record, SqlServerShell sqlServerShell) : this()
        {
            SetRecordInfo(record);
            _sqlServerShell = sqlServerShell;
        }

        private void SetRecordInfo(Record record)
        {
            _record = record;

            textBoxUid.Text = _record.Uid.ToString();
            textBoxDate.Text = _record.Date.ToString("F");
            textBoxCarNumber.Text = _record.CarNumber;

            using (var ms = new MemoryStream(_record.Image))
            {
                pictureBoxImage.Image = Image.FromStream(ms);
            }
        }

        private void ButtonChange_Click(object sender, System.EventArgs e)
        {
            textBoxCarNumber.ReadOnly = false;
            buttonSave.Visible = true;
            buttonChange.Visible = false;
        }

        private void ButtonSave_Click(object sender, System.EventArgs e)
        {
            var carNumberText = textBoxCarNumber.Text.Trim();
            if (string.IsNullOrEmpty(carNumberText))
            {
                ShowWarningBox("Пустое поле для логина.");
                return;
            }

            buttonChange.Visible = true;
            buttonSave.Visible = false;
            textBoxCarNumber.ReadOnly = true;

            _sqlServerShell.UpdateCell(nameof(Record.CarNumber), carNumberText
                    , $"WHERE {nameof(Record.Uid)} = '{Record.Uid}'", Record.TableName);

            RecordChanged?.Invoke(this, Record);
        }

        protected void ShowWarningBox(string text, string header = "Ошибка во время проверки значений"
            , bool isError = false)
        {
            MessageBox.Show(text, header, MessageBoxButtons.OK
                , isError ? MessageBoxIcon.Error : MessageBoxIcon.Warning);
        }
    }
}
