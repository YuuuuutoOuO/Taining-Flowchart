using System.Collections.Generic;
using System.Linq;

namespace Taining.Function
{
    public static class UpdateTotalTime
    {
        /// <summary>
        /// 按照流程連線計算「程序」的總時間（自己 + 直接連入的子程序、物件 Time），不累加其他「程序」的時間。
        /// </summary>
        public static void Update(List<NodeData> nodeList)
        {
            foreach (var programNode in nodeList.Where(n => n.ShapeType == "程序"))
            {
                double total = 0;
                // 加上自己
                if (double.TryParse(programNode.Time, out double selfTime))
                    total += selfTime;

                // 找所有「直接連到該程序」的節點
                var connected = nodeList.Where(n =>
                    !string.IsNullOrWhiteSpace(n.NextStepId) &&
                    n.NextStepId.Split(new[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Contains(programNode.StepId)
                    && n.ShapeType != "程序" // 只加非程序
                );
                foreach (var n in connected)
                {
                    if (double.TryParse(n.Time, out double t))
                        total += t;
                }
                programNode.TotalTime = total.ToString();
            }
        }
    }
}
