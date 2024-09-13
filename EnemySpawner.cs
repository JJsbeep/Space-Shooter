using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zap_program2024.Entities;

namespace zap_program2024
{
    public class Spawner : Form
    {
        int enemyLinesAmount = 4;

        const int initialXpos = 143;
        const int initialYpos = 292;
        const int shiftX = 77;
        const int shiftY = 72;
        const int enemiesPerLine = 19;

        Vector2d spawnCoordinates = new(initialXpos, initialYpos);

        public List<BasicEnemyEntity>? basicEnemyWave = new List<BasicEnemyEntity>();
        public List<MidEnemyEntity> midEnemyWave = new List<MidEnemyEntity>();
        public List<HardEnemyEntity> hardEnemyWave = new List<HardEnemyEntity>();
        public List<BossEnemyEntity> bossEnemyWave = new List<BossEnemyEntity>();


        //amount of enemies of each entity on each line
        public List<int[]> NumsOfEnemiesOnLines = new List<int[]>()
        {
            new int[] {19, 0, 0, 0},
            new int[] {10, 9, 0, 0},
            new int[] {5, 7, 7, 0},
            new int[] {3, 4, 8, 3},
        };

        public void AddEnemiesToList(int amount, int enemyDifficulty)
        {
            switch (enemyDifficulty)
            {
                case 1:
                    for (int i = 0; i < amount; i++)
                    {
                        BasicEnemyEntity entity = new BasicEnemyEntity();
                        entity.XPos = spawnCoordinates.X;
                        entity.YPos = spawnCoordinates.Y;
                        entity.InitializePicBox();
                        basicEnemyWave.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 2:
                    for (int i = 0; i < amount; i++)
                    {
                        MidEnemyEntity entity = new MidEnemyEntity();
                        entity.XPos = spawnCoordinates.X;
                        entity.YPos = spawnCoordinates.Y;
                        entity.InitializePicBox();
                        midEnemyWave.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 3:
                    for (int i = 0; i < amount; i++)
                    {
                        HardEnemyEntity entity = new HardEnemyEntity();
                        entity.XPos = spawnCoordinates.X;
                        entity.YPos = spawnCoordinates.Y;
                        entity.InitializePicBox();
                        hardEnemyWave.Add(entity);
                        spawnCoordinates.X += shiftX;
                    }
                    break;
                case 4:
                    for (int i = 0; i < amount; i++)
                    {
                        BossEnemyEntity entity = new BossEnemyEntity();
                        entity.XPos = spawnCoordinates.X;
                        entity.YPos = spawnCoordinates.Y;
                        entity.InitializePicBox();
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
        public void SpawnInitialEnemyWave(object sender, EventArgs e)
        {
            var enemyDifficutly = 0;
            var enemyCounter = 0;
            foreach (var enemyLine in NumsOfEnemiesOnLines)
            {
                enemyDifficutly = 1;
                foreach(var enemyAmount in enemyLine)
                {
                    AddEnemiesToList(enemyAmount, enemyDifficutly);
                    enemyCounter += enemyAmount;
                    enemyDifficutly++;
                    if (enemyCounter == enemiesPerLine) 
                    {
                        SetNewLineCoords();
                    }
                }
            }
        }
    }
}
