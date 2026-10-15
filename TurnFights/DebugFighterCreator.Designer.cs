namespace TurnFights
{
    partial class DebugFighterCreator
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
            terminal = new Label();
            input = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(34, 34);
            label1.Name = "label1";
            label1.Size = new Size(253, 25);
            label1.TabIndex = 0;
            label1.Text = "CUSTOM FIGHTER CREATOR";
            // 
            // terminal
            // 
            terminal.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            terminal.Location = new Point(34, 87);
            terminal.Name = "terminal";
            terminal.Size = new Size(1163, 317);
            terminal.TabIndex = 1;
            // 
            // input
            // 
            input.Location = new Point(34, 429);
            input.Name = "input";
            input.PlaceholderText = "Input...";
            input.Size = new Size(1163, 31);
            input.TabIndex = 2;
            input.KeyPress += input_KeyPress;
            // 
            // DebugFighterCreator
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(1225, 517);
            Controls.Add(input);
            Controls.Add(terminal);
            Controls.Add(label1);
            Name = "DebugFighterCreator";
            Text = "DebugFighterCreator";
            Load += DebugFighterCreator_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label terminal;
        private TextBox input;
    }
}