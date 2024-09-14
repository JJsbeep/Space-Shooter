using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zap_program2024.Entities
{
    public class MidEnemyEntity : AbstractEntity
    {
        public int _difficulty;
        public int _firePeriod;
        public int _speed;
        public int _health;
        public int _xPos;
        public int _yPos;
        public int _spawnPeriod;
        public bool _onScreen;
        public MidEnemyEntity()
        {
            _difficulty = 2;
            _firePeriod = 2000;
            _speed = 12;
            _health = 3;
            _xPos = 0;
            _yPos = 0;
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
        public override void Projectile_Tick(object sender, EventArgs e)
        {
            projectile.icon.Left += projectileDirection.X / projectile.speed ;
            projectile.icon.Top += projectileDirection.Y / projectile.speed;
            projectile.deleteOfScreen();
        }
        public override void Shoot(Form screen)
        {
            InitializeProjectile(screen);
            FindShootTarget(screen);
            projectile.projectileSpread.Tick += Projectile_Tick;

        }
        public override void InitializePicBox()
        {
            icon.Name = "MidEnemyPicbox";
            icon.Image = Image.FromStream(new MemoryStream(Images.MidEnemyShip));
            icon.Size = new Size(size.X, size.Y);
            icon.Location = new Point(XPos, YPos);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.Visible = true;
        }
        public override void Move()
        {
            throw new NotImplementedException();
        }
    }
}
