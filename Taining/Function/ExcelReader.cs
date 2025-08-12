using OfficeOpenXml; // EPPlus 套件
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Taining.Function
{
    /// <summary>
    /// 流程圖節點資料結構
    /// </summary>
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

    /// <summary>
    /// 讀取 Excel，驗證資料並回傳節點清單 JSON
    /// </summary>
    public static class ExcelReader
    {
        /// <summary>
        /// 主方法：從 Excel 檔案讀取節點資料，驗證格式並回傳 JSON 字串
        /// </summary>
        public static string ReadExcelDataAsJson(string filePath)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Ting");
            using var package = new ExcelPackage(new FileInfo(filePath));
            var ws = package.Workbook.Worksheets[0];

            var nodeList = ReadNodeListFromWorksheet(ws);
            var formatErrors = CheckFormatErrors(nodeList);

            if (formatErrors.Count > 0)
            {
                ShowError("檔案格式錯誤：\n" + string.Join("\n", formatErrors), "Excel 內容錯誤");
                return "[]";
            }

            var duplicateStepIds = CheckDuplicateStepIds(nodeList);
            if (duplicateStepIds.Count > 0)
            {
                ShowError("StepId 不可重複，以下 StepId 有重複：\n" + string.Join(", ", duplicateStepIds), "StepId 重複警告");
                return "[]";
            }

            // 補上開始/結束節點與計算總時間
            UpdateNodeListAndTotalTime(nodeList);

            // 以格式化 JSON 輸出
            return JsonSerializer.Serialize(nodeList, new JsonSerializerOptions { WriteIndented = true });
        }

        /// <summary>
        /// 從 Excel 工作表讀取所有 NodeData
        /// </summary>
        private static List<NodeData> ReadNodeListFromWorksheet(ExcelWorksheet ws)
        {
            int rowCount = ws.Dimension.End.Row;
            var list = new List<NodeData>();

            for (int row = 2; row <= rowCount; row++)
            {
                var node = new NodeData
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
                };
                list.Add(node);
            }
            return list;
        }

        /// <summary>
        /// 檢查每個節點的格式錯誤（目前只驗證 Time 是否為數字或空）
        /// </summary>
        private static List<string> CheckFormatErrors(List<NodeData> nodeList)
        {
            var errors = new List<string>();
            for (int i = 0; i < nodeList.Count; i++)
            {
                var node = nodeList[i];
                var timeText = node.Time;
                if (!string.IsNullOrEmpty(timeText) && !double.TryParse(timeText, out _))
                {
                    errors.Add($"第 {i + 2} 列的『執行時間（Time）』必須為空或是數字，目前值：'{timeText}'");
                }
            }
            return errors;
        }

        /// <summary>
        /// 檢查 StepId 是否唯一，回傳所有重複的 StepId 清單
        /// </summary>
        private static List<string> CheckDuplicateStepIds(List<NodeData> nodeList)
        {
            var stepIdCount = new Dictionary<string, int>();
            foreach (var node in nodeList)
            {
                if (!string.IsNullOrEmpty(node.StepId))
                {
                    if (!stepIdCount.ContainsKey(node.StepId))
                        stepIdCount[node.StepId] = 1;
                    else
                        stepIdCount[node.StepId]++;
                }
            }
            // 回傳出現次數超過1次的StepId
            return stepIdCount.Where(kv => kv.Value > 1).Select(kv => kv.Key).ToList();
        }

        /// <summary>
        /// 彈出錯誤訊息視窗
        /// </summary>
        private static void ShowError(string message, string title)
        {
            System.Windows.Forms.MessageBox.Show(
                message,
                title,
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error
            );
        }

        /// <summary>
        /// 自動補「開始」與「結束」節點並計算主流程總時間
        /// </summary>
        private static void UpdateNodeListAndTotalTime(List<NodeData> nodeList)
        {
            string _startStepId = "Start"; // 開始節點 StepId
            string _endStepId = "End";     // 結束節點 StepId
            string _startDescription = "起始";

            // 找主流程節點（ShapeType=程序，排除0/999）
            var mainPrograms = nodeList
                .Where(n => n.ShapeType == "線上" && n.StepId != "0" && n.StepId != "999")
                .OrderBy(n => n.StepId)
                .ToList();

            // 補「開始」節點
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

            // 補「結束」節點
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
                startNode.NextStepId = mainPrograms.First().StepId;
            else
                startNode.NextStepId = _endStepId;

            // 最後一個主流程指向結束
            if (mainPrograms.Any())
                mainPrograms.Last().NextStepId = _endStepId;

            // 計算所有「程序」節點的總時間
            foreach (var programNode in nodeList.Where(n => n.ShapeType == "線上"))
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
