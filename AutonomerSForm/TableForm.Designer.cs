namespace AutonomerSForm
{
    partial class TableForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.действияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTable = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelRecordsCount = new System.Windows.Forms.Label();
            this.labelEstimatedCarsCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonFindByKeyWord = new System.Windows.Forms.Button();
            this.textBoxKeyWord = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelKeyWordsCounter = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.menuStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.действияToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(949, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // действияToolStripMenuItem
            // 
            this.действияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateTableToolStripMenuItem,
            this.executeScriptToolStripMenuItem,
            this.closeAppToolStripMenuItem});
            this.действияToolStripMenuItem.Name = "действияToolStripMenuItem";
            this.действияToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.действияToolStripMenuItem.Text = "Действия";
            // 
            // updateTableToolStripMenuItem
            // 
            this.updateTableToolStripMenuItem.Name = "updateTableToolStripMenuItem";
            this.updateTableToolStripMenuItem.Size = new System.Drawing.Size(345, 22);
            this.updateTableToolStripMenuItem.Text = "Обновить таблицу";
            this.updateTableToolStripMenuItem.Click += new System.EventHandler(this.UpdateTableToolStripMenuItem_Click);
            // 
            // executeScriptToolStripMenuItem
            // 
            this.executeScriptToolStripMenuItem.Name = "executeScriptToolStripMenuItem";
            this.executeScriptToolStripMenuItem.Size = new System.Drawing.Size(345, 22);
            this.executeScriptToolStripMenuItem.Text = "Запустить скрипт по обработке последних видео";
            this.executeScriptToolStripMenuItem.Click += new System.EventHandler(this.ExecuteScriptToolStripMenuItem_Click);
            // 
            // closeAppToolStripMenuItem
            // 
            this.closeAppToolStripMenuItem.Name = "closeAppToolStripMenuItem";
            this.closeAppToolStripMenuItem.Size = new System.Drawing.Size(345, 22);
            this.closeAppToolStripMenuItem.Text = "Закрыть программу";
            this.closeAppToolStripMenuItem.Click += new System.EventHandler(this.CloseAppToolStripMenuItem_Click);
            // 
            // panelTable
            // 
            this.panelTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTable.AutoScroll = true;
            this.panelTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTable.Location = new System.Drawing.Point(12, 139);
            this.panelTable.Name = "panelTable";
            this.panelTable.Size = new System.Drawing.Size(925, 583);
            this.panelTable.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Таблица с записями:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Всего распознано:";
            // 
            // labelRecordsCount
            // 
            this.labelRecordsCount.Location = new System.Drawing.Point(319, 44);
            this.labelRecordsCount.Name = "labelRecordsCount";
            this.labelRecordsCount.Size = new System.Drawing.Size(51, 20);
            this.labelRecordsCount.TabIndex = 7;
            this.labelRecordsCount.Text = "0";
            this.labelRecordsCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelEstimatedCarsCount
            // 
            this.labelEstimatedCarsCount.Location = new System.Drawing.Point(319, 72);
            this.labelEstimatedCarsCount.Name = "labelEstimatedCarsCount";
            this.labelEstimatedCarsCount.Size = new System.Drawing.Size(51, 20);
            this.labelEstimatedCarsCount.TabIndex = 9;
            this.labelEstimatedCarsCount.Text = "0";
            this.labelEstimatedCarsCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(305, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Предположительно машин на стоянке:";
            // 
            // buttonFindByKeyWord
            // 
            this.buttonFindByKeyWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFindByKeyWord.Location = new System.Drawing.Point(17, 60);
            this.buttonFindByKeyWord.Name = "buttonFindByKeyWord";
            this.buttonFindByKeyWord.Size = new System.Drawing.Size(307, 32);
            this.buttonFindByKeyWord.TabIndex = 13;
            this.buttonFindByKeyWord.Text = "Найти по ключевому слову";
            this.buttonFindByKeyWord.UseVisualStyleBackColor = true;
            this.buttonFindByKeyWord.Click += new System.EventHandler(this.ButtonFindByKeyWord_Click);
            // 
            // textBoxKeyWord
            // 
            this.textBoxKeyWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxKeyWord.Location = new System.Drawing.Point(17, 26);
            this.textBoxKeyWord.Name = "textBoxKeyWord";
            this.textBoxKeyWord.Size = new System.Drawing.Size(448, 26);
            this.textBoxKeyWord.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(330, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 14;
            this.label3.Text = "Найдено:";
            // 
            // labelKeyWordsCounter
            // 
            this.labelKeyWordsCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelKeyWordsCounter.AutoSize = true;
            this.labelKeyWordsCounter.Location = new System.Drawing.Point(448, 66);
            this.labelKeyWordsCounter.Name = "labelKeyWordsCounter";
            this.labelKeyWordsCounter.Size = new System.Drawing.Size(17, 20);
            this.labelKeyWordsCounter.TabIndex = 15;
            this.labelKeyWordsCounter.Text = "0";
            this.labelKeyWordsCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.textBoxKeyWord);
            this.groupBox1.Controls.Add(this.labelKeyWordsCounter);
            this.groupBox1.Controls.Add(this.buttonFindByKeyWord);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(455, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(482, 106);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Поиск записей";
            // 
            // TableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(949, 734);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelEstimatedCarsCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelRecordsCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panelTable);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TableForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Просмотр списка записей";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem действияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateTableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executeScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAppToolStripMenuItem;
        private System.Windows.Forms.Panel panelTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelRecordsCount;
        private System.Windows.Forms.Label labelEstimatedCarsCount;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Button buttonFindByKeyWord;
        private System.Windows.Forms.TextBox textBoxKeyWord;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelKeyWordsCounter;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

