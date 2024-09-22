using System.Runtime.CompilerServices;
using System.Windows.Forms;
using zap_program2024.Entities;
using zap_program2024.Spawning;

namespace zap_program2024
{
    public partial class GameWindow : Form
    {
        public readonly int[] upgradeCodes = { 0, 1, 2, 3 };
        public EnemyController enemyController;
        public ScoreLabel scoreBar;
        public HealthBar healthBar;
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
            healthBar = new HealthBar(this);
            upgradeMessage = new UpgradeMessage(this);
            coin = new Coin(this);
        }
        public void SetAutoMode(bool isOn)
        {
            hero.AutoModeOn = isOn;
        }
        public void spaceship_shooter_Load(object sender, EventArgs e)
        {
            hero.Initialize();
            scoreBar.Initialize();
            healthBar.Initialize();
            enemyController.InitializeController();
            upgradeMessage.Initialize();
            coin.Initialize();
            enemyController.SpawnInitialEnemyWave();
        }
        private void MainEvent(object sender, EventArgs e)
        {
            hero.Move();
            scoreBar.CheckUpgrade();
            if (scoreBar.UpgradeReady)
            {
                coinPicked = coin.PickedUp();
                if (!coin.Available && !coinAppeared)
                {
                    coin.Appear();
                    coinAppeared = true;
                }
                else if (coin.PickedUp() && coinAppeared)
                {
                    upgradeMessage.Show();
                    upgradeAvailable = true;
                    if (hero.AutoModeOn) 
                    {
                        hero.AutoUpgrade();
                    }
                }
            }
        }

        private void KeyIsDown(object? sender, KeyEventArgs e)
        {
            if (hero.AutoModeOn) { return; }
            if (e.KeyCode is Keys.D or Keys.Right)
            {
                hero.moving = true;
                hero.movingLeft = false;
            }
            if (e.KeyCode is Keys.A or Keys.Left)
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
            if (e.KeyCode is Keys.D1 or Keys.NumPad1)
            {
                if (scoreBar.UpgradeReady && upgradeAvailable)
                {
                    hero.PerformUpgrade(upgradeCodes[1]);
                    SetAfterCoinPicked();
                }
            }
            if (e.KeyCode is Keys.D2 or Keys.NumPad2)
            {
                if (scoreBar.UpgradeReady && upgradeAvailable)
                {
                    hero.PerformUpgrade(upgradeCodes[2]);
                    SetAfterCoinPicked();
                }
            }
            if (e.KeyCode is Keys.D3 or Keys.NumPad3)
            {
                if (scoreBar.UpgradeReady && upgradeAvailable)
                {
                    hero.PerformUpgrade(upgradeCodes[3]);
                    SetAfterCoinPicked();
                }
            }
        }
        public void SetAfterCoinPicked()
        {
            scoreBar.UpgradeReady = false;
            upgradeMessage.Hide();
            coinAppeared = false;
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode is Keys.D or Keys.Right)
            {
                hero.moving = false;
                hero.movingLeft = false;
            }
            if (e.KeyCode is Keys.A or Keys.Left)
            {
                hero.moving = false;
                hero.movingLeft = false;
            }
        }

        public void GameOver()
        {
            GameTimer.Stop();
            enemyController.Stop();
            MessageBox.Show($"GAME OVER\nSCORE: {scoreBar.ScoreCount}");
        }
    }
}
