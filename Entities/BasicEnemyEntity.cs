using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace zap_program2024.Entities
{
    public class BasicEnemyEntity : AbstractEntity
    {
        public Random rnd = new Random();

        public int _firePeriod;
        public int _difficulty;
        public int _speed;
        public int _health;
        public int _xPos;
        public int _yPos;
        public int _spawnPeriod;
        public bool _onScreen;
        Projectile projectile = new Projectile();
        public BasicEnemyEntity()
        {
            _firePeriod = 2000;
            _difficulty = 1;
            _speed = 9;
            _health = 1;
            _xPos = 0;
            _yPos = 0;
            _spawnPeriod = 2;
            _onScreen = false;
            projectile.icon.Image = Image.FromStream(new MemoryStream(Images.enemyProjectile));
        }
        protected override int FirePeriod
        {
            get => _firePeriod;
        }
        protected override int Difficulty
        {
            get => _difficulty;
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
        public override void InitializePicBox()
        {
            Speed = 3;
            Health = 1;
            icon.Name = "BasicEnemyPicbox";
            icon.Image = Image.FromStream(new MemoryStream(Images.BasicEnemyShip));
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
