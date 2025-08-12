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
            InitializeComponent(); // 建立表單上的所有元件A
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("歡迎使用流程圖繪製工具！\n請先讀取 Excel 檔案，然後再進行其他操作。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            changeNodeToolStripMenuItem.Click += ToolStripMenuItem_Click;
            deleteNodeToolStripMenuItem.Click += deleteNodeToolStripMenuItem_Click;
            createNodeToolStripMenuItem.Click += createNodeToolStripMenuItem_Click;
            editNodeToolStripMenuItem.Click += editNodeToolStripMenuItem_Click;
            //

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
            var viewer = (GViewer)sender;
            object obj = null; // 先宣告

            // 防呆：流程圖沒載入時，不顯示選單，直接 return
            if (viewer.Graph == null)
                return;

            try
            {
                obj = viewer.GetObjectAt(new System.Drawing.Point(e.X, e.Y));
                selectedNodeId = null; // 預設沒有選擇節點
            }
            catch (Exception ex)
            {
                MessageBox.Show($"處理滑鼠事件時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // 判斷是否點到節點
                if (obj is IViewerObject viewerObj && viewerObj.DrawingObject is Microsoft.Msagl.Drawing.Node node)
                {
                    selectedNodeId = node.Id;
                    changeNodeToolStripMenuItem.Enabled = true;
                    deleteNodeToolStripMenuItem.Enabled = true;
                    editNodeToolStripMenuItem.Enabled = true;
                }
                else
                {
                    // 沒有選節點則禁用（只有「新增」可以用）
                    changeNodeToolStripMenuItem.Enabled = false;
                    deleteNodeToolStripMenuItem.Enabled = false;
                    editNodeToolStripMenuItem.Enabled = false;
                }
                // 新增節點永遠啟用
                createNodeToolStripMenuItem.Enabled = true;

                contextMenuNode.Show(cv_flowchart, e.Location);
            }
        }



        // 處理右鍵選單的「變更連線」選項被點選
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CvFlowchartMenu.ChangeEdge(nodeList, selectedNodeId, cv_flowchart);
        }

        private void deleteNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CvFlowchartMenu.DeleteNode(nodeList, selectedNodeId, cv_flowchart);
            selectedNodeId = null; // 這裡自行重置
        }

        private void editNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CvFlowchartMenu.EditNode(nodeList, selectedNodeId, cv_flowchart);
        }

        private void createNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CvFlowchartMenu.CreateNode(nodeList, cv_flowchart);
        }

        // 處理【從資料庫讀取】按鈕點擊事件
        private void bm_readDB_Click(object sender, EventArgs e)
        {
            MessageBox.Show("目前還沒實作從資料庫讀取功能，請先讀取 Excel 檔案。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 處理【儲存流程圖】按鈕點擊事件
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

        // 處理【清除流程圖】按鈕點擊事件
        private void bm_cvClear_Click(object sender, EventArgs e)
        {
            cv_flowchart.Graph = null;
            cv_flowchart.Invalidate();
            nodeList?.Clear();    // 清除節點資料
            selectedNodeId = null;
            label1.Text = "";
        }

        // 處理右鍵選單開啟事件（目前沒有特別處理）
        private void contextMenuNode_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}

/*
 代修正:
 ADD EDIT的combox要不要天加文字描述，而不是只有編號:>
 */

/*
 講解重點:
    1. 使用者介面：Form1 類別是主要的視窗表單，包含流程圖繪製區域和各種按鈕，並說明使用C#的優勢
    2. 讀取 Excel 檔案：使用 OpenFileDialog 讓使用者選擇 Excel 檔，然後執行時間需要為數字或空
    3. 繪製流程圖：使用 MSAGL 繪製流程圖，並在滑鼠事件中處理節點的選擇和操作。(右鍵功能需要重點講解)
 */
