namespace zap_program2024
{
    partial class SpaceshipShooter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpaceshipShooter));
            HeroShip = new PictureBox();
            BasicEnemy = new PictureBox();
            MidEnemy = new PictureBox();
            HardEnemy = new PictureBox();
            BossEnemy = new PictureBox();
            ScoreBar = new Label();
            pictureBox1 = new PictureBox();
            GameTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)HeroShip).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BasicEnemy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MidEnemy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)HardEnemy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BossEnemy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // HeroShip
            // 
            HeroShip.Image = (Image)resources.GetObject("HeroShip.Image");
            HeroShip.Location = new Point(589, 648);
            HeroShip.Name = "HeroShip";
            HeroShip.Size = new Size(103, 103);
            HeroShip.SizeMode = PictureBoxSizeMode.StretchImage;
            HeroShip.TabIndex = 0;
            HeroShip.TabStop = false;
            HeroShip.Click += HeroShip_Click;
            // 
            // BasicEnemy
            // 
            BasicEnemy.Image = (Image)resources.GetObject("BasicEnemy.Image");
            BasicEnemy.Location = new Point(350, 78);
            BasicEnemy.Name = "BasicEnemy";
            BasicEnemy.Size = new Size(93, 101);
            BasicEnemy.SizeMode = PictureBoxSizeMode.StretchImage;
            BasicEnemy.TabIndex = 1;
            BasicEnemy.TabStop = false;
            BasicEnemy.Click += BasicEnemy_Click;
            // 
            // MidEnemy
            // 
            MidEnemy.Image = (Image)resources.GetObject("MidEnemy.Image");
            MidEnemy.Location = new Point(508, 78);
            MidEnemy.Name = "MidEnemy";
            MidEnemy.Size = new Size(107, 101);
            MidEnemy.SizeMode = PictureBoxSizeMode.StretchImage;
            MidEnemy.TabIndex = 2;
            MidEnemy.TabStop = false;
            // 
            // HardEnemy
            // 
            HardEnemy.Image = (Image)resources.GetObject("HardEnemy.Image");
            HardEnemy.Location = new Point(663, 49);
            HardEnemy.Name = "HardEnemy";
            HardEnemy.Size = new Size(140, 151);
            HardEnemy.SizeMode = PictureBoxSizeMode.StretchImage;
            HardEnemy.TabIndex = 3;
            HardEnemy.TabStop = false;
            // 
            // BossEnemy
            // 
            BossEnemy.Image = (Image)resources.GetObject("BossEnemy.Image");
            BossEnemy.Location = new Point(851, 60);
            BossEnemy.Name = "BossEnemy";
            BossEnemy.Size = new Size(156, 140);
            BossEnemy.SizeMode = PictureBoxSizeMode.StretchImage;
            BossEnemy.TabIndex = 4;
            BossEnemy.TabStop = false;
            BossEnemy.Click += BossEnemy_Click;
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
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(632, 599);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(10, 10);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // GameTimer
            // 
            GameTimer.Enabled = true;
            GameTimer.Interval = 20;
            GameTimer.Tick += MainEvent;
            // 
            // SpaceshipShooter
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 0, 64);
            ClientSize = new Size(1282, 753);
            Controls.Add(pictureBox1);
            Controls.Add(ScoreBar);
            Controls.Add(BossEnemy);
            Controls.Add(MidEnemy);
            Controls.Add(BasicEnemy);
            Controls.Add(HeroShip);
            Controls.Add(HardEnemy);
            Name = "SpaceshipShooter";
            Text = "Form1";
            Load += spaceship_shooter_Load;
            KeyDown += KeyIsDown;
            KeyUp += KeyIsUp;
            ((System.ComponentModel.ISupportInitialize)HeroShip).EndInit();
            ((System.ComponentModel.ISupportInitialize)BasicEnemy).EndInit();
            ((System.ComponentModel.ISupportInitialize)MidEnemy).EndInit();
            ((System.ComponentModel.ISupportInitialize)HardEnemy).EndInit();
            ((System.ComponentModel.ISupportInitialize)BossEnemy).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox BasicEnemy;
        private PictureBox MidEnemy;
        private PictureBox HardEnemy;
        private PictureBox BossEnemy;
        private Label ScoreBar;
        private PictureBox pictureBox1;
        private System.Windows.Forms.Timer GameTimer;
        public PictureBox HeroShip;
    }
}
