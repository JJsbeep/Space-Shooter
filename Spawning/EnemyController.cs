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
        readonly GameWindow _screen;

        public EnemyController(GameWindow form)
        {
            _screen = form;
        }
        private const int EnemyWidth = 77;
        private const int EnemyHeight = 72;
        private const int EnemiesAmount = 12;
        private int _currShipIndex = 0;
        readonly NextEnemyPicker _nextEnemyPicker = new NextEnemyPicker();
        readonly Random _rnd = new Random();
        private const int InitialXpos = 500;
        private const int InitialYpos = 292;
        private const int ShiftX = 77;
        private const int ShiftY = 30;
        private const int EnemiesPerLine = 3;
        private const int DifficultyIncrement = 50;
        private const int PeriodBound = 500;
        private const int LowestAmount = 3;
        private const int WaveAmount = 5;

        private static readonly List<(int, int)> EnemyDifficulutyChanceToSpawn =
        [
            (1, 65),
            (2, 20),
            (3, 10),
            (4, 5)
        ];

        private Vector2d _spawnCoordinates = new(InitialXpos, InitialYpos);

        private readonly List<AbstractEntity> _enemies = [];

        private readonly int _movingPeriod = 2500;
        private readonly int _spawnPeriod = 3500;

        public List<int[]> NumsOfEnemiesOnLines = new List<int[]>() //each column determines type of enemies and value in it amount of enemies that will be spawned in lines
        {
            new int[] {3, 0, 0, 0},
            new int[] {1, 2, 0, 0},
            new int[] {1, 1, 1, 0},
            new int[] {0, 1, 1, 1},
        };

        private readonly Timer _moveControlTimer = new Timer();
        private readonly Timer _spawnControlTimer = new Timer();

        public void ResetIndexing()
        {
            if(_currShipIndex == EnemiesAmount)
            {
                _currShipIndex = 0;
            }
        }
        private void InitializeMoveTimer()
        {
            _moveControlTimer.Interval = _movingPeriod;
            _moveControlTimer.Enabled = true;
            _moveControlTimer.Start();
            _moveControlTimer.Tick += MoveTimer_Tick;
        }
        private void InitializeSpawnTimer()
        {
            _spawnControlTimer.Interval = _spawnPeriod;
            _spawnControlTimer.Enabled = true;
            _spawnControlTimer.Start();
            _spawnControlTimer.Tick += SpawnTimer_Tick;
        }
        public void InitializeController()
        {
            InitializeMoveTimer();
            InitializeSpawnTimer();
        }
        private void DeleteDead()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].Dead)
                {
                    _enemies.RemoveAt(i);
                }
            }
        }
        public void MoveTimer_Tick(object? sender, EventArgs e)
        {
            DeleteDead();
            MoveEnemy();
            _currShipIndex++;
            ResetIndexing();
        }
        public void SpawnTimer_Tick(object? sender, EventArgs e)
        {
            if (_enemies.Count <= LowestAmount)
            {
                SpawnWave(WaveAmount);
            }
            Respawn();
        }
        private AbstractEntity CreateEnemy(int enemyDifficulty)
        {
            AbstractEntity enemy;
            switch (enemyDifficulty)
            {
                case 1:
                    enemy = new BasicEnemyEntity(_screen);
                    break;
                case 2:
                    enemy = new MidEnemyEntity(_screen);
                    break;
                case 3:
                    enemy = new HardEnemyEntity(_screen);
                    break;
                case 4:
                    enemy = new BossEnemyEntity(_screen);
                    break;
                default:
                    throw new Exception($"Unknown difficulty: {enemyDifficulty}");
            }
            return enemy;
        }
        public void SpawnInitialEnemyGroup(int amount, int enemyDifficulty)
        {
            for (int i = 0; i < amount; i++)
            {
                var newEnemy = CreateEnemy(enemyDifficulty);
                SpawnEnemy(newEnemy);
                _enemies.Add(newEnemy);
                _spawnCoordinates.X += ShiftX;
            }
        }
        private void SetNewLineCoordinates()
        {
            _spawnCoordinates.Y -= ShiftY;
            _spawnCoordinates.X = InitialXpos;
        }
        private void SpawnEnemy(AbstractEntity enemy)
        {
            enemy.XPos = _spawnCoordinates.X;
            enemy.YPos = _spawnCoordinates.Y;
            enemy.Initialize();
            _screen.Controls.Add(enemy.icon);
            enemy.InitializeLifeTimer();
        }
        public void SpawnInitialEnemyWave()
        {
            var enemyDifficulty = 0;
            var enemyCounter = 0;
            foreach (var enemyLine in NumsOfEnemiesOnLines)
            {
                enemyDifficulty = 1;
                enemyCounter = 0;
                foreach (var enemyAmount in enemyLine)
                {
                    SpawnInitialEnemyGroup(enemyAmount, enemyDifficulty);
                    enemyCounter += enemyAmount;
                    enemyDifficulty++;
                    if (enemyCounter >= EnemiesPerLine)
                    {
                        SetNewLineCoordinates();
                    }
                }
            }
        }
        private void SpawnWave(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Respawn();
            }
        }
        private void FindValidIndex()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i] != null && !_enemies[i].Dead)
                {
                    _currShipIndex = i;
                }
            }
        }
        public void ValidateIndex()
        {
            if(_currShipIndex > _enemies.Count - 1)
            {
                FindValidIndex();
            }
        }
        public void MoveEnemy()
        {
            DeleteDead();
            ValidateIndex();
            _enemies[_currShipIndex].StartOperating();
        }
        public void GetRandomSpawnCoords()
        {
            _spawnCoordinates.X = _rnd.Next(EnemyWidth, _screen.Width - EnemyWidth);
            _spawnCoordinates.Y = _rnd.Next(EnemyHeight, _screen.Height / 3);
        }
        private void Respawn()
        {
            var newEnemyDifficulty = _nextEnemyPicker.GetBasedOnProbability(EnemyDifficulutyChanceToSpawn);
            var newEnemy = CreateEnemy(newEnemyDifficulty);
            GetRandomSpawnCoords();
            SpawnEnemy(newEnemy);
            _enemies.Add(newEnemy);
        }
        public void IncreaseDifficulty()
        {
            var j = 0;
            var increment = 3; //value that determines shift in chances of spawning enemy, based on their strength
            for (var i = 0; i < EnemyDifficulutyChanceToSpawn.Count; i++)
            {
                if (EnemyDifficulutyChanceToSpawn[i].Item2 > 0)
                {
                    j = i;     
                    if (EnemyDifficulutyChanceToSpawn[i].Item2 < increment)
                    {
                        increment = EnemyDifficulutyChanceToSpawn[i].Item2;
                    }
                    ShiftSpawnChances(i, -increment); //chances to spawn a weaker ship will dencrease
                    break;
                }
            }
            for (var i = j + 1; i < EnemyDifficulutyChanceToSpawn.Count; i++)
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
            var newDifficultyChance = (EnemyDifficulutyChanceToSpawn[index].Item1, EnemyDifficulutyChanceToSpawn[index].Item2 + shift);
            EnemyDifficulutyChanceToSpawn[index] = newDifficultyChance;
        }
        private void QuickenSpawning()
        {
            if (_spawnControlTimer.Interval >= PeriodBound && _moveControlTimer.Interval >= PeriodBound)
            {
                _spawnControlTimer.Interval -= DifficultyIncrement;
                _moveControlTimer.Interval -= DifficultyIncrement;
            }
        }
        public void Stop()
        {
            _spawnControlTimer.Stop();
            _moveControlTimer.Stop();
        }

        public void Resume()
        {
            _spawnControlTimer.Start();
            _moveControlTimer.Start();
        }
    }
}
