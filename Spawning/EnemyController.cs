using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zap_program2024.Entities;
using Timer = System.Windows.Forms.Timer;

namespace zap_program2024.Spawning
{
    public class EnemyController
    {
        Form screen;

        public EnemyController(Form form)
        {
            screen = form;
        }

        private const int enemiesAmount = 12;
        private const int enemyLinesAmount = 4;
        private int currShipIndex = 0;
        int lineCounter = 0;
        NextEnemyPicker nextEnemyPicker = new NextEnemyPicker();
        Random rnd = new Random();
        private const int initialXpos = 500;
        private const int initialYpos = 292;
        private const int shiftX = 77;
        private const int shiftY = 30;
        private const int enemiesPerLine = 3;

        private bool enemyGone = false;

        private List<(int, int)> enemyDifficulutyChanceToSpawn = new List<(int, int)>()
        {
            (1, 65),
            (2, 20),
            (3, 10),
            (4, 5),
        };

        private Vector2d spawnCoordinates = new(initialXpos, initialYpos);

        private List<AbstractEntity> Enemies = new List<AbstractEntity>();

        private int movingPeriod = 1750;
        private int spawnPeriod = 4000;
        //amount of enemies of each entity on each line
        public List<int[]> NumsOfEnemiesOnLines = new List<int[]>()
        {
            new int[] {3, 0, 0, 0},
            new int[] {1, 2, 0, 0},
            new int[] {1, 1, 1, 0},
            new int[] {0, 1, 1, 1},
        };

        Timer MoveControlTimer = new Timer();
        Timer SpawnControlTimer = new Timer();

        public void ResetIndexing()
        {
            if(currShipIndex >= enemiesAmount)
            {
                enemyGone = true;
                currShipIndex = 0;
            }
        }
        private void InitializeMoveTimer()
        {
            MoveControlTimer.Interval = movingPeriod;
            MoveControlTimer.Enabled = true;
            MoveControlTimer.Start();
            MoveControlTimer.Tick += MoveTimer_Tick;
        }
        private void InitializeSpawnTimer()
        {
            SpawnControlTimer.Interval = spawnPeriod;
            SpawnControlTimer.Enabled = true;
            SpawnControlTimer.Start();
            SpawnControlTimer.Tick += SpawnTimer_Tick;
        }
        public void InitializeController()
        {
            InitializeMoveTimer();
            InitializeSpawnTimer();
        }
        private void DeleteDead()
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].Dead)
                {
                    Enemies.RemoveAt(i);
                }
            }
        }
        public void MoveTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemy();
            currShipIndex++;
            ResetIndexing();
        }
        public void SpawnTimer_Tick(object sender, EventArgs e)
        {
            DeleteDead();
            Respawn();
        }
        private AbstractEntity CreateEnemy(int enemydifficulty)
        {
            AbstractEntity enemy;
            switch (enemydifficulty)
            {
                case 1:
                    enemy = new BasicEnemyEntity(screen);
                    break;
                case 2:
                    enemy = new MidEnemyEntity(screen);
                    break;
                case 3:
                    enemy = new HardEnemyEntity(screen);
                    break;
                case 4:
                    enemy = new BossEnemyEntity(screen);
                    break;
                default:
                    throw new Exception($"Unknown difficulty: {enemydifficulty}");
            }
            return enemy;
        }
        public void SpawnEnemyGroup(int amount, int enemyDifficulty)
        {
            //detect what kind of enemy is going to be spawned
            switch (enemyDifficulty)
            {
                case 1:
                    for (int i = 0; i < amount; i++)
                    {
                        BasicEnemyEntity entity = new BasicEnemyEntity(screen);
                        SpawnEnemy(entity);
                        Enemies.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 2:
                    for (int i = 0; i < amount; i++)
                    {
                        MidEnemyEntity entity = new MidEnemyEntity(screen);
                        SpawnEnemy(entity);
                        Enemies.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 3:
                    for (int i = 0; i < amount; i++)
                    {
                        HardEnemyEntity entity = new HardEnemyEntity(screen);
                        SpawnEnemy(entity);
                        Enemies.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 4:
                    for (int i = 0; i < amount; i++)
                    {
                        BossEnemyEntity entity = new BossEnemyEntity(screen);
                        SpawnEnemy(entity);
                        Enemies.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid enemy difficulty");
            }
        }
        private void SetNewLineCoordinates()
        {
            spawnCoordinates.Y -= shiftY;
            spawnCoordinates.X = initialXpos;
        }
        private void SpawnEnemy(AbstractEntity enemy)
        {
            enemy.XPos = spawnCoordinates.X;
            enemy.YPos = spawnCoordinates.Y;
            enemy.Initialize();
            screen.Controls.Add(enemy.icon);
            Enemies.Add(enemy);
            enemy.InitializeLifeTimer();
        }
        public void SpawnInitialEnemyWave()
        {
            var enemyDifficutly = 0;
            var enemyCounter = 0;
            foreach (var enemyLine in NumsOfEnemiesOnLines)
            {
                enemyDifficutly = 1;
                enemyCounter = 0;
                foreach (var enemyAmount in enemyLine)
                {
                    SpawnEnemyGroup(enemyAmount, enemyDifficutly);
                    enemyCounter += enemyAmount;
                    enemyDifficutly++;
                    if (enemyCounter >= enemiesPerLine)
                    {
                        SetNewLineCoordinates();
                    }
                }
            }
        }
        public bool ValidIndex()
        {
            if(currShipIndex > Enemies.Count - 1)
            {
                currShipIndex = 0;
                return false;
            }
            return true;
        }
        public void MoveEnemy()
        {
            if (!enemyGone && ValidIndex())
            {
                Enemies[currShipIndex].StartOperating();
            }
        }
        public void GetRandomSpawnCoords()
        {
            spawnCoordinates.X = rnd.Next(0, screen.Width);
            spawnCoordinates.Y = rnd.Next(0, screen.Height / 2);
        }
        private void Respawn()
        {
            var newEnemyDifficulty = nextEnemyPicker.GetBasedOnProbability(enemyDifficulutyChanceToSpawn);
            var newEnemy = CreateEnemy(newEnemyDifficulty);
            GetRandomSpawnCoords();
            SpawnEnemy(newEnemy);
        }

    }
}
