using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace zap_program2024.Entities
{
    public class BossEnemyEntity : AbstractEntity
    {
        private int _firePeriod;
        private int _projectileSpeed;
        private int _speed;
        private int _health;
        private int _xPos;
        private int _yPos;
        private bool _onScreen;
        private bool _dead;
        private Vector2d _projectileSize;

        public BossEnemyEntity(GameWindow form) : base(form)
        {
            _firePeriod = 2000;
            _projectileSpeed = 7;
            _speed = 3;
            _health = 4;
            _xPos = 0;
            _yPos = 0;
            _dead = false;
            _onScreen = false;
            _projectileSize = new Vector2d(39, 10);
        }
        protected override int FirePeriod
        {
            get => _firePeriod;
            set => _firePeriod = value;
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
        protected override Vector2d ProjectileSize
        {
            get => _projectileSize;
            set => _projectileSize = value;
        }
        protected override void InitializePicBox()
        {
            icon.Name = "BossEnemyPicbox";
            icon.Tag = "Enemy";
            icon.Image = Image.FromFile(@"..\..\..\images\BossShip.png"); ;
            base.InitializePicBox();
        }
        protected override void GetMoveDirection()
        {
            LocateHero();
            moveDirection.X = heroLocation.X - icon.Location.X;
            moveDirection.Y = heroLocation.Y - icon.Location.Y;
        }
        protected override void MoveCurvy(PictureBox pictureBox, Vector2d shifts)
        {
            PrepareMovingToHero();
            base.MoveCurvy(icon, moveShifts);
        }
        public override void Move_Tick(object sender, EventArgs e)
        {
            MoveCurvy(icon, moveShifts);
        }
    }
}
 