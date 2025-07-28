using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taining.Function
{
    internal class GetText
    {
        public static string GetNodeText(NodeData n)
        {
            var lines = new List<string>();
            if (!string.IsNullOrWhiteSpace(n.StepId)) lines.Add("步驟號碼: " + n.StepId);
            if (!string.IsNullOrWhiteSpace(n.Description)) lines.Add("\n"+n.Description+ "\n");
            if (!string.IsNullOrWhiteSpace(n.ProcessId)) lines.Add("編號: "+ n.ProcessId);
            //if (!string.IsNullOrWhiteSpace(n.NextStepId)) lines.Add("→ " + n.NextStepId);
            //if (!string.IsNullOrWhiteSpace(n.AltText)) lines.Add("" + n.AltText);
            //if (!string.IsNullOrWhiteSpace(n.ShapeType)) lines.Add("類型: " + n.ShapeType);
            if (!string.IsNullOrWhiteSpace(n.Time)) lines.Add("執行時間: " + n.Time);
            if (!string.IsNullOrWhiteSpace(n.TotalTime)) lines.Add("總花費時間: " + n.TotalTime);

            return string.Join("\n", lines);
        }

    }
}
