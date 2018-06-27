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
            this.UnitsList = new System.Windows.Forms.ListBox();
            this.miniMap1 = new MapView.MiniMap();
            this.viewPort1 = new MapView.ViewPort();
            this.SuspendLayout();
            // 
            // UnitsList
            // 
            this.UnitsList.FormattingEnabled = true;
            this.UnitsList.Location = new System.Drawing.Point(1039, 644);
            this.UnitsList.Name = "UnitsList";
            this.UnitsList.Size = new System.Drawing.Size(120, 95);
            this.UnitsList.TabIndex = 2;
            // 
            // miniMap1
            // 
            this.miniMap1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.miniMap1.Location = new System.Drawing.Point(403, 612);
            this.miniMap1.Name = "miniMap1";
            this.miniMap1.Size = new System.Drawing.Size(419, 192);
            this.miniMap1.TabIndex = 1;
            this.miniMap1.Text = "miniMap1";
            this.miniMap1.ViewPortHeigth = 0;
            this.miniMap1.ViewPortWidth = 0;
            this.miniMap1.World = null;
            this.miniMap1.MapClick += new System.EventHandler<MapView.MapClickEventArgs>(this.miniMap1_MapClick);
            // 
            // viewPort1
            // 
            this.viewPort1.Location = new System.Drawing.Point(12, 12);
            this.viewPort1.Name = "viewPort1";
            this.viewPort1.Size = new System.Drawing.Size(1244, 594);
            this.viewPort1.TabIndex = 0;
            this.viewPort1.Text = "viewPort1";
            this.viewPort1.MapMove += new System.EventHandler<MapView.MapMoveEventArgs>(this.viewPort1_MapMove);
            this.viewPort1.CellSelected += new System.EventHandler<MapView.CellSelectedEventArgs>(this.viewPort1_CellSelected);
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 816);
            this.Controls.Add(this.UnitsList);
            this.Controls.Add(this.miniMap1);
            this.Controls.Add(this.viewPort1);
            this.KeyPreview = true;
            this.Name = "Map";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Map_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private ViewPort viewPort1;
        private MiniMap miniMap1;
        private System.Windows.Forms.ListBox UnitsList;
    }
}

