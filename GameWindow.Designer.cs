﻿namespace zap_program2024
{
    public partial class GameWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameWindow));
            GameTimer = new System.Windows.Forms.Timer(components);
            SettingsButton = new Button();
            SuspendLayout();
            // 
            // GameTimer
            // 
            GameTimer.Enabled = true;
            GameTimer.Interval = 40;
            GameTimer.Tick += MainEvent;
            // 
            // SettingsButton
            // 
            SettingsButton.BackColor = Color.Transparent;
            SettingsButton.BackgroundImage = (Image)resources.GetObject("SettingsButton.BackgroundImage");
            SettingsButton.Image = (Image)resources.GetObject("SettingsButton.Image");
            SettingsButton.Location = new Point(1217, 41);
            SettingsButton.Name = "SettingsButton";
            SettingsButton.Size = new Size(29, 30);
            SettingsButton.TabIndex = 0;
            SettingsButton.TextAlign = ContentAlignment.MiddleRight;
            SettingsButton.UseVisualStyleBackColor = false;
            SettingsButton.Click += SettingsButtonClick;
            SettingsButton.TabStop = false;
            // 
            // GameWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1282, 753);
            Controls.Add(SettingsButton);
            Name = "GameWindow";
            Text = "Form1";
            Load += spaceship_shooter_Load;
            KeyPreview = true;
            KeyDown += KeyIsDown;
            KeyUp += KeyIsUp;
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer GameTimer;
        private Button SettingsButton;
    }
}
