using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zap_program2024.Entities
{
    public class HeroEntity : AbstractEntity
    {
        protected Projectile projectile = new Projectile();

        private bool moving;
        private bool movingLeft;
        private bool autoMode;

        public int _firePeriod;
        
        //variables to keep track of score
        public int score;
        public int scoreTime;

        public int _speed;
        public int _health;
        public int _xPos;
        public int _yPos;
        public bool _onScreen;
        public HeroEntity()
        {
            _firePeriod = 500;
            _speed = 23;
            _health = 3;
            _xPos = 593;
            _yPos = 638;
            size.X = 115;
            size.Y = 130;
            _onScreen = true;
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
        public override  int YPos
        {
            get => _yPos;
        }
        public override bool OnScreen 
        { 
            get => _onScreen; 
            set => _onScreen = value; 
        }
        public override void Projectile_Tick(object sender, EventArgs e)
        {
            projectile.icon.Top -= projectile.speed;
            projectile.deleteOfScreen();    
        }
        public override void Shoot(Form screen)
        {
            InitializeProjectile(screen);
            projectile.projectileSpread.Tick += Projectile_Tick;
        }
        public override void InitializePicBox()
        {
            icon.Name = "HeroPicbox";
            icon.Image = Image.FromStream(new MemoryStream(Images.HeroShip));
            icon.Size = new Size(size.X, size.Y);
            icon.Location = new Point(XPos, YPos);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.Visible = true;
        }
        public override void Move()
        {
            if (OnScreen)
            {
                if (!autoMode)
                { 
                    if (moving && movingLeft)
                    {
                        icon.Location = new Point(XPos - Speed, YPos);
                    }
                    else if (moving && !movingLeft)
                    {
                        icon.Location = new Point(XPos + Speed, YPos);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
