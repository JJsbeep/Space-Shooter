namespace zap_program2024
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Broadway", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.Maroon;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.Location = new Point(241, 55);
            button1.Name = "button1";
            button1.Size = new Size(238, 51);
            button1.TabIndex = 0;
            button1.Text = "Exit";
            button1.UseVisualStyleBackColor = true;
            button1.Click += ExitButton_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Broadway", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.Maroon;
            button2.Image = (Image)resources.GetObject("button2.Image");
            button2.Location = new Point(241, 147);
            button2.Name = "button2";
            button2.Size = new Size(238, 55);
            button2.TabIndex = 1;
            button2.Text = "Resume";
            button2.UseVisualStyleBackColor = true;
            button2.Click += ResumeButton_Click;
            // 
            // SettingsFormcs
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(758, 259);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "SettingsFormcs";
            Text = "SettingsFormcs";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
    }
}