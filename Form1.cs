using zap_program2024.Entities;
namespace zap_program2024
{
    public partial class SpaceshipShooter : Form
    {
        const int maxSizeX = 1179;
        const int maxSIzeY = 800;
        const int minSizeX = 10;

        bool moveR;
        bool moveL;
        bool isShooting;
        bool isGameOver;

        int score;
        int bulletSpeed;
        int heroSpeed = 12;
        int enemySpeed = 8;

        public SpaceshipShooter()
        {
            InitializeComponent();
            ResetGame();
        }



        private void MainEvent(object sender, EventArgs e)
        {
            ScoreBar.Text = score.ToString();
            MoveEnemies();

            if (moveL == true && HeroShip.Left > minSizeX)
            {
                HeroShip.Left -= heroSpeed;
            }
            if (moveR == true && HeroShip.Left < maxSizeX)
            {
                HeroShip.Left += heroSpeed;
            }


        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                moveL = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                moveR = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                moveL = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                moveR = false;
            }

        }

        private void MoveEnemies()
        {
            BasicEnemy.Top += enemySpeed;
            MidEnemy.Top += enemySpeed;
            HardEnemy.Top += enemySpeed;
            BossEnemy.Top += enemySpeed;
        }

        private void ResetGame()
        {
            GameTimer.Start();
        }

        private void GameOver()
        {

        }

        private void spaceship_shooter_Load(object sender, EventArgs e)
        {

        }

        private void HeroShip_Click(object sender, EventArgs e)
        {

        }

        private void BasicEnemy_Click(object sender, EventArgs e)
        {

        }

        private void BossEnemy_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
