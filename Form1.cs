using System.Runtime.CompilerServices;
using System.Windows.Forms;
using zap_program2024.Entities;

namespace zap_program2024
{
    public partial class GameWindow : Form
    {
        public const int width = 1300;
        public const int heigth = 800;

        public HeroEntity hero = new HeroEntity();

        public void LoadWindow(object sender, EventArgs e)
        {
            
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
