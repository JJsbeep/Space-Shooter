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
        private const int enemiesAmount = 12;
        private const int enemyLinesAmount = 4;
        private int currShipIndex = 0;
        int lineCounter = 0;

        private const int initialXpos = 500;
        private const int initialYpos = 292;
        private const int shiftX = 77;
        private const int shiftY = 30;
        private const int enemiesPerLine = 3;

        private Vector2d spawnCoordinates = new(initialXpos, initialYpos);

        private List<AbstractEntity> Enemies = new List<AbstractEntity>();

        private int movingPeriod = 1500;
        //amount of enemies of each entity on each line
        public List<int[]> NumsOfEnemiesOnLines = new List<int[]>()
        {
            new int[] {3, 0, 0, 0},
            new int[] {1, 2, 0, 0},
            new int[] {1, 1, 1, 0},
            new int[] {0, 1, 1, 1},
        };

        Timer EntityControlTimer = new Timer();
        Form screen;
        
        public EnemyController(Form form)
        {
            screen = form;
        }
        public void ResetIndexing()
        {
            if(currShipIndex == enemiesAmount)
            {
                currShipIndex = 0;
            }
        }
        public void InitializeController()
        {
            EntityControlTimer.Interval = movingPeriod;
            EntityControlTimer.Enabled = true;
            EntityControlTimer.Start();
            EntityControlTimer.Tick += EntityControllerTimer_Tick;
        }
        public void EntityControllerTimer_Tick(object sender, EventArgs e)
        {
            moveEnemy();
            currShipIndex++;
            ResetIndexing();
        }
        public void SpawnEnemyGroup(int amount, int enemyDifficulty, Form screen)
        {
            //detect what kind of enemy is going to be spawned
            switch (enemyDifficulty)
            {
                case 1:
                    for (int i = 0; i < amount; i++)
                    {
                        BasicEnemyEntity entity = new BasicEnemyEntity(screen);
                        SpawnEnemy(entity, screen);
                        Enemies.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 2:
                    for (int i = 0; i < amount; i++)
                    {
                        MidEnemyEntity entity = new MidEnemyEntity(screen);
                        SpawnEnemy(entity, screen);
                        Enemies.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 3:
                    for (int i = 0; i < amount; i++)
                    {
                        HardEnemyEntity entity = new HardEnemyEntity(screen);
                        SpawnEnemy(entity, screen);
                        Enemies.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 4:
                    for (int i = 0; i < amount; i++)
                    {
                        BossEnemyEntity entity = new BossEnemyEntity(screen);
                        SpawnEnemy(entity, screen);
                        Enemies.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid enemy difficulty");
            }
        }
        private void SetNewLineCoords()
        {
            spawnCoordinates.Y -= shiftY;
            spawnCoordinates.X = initialXpos;
        }
        private void SpawnEnemy(AbstractEntity enemy, Form screen)
        {
            enemy.XPos = spawnCoordinates.X;
            enemy.YPos = spawnCoordinates.Y;
            enemy.Initialize();
            screen.Controls.Add(enemy.icon);
        }
        public void SpawnInitialEnemyWave(Form screen)
        {
            var enemyDifficutly = 0;
            var enemyCounter = 0;
            foreach (var enemyLine in NumsOfEnemiesOnLines)
            {
                enemyDifficutly = 1;
                enemyCounter = 0;
                foreach (var enemyAmount in enemyLine)
                {
                    SpawnEnemyGroup(enemyAmount, enemyDifficutly, screen);
                    enemyCounter += enemyAmount;
                    enemyDifficutly++;
                    if (enemyCounter >= enemiesPerLine)
                    {
                        SetNewLineCoords();
                    }
                }
            }
        }
        public void moveEnemy()
        {
            if (!Enemies[currShipIndex].Dead)
            {
                Enemies[currShipIndex].InitializeTimers();
            }
        }
    }
}
