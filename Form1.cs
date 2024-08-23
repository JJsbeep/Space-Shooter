using System.Runtime.CompilerServices;
using System.Windows.Forms;
using zap_program2024.Entities;

namespace zap_program2024
{
    public partial class GameWindow : Form
    {
        /*public HeroEntity hero = new HeroEntity();
        
        public List<BasicEnemyEntity> basicEnemyWave = new List<BasicEnemyEntity>();
        public List<MidEnemyEntity> midEnemyWave = new List<MidEnemyEntity>();
        public List<HardEnemyEntity> hardEnemyWave = new List<HardEnemyEntity>();
        public List<BossEnemyEntity> bossEnemyEntities = new List<BossEnemyEntity>();*/
        



        public GameWindow()
        {
            InitializeComponent();
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
    }
}
