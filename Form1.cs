using System.Runtime.CompilerServices;
using System.Windows.Forms;
using zap_program2024.Entities;

namespace zap_program2024
{
    public partial class GameWindow : Form
    {
        public HeroEntity hero = new HeroEntity();

        const int basicEnemiesAmount = 44;
        const int midEnemiesAmount = 13;
        const int hardEnemiesAmount = 6;
        const int bossEnemiesAmount = 2;

        public List<BasicEnemyEntity> basicEnemyWave = new List<BasicEnemyEntity>(basicEnemiesAmount);
        public List<MidEnemyEntity> midEnemyWave = new List<MidEnemyEntity>(midEnemiesAmount);
        public List<HardEnemyEntity> hardEnemyWave = new List<HardEnemyEntity>(hardEnemiesAmount);
        public List<BossEnemyEntity> bossEnemyEntities = new List<BossEnemyEntity>(bossEnemiesAmount);


        public void LoadWindow(object sender, EventArgs e)
        {
            const int firstLineBasicEnemies = 19;
            const int secondLineBasicEnemies = 29;
            const int thirdLineBasicEnemies = 36;
            const int secondLineMidEnemies = 7;
            const int thirdLineMidEnemies = 3;
            const int thirdLineHardEnemies = 4;
        }
        public void InitializeEnemies()
        {

        }
        public GameWindow()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            ResetGame();
        }



        private void MainEvent(object sender, EventArgs e)
        {

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {

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

        private void ScoreBar_Click(object sender, EventArgs e)
        {

        }
    }
}
