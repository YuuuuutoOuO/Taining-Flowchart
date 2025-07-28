using System.Collections.Generic;
using System.Linq;

namespace Taining.Function
{
    public static class UpdateTotalTime
    {
        public static void Update(List<NodeData> nodeList)
        {
            foreach (var programNode in nodeList.Where(n => n.ShapeType == "程序"))
            {
                var visited = new HashSet<string>(); // 避免重複加總
                double total = 0;
                // 加上自己
                if (double.TryParse(programNode.Time, out double selfTime))
                    total += selfTime;
                // 遞迴加總所有「間接或直接連到此程序」的非程序節點
                total += SumAllConnected(nodeList, programNode.StepId, visited);
                programNode.TotalTime = total.ToString();
            }
        }

        // 遞迴找所有經過的非「程序」節點
        private static double SumAllConnected(List<NodeData> nodeList, string targetStepId, HashSet<string> visited)
        {
            double sum = 0;
            var connectedNodes = nodeList
                .Where(n =>
                    !string.IsNullOrWhiteSpace(n.NextStepId) &&
                    n.NextStepId.Split(new[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Contains(targetStepId)
                    && n.ShapeType != "程序"
                    && !visited.Contains(n.StepId)
                ).ToList();

            foreach (var node in connectedNodes)
            {
                visited.Add(node.StepId); // 標記已走過
                if (double.TryParse(node.Time, out double t))
                    sum += t;
                // 遞迴往前追
                sum += SumAllConnected(nodeList, node.StepId, visited);
            }
            return sum;
        }
    }
}
