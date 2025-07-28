// 引用 MSAGL 的繪圖與視覺化命名空間
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;

// 引用自己專案裡的功能模組
using Taining.Function;

namespace Taining
{
    // Form1 視窗表單主類別
    public partial class Form1 : Form
    {
        // 儲存從 Excel 轉成的 JSON 字串
        private string exlce_json;

        // 儲存節點與連線資料的清單，每個元素是 NodeData 物件
        private List<NodeData> nodeList = new List<NodeData>();

        // 記錄使用者右鍵點選的節點Id（後續操作會用到）
        private string selectedNodeId = null;

        // 建構子：初始化表單元件
        public Form1()
        {
            InitializeComponent(); // 建立表單上的所有元件
        }

        // 處理【讀取 Excel 檔案】按鈕點擊事件
        private void bm_readfile_Click(object sender, EventArgs e)
        {
            // 用檔案選擇對話框讓使用者選 Excel 檔
            using (var ofd = new OpenFileDialog())
            {
                // 限定只顯示 Excel 檔案 (*.xlsx)
                ofd.Filter = "Excel 檔案 (*.xlsx)|*.xlsx";
                ofd.Title = "請選擇要匯入的 Excel 檔";

                // 如果使用者按下「開啟」
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // 1. 讀取選擇的 Excel 檔，轉成 JSON 字串
                    exlce_json = Taining.Function.ExcelReader.ReadExcelDataAsJson(ofd.FileName);
                    Console.WriteLine(exlce_json); // (可省略) 印出 JSON 內容供除錯

                    // 2. 將 JSON 反序列化還原成節點物件清單，放進 nodeList
                    nodeList = System.Text.Json.JsonSerializer.Deserialize<List<Taining.Function.NodeData>>(exlce_json);

                    // 3. 將資料內容以表格格式顯示在 label1 上（方便檢查）
                    var sb = new System.Text.StringBuilder();
                    // 加上欄位名稱
                    sb.AppendLine("StepId\tDescription\tNextStepId\tConnectorLabel\tShapeType\tAltText\tTime\tTotalTime");

                    // 每個節點資料加一行
                    foreach (var d in nodeList)
                    {
                        sb.AppendLine($"{d.StepId}\t{d.Description}\t{d.NextStepId}\t{d.ShapeType}\t{d.AltText}\t{d.Time}\t{d.TotalTime}");
                    }

                    // 顯示在 label1
                    label1.Text = sb.ToString();

                    // 4. 讀檔後，立即根據資料自動畫流程圖
                    // 先把 nodeList 轉回 JSON（因為繪圖模組吃 JSON）
                    var json = System.Text.Json.JsonSerializer.Serialize(nodeList);

                    // 呼叫自訂流程圖繪製函式（Painting.DrawFlowChart）
                    Taining.Function.Painting.DrawFlowChart(json, cv_flowchart);
                }
            }
        }

        // 處理流程圖區域的滑鼠彈起事件（主要處理右鍵）
        private void cv_flowchart_MouseUp(object sender, MouseEventArgs e)
        {
            // 檢查是不是按右鍵
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // 取得 viewer 物件（流程圖的繪圖控制項）
                var viewer = (GViewer)sender;

                // 取得滑鼠游標下方的圖形物件（可能是節點或線）
                var obj = viewer.GetObjectAt(new System.Drawing.Point(e.X, e.Y));

                // 如果真的點到圖形物件，且是 MSAGL 節點
                if (obj != null && obj is IViewerObject viewerObj)
                {
                    if (viewerObj.DrawingObject is Microsoft.Msagl.Drawing.Node node)
                    {
                        // 記下被選取的節點 Id
                        selectedNodeId = node.Id;

                        // 顯示右鍵選單在當前滑鼠位置
                        contextMenuNode.Show(cv_flowchart, e.Location);
                    }
                }
            }
        }

        // 處理右鍵選單選項被點選（例如變更連線對象）
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 這裡直接用 nodeList 傳到選擇目標表單（不用再解析 JSON）
            var selectForm = new SelectTargetForm(nodeList, selectedNodeId);

            // 如果選擇對話框點了 OK（代表有選到新目標節點）
            if (selectForm.ShowDialog() == DialogResult.OK)
            {
                // 取得新選擇的目標節點 Id
                string newTargetId = selectForm.SelectedTargetId;

                // 找出目前被選的節點
                var node = nodeList.FirstOrDefault(n => n.StepId == selectedNodeId);

                // 如果找到節點，且新目標不為空，就更新 NextStepId
                if (node != null && !string.IsNullOrEmpty(newTargetId))
                {
                    node.NextStepId = newTargetId;
                }

                // 更新完資料，要重新繪製流程圖
                var json = System.Text.Json.JsonSerializer.Serialize(nodeList);
                Taining.Function.Painting.DrawFlowChart(json, cv_flowchart);
            }
        }

        // 處理【重新顯示流程圖】按鈕點擊事件
        private void bm_showfc_Click(object sender, EventArgs e)
        {
            // 用目前 nodeList 內容（不直接用 JSON 字串），重繪流程圖
            var json = System.Text.Json.JsonSerializer.Serialize(nodeList);
            Taining.Function.Painting.DrawFlowChart(json, cv_flowchart);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("歡迎使用流程圖繪製工具！\n請先讀取 Excel 檔案，然後再進行其他操作。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bm_readDB_Click(object sender, EventArgs e)
        {
            MessageBox.Show("目前還沒實作從資料庫讀取功能，請先讀取 Excel 檔案。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bm_fileSave_Click(object sender, EventArgs e)
        {
            // 1. 先檢查流程圖（nodeList）有無資料
            if (nodeList == null || nodeList.Count == 0)
            {
                MessageBox.Show("目前沒有流程圖可儲存，請先產生流程圖！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // 直接中斷，不要再做後續任何動作！
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel 檔案 (*.xlsx)|*.xlsx";
                sfd.Title = "請選擇儲存位置";
                sfd.FileName = "流程圖儲存檔.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // [1] 取得目前所有節點的座標（用 StepId 當 key）
                    var positions = new Dictionary<string, System.Drawing.PointF>();
                    if (cv_flowchart.Graph != null)
                    {
                        foreach (var n in cv_flowchart.Graph.Nodes)
                        {
                            if (n.GeometryNode != null)
                            {
                                positions[n.Id] = new System.Drawing.PointF(
                                    (float)n.GeometryNode.Center.X,
                                    (float)n.GeometryNode.Center.Y
                                );
                            }
                        }
                    }

                    // [2] 儲存
                    var status_save = Taining.Function.ExcelSaver.SaveNodeListToExcel(
                        sfd.FileName, nodeList, positions
                    );
                    if (status_save != "Failed")
                    {
                        MessageBox.Show("儲存完成！");
                    }
                }
            }
        }

        private void bm_cvClear_Click(object sender, EventArgs e)
        {
            cv_flowchart.Graph = null;
            cv_flowchart.Invalidate();
            nodeList?.Clear();    // 清除節點資料
            selectedNodeId = null;
            label1.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
