using OfficeOpenXml;            // EPPlus 套件
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Taining.Function
{
    /*
     * 114514 1919810
     */
    public class NodeData
    {
        public string StepId { get; set; }
        public string ProcessId { get; set; }
        public string Description { get; set; }
        public string NextStepId { get; set; }
        public string ShapeType { get; set; }
        public string AltText { get; set; }
        public string Time { get; set; }
        public string TotalTime { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
    }

    public static class ExcelReader
    {
        public static string ReadExcelDataAsJson(string filePath)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Ting");
            using var package = new ExcelPackage(new FileInfo(filePath));
            var ws = package.Workbook.Worksheets[0];
            int rowCount = ws.Dimension.End.Row;

            var list = new List<NodeData>();

            // 1. 讀Excel
            for (int row = 2; row <= rowCount; row++)
            {
                list.Add(new NodeData
                {
                    StepId = ws.Cells[row, 1].Text.Trim(),
                    Description = ws.Cells[row, 2].Text.Trim(),
                    ProcessId = ws.Cells[row, 3].Text.Trim(),
                    NextStepId = ws.Cells[row, 4].Text.Trim(),
                    ShapeType = ws.Cells[row, 5].Text.Trim(),
                    AltText = ws.Cells[row, 6].Text.Trim(),
                    Time = ws.Cells[row, 7].Text.Trim(),
                    TotalTime = ws.Cells[row, 8].Text.Trim(),
                    X = ws.Cells[row, 9].Text.Trim(),
                    Y = ws.Cells[row, 10].Text.Trim(),
                });
            }

            // 2. 自動補開始/結束與總時間
            UpdateNodeListAndTotalTime(list);

            // 3. 回傳 JSON
            return JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        }

        // --- 把 UpdateTotalTime 內容整合成私有靜態方法 ---
        private static void UpdateNodeListAndTotalTime(List<NodeData> nodeList)
        {
        string _startStepId = "Start"; // 開始節點的 StepId
        string _endStepId = "End"; // 結束節點的 StepId
        string _startDescription = "起始";
        // 找主流程
        var mainPrograms = nodeList
                .Where(n => n.ShapeType == "程序" && n.StepId != "0" && n.StepId != "999")
                .OrderBy(n => n.StepId)
                .ToList();

            // 補「開始」
            var startNode = nodeList.FirstOrDefault(n => n.StepId == "0");
            if (startNode == null)
            {
                startNode = new NodeData
                {
                    StepId = _startStepId,
                    Description = "開始",
                    ShapeType = _startDescription
                };
                nodeList.Insert(0, startNode);
            }

            // 補「結束」
            var endNode = nodeList.FirstOrDefault(n => n.StepId == "999");
            if (endNode == null)
            {
                endNode = new NodeData
                {
                    StepId = _endStepId,
                    Description = "結束",
                    ShapeType = _startDescription
                };
                nodeList.Add(endNode);
            }

            // 開始節點指向主流程第一個
            if (mainPrograms.Any())
            {
                startNode.NextStepId = mainPrograms.First().StepId;
            }
            else
            {
                startNode.NextStepId = _endStepId;
            }

            // 最後一個主流程指向結束
            if (mainPrograms.Any())
            {
                mainPrograms.Last().NextStepId = _endStepId;
            }

            // 所有程序節點加總
            foreach (var programNode in nodeList.Where(n => n.ShapeType == "程序"))
            {
                string prefix = programNode.StepId;
                var relatedNodes = nodeList
                    .Where(n => n.StepId != null && n.StepId.StartsWith(prefix))
                    .ToList();

                double total = 0;
                foreach (var n in relatedNodes)
                {
                    if (double.TryParse(n.Time, out double t))
                        total += t;
                }
                programNode.TotalTime = total.ToString();
            }
        }
    }
}
