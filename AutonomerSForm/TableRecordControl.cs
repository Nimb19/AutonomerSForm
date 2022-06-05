using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AutonomerSForm
{
    public partial class TableRecordControl : UserControl
    {
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

        public TableRecordControl(Record record) : this()
        {
            SetRecordInfo(record);
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
    }
}
