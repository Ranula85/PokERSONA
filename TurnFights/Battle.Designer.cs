namespace TurnFights
{
    partial class Battle
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
            label1 = new Label();
            fighter1_Name = new Label();
            fighter1_Stats = new Label();
            fighter1_move1_btt = new Button();
            fighter1_move1_info = new Label();
            fighter1_move2_info = new Label();
            fighter1_move2_btt = new Button();
            fighter1_move3_info = new Label();
            fighter1_move3_btt = new Button();
            fighter1_move4_info = new Label();
            fighter1_move4_btt = new Button();
            fighter2_move4_info = new Label();
            fighter2_move4_btt = new Button();
            fighter2_move3_info = new Label();
            fighter2_move3_btt = new Button();
            fighter2_move2_info = new Label();
            fighter2_move2_btt = new Button();
            fighter2_move1_info = new Label();
            fighter2_move1_btt = new Button();
            fighter2_Stats = new Label();
            fighter2_Name = new Label();
            fighter1TurnBox = new Label();
            fighter2TurnBox = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(30, 9);
            label1.Name = "label1";
            label1.Size = new Size(965, 138);
            label1.TabIndex = 0;
            label1.Text = "Let's fight!";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // fighter1_Name
            // 
            fighter1_Name.AutoSize = true;
            fighter1_Name.BackColor = Color.Transparent;
            fighter1_Name.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            fighter1_Name.Location = new Point(60, 237);
            fighter1_Name.Name = "fighter1_Name";
            fighter1_Name.Size = new Size(245, 45);
            fighter1_Name.TabIndex = 1;
            fighter1_Name.Text = "Fighter 1 name";
            fighter1_Name.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // fighter1_Stats
            // 
            fighter1_Stats.AutoSize = true;
            fighter1_Stats.BackColor = Color.Transparent;
            fighter1_Stats.Font = new Font("Segoe UI", 11F);
            fighter1_Stats.Location = new Point(60, 294);
            fighter1_Stats.Name = "fighter1_Stats";
            fighter1_Stats.Size = new Size(148, 30);
            fighter1_Stats.TabIndex = 2;
            fighter1_Stats.Text = "Fighter 1 stats";
            // 
            // fighter1_move1_btt
            // 
            fighter1_move1_btt.BackColor = Color.Transparent;
            fighter1_move1_btt.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            fighter1_move1_btt.Location = new Point(60, 437);
            fighter1_move1_btt.Name = "fighter1_move1_btt";
            fighter1_move1_btt.Size = new Size(139, 51);
            fighter1_move1_btt.TabIndex = 3;
            fighter1_move1_btt.Text = "moveType";
            fighter1_move1_btt.UseVisualStyleBackColor = false;
            fighter1_move1_btt.Click += fighter1_move1_btt_Click;
            // 
            // fighter1_move1_info
            // 
            fighter1_move1_info.AutoSize = true;
            fighter1_move1_info.BackColor = Color.Transparent;
            fighter1_move1_info.Font = new Font("Segoe UI", 11F);
            fighter1_move1_info.Location = new Point(60, 491);
            fighter1_move1_info.Name = "fighter1_move1_info";
            fighter1_move1_info.Size = new Size(113, 30);
            fighter1_move1_info.TabIndex = 4;
            fighter1_move1_info.Text = "moveStats";
            // 
            // fighter1_move2_info
            // 
            fighter1_move2_info.AutoSize = true;
            fighter1_move2_info.BackColor = Color.Transparent;
            fighter1_move2_info.Font = new Font("Segoe UI", 11F);
            fighter1_move2_info.Location = new Point(253, 491);
            fighter1_move2_info.Name = "fighter1_move2_info";
            fighter1_move2_info.Size = new Size(113, 30);
            fighter1_move2_info.TabIndex = 6;
            fighter1_move2_info.Text = "moveStats";
            // 
            // fighter1_move2_btt
            // 
            fighter1_move2_btt.BackColor = Color.Transparent;
            fighter1_move2_btt.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            fighter1_move2_btt.Location = new Point(254, 437);
            fighter1_move2_btt.Name = "fighter1_move2_btt";
            fighter1_move2_btt.Size = new Size(139, 51);
            fighter1_move2_btt.TabIndex = 5;
            fighter1_move2_btt.Text = "moveType";
            fighter1_move2_btt.UseVisualStyleBackColor = false;
            fighter1_move2_btt.Click += fighter1_move2_btt_Click;
            // 
            // fighter1_move3_info
            // 
            fighter1_move3_info.AutoSize = true;
            fighter1_move3_info.BackColor = Color.Transparent;
            fighter1_move3_info.Font = new Font("Segoe UI", 11F);
            fighter1_move3_info.Location = new Point(60, 630);
            fighter1_move3_info.Name = "fighter1_move3_info";
            fighter1_move3_info.Size = new Size(113, 30);
            fighter1_move3_info.TabIndex = 8;
            fighter1_move3_info.Text = "moveStats";
            // 
            // fighter1_move3_btt
            // 
            fighter1_move3_btt.BackColor = Color.Transparent;
            fighter1_move3_btt.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            fighter1_move3_btt.Location = new Point(60, 576);
            fighter1_move3_btt.Name = "fighter1_move3_btt";
            fighter1_move3_btt.Size = new Size(139, 51);
            fighter1_move3_btt.TabIndex = 7;
            fighter1_move3_btt.Text = "moveType";
            fighter1_move3_btt.UseVisualStyleBackColor = false;
            fighter1_move3_btt.Click += fighter1_move3_btt_Click;
            // 
            // fighter1_move4_info
            // 
            fighter1_move4_info.AutoSize = true;
            fighter1_move4_info.BackColor = Color.Transparent;
            fighter1_move4_info.Font = new Font("Segoe UI", 11F);
            fighter1_move4_info.Location = new Point(253, 630);
            fighter1_move4_info.Name = "fighter1_move4_info";
            fighter1_move4_info.Size = new Size(113, 30);
            fighter1_move4_info.TabIndex = 10;
            fighter1_move4_info.Text = "moveStats";
            // 
            // fighter1_move4_btt
            // 
            fighter1_move4_btt.BackColor = Color.Transparent;
            fighter1_move4_btt.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            fighter1_move4_btt.Location = new Point(254, 576);
            fighter1_move4_btt.Name = "fighter1_move4_btt";
            fighter1_move4_btt.Size = new Size(139, 51);
            fighter1_move4_btt.TabIndex = 9;
            fighter1_move4_btt.Text = "moveType";
            fighter1_move4_btt.UseVisualStyleBackColor = false;
            fighter1_move4_btt.Click += fighter1_move4_btt_Click;
            // 
            // fighter2_move4_info
            // 
            fighter2_move4_info.AutoSize = true;
            fighter2_move4_info.BackColor = Color.Transparent;
            fighter2_move4_info.Font = new Font("Segoe UI", 11F);
            fighter2_move4_info.Location = new Point(781, 630);
            fighter2_move4_info.Name = "fighter2_move4_info";
            fighter2_move4_info.Size = new Size(113, 30);
            fighter2_move4_info.TabIndex = 20;
            fighter2_move4_info.Text = "moveStats";
            // 
            // fighter2_move4_btt
            // 
            fighter2_move4_btt.BackColor = Color.Transparent;
            fighter2_move4_btt.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            fighter2_move4_btt.Location = new Point(781, 576);
            fighter2_move4_btt.Name = "fighter2_move4_btt";
            fighter2_move4_btt.Size = new Size(139, 51);
            fighter2_move4_btt.TabIndex = 19;
            fighter2_move4_btt.Text = "moveType";
            fighter2_move4_btt.UseVisualStyleBackColor = false;
            fighter2_move4_btt.Click += fighter2_move4_btt_Click;
            // 
            // fighter2_move3_info
            // 
            fighter2_move3_info.AutoSize = true;
            fighter2_move3_info.BackColor = Color.Transparent;
            fighter2_move3_info.Font = new Font("Segoe UI", 11F);
            fighter2_move3_info.Location = new Point(586, 630);
            fighter2_move3_info.Name = "fighter2_move3_info";
            fighter2_move3_info.Size = new Size(113, 30);
            fighter2_move3_info.TabIndex = 18;
            fighter2_move3_info.Text = "moveStats";
            // 
            // fighter2_move3_btt
            // 
            fighter2_move3_btt.BackColor = Color.Transparent;
            fighter2_move3_btt.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            fighter2_move3_btt.Location = new Point(587, 576);
            fighter2_move3_btt.Name = "fighter2_move3_btt";
            fighter2_move3_btt.Size = new Size(139, 51);
            fighter2_move3_btt.TabIndex = 17;
            fighter2_move3_btt.Text = "moveType";
            fighter2_move3_btt.UseVisualStyleBackColor = false;
            fighter2_move3_btt.Click += fighter2_move3_btt_Click;
            // 
            // fighter2_move2_info
            // 
            fighter2_move2_info.AutoSize = true;
            fighter2_move2_info.BackColor = Color.Transparent;
            fighter2_move2_info.Font = new Font("Segoe UI", 11F);
            fighter2_move2_info.Location = new Point(781, 491);
            fighter2_move2_info.Name = "fighter2_move2_info";
            fighter2_move2_info.Size = new Size(113, 30);
            fighter2_move2_info.TabIndex = 16;
            fighter2_move2_info.Text = "moveStats";
            // 
            // fighter2_move2_btt
            // 
            fighter2_move2_btt.BackColor = Color.Transparent;
            fighter2_move2_btt.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            fighter2_move2_btt.Location = new Point(781, 437);
            fighter2_move2_btt.Name = "fighter2_move2_btt";
            fighter2_move2_btt.Size = new Size(139, 51);
            fighter2_move2_btt.TabIndex = 15;
            fighter2_move2_btt.Text = "moveType";
            fighter2_move2_btt.UseVisualStyleBackColor = false;
            fighter2_move2_btt.Click += fighter2_move2_btt_Click;
            // 
            // fighter2_move1_info
            // 
            fighter2_move1_info.AutoSize = true;
            fighter2_move1_info.BackColor = Color.Transparent;
            fighter2_move1_info.Font = new Font("Segoe UI", 11F);
            fighter2_move1_info.Location = new Point(586, 491);
            fighter2_move1_info.Name = "fighter2_move1_info";
            fighter2_move1_info.Size = new Size(113, 30);
            fighter2_move1_info.TabIndex = 14;
            fighter2_move1_info.Text = "moveStats";
            // 
            // fighter2_move1_btt
            // 
            fighter2_move1_btt.BackColor = Color.Transparent;
            fighter2_move1_btt.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            fighter2_move1_btt.Location = new Point(587, 437);
            fighter2_move1_btt.Name = "fighter2_move1_btt";
            fighter2_move1_btt.Size = new Size(139, 51);
            fighter2_move1_btt.TabIndex = 13;
            fighter2_move1_btt.Text = "moveType";
            fighter2_move1_btt.UseVisualStyleBackColor = false;
            fighter2_move1_btt.Click += fighter2_move1_btt_Click;
            // 
            // fighter2_Stats
            // 
            fighter2_Stats.AutoSize = true;
            fighter2_Stats.BackColor = Color.Transparent;
            fighter2_Stats.Font = new Font("Segoe UI", 11F);
            fighter2_Stats.Location = new Point(833, 294);
            fighter2_Stats.Name = "fighter2_Stats";
            fighter2_Stats.Size = new Size(148, 30);
            fighter2_Stats.TabIndex = 12;
            fighter2_Stats.Text = "Fighter 2 stats";
            fighter2_Stats.TextAlign = ContentAlignment.TopRight;
            // 
            // fighter2_Name
            // 
            fighter2_Name.AutoSize = true;
            fighter2_Name.BackColor = Color.Transparent;
            fighter2_Name.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            fighter2_Name.Location = new Point(587, 237);
            fighter2_Name.Name = "fighter2_Name";
            fighter2_Name.Size = new Size(245, 45);
            fighter2_Name.TabIndex = 11;
            fighter2_Name.Text = "Fighter 2 name";
            fighter2_Name.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // fighter1TurnBox
            // 
            fighter1TurnBox.AutoEllipsis = true;
            fighter1TurnBox.BackColor = Color.Transparent;
            fighter1TurnBox.BorderStyle = BorderStyle.Fixed3D;
            fighter1TurnBox.Location = new Point(30, 211);
            fighter1TurnBox.Name = "fighter1TurnBox";
            fighter1TurnBox.Size = new Size(432, 481);
            fighter1TurnBox.TabIndex = 21;
            // 
            // fighter2TurnBox
            // 
            fighter2TurnBox.AutoEllipsis = true;
            fighter2TurnBox.BackColor = Color.Transparent;
            fighter2TurnBox.BorderStyle = BorderStyle.Fixed3D;
            fighter2TurnBox.Location = new Point(563, 211);
            fighter2TurnBox.Name = "fighter2TurnBox";
            fighter2TurnBox.Size = new Size(432, 481);
            fighter2TurnBox.TabIndex = 22;
            // 
            // Battle
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.RosyBrown;
            ClientSize = new Size(1007, 751);
            Controls.Add(fighter2_move4_info);
            Controls.Add(fighter2_move4_btt);
            Controls.Add(fighter2_move3_info);
            Controls.Add(fighter2_move3_btt);
            Controls.Add(fighter2_move2_info);
            Controls.Add(fighter2_move2_btt);
            Controls.Add(fighter2_move1_info);
            Controls.Add(fighter2_move1_btt);
            Controls.Add(fighter2_Stats);
            Controls.Add(fighter2_Name);
            Controls.Add(fighter1_move4_info);
            Controls.Add(fighter1_move4_btt);
            Controls.Add(fighter1_move3_info);
            Controls.Add(fighter1_move3_btt);
            Controls.Add(fighter1_move2_info);
            Controls.Add(fighter1_move2_btt);
            Controls.Add(fighter1_move1_info);
            Controls.Add(fighter1_move1_btt);
            Controls.Add(fighter1_Stats);
            Controls.Add(fighter1_Name);
            Controls.Add(label1);
            Controls.Add(fighter1TurnBox);
            Controls.Add(fighter2TurnBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Battle";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Battle";
            Load += Battle_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label fighter1_Name;
        private Label fighter1_Stats;
        private Button fighter1_move1_btt;
        private Label fighter1_move1_info;
        private Label fighter1_move2_info;
        private Button fighter1_move2_btt;
        private Label fighter1_move3_info;
        private Button fighter1_move3_btt;
        private Label fighter1_move4_info;
        private Button fighter1_move4_btt;
        private Label fighter2_move4_info;
        private Button fighter2_move4_btt;
        private Label fighter2_move3_info;
        private Button fighter2_move3_btt;
        private Label fighter2_move2_info;
        private Button fighter2_move2_btt;
        private Label fighter2_move1_info;
        private Button fighter2_move1_btt;
        private Label fighter2_Stats;
        private Label fighter2_Name;
        private Label fighter1TurnBox;
        private Label fighter2TurnBox;
    }
}