using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Taining.Function
{
    public static class Painting
    {
        public static void DrawFlowChart(string json, GViewer gViewer)
        {
            try
            {
                var oldPositions = GetOldPositions(gViewer);
                var nodesRaw = System.Text.Json.JsonSerializer.Deserialize<List<NodeData>>(json);
                UpdateTotalTime.Update(nodesRaw);

                var graph = new Graph("流程圖");
                graph.Attr.LayerDirection = LayerDirection.LR;

                AddNodesByContains(graph, nodesRaw, "起始", Microsoft.Msagl.Drawing.Color.LightBlue);
                AddNodesByContains(graph, nodesRaw, "線上", Microsoft.Msagl.Drawing.Color.LightGreen);
                AddNodesByContains(graph, nodesRaw, "預裝", Microsoft.Msagl.Drawing.Color.Yellow);
                AddNodesByContains(graph, nodesRaw, "選配", Microsoft.Msagl.Drawing.Color.White);
                AddNodes_OthersAsRed(graph, nodesRaw);

                AddInvisibleMainEdges(graph, nodesRaw);
                AddAllEdges(graph, nodesRaw);

                // 判斷是否所有節點都有座標或舊位置
                bool allHavePositions = nodesRaw.All(nd =>
                    (!string.IsNullOrWhiteSpace(nd.X) && !string.IsNullOrWhiteSpace(nd.Y)) ||
                    oldPositions.ContainsKey(nd.StepId)
                );

                if (allHavePositions)
                {
                    // 全部有座標 → 不跑自動 layout
                    gViewer.NeedToCalculateLayout = false;
                    gViewer.Graph = graph;
                    SetNodePositions(gViewer.Graph, nodesRaw, oldPositions);
                }
                else
                {
                    // 有缺座標 → 跑一次 layout，讓沒座標的節點自動排
                    gViewer.NeedToCalculateLayout = true;
                    gViewer.Graph = graph;
                    // 再套用已知座標（覆蓋 layout 的部分節點）
                    SetNodePositions(gViewer.Graph, nodesRaw, oldPositions);
                }

                gViewer.Invalidate();
            }
            catch
            {
                gViewer.Graph = new Microsoft.Msagl.Drawing.Graph();
                gViewer.Invalidate();
            }
        }

        private static Dictionary<string, Microsoft.Msagl.Core.Geometry.Point> GetOldPositions(GViewer gViewer)
        {
            var oldPositions = new Dictionary<string, Microsoft.Msagl.Core.Geometry.Point>();
            if (gViewer.Graph != null)
            {
                foreach (var n in gViewer.Graph.Nodes)
                {
                    if (n.GeometryNode != null)
                        oldPositions[n.Id] = n.GeometryNode.Center;
                }
            }
            return oldPositions;
        }

        private static void AddNodesByContains(Graph graph, List<NodeData> nodesRaw, string keyword, Microsoft.Msagl.Drawing.Color color)
        {
            var typeNodes = nodesRaw.Where(n => (n.ShapeType ?? "").Contains(keyword)).ToList();
            foreach (var n in typeNodes)
            {
                var node = graph.AddNode(n.StepId);
                node.LabelText = (n.StepId == "Start" || n.StepId == "End")
                    ? n.Description
                    : Function.GetText.GetNodeText(n);
                node.Attr.FillColor = color;
                node.Attr.Shape = Shape.Box;
            }
        }

        private static void AddNodes_OthersAsRed(Graph graph, List<NodeData> nodesRaw)
        {
            var knownTypes = new HashSet<string> { "起始", "線上", "預裝", "選配" };
            var otherNodes = nodesRaw.Where(n => !knownTypes.Contains((n.ShapeType ?? "").Trim())).ToList();
            foreach (var n in otherNodes)
            {
                var node = graph.AddNode(n.StepId);
                node.LabelText = (n.StepId == "Start" || n.StepId == "End")
                    ? n.Description
                    : Function.GetText.GetNodeText(n);
                node.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                node.Attr.Shape = Shape.Box;
            }
        }

        private static void AddInvisibleMainEdges(Graph graph, List<NodeData> nodesRaw)
        {
            var greenNodes = nodesRaw.Where(n => (n.ShapeType ?? "").Trim() == "線上").ToList();
            for (int i = 1; i < greenNodes.Count; i++)
            {
                string fromId = greenNodes[i - 1].StepId;
                string toId = greenNodes[i].StepId;
                bool hasRealEdge = nodesRaw.Any(n => n.StepId == fromId &&
                                                     (n.NextStepId ?? "").Split(new[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(x => x.Trim()).Contains(toId));
                if (!hasRealEdge)
                {
                    var e = graph.AddEdge(fromId, toId);
                    e.Attr.Color = Microsoft.Msagl.Drawing.Color.Transparent;
                    e.Attr.LineWidth = 3;
                    e.Attr.ArrowheadAtTarget = ArrowStyle.None;
                }
            }
        }

        private static void AddAllEdges(Graph graph, List<NodeData> nodesRaw)
        {
            foreach (var n in nodesRaw)
            {
                if (!string.IsNullOrWhiteSpace(n.NextStepId))
                {
                    var nextIds = n.NextStepId.Split(new[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries);
                    foreach (var nextId in nextIds)
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(n.StepId) && !string.IsNullOrWhiteSpace(nextId))
                                graph.AddEdge(n.StepId, nextId.Trim());
                        }
                        catch
                        {
                            // 忽略單筆錯誤
                        }
                    }
                }
            }
        }

        // 確保每個節點都有 GeometryNode，並套用座標
        private static void SetNodePositions(Graph graph, List<NodeData> nodesRaw, Dictionary<string, Microsoft.Msagl.Core.Geometry.Point> oldPositions)
        {
            foreach (var n in graph.Nodes)
            {
                // 確保 GeometryNode 存在
                if (n.GeometryNode == null)
                {
                    var geomNode = new Microsoft.Msagl.Core.Layout.Node(
                        Microsoft.Msagl.Core.Geometry.Curves.CurveFactory.CreateRectangle(50, 30, new Microsoft.Msagl.Core.Geometry.Point(0, 0))
                    );
                    n.GeometryNode = geomNode;
                }

                var nodeData = nodesRaw.FirstOrDefault(nd => nd.StepId == n.Id);

                if (nodeData != null &&
                    !string.IsNullOrWhiteSpace(nodeData.X) &&
                    !string.IsNullOrWhiteSpace(nodeData.Y) &&
                    double.TryParse(nodeData.X, NumberStyles.Float, CultureInfo.InvariantCulture, out double x) &&
                    double.TryParse(nodeData.Y, NumberStyles.Float, CultureInfo.InvariantCulture, out double y))
                {
                    // 用 Excel / nodeList 的座標
                    n.GeometryNode.Center = new Microsoft.Msagl.Core.Geometry.Point(x, y);
                }
                else if (oldPositions.TryGetValue(n.Id, out var pos))
                {
                    // 用舊位置（右鍵編輯時用）
                    n.GeometryNode.Center = pos;
                }
                // 否則保留 layout 的位置（自動排版結果）
            }
        }
    }
}
