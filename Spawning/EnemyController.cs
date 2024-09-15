using System;
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
        int enemyLinesAmount = 4;

        const int initialXpos = 143;
        const int initialYpos = 292;
        const int shiftX = 77;
        const int shiftY = 30;
        const int enemiesPerLine = 15;

        Vector2d spawnCoordinates = new(initialXpos, initialYpos);

        public List<BasicEnemyEntity> basicEnemyWave = new List<BasicEnemyEntity>();
        public List<MidEnemyEntity> midEnemyWave = new List<MidEnemyEntity>();
        public List<HardEnemyEntity> hardEnemyWave = new List<HardEnemyEntity>();
        public List<BossEnemyEntity> bossEnemyWave = new List<BossEnemyEntity>();

        //amount of enemies of each entity on each line
        public List<int[]> NumsOfEnemiesOnLines = new List<int[]>()
        {
            new int[] {15, 0, 0, 0},
            new int[] {8, 7, 0, 0},
            new int[] {4, 6, 5, 0},
            new int[] {2, 3, 6, 4},
        };
        public void SpawnEnemyGroup(int amount, int enemyDifficulty, Form screen)
        {
            //detect what kind of enemy is going to be spawned
            switch (enemyDifficulty)
            {
                case 1:
                    for (int i = 0; i < amount; i++)
                    {
                        BasicEnemyEntity entity = new BasicEnemyEntity();
                        SpawnEnemy(entity, screen);
                        basicEnemyWave.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 2:
                    for (int i = 0; i < amount; i++)
                    {
                        MidEnemyEntity entity = new MidEnemyEntity();
                        SpawnEnemy(entity, screen);
                        midEnemyWave.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 3:
                    for (int i = 0; i < amount; i++)
                    {
                        HardEnemyEntity entity = new HardEnemyEntity();
                        SpawnEnemy(entity, screen);
                        hardEnemyWave.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 4:
                    for (int i = 0; i < amount; i++)
                    {
                        BossEnemyEntity entity = new BossEnemyEntity();
                        SpawnEnemy(entity, screen);
                        bossEnemyWave.Add(entity);
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
            enemy.Initialize(screen);
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
        public void moveFirstWave(Form screen, Timer timer)
        {
            foreach(var ship in basicEnemyWave)
            {
                ship.Move(screen, timer);
            }
        }
        public void EnemyShoot(Form screen)
        {
            foreach (var ship in midEnemyWave)
            {
                ship.Shoot(screen);
            }
        }
        /*public void DeleteShotShips(Form screen)
        {
            foreach (var ship in basicEnemyWave) 
            {
                if (ship.GotHit())
            }
        }*/
    }
}
