using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zap_program2024.Entities
{
    public class HardEnemyEntity : AbstractEntity
    {
        public int _difficulty;
        public int _speed;
        public int _health;
        public int _xPos;
        public int _yPos;
        public int _spawnPeriod;
        public bool _onScreen;
        public HardEnemyEntity()
        {
            _difficulty = 3;
            _speed = 15;
            _health = 5;
            _xPos = 0;
            _yPos = 0;
            _onScreen = false;
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
        public override void Shoot(PictureBox _projectile)
        {
            throw new NotImplementedException();
        }
        public override void InitializePicBox()
        {
            icon.Name = "HardEnemyPicbox";
            icon.Image = Image.FromStream(new MemoryStream(Images.HardEnemyShip));
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
