using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zap_program2024
{
    public  class ScoreLabel
    {
        Form screen;
        public ScoreLabel(GameWindow form) 
        {
            screen = form;
        }
        public Label Score = new Label();
        public int ScoreCount { get; set; } = 0;
        public bool UpgradeReady { get; set; } = false;
        private int upgradeCheckpoint = 5;
        private int checkpointsDistance = 5;
        public void Initialize()
        {
            ScoreCount = 0;
            Score.Size = new Size(87, 30);
            Score.Location = new Point(50, 50);
            Score.Font = new Font("Calibri", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Score.Text = $"SCORE: {ScoreCount}";
            Score.ForeColor = Color.Maroon;
            Score.BackColor = Color.Transparent;
            Score.Visible = true;
            screen.Controls.Add(Score);
        }
        public void Update()
        {
            ScoreCount++;
            Score.Text = $"SCORE: {ScoreCount}";
            CheckUpgrade();
        }
        public void CheckUpgrade()
        {
            if (ScoreCount == upgradeCheckpoint)
            {
                UpgradeReady = true;
                upgradeCheckpoint += checkpointsDistance;
            }

        }
    }
}
