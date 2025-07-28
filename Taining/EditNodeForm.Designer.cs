namespace Taining
{
    partial class EditNodeForm
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
            txtShapeType_error = new Label();
            txtDescription_error = new Label();
            txtStepId_error = new Label();
            btnOK = new Button();
            label8 = new Label();
            txtShapeType = new ComboBox();
            txtTime = new TextBox();
            label7 = new Label();
            label5 = new Label();
            txtProcessId = new TextBox();
            label3 = new Label();
            txtDescription = new TextBox();
            label2 = new Label();
            txtStepId = new TextBox();
            txtTime_error = new Label();
            label1 = new Label();
            txtNextStepId = new ComboBox();
            SuspendLayout();
            // 
            // txtShapeType_error
            // 
            txtShapeType_error.AutoSize = true;
            txtShapeType_error.ForeColor = Color.Red;
            txtShapeType_error.Location = new Point(109, 176);
            txtShapeType_error.Name = "txtShapeType_error";
            txtShapeType_error.Size = new Size(22, 15);
            txtShapeType_error.TabIndex = 35;
            txtShapeType_error.Text = "---";
            // 
            // txtDescription_error
            // 
            txtDescription_error.AutoSize = true;
            txtDescription_error.ForeColor = Color.Red;
            txtDescription_error.Location = new Point(109, 62);
            txtDescription_error.Name = "txtDescription_error";
            txtDescription_error.Size = new Size(22, 15);
            txtDescription_error.TabIndex = 34;
            txtDescription_error.Text = "---";
            // 
            // txtStepId_error
            // 
            txtStepId_error.AutoSize = true;
            txtStepId_error.ForeColor = Color.Red;
            txtStepId_error.Location = new Point(109, 9);
            txtStepId_error.Name = "txtStepId_error";
            txtStepId_error.Size = new Size(22, 15);
            txtStepId_error.TabIndex = 33;
            txtStepId_error.Text = "---";
            // 
            // btnOK
            // 
            btnOK.Location = new Point(12, 370);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(243, 23);
            btnOK.TabIndex = 32;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 9);
            label8.Name = "label8";
            label8.Size = new Size(91, 15);
            label8.TabIndex = 31;
            label8.Text = "程序步驟識別碼";
            // 
            // txtShapeType
            // 
            txtShapeType.FormattingEnabled = true;
            txtShapeType.Items.AddRange(new object[] { "程序", "子程序", "物件" });
            txtShapeType.Location = new Point(12, 194);
            txtShapeType.Name = "txtShapeType";
            txtShapeType.Size = new Size(240, 23);
            txtShapeType.TabIndex = 29;
            // 
            // txtTime
            // 
            txtTime.Location = new Point(12, 253);
            txtTime.Name = "txtTime";
            txtTime.Size = new Size(240, 23);
            txtTime.TabIndex = 28;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 235);
            label7.Name = "label7";
            label7.Size = new Size(55, 15);
            label7.TabIndex = 27;
            label7.Text = "執行時間";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 176);
            label5.Name = "label5";
            label5.Size = new Size(31, 15);
            label5.TabIndex = 26;
            label5.Text = "類型";
            // 
            // txtProcessId
            // 
            txtProcessId.Location = new Point(12, 134);
            txtProcessId.Name = "txtProcessId";
            txtProcessId.Size = new Size(240, 23);
            txtProcessId.TabIndex = 25;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 116);
            label3.Name = "label3";
            label3.Size = new Size(31, 15);
            label3.TabIndex = 24;
            label3.Text = "編號";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(12, 80);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(240, 23);
            txtDescription.TabIndex = 23;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 62);
            label2.Name = "label2";
            label2.Size = new Size(79, 15);
            label2.TabIndex = 22;
            label2.Text = "程序步驟描述";
            // 
            // txtStepId
            // 
            txtStepId.Location = new Point(12, 27);
            txtStepId.Name = "txtStepId";
            txtStepId.Size = new Size(240, 23);
            txtStepId.TabIndex = 21;
            // 
            // txtTime_error
            // 
            txtTime_error.AutoSize = true;
            txtTime_error.ForeColor = Color.Red;
            txtTime_error.Location = new Point(109, 235);
            txtTime_error.Name = "txtTime_error";
            txtTime_error.Size = new Size(22, 15);
            txtTime_error.TabIndex = 36;
            txtTime_error.Text = "---";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 289);
            label1.Name = "label1";
            label1.Size = new Size(103, 15);
            label1.TabIndex = 39;
            label1.Text = "下一個步驟識別碼";
            // 
            // txtNextStepId
            // 
            txtNextStepId.FormattingEnabled = true;
            txtNextStepId.Items.AddRange(new object[] { "程序", "子程序", "物件" });
            txtNextStepId.Location = new Point(12, 307);
            txtNextStepId.Name = "txtNextStepId";
            txtNextStepId.Size = new Size(240, 23);
            txtNextStepId.TabIndex = 40;
            // 
            // EditNodeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(266, 412);
            Controls.Add(txtNextStepId);
            Controls.Add(label1);
            Controls.Add(txtTime_error);
            Controls.Add(txtShapeType_error);
            Controls.Add(txtDescription_error);
            Controls.Add(txtStepId_error);
            Controls.Add(btnOK);
            Controls.Add(label8);
            Controls.Add(txtShapeType);
            Controls.Add(txtTime);
            Controls.Add(label7);
            Controls.Add(label5);
            Controls.Add(txtProcessId);
            Controls.Add(label3);
            Controls.Add(txtDescription);
            Controls.Add(label2);
            Controls.Add(txtStepId);
            Name = "EditNodeForm";
            Text = "EditNodeForm";
            Load += EditNodeForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label txtShapeType_error;
        private Label txtDescription_error;
        private Label txtStepId_error;
        private Button btnOK;
        private Label label8;
        private ComboBox txtShapeType;
        private TextBox txtTime;
        private Label label7;
        private Label label5;
        private TextBox txtProcessId;
        private Label label3;
        private TextBox txtDescription;
        private Label label2;
        private TextBox txtStepId;
        private Label txtTime_error;
        private Label label1;
        private ComboBox txtNextStepId;
    }
}