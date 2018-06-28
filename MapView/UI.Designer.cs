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
            this.label2 = new System.Windows.Forms.Label();
            this.foodCountLabel = new System.Windows.Forms.Label();
            this.populationListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cityMap1 = new MapView.CityMap();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Запас еды:";
            // 
            // foodCountLabel
            // 
            this.foodCountLabel.AutoSize = true;
            this.foodCountLabel.Location = new System.Drawing.Point(99, 161);
            this.foodCountLabel.Name = "foodCountLabel";
            this.foodCountLabel.Size = new System.Drawing.Size(35, 13);
            this.foodCountLabel.TabIndex = 2;
            this.foodCountLabel.Text = "label4";
            // 
            // populationListView
            // 
            this.populationListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.populationListView.Location = new System.Drawing.Point(12, 12);
            this.populationListView.Name = "populationListView";
            this.populationListView.Size = new System.Drawing.Size(214, 134);
            this.populationListView.TabIndex = 3;
            this.populationListView.UseCompatibleStateImageBehavior = false;
            this.populationListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 133;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Количество";
            this.columnHeader2.Width = 76;
            // 
            // cityMap1
            // 
            this.cityMap1.Location = new System.Drawing.Point(254, 98);
            this.cityMap1.Name = "cityMap1";
            this.cityMap1.Size = new System.Drawing.Size(300, 300);
            this.cityMap1.TabIndex = 0;
            this.cityMap1.Text = "cityMap1";
            this.cityMap1.World = null;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 224);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(122, 121);
            this.listBox1.TabIndex = 4;
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.populationListView);
            this.Controls.Add(this.foodCountLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cityMap1);
            this.Name = "UI";
            this.Text = "UI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CityMap cityMap1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label foodCountLabel;
        private System.Windows.Forms.ListView populationListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListBox listBox1;
    }
}