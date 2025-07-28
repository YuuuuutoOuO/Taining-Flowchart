using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Msagl.GraphViewerGdi;
using Taining.Function;

namespace Taining
{
    public static class CvFlowchartMenu
    {
        // 變更連線
        public static void ChangeEdge(List<NodeData> nodeList, string selectedNodeId, GViewer cv_flowchart)
        {
            var selectForm = new SelectTargetForm(nodeList, selectedNodeId);
            if (selectForm.ShowDialog() == DialogResult.OK)
            {
                string newTargetId = selectForm.SelectedTargetId;
                var node = nodeList.FirstOrDefault(n => n.StepId == selectedNodeId);
                if (node != null && !string.IsNullOrEmpty(newTargetId))
                {
                    node.NextStepId = newTargetId;
                }
                var json = System.Text.Json.JsonSerializer.Serialize(nodeList);
                Painting.DrawFlowChart(json, cv_flowchart);
            }
        }

        // 刪除節點
        public static void DeleteNode(List<NodeData> nodeList, string selectedNodeId, GViewer cv_flowchart)
        {
            if (string.IsNullOrEmpty(selectedNodeId)) return;
            var nodeToDelete = nodeList.FirstOrDefault(n => n.StepId == selectedNodeId);
            if (nodeToDelete == null) return;
            if (nodeToDelete.StepId == "Start" || nodeToDelete.StepId == "End")
            {
                MessageBox.Show("起始/結束節點不可刪除！");
                return;
            }
            var result = MessageBox.Show(
                $"確定要刪除節點 [{nodeToDelete.StepId}] 嗎？",
                "刪除節點確認",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;
            nodeList.Remove(nodeToDelete);
            foreach (var n in nodeList)
            {
                if (!string.IsNullOrEmpty(n.NextStepId))
                {
                    var targets = n.NextStepId
                        .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Where(id => id != selectedNodeId)
                        .ToArray();
                    n.NextStepId = string.Join(",", targets);
                }
            }
            var json = System.Text.Json.JsonSerializer.Serialize(nodeList);
            Painting.DrawFlowChart(json, cv_flowchart);
        }

        // 編輯節點
        public static void EditNode(List<NodeData> nodeList, string selectedNodeId, GViewer cv_flowchart)
        {
            var node = nodeList.FirstOrDefault(n => n.StepId == selectedNodeId);
            if (node == null) return;

            // 傳所有 StepId（除了自己）給 EditNodeForm
            var allStepIds = nodeList.Select(n => n.StepId).Where(id => id != selectedNodeId).ToList();
            var editForm = new EditNodeForm(node, allStepIds);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                var updated = editForm.UpdatedNode;
                node.Description = updated.Description;
                node.ProcessId = updated.ProcessId;
                node.NextStepId = updated.NextStepId;
                node.ShapeType = updated.ShapeType;
                node.AltText = updated.AltText;
                node.Time = updated.Time;
                node.TotalTime = updated.TotalTime;

                var json = System.Text.Json.JsonSerializer.Serialize(nodeList);
                Painting.DrawFlowChart(json, cv_flowchart);
            }
        }

        // 新增節點
        public static void CreateNode(List<NodeData> nodeList, GViewer cv_flowchart)
        {
            // 傳目前所有 StepId 給 AddNodeForm
            var allStepIds = nodeList.Select(n => n.StepId).ToList();
            var addNodeForm = new AddNodeForm(allStepIds);
            if (addNodeForm.ShowDialog() == DialogResult.OK)
            {
                var newNode = addNodeForm.NewNode;
                if (nodeList.Any(n => n.StepId == newNode.StepId))
                {
                    MessageBox.Show("步驟識別碼已存在，請重新輸入。");
                    return;
                }
                nodeList.Add(newNode);
                var json = System.Text.Json.JsonSerializer.Serialize(nodeList);
                Painting.DrawFlowChart(json, cv_flowchart);
            }
        }
    }
}
