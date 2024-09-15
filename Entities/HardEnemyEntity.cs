using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;


namespace zap_program2024.Entities
{
    public class HardEnemyEntity : AbstractEntity
    {
        public int _difficulty;
        public int _firePeriod;
        public int _speed;
        public int _health;
        public int _xPos;
        public int _yPos;
        public int _spawnPeriod;
        public bool _onScreen;

        protected double _amplitude;
        protected double _frequency;
        protected double _timeElapsed;
        public HardEnemyEntity()
        {
            _difficulty = 3;
            _speed = 15;
            _health = 5;
            _xPos = 0;
            _yPos = 0;
            _amplitude = 70;
            _frequency = 0.3;
            _timeElapsed = 0;
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
        protected override double Amplitude
        {
            get => _amplitude;
        }
        protected override double Frequency
        {
            get => _frequency;
        }
        protected override double TimeElapsed
        {
            get => _timeElapsed;
            set => _timeElapsed = value;
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
            icon.Name = "HardEnemyPicbox";
            icon.Image = Image.FromFile(@"..\..\..\images\HardEnemyShip.png"); ;
            icon.Size = new Size(size.X, size.Y);
            icon.Location = new Point(XPos, YPos);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.Visible = true;
            icon.BackColor = Color.Transparent;
        }
        public override void GetMoveDirection(Form screen)
        {
            LocateHero(screen);
            moveDirection.X = heroLocation.X - icon.Location.X;
            moveDirection.Y = heroLocation.Y - icon.Location.Y;
        }
        public override void Move(Form screen, Timer timer)
        {
            base.MoveCurvy(screen, timer);
        }
    }
}
