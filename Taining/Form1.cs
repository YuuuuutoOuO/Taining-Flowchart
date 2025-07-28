// �ޥ� MSAGL ��ø�ϻP��ı�ƩR�W�Ŷ�
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;

// �ޥΦۤv�M�׸̪��\��Ҳ�
using Taining.Function;

namespace Taining
{
    // Form1 �������D���O
    public partial class Form1 : Form
    {
        // �x�s�q Excel �ন�� JSON �r��
        private string exlce_json;

        // �x�s�`�I�P�s�u��ƪ��M��A�C�Ӥ����O NodeData ����
        private List<NodeData> nodeList = new List<NodeData>();

        // �O���ϥΪ̥k���I�諸�`�IId�]����ާ@�|�Ψ�^
        private string selectedNodeId = null;

        // �غc�l�G��l�ƪ�椸��
        public Form1()
        {
            InitializeComponent(); // �إߪ��W���Ҧ�����
        }

        // �B�z�iŪ�� Excel �ɮסj���s�I���ƥ�
        private void bm_readfile_Click(object sender, EventArgs e)
        {
            // ���ɮ׿�ܹ�ܮ����ϥΪ̿� Excel ��
            using (var ofd = new OpenFileDialog())
            {
                // ���w�u��� Excel �ɮ� (*.xlsx)
                ofd.Filter = "Excel �ɮ� (*.xlsx)|*.xlsx";
                ofd.Title = "�п�ܭn�פJ�� Excel ��";

                // �p�G�ϥΪ̫��U�u�}�ҡv
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // 1. Ū����ܪ� Excel �ɡA�ন JSON �r��
                    exlce_json = Taining.Function.ExcelReader.ReadExcelDataAsJson(ofd.FileName);
                    Console.WriteLine(exlce_json); // (�i�ٲ�) �L�X JSON ���e�Ѱ���

                    // 2. �N JSON �ϧǦC���٭즨�`�I����M��A��i nodeList
                    nodeList = System.Text.Json.JsonSerializer.Deserialize<List<Taining.Function.NodeData>>(exlce_json);

                    // 3. �N��Ƥ��e�H���榡��ܦb label1 �W�]��K�ˬd�^
                    var sb = new System.Text.StringBuilder();
                    // �[�W���W��
                    sb.AppendLine("StepId\tDescription\tNextStepId\tConnectorLabel\tShapeType\tAltText\tTime\tTotalTime");

                    // �C�Ӹ`�I��ƥ[�@��
                    foreach (var d in nodeList)
                    {
                        sb.AppendLine($"{d.StepId}\t{d.Description}\t{d.NextStepId}\t{d.ShapeType}\t{d.AltText}\t{d.Time}\t{d.TotalTime}");
                    }

                    // ��ܦb label1
                    label1.Text = sb.ToString();

                    // 4. Ū�ɫ�A�ߧY�ھڸ�Ʀ۰ʵe�y�{��
                    // ���� nodeList ��^ JSON�]�]��ø�ϼҲզY JSON�^
                    var json = System.Text.Json.JsonSerializer.Serialize(nodeList);

                    // �I�s�ۭq�y�{��ø�s�禡�]Painting.DrawFlowChart�^
                    Taining.Function.Painting.DrawFlowChart(json, cv_flowchart);
                }
            }
        }

        // �B�z�y�{�ϰϰ쪺�ƹ��u�_�ƥ�]�D�n�B�z�k��^
        private void cv_flowchart_MouseUp(object sender, MouseEventArgs e)
        {
            // �ˬd�O���O���k��
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // ���o viewer ����]�y�{�Ϫ�ø�ϱ���^
                var viewer = (GViewer)sender;

                // ���o�ƹ���ФU�誺�ϧΪ���]�i��O�`�I�νu�^
                var obj = viewer.GetObjectAt(new System.Drawing.Point(e.X, e.Y));

                // �p�G�u���I��ϧΪ���A�B�O MSAGL �`�I
                if (obj != null && obj is IViewerObject viewerObj)
                {
                    if (viewerObj.DrawingObject is Microsoft.Msagl.Drawing.Node node)
                    {
                        // �O�U�Q������`�I Id
                        selectedNodeId = node.Id;

                        // ��ܥk����b��e�ƹ���m
                        contextMenuNode.Show(cv_flowchart, e.Location);
                    }
                }
            }
        }

        // �B�z�k����ﶵ�Q�I��]�Ҧp�ܧ�s�u��H�^
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // �o�̪����� nodeList �Ǩ��ܥؼЪ��]���ΦA�ѪR JSON�^
            var selectForm = new SelectTargetForm(nodeList, selectedNodeId);

            // �p�G��ܹ�ܮ��I�F OK�]�N�����s�ؼи`�I�^
            if (selectForm.ShowDialog() == DialogResult.OK)
            {
                // ���o�s��ܪ��ؼи`�I Id
                string newTargetId = selectForm.SelectedTargetId;

                // ��X�ثe�Q�諸�`�I
                var node = nodeList.FirstOrDefault(n => n.StepId == selectedNodeId);

                // �p�G���`�I�A�B�s�ؼФ����šA�N��s NextStepId
                if (node != null && !string.IsNullOrEmpty(newTargetId))
                {
                    node.NextStepId = newTargetId;
                }

                // ��s����ơA�n���sø�s�y�{��
                var json = System.Text.Json.JsonSerializer.Serialize(nodeList);
                Taining.Function.Painting.DrawFlowChart(json, cv_flowchart);
            }
        }

        // �B�z�i���s��ܬy�{�ϡj���s�I���ƥ�
        private void bm_showfc_Click(object sender, EventArgs e)
        {
            // �Υثe nodeList ���e�]�������� JSON �r��^�A��ø�y�{��
            var json = System.Text.Json.JsonSerializer.Serialize(nodeList);
            Taining.Function.Painting.DrawFlowChart(json, cv_flowchart);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("�w��ϥάy�{��ø�s�u��I\n�Х�Ū�� Excel �ɮסA�M��A�i���L�ާ@�C", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bm_readDB_Click(object sender, EventArgs e)
        {
            MessageBox.Show("�ثe�٨S��@�q��ƮwŪ���\��A�Х�Ū�� Excel �ɮסC", "����", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bm_fileSave_Click(object sender, EventArgs e)
        {
            // 1. ���ˬd�y�{�ϡ]nodeList�^���L���
            if (nodeList == null || nodeList.Count == 0)
            {
                MessageBox.Show("�ثe�S���y�{�ϥi�x�s�A�Х����ͬy�{�ϡI", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // �������_�A���n�A���������ʧ@�I
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel �ɮ� (*.xlsx)|*.xlsx";
                sfd.Title = "�п���x�s��m";
                sfd.FileName = "�y�{���x�s��.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // [1] ���o�ثe�Ҧ��`�I���y�С]�� StepId �� key�^
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

                    // [2] �x�s
                    var status_save = Taining.Function.ExcelSaver.SaveNodeListToExcel(
                        sfd.FileName, nodeList, positions
                    );
                    if (status_save != "Failed")
                    {
                        MessageBox.Show("�x�s�����I");
                    }
                }
            }
        }

        private void bm_cvClear_Click(object sender, EventArgs e)
        {
            cv_flowchart.Graph = null;
            cv_flowchart.Invalidate();
            nodeList?.Clear();    // �M���`�I���
            selectedNodeId = null;
            label1.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
