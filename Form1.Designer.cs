namespace zap_program2024
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
            ScoreBar = new Label();
            GameTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // ScoreBar
            // 
            ScoreBar.AutoSize = true;
            ScoreBar.Font = new Font("Stencil", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ScoreBar.ForeColor = Color.Maroon;
            ScoreBar.Location = new Point(50, 49);
            ScoreBar.Name = "ScoreBar";
            ScoreBar.Size = new Size(87, 21);
            ScoreBar.TabIndex = 5;
            ScoreBar.Text = "SCORE: 0\r\n";
            ScoreBar.Click += ScoreBar_Click;
            // 
            // GameTimer
            // 
            GameTimer.Enabled = true;
            GameTimer.Interval = 40;
            GameTimer.Tick += MainEvent;
            // 
            // GameWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 0, 64);
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1282, 753);
            Controls.Add(ScoreBar);
            Name = "GameWindow";
            Text = "Form1";
            Load += spaceship_shooter_Load;
            KeyDown += KeyIsDown;
            KeyUp += KeyIsUp;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label ScoreBar;
        private System.Windows.Forms.Timer GameTimer;
    }
}
