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
        public bool gamePaused = false;
        public SettingsForm settingsForm;
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
            settingsForm = new SettingsForm(this);
            this.Focus();
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
        public void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (hero.AutoModeOn) { return; }
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                hero.moving = true;
                hero.movingLeft = false;
            }
            if (e.KeyCode == Keys.A || e.KeyCode ==  Keys.Left)
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
                    SetAfterCoinPicked();
                }
            }
            if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
            {
                if (scoreBar.UpgradeReady && upgradeAvailable)
                {
                    hero.PerformUpgrade(upgradeCodes[2]);
                    SetAfterCoinPicked();
                }
            }
            if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
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
        public void GameOver()
        {
            GameTimer.Stop();
            enemyController.Stop();
            MessageBox.Show($"GAME OVER\nSCORE: {scoreBar.ScoreCount}");
        }

        public void Resume()
        {
            gamePaused = false; 
            enemyController.Resume();
            coin.pickTimer.Start();
        }

        private void SettingsButtonClick(object sender, EventArgs e)
        {
            settingsForm.Show();
            gamePaused = true;
            enemyController.Stop();
            coin.pickTimer.Stop();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.D || keyData == Keys.Right ||
                keyData == Keys.A || keyData == Keys.Left ||
                keyData == Keys.Space ||
                keyData == Keys.D1 || keyData == Keys.NumPad1 ||
                keyData == Keys.D2 || keyData == Keys.NumPad2 ||
                keyData == Keys.D3 || keyData == Keys.NumPad3)
            {
                KeyIsDown(this, new KeyEventArgs(keyData));
                return true; // Indicate that the key event was handled
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
