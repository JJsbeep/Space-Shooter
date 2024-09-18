using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Timer = System.Windows.Forms.Timer;

namespace zap_program2024.Entities
{
    public class HeroEntity : AbstractEntity
    {
        private DateTime lastShotTime = DateTime.MinValue;
        private TimeSpan shotCooldown = TimeSpan.FromSeconds(0.5);

        public bool moving;
        public bool movingLeft;
        public bool _autoMode;

        public int _firePeriod;
        
        //variables to keep track of score
        public int score;
        public int scoreTime;

        public int _speed;
        public int _health;
        public int _xPos;
        public int _yPos;
        public bool _onScreen;
        public bool _dead;
        public HeroEntity(Form form) : base(form)
        {
            _firePeriod = 500;
            _speed = 10;
            _health = 3;
            _xPos = 593;
            _yPos = 638;
            _dead = false;
            size.X = 115;
            size.Y = 130;
            _onScreen = true;
            _autoMode = false;
        }
        public bool ShootingReady
        {
            get;
            set;
        } = true;
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
        public bool AutoModeOn
        {
            get => _autoMode;
            set => _autoMode = value;
        }
        public override bool Dead
        {
            get => _dead;
            set => _dead = value;
        }
        public override void InitializePicBox()
        {
            base.InitializePicBox();
            icon.Name = "HeroPicbox";
            icon.Tag = "Hero";
            icon.Image = Image.FromFile(@"..\..\..\images\HeroShip.png");
            icon.Size = new Size(size.X, size.Y);
        }
        private void MoveLeft()
        {
            if (icon.Left - Speed >= 0)
            {
                icon.Left -= Speed;
            }
            else
            {
                icon.Left = 0; // Ensure it doesn't go off the screen
            }
        }
        private void MoveRight()
        {
            if (icon.Right + Speed <= screen.ClientSize.Width)
            {
                icon.Left += Speed;
            }
            else
            {
                icon.Left = screen.ClientSize.Width - icon.Width; // Ensure it doesn't go off the screen
            }
        }
        private void MoveManual()
        {
            if (moving)
            {
                if (movingLeft)
                {
                    MoveLeft();
                }
                else
                {
                    MoveRight();
                }
            }
        }
        public void Move()
        {
            if (IsAlive())
            {
                if (AutoModeOn)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    MoveManual();
                }
            }
        }
        protected override void InitializeProjectile(Projectile projectileToSet)
        {
            Vector2d spawnCoordinates = new((icon.Left + icon.Width / 2), icon.Bottom);
            projectileToSet.icon.Image = Image.FromFile(@"..\..\..\images\projectile.png");
            projectileToSet.icon.Tag = "ProjectileHero";
            projectileToSet.Spawn(spawnCoordinates, ProjectileSize);
            projectileToSet.projectileSpread.Tick += projectileToSet.TravelUp;
            projectileToSet.projectileSpread.Start();
        }
        public bool FireReady()
        {
            if (DateTime.Now - lastShotTime >= shotCooldown)
            {
                lastShotTime = DateTime.Now;
                return true;
            }
            return false;
        }
        protected override bool IsAlive()
        {
            if (GotHit("ProjectileEnemy") /*|| GotHit("Enemy")*/)
            {
                Health--;
                if (Health == 0)
                {
                    Dead = true;
                    DeleteObject(true);
                    return false;
                }
                return true;
            }
            else { return true; }
        }
        public void HeroShoot()
        {
            if (!Dead)
            {
                Projectile projectile = new Projectile(screen);
                firedProjectiles.Add(projectile);
                InitializeProjectile(projectile);
                DeleteObject();
            }
        }
        public override void Initialize()
        {
            InitializePicBox();
        }
    }
}
