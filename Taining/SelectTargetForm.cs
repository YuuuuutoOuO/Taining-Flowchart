using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Taining.Function; // 如果 NodeData 在這個命名空間

namespace Taining
{
    public partial class SelectTargetForm : Form
    {
        // 用來記錄目前可選節點（不包含自己）
        private List<NodeData> comboSource = new List<NodeData>();

        // 給外部取得選到哪個 StepId
        public string SelectedTargetId
        {
            get
            {
                if (comboBox1.SelectedIndex >= 0 && comboBox1.SelectedIndex < comboSource.Count)
                    return comboSource[comboBox1.SelectedIndex].StepId;
                return null;
            }
        }

        public SelectTargetForm(List<NodeData> allNodes, string excludeId)
        {
            InitializeComponent();

            // 排除自己
            comboSource = allNodes.FindAll(n => n.StepId != excludeId);

            // ComboBox 顯示 "StepId - Description"
            comboBox1.Items.Clear();
            foreach (var n in comboSource)
                comboBox1.Items.Add($"{n.StepId} - {n.Description}");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
                this.DialogResult = DialogResult.OK;
            else
                MessageBox.Show("請選擇一個目標節點！");
        }

        private void SelectTargetForm_Load(object sender, EventArgs e)
        {
            // 可以留空或加初始處理
        }
    }
}
