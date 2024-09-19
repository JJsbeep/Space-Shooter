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
        public readonly int[] upgradeCodes = { 0, 1, 2, 3 };
        public EnemyController enemyController;
        public ScoreLabel scoreBar;
        public UpgradeMessage upgradeMessage;
        public HeroEntity hero;
        public Coin coin;
        public bool upgradeAvailable = false;
        public bool coinAppeared = false;
        public bool coinPicked = false;
        public GameWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            hero = new HeroEntity(this);
            enemyController = new EnemyController(this);
            scoreBar = new ScoreLabel(this);
            upgradeMessage = new UpgradeMessage(this);
            coin = new Coin(this);
        }
        public void spaceship_shooter_Load(object sender, EventArgs e)
        {
            hero.Initialize();
            scoreBar.Initialize();
            enemyController.InitializeController();
            upgradeMessage.Initialize();
            coin.Initilaize();
            enemyController.SpawnInitialEnemyWave();
            //this.Controls.Add(hero.icon);
        }
        private void MainEvent(object sender, EventArgs e)
        {
            hero.Move();
            scoreBar.CheckUpgrade();
            if (scoreBar.UpgradeReady)
            {
                 coinPicked = coin.PickedUp();
                //scoreBar.Update();
                if (!coin.Availablable && !coinAppeared)
                {
                    coin.Appear();
                    coinAppeared = true;
                }
                else if (coin.PickedUp() && coinAppeared)
                {
                    upgradeMessage.Show();
                    upgradeAvailable = true;
                    //coinAppeared = false;
                }
            }
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
            if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
            {
                if (scoreBar.UpgradeReady && upgradeAvailable)
                {
                    hero.PerformUpgrade(upgradeCodes[1]);
                    scoreBar.UpgradeReady = false;
                    upgradeMessage.Hide();
                    coinAppeared = false;
                }
            }
            if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
            {
                if (scoreBar.UpgradeReady && upgradeAvailable)
                {
                    hero.PerformUpgrade(upgradeCodes[2]);
                    scoreBar.UpgradeReady = false;
                    upgradeMessage.Hide();
                    coinAppeared = false;
                }
            }
            if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
            {
                if (scoreBar.UpgradeReady && upgradeAvailable)
                {
                    hero.PerformUpgrade(upgradeCodes[3]);
                    scoreBar.UpgradeReady = false;
                    upgradeMessage.Hide();
                    coinAppeared = false;
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
