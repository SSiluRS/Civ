namespace MapView
{
    partial class Form1
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
            this.miniMap1 = new MapView.MiniMap();
            this.viewPort1 = new MapView.ViewPort();
            this.SuspendLayout();
            // 
            // miniMap1
            // 
            this.miniMap1.Location = new System.Drawing.Point(476, 651);
            this.miniMap1.Name = "miniMap1";
            this.miniMap1.Size = new System.Drawing.Size(310, 153);
            this.miniMap1.TabIndex = 20;
            this.miniMap1.Text = "miniMap1";
            this.miniMap1.ViewPortHeigth = 0;
            this.miniMap1.ViewPortWidth = 0;
            this.miniMap1.MapClick += new System.EventHandler<MapView.MapClickEventArgs>(this.miniMap1_MapClick);
            // 
            // viewPort1
            // 
            this.viewPort1.Location = new System.Drawing.Point(12, 12);
            this.viewPort1.Name = "viewPort1";
            this.viewPort1.Size = new System.Drawing.Size(1244, 633);
            this.viewPort1.TabIndex = 19;
            this.viewPort1.Text = "viewPort1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 816);
            this.Controls.Add(this.miniMap1);
            this.Controls.Add(this.viewPort1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private ViewPort viewPort1;
        private MiniMap miniMap1;
    }
}

