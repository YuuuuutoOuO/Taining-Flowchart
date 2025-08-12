using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Taining.Function; // 假設 NodeData 在這

namespace Taining
{
    public partial class EditNodeForm : Form
    {
        public NodeData UpdatedNode { get; private set; }

        // 支援所有 StepId 下拉清單
        public EditNodeForm(NodeData node, List<string> allStepIds)
        {
            InitializeComponent();
            txtStepId.Text = node.StepId;
            txtStepId.ReadOnly = true;
            txtDescription.Text = node.Description;
            txtProcessId.Text = node.ProcessId;

            // 設定 ComboBox 下拉選單
            if (txtNextStepId is ComboBox cb)
            {
                cb.Items.Clear();
                cb.Items.AddRange(allStepIds.Where(id => id != node.StepId).ToArray());
            }
            txtNextStepId.Text = node.NextStepId;
            txtShapeType.Text = node.ShapeType;
            txtTime.Text = node.Time;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool hasError = false;
            txtDescription_error.Visible = false;
            txtShapeType_error.Visible = false;
            txtTime_error.Visible = false;

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

            UpdatedNode = new NodeData
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

        private void EditNodeForm_Load(object sender, EventArgs e)
        {
            txtStepId_error.Visible = false;
            txtDescription_error.Visible = false;
            txtShapeType_error.Visible = false;
            txtTime_error.Visible = false;
        }
    }
}
