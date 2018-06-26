namespace MapView
{
    partial class UI
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
            this.cityMap1 = new MapView.CityMap();
            this.SuspendLayout();
            // 
            // cityMap1
            // 
            this.cityMap1.Location = new System.Drawing.Point(254, 98);
            this.cityMap1.Name = "cityMap1";
            this.cityMap1.Size = new System.Drawing.Size(300, 300);
            this.cityMap1.TabIndex = 0;
            this.cityMap1.Text = "cityMap1";
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cityMap1);
            this.Name = "UI";
            this.Text = "UI";
            this.ResumeLayout(false);

        }

        #endregion

        private CityMap cityMap1;
    }
}