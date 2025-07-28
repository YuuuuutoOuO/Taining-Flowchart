using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Taining.Function
{
    public static class ExcelSaver
    {
        public static string SaveNodeListToExcel(string filePath, List<NodeData> nodeList, Dictionary<string, PointF> positions)
        {
            // EPPlus 5.0+ 採用靜態屬性 ExcelPackage.LicenseContext 來設置授權模式
            ExcelPackage.License.SetNonCommercialPersonal("Ting");

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("流程圖");

                // 標題列
                ws.Cells[1, 1].Value = "程序步驟識別碼";
                ws.Cells[1, 2].Value = "程序步驟描述";
                ws.Cells[1, 3].Value = "工序識別碼";
                ws.Cells[1, 4].Value = "下一個步驟識別碼";
                ws.Cells[1, 5].Value = "圖形類型";
                ws.Cells[1, 6].Value = "替代文字";
                ws.Cells[1, 7].Value = "執行時間";
                ws.Cells[1, 8].Value = "總時間";
                ws.Cells[1, 9].Value = "X";
                ws.Cells[1, 10].Value = "Y";

                int row = 2;
                foreach (var n in nodeList)
                {
                    // ❗ 忽略 Start 和 End 節點
                    if (n.StepId == "Start" || n.StepId == "End")
                        continue;

                    ws.Cells[row, 1].Value = n.StepId;
                    ws.Cells[row, 2].Value = n.Description;
                    ws.Cells[row, 3].Value = n.ProcessId;
                    ws.Cells[row, 4].Value = n.NextStepId;
                    ws.Cells[row, 5].Value = n.ShapeType;
                    ws.Cells[row, 6].Value = n.AltText;
                    ws.Cells[row, 7].Value = n.Time;
                    ws.Cells[row, 8].Value = n.TotalTime;

                    if (positions != null && positions.TryGetValue(n.StepId, out var pt))
                    {
                        ws.Cells[row, 9].Value = pt.X;
                        ws.Cells[row, 10].Value = pt.Y;
                    }

                    row++;
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                try
                {
                    package.SaveAs(new FileInfo(filePath));
                }
                catch (IOException ex)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "檔案正在被其他程式使用，請關閉後再試！\n\n" + ex.Message,
                        "存檔失敗",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error
                    );
                    return "Failed";

                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "存檔失敗：" + ex.Message,
                        "錯誤",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error
                    );
                    return "Failed";

                }
                return "Finish";

            }
        }
    }
}
