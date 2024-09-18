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

        public EnemyController enemyController;

        public HeroEntity hero;
        public GameWindow()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            hero = new HeroEntity(this);
            enemyController = new EnemyController(this);
        }
        public void spaceship_shooter_Load(object sender, EventArgs e)
        {
            hero.Initialize();
            enemyController.SpawnInitialEnemyWave(this);
            this.Controls.Add(hero.icon);
            enemyController.InitializeController();
        }
        private void MainEvent(object sender, EventArgs e)
        {
            hero.Move();
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
            if (e.KeyCode == Keys.Space)
            {
                hero.ShootingReady = hero.FireReady();
                if (hero.ShootingReady)
                {
                    hero.HeroShoot();
                }
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

        private void ResetGame()
        {
            GameTimer.Start();
        }

        private void GameOver()
        {

        }

        private void ScoreBar_Click(object sender, EventArgs e)
        {

        }
    }
}
