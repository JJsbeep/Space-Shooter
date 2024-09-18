using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;


namespace zap_program2024.Entities
{
    public class HardEnemyEntity : AbstractEntity
    {
        private int _difficulty;
        private int _firePeriod;
        private int _projectileSpeed;
        private int _speed;
        private int _health;
        private int _xPos;
        private int _yPos;
        private int _spawnPeriod;
        private bool _onScreen;
        private bool _dead;
        public HardEnemyEntity(Form form) : base(form)
        {
            _firePeriod = 1700;
            _projectileSpeed = 8;
            _difficulty = 3;
            _speed = 3;
            _health = 5;
            _xPos = 0;
            _yPos = 0;
            _dead = false;
            _onScreen = false;
        }
        protected override int Difficulty
        {
            get => _difficulty;
        }
        protected override int FirePeriod
        {
            get => _firePeriod;
        }
        protected override int ProjectileSpeed
        {
            get => _projectileSpeed;
            set => _projectileSpeed = value;
        }
        public override int Speed
        {
            get => _speed;
            set => _speed = value;
        }
        public override int Health
        {
            get => _health;
            set => _health = value;
        }
        public override int XPos
        {
            get => _xPos;
            set => _xPos = value;
        }
        public override int YPos
        {
            get => _yPos; 
            set => _yPos = value;
        }
        public override int SpawnPeriod
        {
            get => _spawnPeriod;
            set => _spawnPeriod = value;
        }
        public override bool OnScreen
        {
            get => _onScreen;
            set => _onScreen = value;
        }
        public override bool Dead
        {
            get => _dead;
            set => _dead = value;
        }
        public override void InitializePicBox()
        {
            icon.Name = "HardEnemyPicbox";
            icon.Tag = "HardEnemy";
            icon.Tag = "Enemy";
            icon.Image = Image.FromFile(@"..\..\..\images\HardEnemyShip.png");
            base.InitializePicBox();
        }
        public override void GetMoveDirection()
        {
            LocateHero();
            moveDirection.X = heroLocation.X - icon.Location.X;
            moveDirection.Y = heroLocation.Y - icon.Location.Y;
        }
        public override void Move_Tick(object sender, EventArgs e)
        {
            if (IsAlive())
            {
                base.MoveStraight(icon, moveShifts);
            }
        }
    }
}
