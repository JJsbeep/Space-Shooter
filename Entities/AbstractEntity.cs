using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using System.Runtime.CompilerServices;
namespace zap_program2024.Entities
{
    public record struct Vector2d(int X, int Y);
    public abstract class AbstractEntity
    {
        protected const int windowHeight = 800;
        protected const int windowWidth = 1300;

        protected Vector2d size = new (77,72);
        protected Vector2d curveCheckpoints = new (1, 25);
        protected Vector2d shootTargetCoordinates = new Vector2d();
        protected Vector2d moveDestination = new Vector2d();
        protected Vector2d heroLocation = new Vector2d();
        protected Vector2d projectileDirection = new Vector2d();
        protected Vector2d moveDirection = new Vector2d();
        protected Vector2d moveShifts = new Vector2d();
        protected Vector2d shootShifts = new Vector2d();
        public Vector2d spawnCoordinates = new Vector2d();
        protected Projectile projectile = new Projectile();
        
        protected Random rnd = new Random();

        private int moveDistance;
        private int shootDistance;

        private int counter = 1;
        private int sign = 1;

        protected virtual int FirePeriod { get; } = 2000;
        protected virtual int Difficulty { get; set; } = 0;
        public virtual int Speed { get; set; } = 1;
        public virtual int Health { get; set; } = 1;
        public virtual int SpawnPeriod { get; set; }
        public virtual int XPos { get; set; } = 0;
        public virtual int YPos { get; set; } = 0;
        public virtual bool OnScreen { get; set; }
        public virtual bool Dead {  get; set; }
        
        public PictureBox icon = new PictureBox();
        public Timer shootTimer = new Timer();
        public Timer moveTimer = new Timer();
        public Form screen;
        public AbstractEntity(Form form)
        {
            screen = form;
        }
        public bool IsOnScreen(PictureBox pictureBox)
        {
            if (windowHeight >= pictureBox.Top && pictureBox.Bottom >= 0 && windowWidth >= pictureBox.Left && pictureBox.Right >= 0)
            {
                return true;
            }
            return false;
        }
        public void SetCoordinates(int x, int y)
        {
            icon.Location = new Point(x, y);
        }

        public bool GotHit(Projectile missile)
        {
            return missile.icon.Bounds.IntersectsWith(icon.Bounds);
        }
        public void DeleteObject()
        {
            OnScreen = IsOnScreen(icon);
            if (!OnScreen)
            {
                screen.Controls.Remove(icon);
                icon.Dispose();
                shootTimer.Stop();
                shootTimer.Dispose();
                moveTimer.Stop();
                moveTimer.Dispose();
                moveTimer = null;
                shootTimer = null;
                icon = null;
                Dead = true;
            }
        }
        public void SpawnEntity()
        {
            screen.Controls.Add(icon);
        }
        private bool TargetIsHero(PictureBox pictureBox)
        {
            if (pictureBox.Tag != null && pictureBox.Tag.ToString() == "Hero")
            {
                return true;
            }
            return false;
        }
        protected void LocateHero()
        {
            foreach (var control in screen.Controls)
            {
                if (control is PictureBox pictureBox && TargetIsHero(pictureBox))
                {
                    heroLocation.X = pictureBox.Location.X;
                    heroLocation.Y = pictureBox.Location.Y;
                    return;
                }
            }
        }
        protected void AimForHero()
        {
            LocateHero();
            shootTargetCoordinates.X = heroLocation.X;
            shootTargetCoordinates.Y = heroLocation.Y;
        }
        protected void GetRandomTarget()
        {
            shootTargetCoordinates.X = rnd.Next(0, windowWidth);
            shootTargetCoordinates.Y = windowHeight;
        }
        public virtual void GetProjectileDirection()
        {
            projectileDirection.X = shootTargetCoordinates.X - projectile.icon.Location.X;
            projectileDirection.Y = shootTargetCoordinates.Y - projectile.icon.Location.Y;
        }
        public virtual void GetMoveDirection()
        {
            if (moveDestination.X < icon.Location.X) { sign = -1; }
            else {  sign = 1; }
            moveDirection.X = moveDestination.X - icon.Location.X;
            moveDirection.Y = moveDestination.Y - icon.Location.Y;
        }
        public virtual void GetMoveDestination()
        {
            moveDestination.X = rnd.Next(0, windowWidth);
            moveDestination.Y = windowHeight;
        }
        private int GetDistance(Vector2d directionVector)
        {
            var distance = 0;
            distance = (int)Math.Sqrt(Math.Pow(directionVector.X, 2) + Math.Pow(directionVector.Y, 2));
            return distance;
        }
        private Vector2d GetShifts(int distance, int speed, Vector2d directionVector)
        {
            Vector2d shifts = new Vector2d();
            var movesAmount = 0;
            movesAmount = distance / speed;
            shifts.X = directionVector.X / movesAmount;
            shifts.Y = directionVector.Y / movesAmount;
            return shifts;
        }
        protected virtual void InitializeProjectile()
        {
            Vector2d spawnCoordinates = new((icon.Left + icon.Width) / 2, icon.Top);
            projectile.icon.Image = Image.FromFile(@"..\..\..\images\EnemyProjectile.png");
            projectile.Spawn(screen, spawnCoordinates);
        }
        public virtual void Projectile_Tick(object sender, EventArgs e)
        {
            MoveStraight(projectile.icon, shootShifts);
            projectile.deleteOfScreen();
        }
        public virtual void Shoot(object sender, EventArgs e)
        {
            if (projectile == null || projectile.icon == null)
            {
                // Handle the null case, possibly reinitialize or log an error
                return;
            }
            InitializeProjectile();
            LocateHero();
            GetProjectileDirection();
            projectile.projectileSpread.Tick += Projectile_Tick;
        }

        protected void MoveStraight(PictureBox pictureBox, Vector2d shifts)
        {
            pictureBox.Left += shifts.X;
            pictureBox.Top += shifts.Y;
            DeleteObject();
        }
        protected void MoveCurvy(PictureBox pictureBox, Vector2d shifts)
        {
            if (curveCheckpoints.X < counter && counter < curveCheckpoints.Y / 2)
            {
                pictureBox.Left = pictureBox.Left + sign * Math.Max(shifts.X, Speed);
            }
            else if (curveCheckpoints.Y / 2 <= counter && counter < curveCheckpoints.Y)
            {
                pictureBox.Top += Math.Max(shifts.Y, Speed);
            }
            else { counter = 1; }
            counter++;
            DeleteObject();
        }
        public virtual void Initialize()
        {
            InitializePicBox();
            GetMoveDestination();
            GetMoveDirection();
            AimForHero();
            GetProjectileDirection();
            screen.Controls.Add(icon);
            moveDistance = GetDistance(moveDirection);
            shootDistance = GetDistance(projectileDirection);
            moveShifts = GetShifts(moveDistance, Speed, moveDirection);
            shootShifts = GetShifts(shootDistance, projectile.Speed, projectileDirection);
        }
        protected void InitializeShootingTimer()
        {
            shootTimer.Interval = FirePeriod;
            shootTimer.Tick += Shoot;
            shootTimer.Start();
        }
        protected void InitializeMovingTimer()
        {
            moveTimer.Interval = Speed;
            moveTimer.Tick += Move_Tick;
            moveTimer.Start();
        }
        public virtual void InitializeTimers()
        {
            InitializeMovingTimer();
            InitializeShootingTimer();
        }
        public abstract void InitializePicBox();
        public virtual void Move_Tick(object sender, EventArgs e) { }
    }
}