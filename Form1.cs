using System.Runtime.CompilerServices;
using System.Windows.Forms;
using zap_program2024.Entities;
using zap_program2024.Spawning;

namespace zap_program2024
{
    public partial class GameWindow : Form
    {
        public const int width = 1300;
        public const int heigth = 800;

        public EnemyController enemyController = new EnemyController();

        public HeroEntity hero = new HeroEntity();

        public void spaceship_shooter_Load(object sender, EventArgs e)
        {
            hero.Initialize(this);
            enemyController.SpawnInitialEnemyWave(this);
            this.Controls.Add(hero.icon);
        }
        public GameWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            ResetGame();
        }
        private void MainEvent(object sender, EventArgs e)
        {
            enemyController.moveFirstWave(this, GameTimer);
            hero.Move(this, GameTimer);
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                hero.moving = true;
                hero.movingLeft = false;
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                hero.moving = true;
                hero.movingLeft = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                hero.moving = false;
                hero.movingLeft = false;
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                hero.moving = false;
                hero.movingLeft = false;
            }
        }

        private void MoveEnemies()
        {

        }

        private void ResetGame()
        {
            GameTimer.Start();
        }

        private void GameOver()
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

        private void ScoreBar_Click(object sender, EventArgs e)
        {

        }
    }
}
