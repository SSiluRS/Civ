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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Units", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Buildings", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("TradeGoods", System.Windows.Forms.HorizontalAlignment.Left);
            this.label2 = new System.Windows.Forms.Label();
            this.foodCountLabel = new System.Windows.Forms.Label();
            this.populationListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.cityMap1 = new MapView.CityMap();
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
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 224);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(122, 121);
            this.listBox1.TabIndex = 4;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(588, 151);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(357, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(588, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(357, 32);
            this.label1.TabIndex = 9;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(677, 420);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(195, 37);
            this.button1.TabIndex = 10;
            this.button1.Text = "Выбрать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView1
            // 
            this.listView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            listViewGroup1.Header = "Units";
            listViewGroup1.Name = "UnitsGroup";
            listViewGroup2.Header = "Buildings";
            listViewGroup2.Name = "BuildingsGroup";
            listViewGroup3.Header = "TradeGoods";
            listViewGroup3.Name = "TradeGoodsGroup";
            this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.listView1.Location = new System.Drawing.Point(588, 183);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(177, 213);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(772, 183);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(173, 212);
            this.listBox2.TabIndex = 11;
            this.listBox2.SelectedValueChanged += new System.EventHandler(this.listBox2_SelectedValueChanged);
            // 
            // cityMap1
            // 
            this.cityMap1.Location = new System.Drawing.Point(262, 76);
            this.cityMap1.Name = "cityMap1";
            this.cityMap1.Size = new System.Drawing.Size(320, 320);
            this.cityMap1.TabIndex = 0;
            this.cityMap1.Text = "cityMap1";
            this.cityMap1.World = null;
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 530);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.progressBar1);
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
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ListBox listBox2;
    }
}