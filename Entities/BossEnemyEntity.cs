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

        public BossEnemyEntity()
        {
            _firePeriod = 500;
            _difficulty = 4;
            _speed = 18;
            _health = 10;
            _xPos = 0;
            _yPos = 0;
            _amplitude = 80;
            _frequency = 0.4;
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
        public override void Projectile_Tick(object sender, EventArgs e)
        {
            projectile.TimeElapsed += projectile.projectileSpread.Interval / 1000.0;
            projectile.icon.Left += (int)(projectileDirection.X * projectile.TimeElapsed); ;
            projectile.icon.Top += (int)(projectile.Amplitude * Math.Sin(projectile.Frequency * projectile.TimeElapsed));
            projectile.deleteOfScreen();
        }
        public override void InitializePicBox()
        {
            icon.Name = "BossEnemyPicbox";
            icon.Image = Image.FromStream(new MemoryStream(Images.BossShip));
            icon.Size = new Size(size.X, size.Y);
            icon.Location = new Point(XPos, YPos);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.Visible = true;
        }
        public override void Move(Form screen, Timer timer)
        {
            base.MoveCurvy(screen, timer);
        }
    }
}
