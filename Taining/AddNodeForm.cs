using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Taining.Function;

namespace Taining
{
    public partial class AddNodeForm : Form
    {
        public NodeData NewNode { get; private set; }

        // 建構子允許帶入目前畫布所有 StepId
        public AddNodeForm(List<string> allStepIds = null)
        {
            InitializeComponent();

            // 設定 ComboBox
            if (allStepIds != null && txtNextStepId is ComboBox cb)
            {
                cb.Items.Clear();
                cb.Items.AddRange(allStepIds.ToArray());
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool hasError = false;
            txtStepId_error.Visible = false;
            txtDescription_error.Visible = false;
            txtShapeType_error.Visible = false;
            txtTime_error.Visible = false;

            if (string.IsNullOrWhiteSpace(txtStepId.Text))
            {
                txtStepId_error.Text = "步驟識別碼不得為空";
                txtStepId_error.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                txtDescription_error.Text = "程序步驟描述不得為空";
                txtDescription_error.Visible = true;
                hasError = true;
            }
            if (string.IsNullOrWhiteSpace(txtShapeType.Text))
            {
                txtShapeType_error.Text = "類型不得為空";
                txtShapeType_error.Visible = true;
                hasError = true;
            }
            if (!string.IsNullOrWhiteSpace(txtTime.Text) && !double.TryParse(txtTime.Text, out _))
            {
                txtTime_error.Text = "必須為空或是輸入數字";
                txtTime_error.Visible = true;
                hasError = true;
            }
            if (hasError) return;

            NewNode = new NodeData
            {
                StepId = txtStepId.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                ProcessId = txtProcessId.Text.Trim(),
                NextStepId = txtNextStepId.Text.Trim(),
                ShapeType = txtShapeType.Text.Trim(),
                Time = txtTime.Text.Trim(),
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AddNodeForm_Load(object sender, EventArgs e)
        {
            txtStepId_error.Visible = false;
            txtDescription_error.Visible = false;
            txtShapeType_error.Visible = false;
            txtTime_error.Visible = false;
        }
    }
}
