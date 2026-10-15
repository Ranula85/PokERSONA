namespace TurnFights
{
    partial class HelpInfo
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
            circleImg = new PictureBox();
            mapCircleBtt = new Button();
            mapTableBtt = new Button();
            tableImg = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)circleImg).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tableImg).BeginInit();
            SuspendLayout();
            // 
            // circleImg
            // 
            circleImg.Image = Properties.Resources.elementCircle;
            circleImg.Location = new Point(12, 12);
            circleImg.Name = "circleImg";
            circleImg.Size = new Size(633, 633);
            circleImg.SizeMode = PictureBoxSizeMode.Zoom;
            circleImg.TabIndex = 0;
            circleImg.TabStop = false;
            // 
            // mapCircleBtt
            // 
            mapCircleBtt.Enabled = false;
            mapCircleBtt.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            mapCircleBtt.Location = new Point(789, 12);
            mapCircleBtt.Name = "mapCircleBtt";
            mapCircleBtt.Size = new Size(112, 46);
            mapCircleBtt.TabIndex = 1;
            mapCircleBtt.Text = "Circle";
            mapCircleBtt.UseVisualStyleBackColor = true;
            mapCircleBtt.Click += mapCircleBtt_Click;
            // 
            // mapTableBtt
            // 
            mapTableBtt.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            mapTableBtt.Location = new Point(789, 73);
            mapTableBtt.Name = "mapTableBtt";
            mapTableBtt.Size = new Size(112, 46);
            mapTableBtt.TabIndex = 2;
            mapTableBtt.Text = "Table";
            mapTableBtt.UseVisualStyleBackColor = true;
            mapTableBtt.Click += mapTableBtt_Click;
            // 
            // tableImg
            // 
            tableImg.Image = Properties.Resources.elementTable;
            tableImg.Location = new Point(12, 12);
            tableImg.Name = "tableImg";
            tableImg.Size = new Size(710, 706);
            tableImg.SizeMode = PictureBoxSizeMode.Zoom;
            tableImg.TabIndex = 3;
            tableImg.TabStop = false;
            tableImg.Visible = false;
            // 
            // HelpInfo
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(913, 730);
            Controls.Add(tableImg);
            Controls.Add(mapTableBtt);
            Controls.Add(mapCircleBtt);
            Controls.Add(circleImg);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "HelpInfo";
            Text = "HelpInfo";
            ((System.ComponentModel.ISupportInitialize)circleImg).EndInit();
            ((System.ComponentModel.ISupportInitialize)tableImg).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox circleImg;
        private Button mapCircleBtt;
        private Button mapTableBtt;
        private PictureBox tableImg;
    }
}