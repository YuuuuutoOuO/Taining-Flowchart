namespace Taining
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            changeTargetMenuItem = new ToolStripMenuItem();
            contextMenuNode = new ContextMenuStrip(components);
            toolStripMenuItem1 = new ToolStripMenuItem();
            bm_readfile = new Button();
            label1 = new Label();
            cv_flowchart = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            splitContainer1 = new SplitContainer();
            bm_cvClear = new Button();
            bm_fileSave = new Button();
            bm_readDB = new Button();
            pictureBox1 = new PictureBox();
            toolTip1 = new ToolTip(components);
            contextMenuNode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // changeTargetMenuItem
            // 
            changeTargetMenuItem.Name = "changeTargetMenuItem";
            changeTargetMenuItem.Size = new Size(122, 22);
            changeTargetMenuItem.Text = "修改節點";
            // 
            // contextMenuNode
            // 
            contextMenuNode.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1 });
            contextMenuNode.Name = "contextMenuNode";
            contextMenuNode.Size = new Size(123, 26);
            contextMenuNode.Click += ToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(122, 22);
            toolStripMenuItem1.Text = "選擇節點";
            // 
            // bm_readfile
            // 
            bm_readfile.Image = (Image)resources.GetObject("bm_readfile.Image");
            bm_readfile.Location = new Point(24, 31);
            bm_readfile.Name = "bm_readfile";
            bm_readfile.RightToLeft = RightToLeft.No;
            bm_readfile.Size = new Size(46, 46);
            bm_readfile.TabIndex = 1;
            toolTip1.SetToolTip(bm_readfile, "選擇Excel文件");
            bm_readfile.UseVisualStyleBackColor = true;
            bm_readfile.Click += bm_readfile_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 521);
            label1.Name = "label1";
            label1.Size = new Size(46, 15);
            label1.TabIndex = 2;
            label1.Text = "Debug";
            label1.Visible = false;
            // 
            // cv_flowchart
            // 
            cv_flowchart.ArrowheadLength = 10D;
            cv_flowchart.AsyncLayout = false;
            cv_flowchart.AutoScroll = true;
            cv_flowchart.BackColor = Color.White;
            cv_flowchart.BackgroundImageLayout = ImageLayout.None;
            cv_flowchart.BackwardEnabled = false;
            cv_flowchart.BuildHitTree = true;
            cv_flowchart.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.UseSettingsOfTheGraph;
            cv_flowchart.Dock = DockStyle.Fill;
            cv_flowchart.EdgeInsertButtonVisible = true;
            cv_flowchart.FileName = "";
            cv_flowchart.ForwardEnabled = false;
            cv_flowchart.Graph = null;
            cv_flowchart.IncrementalDraggingModeAlways = false;
            cv_flowchart.InsertingEdge = false;
            cv_flowchart.LayoutAlgorithmSettingsButtonVisible = true;
            cv_flowchart.LayoutEditingEnabled = true;
            cv_flowchart.Location = new Point(0, 0);
            cv_flowchart.LooseOffsetForRouting = 0.25D;
            cv_flowchart.MouseHitDistance = 0.05D;
            cv_flowchart.Name = "cv_flowchart";
            cv_flowchart.NavigationVisible = true;
            cv_flowchart.NeedToCalculateLayout = true;
            cv_flowchart.OffsetForRelaxingInRouting = 0.6D;
            cv_flowchart.PaddingForEdgeRouting = 8D;
            cv_flowchart.PanButtonPressed = false;
            cv_flowchart.SaveAsImageEnabled = true;
            cv_flowchart.SaveAsMsaglEnabled = true;
            cv_flowchart.SaveButtonVisible = true;
            cv_flowchart.SaveGraphButtonVisible = true;
            cv_flowchart.SaveInVectorFormatEnabled = true;
            cv_flowchart.Size = new Size(1166, 761);
            cv_flowchart.TabIndex = 4;
            cv_flowchart.TightOffsetForRouting = 0.125D;
            cv_flowchart.ToolBarIsVisible = true;
            cv_flowchart.Transform = (Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation)resources.GetObject("cv_flowchart.Transform");
            cv_flowchart.UndoRedoButtonsVisible = true;
            cv_flowchart.WindowZoomButtonPressed = false;
            cv_flowchart.ZoomF = 1D;
            cv_flowchart.ZoomWindowThreshold = 0.05D;
            cv_flowchart.MouseUp += cv_flowchart_MouseUp;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(bm_cvClear);
            splitContainer1.Panel1.Controls.Add(bm_fileSave);
            splitContainer1.Panel1.Controls.Add(bm_readDB);
            splitContainer1.Panel1.Controls.Add(pictureBox1);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(bm_readfile);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(cv_flowchart);
            splitContainer1.Size = new Size(1264, 761);
            splitContainer1.SplitterDistance = 94;
            splitContainer1.TabIndex = 5;
            // 
            // bm_cvClear
            // 
            bm_cvClear.Image = (Image)resources.GetObject("bm_cvClear.Image");
            bm_cvClear.Location = new Point(24, 174);
            bm_cvClear.Name = "bm_cvClear";
            bm_cvClear.RightToLeft = RightToLeft.No;
            bm_cvClear.Size = new Size(46, 46);
            bm_cvClear.TabIndex = 6;
            toolTip1.SetToolTip(bm_cvClear, "清除畫布");
            bm_cvClear.UseVisualStyleBackColor = true;
            bm_cvClear.Click += bm_cvClear_Click;
            // 
            // bm_fileSave
            // 
            bm_fileSave.Image = (Image)resources.GetObject("bm_fileSave.Image");
            bm_fileSave.Location = new Point(24, 345);
            bm_fileSave.Name = "bm_fileSave";
            bm_fileSave.RightToLeft = RightToLeft.No;
            bm_fileSave.Size = new Size(46, 46);
            bm_fileSave.TabIndex = 5;
            toolTip1.SetToolTip(bm_fileSave, "儲存Excel文件");
            bm_fileSave.UseVisualStyleBackColor = true;
            bm_fileSave.Click += bm_fileSave_Click;
            // 
            // bm_readDB
            // 
            bm_readDB.Image = (Image)resources.GetObject("bm_readDB.Image");
            bm_readDB.Location = new Point(24, 97);
            bm_readDB.Name = "bm_readDB";
            bm_readDB.Size = new Size(46, 46);
            bm_readDB.TabIndex = 4;
            toolTip1.SetToolTip(bm_readDB, "讀取資料庫");
            bm_readDB.UseVisualStyleBackColor = true;
            bm_readDB.Click += bm_readDB_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Cursor = Cursors.No;
            pictureBox1.ErrorImage = (Image)resources.GetObject("pictureBox1.ErrorImage");
            pictureBox1.Image = Properties.Resources.Doro;
            pictureBox1.Location = new Point(7, 549);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(79, 103);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            toolTip1.SetToolTip(pictureBox1, "Doro!");
            pictureBox1.Click += pictureBox1_Click;
            // 
            // Form1
            // 
            AutoScaleMode = AutoScaleMode.None;
            AutoSize = true;
            ClientSize = new Size(1264, 761);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(960, 540);
            Name = "Form1";
            Text = "Taining Flowchart";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            contextMenuNode.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ToolStripMenuItem changeTargetMenuItem;
        private ContextMenuStrip contextMenuNode;
        private Button bm_readfile;
        private Label label1;
        private ToolStripMenuItem toolStripMenuItem1;
        private Microsoft.Msagl.GraphViewerGdi.GViewer cv_flowchart;
        private SplitContainer splitContainer1;
        private PictureBox pictureBox1;
        private Button bm_readDB;
        private Button bm_fileSave;
        private ToolTip toolTip1;
        private Button bm_cvClear;
    }
}
