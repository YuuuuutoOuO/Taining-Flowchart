public partial class SelectTargetForm : Form
{
    public string SelectedTargetId => comboBox1.SelectedItem?.ToString();

    public SelectTargetForm(List<NodeData> allNodes, string excludeId)
    {
        InitializeComponent();
        // 不要讓自己指向自己
        comboBox1.Items.AddRange(allNodes.Where(n => n.StepId != excludeId).Select(n => n.StepId).ToArray());
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
        if (comboBox1.SelectedIndex >= 0)
            this.DialogResult = DialogResult.OK;
        else
            MessageBox.Show("請選擇一個目標節點！");
    }
}
