namespace MapView
{
    partial class Map
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.UnitsList = new System.Windows.Forms.ListBox();
            this.miniMap1 = new MapView.MiniMap();
            this.viewPort1 = new MapView.ViewPort();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ResearchPanelMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.ScienceReportMenuPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // UnitsList
            // 
            this.UnitsList.FormattingEnabled = true;
            this.UnitsList.Location = new System.Drawing.Point(1039, 644);
            this.UnitsList.Name = "UnitsList";
            this.UnitsList.Size = new System.Drawing.Size(120, 95);
            this.UnitsList.TabIndex = 2;
            this.UnitsList.SelectedValueChanged += new System.EventHandler(this.UnitsList_SelectedValueChanged);
            // 
            // miniMap1
            // 
            this.miniMap1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.miniMap1.Location = new System.Drawing.Point(440, 544);
            this.miniMap1.Name = "miniMap1";
            this.miniMap1.Size = new System.Drawing.Size(447, 231);
            this.miniMap1.TabIndex = 1;
            this.miniMap1.Text = "miniMap1";
            this.miniMap1.ViewPortHeigth = 0;
            this.miniMap1.ViewPortWidth = 0;
            this.miniMap1.World = null;
            this.miniMap1.MapClick += new System.EventHandler<MapView.MapClickEventArgs>(this.miniMap1_MapClick);
            // 
            // viewPort1
            // 
            this.viewPort1.Location = new System.Drawing.Point(12, 27);
            this.viewPort1.Name = "viewPort1";
            this.viewPort1.Size = new System.Drawing.Size(1244, 511);
            this.viewPort1.TabIndex = 0;
            this.viewPort1.Text = "viewPort1";
            this.viewPort1.MapMove += new System.EventHandler<MapView.MapMoveEventArgs>(this.viewPort1_MapMove);
            this.viewPort1.CellSelected += new System.EventHandler<MapView.CellSelectedEventArgs>(this.viewPort1_CellSelected);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(34, 685);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Следующий ход";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResearchPanelMenuStrip});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1268, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ResearchPanelMenuStrip
            // 
            this.ResearchPanelMenuStrip.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScienceReportMenuPanel});
            this.ResearchPanelMenuStrip.Name = "ResearchPanelMenuStrip";
            this.ResearchPanelMenuStrip.Size = new System.Drawing.Size(46, 20);
            this.ResearchPanelMenuStrip.Text = "Игра";
            // 
            // ScienceReportMenuPanel
            // 
            this.ScienceReportMenuPanel.Name = "ScienceReportMenuPanel";
            this.ScienceReportMenuPanel.Size = new System.Drawing.Size(189, 22);
            this.ScienceReportMenuPanel.Text = "Меню исследований";
            this.ScienceReportMenuPanel.Click += new System.EventHandler(this.ScienceReportMenuPanel_Click);
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 816);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.UnitsList);
            this.Controls.Add(this.miniMap1);
            this.Controls.Add(this.viewPort1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Map";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Map_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ViewPort viewPort1;
        private MiniMap miniMap1;
        private System.Windows.Forms.ListBox UnitsList;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ResearchPanelMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ScienceReportMenuPanel;
    }
}

