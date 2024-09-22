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
        GameWindow screen;

        public EnemyController(GameWindow form)
        {
            screen = form;
        }
        private const int enemyWidth = 77;
        private const int enemyHeight = 72;
        private const int enemiesAmount = 12;
        private const int enemyLinesAmount = 4;
        private int currShipIndex = 0;
        NextEnemyPicker nextEnemyPicker = new NextEnemyPicker();
        Random rnd = new Random();
        private const int initialXpos = 500;
        private const int initialYpos = 292;
        private const int shiftX = 77;
        private const int shiftY = 30;
        private const int enemiesPerLine = 3;
        private const int difficultyIncrement = 150;
        private const int periodBound = 300;
        private const int lowestAmount = 2;
        private const int waveAmount = 6;

        private static List<(int, int)> enemyDifficulutyChanceToSpawn = new List<(int, int)>()
        {
            (1, 65),
            (2, 20),
            (3, 10),
            (4, 5),
        };

        private Vector2d spawnCoordinates = new(initialXpos, initialYpos);

        private List<AbstractEntity> Enemies = new List<AbstractEntity>();

        private int movingPeriod = 2000;
        private int spawnPeriod = 3000;

        public List<int[]> NumsOfEnemiesOnLines = new List<int[]>() //each column determines type of enemies and value in it amount of enemies that will be spawned in lines
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
            if(currShipIndex == enemiesAmount)
            {
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
            if (Enemies.Count <= lowestAmount)
            {
                SpawnWave(waveAmount);
            }
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
        public void SpawnInitialEnemyGroup(int amount, int enemyDifficulty)
        {
            for (int i = 0; i < amount; i++)
            {
                var newEnemy = CreateEnemy(enemyDifficulty);
                SpawnEnemy(newEnemy);
                Enemies.Add(newEnemy);
                spawnCoordinates.X += shiftX;
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
                    SpawnInitialEnemyGroup(enemyAmount, enemyDifficutly);
                    enemyCounter += enemyAmount;
                    enemyDifficutly++;
                    if (enemyCounter >= enemiesPerLine)
                    {
                        SetNewLineCoordinates();
                    }
                }
            }
        }
        public void SpawnWave(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Respawn();
            }
        }
        private void FindValidIndex()
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i] != null && !Enemies[i].Dead)
                {
                    currShipIndex = i;
                }
            }
        }
        public void ValidateIndex()
        {
            if(currShipIndex > Enemies.Count - 1)
            {
                FindValidIndex();
            }
        }
        public void MoveEnemy()
        {
            DeleteDead();
            ValidateIndex();
            Enemies[currShipIndex].StartOperating();
        }
        public void GetRandomSpawnCoords()
        {
            spawnCoordinates.X = rnd.Next(enemyWidth, screen.Width - enemyWidth);
            spawnCoordinates.Y = rnd.Next(enemyHeight, screen.Height / 3);
        }
        private void Respawn()
        {
            var newEnemyDifficulty = nextEnemyPicker.GetBasedOnProbability(enemyDifficulutyChanceToSpawn);
            var newEnemy = CreateEnemy(newEnemyDifficulty);
            GetRandomSpawnCoords();
            SpawnEnemy(newEnemy);
            Enemies.Add(newEnemy);
        }
        public void Pause()
        {
            foreach (var enemy in Enemies)
            {
                if (enemy != null)
                {
                    enemy.moveTimer.Stop();
                    enemy.shootTimer.Stop();
                }
            }
        }
        public void IncreaseDIfficulty()
        {
            var j = 0;
            var increment = 3; //value that determines shift in chances of spawning enemy, based on their strength
            for (var i = 0; i < enemyDifficulutyChanceToSpawn.Count; i++)
            {
                if (enemyDifficulutyChanceToSpawn[i].Item2 > 0)
                {
                    j = i;     
                    if (enemyDifficulutyChanceToSpawn[i].Item2 < increment)
                    {
                        increment = enemyDifficulutyChanceToSpawn[i].Item2;
                    }
                    ShiftSpawnChances(i, -increment); //chances to spawn a weaker ship will dencrease
                    break;
                }
            }
            for (var i = j + 1; i < enemyDifficulutyChanceToSpawn.Count; i++)
            {
                if (increment > 0)
                {
                    ShiftSpawnChances(i, 1);  //chances to spawn a stronger ship will increase

                    increment--;
                }
                else { break; }
            }
            QuickenSpawning();
        }
        private void ShiftSpawnChances(int index, int shift)
        {
            var newDifficultyChance = (enemyDifficulutyChanceToSpawn[index].Item1, enemyDifficulutyChanceToSpawn[index].Item2 + shift);
            enemyDifficulutyChanceToSpawn[index] = newDifficultyChance;
        }
        private void QuickenSpawning()
        {
            if (SpawnControlTimer.Interval >= periodBound && MoveControlTimer.Interval >= periodBound)
            {
                SpawnControlTimer.Interval -= difficultyIncrement;
                MoveControlTimer.Interval -= difficultyIncrement;
            }
        }
        public void Stop()
        {
            SpawnControlTimer.Stop();
            MoveControlTimer.Stop();
        }
    }
}
