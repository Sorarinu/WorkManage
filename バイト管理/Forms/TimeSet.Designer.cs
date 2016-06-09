namespace WorkManage
{
    partial class TimeSet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeSet));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbWST_h = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbWST_m = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbWET_m = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbWET_h = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbBST_m = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbBST_h = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbBET_m = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbBET_h = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(252, 322);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "登録";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "勤務開始時間";
            // 
            // cmbWST_h
            // 
            this.cmbWST_h.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbWST_h.FormattingEnabled = true;
            this.cmbWST_h.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.cmbWST_h.Location = new System.Drawing.Point(116, 15);
            this.cmbWST_h.MaxLength = 2;
            this.cmbWST_h.Name = "cmbWST_h";
            this.cmbWST_h.Size = new System.Drawing.Size(66, 24);
            this.cmbWST_h.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(188, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "時";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(297, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "分";
            // 
            // cmbWST_m
            // 
            this.cmbWST_m.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbWST_m.FormattingEnabled = true;
            this.cmbWST_m.Items.AddRange(new object[] {
            "00",
            "15",
            "30",
            "45"});
            this.cmbWST_m.Location = new System.Drawing.Point(225, 15);
            this.cmbWST_m.MaxLength = 2;
            this.cmbWST_m.Name = "cmbWST_m";
            this.cmbWST_m.Size = new System.Drawing.Size(66, 24);
            this.cmbWST_m.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(297, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "分";
            // 
            // cmbWET_m
            // 
            this.cmbWET_m.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbWET_m.FormattingEnabled = true;
            this.cmbWET_m.Items.AddRange(new object[] {
            "00",
            "15",
            "30",
            "45"});
            this.cmbWET_m.Location = new System.Drawing.Point(225, 44);
            this.cmbWET_m.MaxLength = 2;
            this.cmbWET_m.Name = "cmbWET_m";
            this.cmbWET_m.Size = new System.Drawing.Size(66, 24);
            this.cmbWET_m.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(188, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "時";
            // 
            // cmbWET_h
            // 
            this.cmbWET_h.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbWET_h.FormattingEnabled = true;
            this.cmbWET_h.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.cmbWET_h.Location = new System.Drawing.Point(116, 44);
            this.cmbWET_h.MaxLength = 2;
            this.cmbWET_h.Name = "cmbWET_h";
            this.cmbWET_h.Size = new System.Drawing.Size(66, 24);
            this.cmbWET_h.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "勤務終了時間";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(297, 81);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 19;
            this.label10.Text = "分";
            // 
            // cmbBST_m
            // 
            this.cmbBST_m.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbBST_m.FormattingEnabled = true;
            this.cmbBST_m.Items.AddRange(new object[] {
            "00",
            "15",
            "30",
            "45"});
            this.cmbBST_m.Location = new System.Drawing.Point(225, 74);
            this.cmbBST_m.MaxLength = 2;
            this.cmbBST_m.Name = "cmbBST_m";
            this.cmbBST_m.Size = new System.Drawing.Size(66, 24);
            this.cmbBST_m.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(188, 81);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 17;
            this.label11.Text = "時";
            // 
            // cmbBST_h
            // 
            this.cmbBST_h.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbBST_h.FormattingEnabled = true;
            this.cmbBST_h.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.cmbBST_h.Location = new System.Drawing.Point(116, 74);
            this.cmbBST_h.MaxLength = 2;
            this.cmbBST_h.Name = "cmbBST_h";
            this.cmbBST_h.Size = new System.Drawing.Size(66, 24);
            this.cmbBST_h.TabIndex = 16;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 81);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 15;
            this.label12.Text = "休憩開始時間";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(297, 112);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 12);
            this.label14.TabIndex = 26;
            this.label14.Text = "分";
            // 
            // cmbBET_m
            // 
            this.cmbBET_m.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbBET_m.FormattingEnabled = true;
            this.cmbBET_m.Items.AddRange(new object[] {
            "00",
            "15",
            "30",
            "45"});
            this.cmbBET_m.Location = new System.Drawing.Point(225, 105);
            this.cmbBET_m.MaxLength = 2;
            this.cmbBET_m.Name = "cmbBET_m";
            this.cmbBET_m.Size = new System.Drawing.Size(66, 24);
            this.cmbBET_m.TabIndex = 25;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(188, 112);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 12);
            this.label15.TabIndex = 24;
            this.label15.Text = "時";
            // 
            // cmbBET_h
            // 
            this.cmbBET_h.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbBET_h.FormattingEnabled = true;
            this.cmbBET_h.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.cmbBET_h.Location = new System.Drawing.Point(116, 105);
            this.cmbBET_h.MaxLength = 2;
            this.cmbBET_h.Name = "cmbBET_h";
            this.cmbBET_h.Size = new System.Drawing.Size(66, 24);
            this.cmbBET_h.TabIndex = 23;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(22, 112);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 12);
            this.label16.TabIndex = 22;
            this.label16.Text = "休憩終了時間";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(35, 282);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(256, 24);
            this.label17.TabIndex = 29;
            this.label17.Text = "※ 休憩時間が無かった場合\r\n　　開始時間と終了時間に「00:00」を指定してください";
            // 
            // txtMemo
            // 
            this.txtMemo.Location = new System.Drawing.Point(24, 170);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new System.Drawing.Size(290, 99);
            this.txtMemo.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 12);
            this.label4.TabIndex = 31;
            this.label4.Text = "【メモ】";
            // 
            // TimeSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(335, 353);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMemo);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cmbBET_m);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.cmbBET_h);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbBST_m);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cmbBST_h);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbWET_m);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbWET_h);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbWST_m);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbWST_h);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimeSet";
            this.Text = "勤務時間登録";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TimeSet_FormClosed);
            this.Load += new System.EventHandler(this.TimeSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbWST_h;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbWST_m;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbWET_m;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbWET_h;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbBST_m;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbBST_h;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbBET_m;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbBET_h;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.Label label4;
    }
}