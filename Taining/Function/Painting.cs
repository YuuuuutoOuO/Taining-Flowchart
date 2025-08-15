using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Routing;
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
                // 先把目前畫面上的座標寫回 nodeList（避免右鍵編輯/新增後丟失位置）
                var nodesRaw = System.Text.Json.JsonSerializer.Deserialize<List<NodeData>>(json);
                UpdateNodeListPositions(nodesRaw, gViewer);

                // 取舊座標（給沒有 X/Y 的節點 fallback）
                var oldPositions = GetOldPositions(gViewer);

                // 更新總時間
                UpdateTotalTime.Update(nodesRaw);

                // 建圖
                var graph = new Graph("流程圖");
                graph.Attr.LayerDirection = LayerDirection.LR;

                AddNodesByContains(graph, nodesRaw, "起始", Microsoft.Msagl.Drawing.Color.LightBlue);
                AddNodesByContains(graph, nodesRaw, "線上", Microsoft.Msagl.Drawing.Color.LightGreen);
                AddNodesByContains(graph, nodesRaw, "預裝", Microsoft.Msagl.Drawing.Color.Yellow);
                AddNodesByContains(graph, nodesRaw, "選配", Microsoft.Msagl.Drawing.Color.White);
                AddNodes_OthersAsRed(graph, nodesRaw);

                AddInvisibleMainEdges(graph, nodesRaw);
                AddAllEdges(graph, nodesRaw);

                // —— 兩段式流程 —— 
                // (1) 先讓 GViewer 進行一次自動佈局（建立邊/節點幾何）
                gViewer.NeedToCalculateLayout = true;
                gViewer.Graph = graph;

                // (2) 立刻覆寫「有 X/Y 或舊位置」的節點座標
                SetNodePositions(gViewer.Graph, nodesRaw, oldPositions);

                // (3) 在新座標下，重新配線（關鍵步驟）
                RouteEdges(gViewer.Graph);

                // (4) 關閉自動 layout
                gViewer.NeedToCalculateLayout = false;
                gViewer.Invalidate();

                // (5) 自動把整張圖縮放到可視區域，避免邊緣被裁切
                gViewer.FitGraphBoundingBox();      // 讓整張圖完整顯示
                gViewer.Invalidate();               // 重繪
            }
            catch
            {
                gViewer.Graph = new Graph();
                gViewer.Invalidate();
            }
        }

        /// <summary>把目前畫面上的座標寫回 nodeList（StepId 對應）</summary>
        private static void UpdateNodeListPositions(List<NodeData> nodeList, GViewer gViewer)
        {
            if (gViewer?.Graph == null) return;
            foreach (var n in gViewer.Graph.Nodes)
            {
                var nd = nodeList.FirstOrDefault(x => x.StepId == n.Id);
                if (nd != null && n.GeometryNode != null)
                {
                    nd.X = n.GeometryNode.Center.X.ToString(CultureInfo.InvariantCulture);
                    nd.Y = n.GeometryNode.Center.Y.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private static Dictionary<string, Microsoft.Msagl.Core.Geometry.Point> GetOldPositions(GViewer gViewer)
        {
            var dict = new Dictionary<string, Microsoft.Msagl.Core.Geometry.Point>();
            if (gViewer?.Graph == null) return dict;

            foreach (var n in gViewer.Graph.Nodes)
            {
                if (n.GeometryNode != null)
                    dict[n.Id] = n.GeometryNode.Center;
            }
            return dict;
        }

        private static void AddNodesByContains(Graph graph, List<NodeData> nodesRaw, string keyword, Microsoft.Msagl.Drawing.Color color)
        {
            var typeNodes = nodesRaw.Where(n => (n.ShapeType ?? "").Contains(keyword)).ToList();
            foreach (var n in typeNodes)
            {
                var node = graph.AddNode(n.StepId);
                node.LabelText = (n.StepId == "Start" || n.StepId == "End") ? n.Description : GetText.GetNodeText(n);
                node.Attr.FillColor = color;
                node.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Box;
            }
        }

        private static void AddNodes_OthersAsRed(Graph graph, List<NodeData> nodesRaw)
        {
            var known = new HashSet<string> { "起始", "線上", "預裝", "選配" };
            var others = nodesRaw.Where(n => !known.Contains((n.ShapeType ?? "").Trim())).ToList();
            foreach (var n in others)
            {
                var node = graph.AddNode(n.StepId);
                node.LabelText = (n.StepId == "Start" || n.StepId == "End") ? n.Description : GetText.GetNodeText(n);
                node.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                node.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Box;
            }
        }

        /// <summary>主流程的補線（透明），但要確定節點存在且沒有實線才補</summary>
        private static void AddInvisibleMainEdges(Graph graph, List<NodeData> nodesRaw)
        {
            var existing = new HashSet<string>(graph.Nodes.Select(nd => nd.Id));
            var main = nodesRaw.Where(n => (n.ShapeType ?? "").Trim() == "線上").ToList();

            for (int i = 1; i < main.Count; i++)
            {
                string fromId = main[i - 1].StepId;
                string toId = main[i].StepId;

                if (!existing.Contains(fromId) || !existing.Contains(toId)) continue;

                bool hasRealEdge = nodesRaw.Any(n => n.StepId == fromId &&
                                          (n.NextStepId ?? "")
                                          .Split(new[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries)
                                          .Select(x => x.Trim())
                                          .Contains(toId));

                if (!hasRealEdge)
                {
                    var e = graph.AddEdge(fromId, toId);
                    e.Attr.Color = Microsoft.Msagl.Drawing.Color.Transparent;
                    e.Attr.LineWidth = 3;
                    e.Attr.ArrowheadAtTarget = ArrowStyle.None;
                }
            }
        }

        /// <summary>依 NextStepId 加所有實線，但要確保目標節點存在、且不是自連</summary>
        private static void AddAllEdges(Graph graph, List<NodeData> nodesRaw)
        {
            var existing = new HashSet<string>(graph.Nodes.Select(nd => nd.Id));

            foreach (var n in nodesRaw)
            {
                if (string.IsNullOrWhiteSpace(n.StepId) || string.IsNullOrWhiteSpace(n.NextStepId))
                    continue;

                var targets = n.NextStepId.Split(new[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (var raw in targets)
                {
                    var to = (raw ?? "").Trim();
                    if (string.IsNullOrWhiteSpace(to)) continue;
                    if (to == n.StepId) continue;                 // 避免自連
                    if (!existing.Contains(to)) continue;         // 目標節點不存在就跳過

                    try { graph.AddEdge(n.StepId, to); }
                    catch { /* 忽略單筆錯誤 */ }
                }
            }
        }

        /// <summary>確保每個節點都有 GeometryNode，並把「有 X/Y 或舊位置」的節點移到指定座標</summary>
        private static void SetNodePositions(Graph graph, List<NodeData> nodesRaw, Dictionary<string, Microsoft.Msagl.Core.Geometry.Point> oldPositions)
        {
            foreach (var n in graph.Nodes)
            {
                if (n.GeometryNode == null)
                {
                    var geomNode = new Microsoft.Msagl.Core.Layout.Node(
                        Microsoft.Msagl.Core.Geometry.Curves.CurveFactory.CreateRectangle(
                            50, 30, new Microsoft.Msagl.Core.Geometry.Point(0, 0))
                    );
                    n.GeometryNode = geomNode;
                }

                var data = nodesRaw.FirstOrDefault(nd => nd.StepId == n.Id);

                if (data != null &&
                    !string.IsNullOrWhiteSpace(data.X) &&
                    !string.IsNullOrWhiteSpace(data.Y) &&
                    double.TryParse(data.X, NumberStyles.Float, CultureInfo.InvariantCulture, out double x) &&
                    double.TryParse(data.Y, NumberStyles.Float, CultureInfo.InvariantCulture, out double y))
                {
                    n.GeometryNode.Center = new Microsoft.Msagl.Core.Geometry.Point(x, y);
                }
                else if (oldPositions.TryGetValue(n.Id, out var pos))
                {
                    n.GeometryNode.Center = pos;
                }
                // 否則保留自動 layout 結果
            }
        }

        /// <summary>只重新計算邊的路徑（不重排節點）</summary>
        private static void RouteEdges(Graph graph)
        {
            var gg = graph.GeometryGraph;
            if (gg == null) return;

            foreach (var e in gg.Edges)
                e.Curve = null;

            double edgePadding = 3;
            double nodePadding = 1;
            double coneAngle = System.Math.PI / 6;

            var router = new SplineRouter(gg, edgePadding, nodePadding, coneAngle);
            router.Run();
        }
    }
}
