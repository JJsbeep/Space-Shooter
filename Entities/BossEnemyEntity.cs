using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public BossEnemyEntity()
        {
            _firePeriod = 500;
            _difficulty = 4;
            _speed = 18;
            _health = 10;
            _xPos = 0;
            _yPos = 0;
            _onScreen = false;
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
        public override void Projectile_Tick(object sender, EventArgs e)
        {
            //Vector2d target = FindShootTarget;
            projectile.deleteOfScreen();
        }
        /*public override void Shoot(Form screen)
        {
            InitializeProjectile(screen);
            projectile.projectileSpread.Tick += Projectile_Tick;
        }*/
        public override void InitializePicBox()
        {
            icon.Name = "BossEnemyPicbox";
            icon.Image = Image.FromStream(new MemoryStream(Images.BossShip));
            icon.Size = new Size(size.X, size.Y);
            icon.Location = new Point(XPos, YPos);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.Visible = true;
        }
        public override void Move()
        {
            throw new NotImplementedException();
        }
        private bool TargetIsHero(PictureBox pictureBox)
        {
            if (pictureBox.Tag != null && pictureBox.Tag.ToString() == "Hero")
            {
                return true;
            }
            return false;
        }
        private Vector2d FindShootTarget(Form screen)
        {
            Vector2d targetCoordinates = new Vector2d();
            foreach(var control in screen.Controls)
            {
                if (control is PictureBox pictureBox && TargetIsHero(pictureBox))
                {
                    targetCoordinates.X = pictureBox.Location.X;
                    targetCoordinates.Y = pictureBox.Location.Y;
                    break;
                }
            }
            return targetCoordinates;
        }

    }
}
