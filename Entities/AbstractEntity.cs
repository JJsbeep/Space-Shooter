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

        private const int second = 1000;

        protected Vector2d size = new (77,72);
        protected Vector2d shootTargetCoordinates = new Vector2d();
        protected Vector2d moveDestination = new Vector2d();
        protected Vector2d heroLocation = new Vector2d();
        protected Vector2d projectileDirection = new Vector2d();
        protected Vector2d moveDirection = new Vector2d();
        protected Vector2d moveShifts = new Vector2d();
        protected Vector2d shootShifts = new Vector2d();
        protected Projectile projectile = new Projectile();
        
        protected Random rnd = new Random();

        private int moveDistance;
        private int shootDistance;

        protected abstract int FirePeriod {  get; }
        protected virtual int Difficulty { get; set; } = 0;
        public virtual int Speed { get; set; } = 7;
        public virtual int Health { get; set; } = 1;
        public virtual int SpawnPeriod { get; set; }
        public virtual int XPos { get; set; }
        public virtual int YPos { get; set; }
        public virtual bool OnScreen { get; set; }
        protected virtual double Amplitude { get; set; } // Amplitude of the sine wave
        protected virtual double Frequency { get; set; } // Frequency of the sine wave
        protected virtual double TimeElapsed { get; set; } // Time elapsed since the projectile was spawned
        public PictureBox icon = new PictureBox();

        public AbstractEntity()
        {
            XPos = 0;
            YPos = 0;
            OnScreen = false;
        }
        public AbstractEntity(int xPos, int yPos)
        {
            this.XPos = xPos;
            this.YPos = yPos;
        }
        public void SetCoordinates(int x, int y)
        {
            icon.Location = new Point(x, y);
        }

        public bool GotHit(Projectile missile)
        {
            return missile.icon.Bounds.IntersectsWith(icon.Bounds);
        }
        public void DeleteObject(Form screen)
        {
            screen.Controls.Remove(icon);
        }

        public void SpawnEntity(Form screen)
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
        protected void LocateHero(Form screen)
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
        protected void AimForHero(Form screen)
        {
            LocateHero(screen);
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
        public virtual void GetMoveDirection(Form screen)
        {
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
        protected virtual void InitializeProjectile(Form screen)
        {
            Vector2d spawnCoordinates = new((icon.Left + icon.Width) / 2, icon.Top);
            projectile.icon.Image = Image.FromFile(@"..\..\..\images\EnemyProjectile.png");
            projectile.Spawn(screen, spawnCoordinates);
            //projectile.projectileSpread.Interval = FirePeriod;
        }
        public virtual void Projectile_Tick(object sender, EventArgs e)
        {
            projectile.icon.Left += shootShifts.X;
            projectile.icon.Top += shootShifts.Y;
            projectile.deleteOfScreen();
        }
        public virtual void Shoot(Form screen)
        {
            InitializeProjectile(screen);
            LocateHero(screen);
            GetProjectileDirection();
            projectile.projectileSpread.Tick += Projectile_Tick;
        }
        protected void MoveStraight(Form screen, Timer timer)
        {
            icon.Left += moveShifts.X;
            icon.Top += moveShifts.Y;
            //DeleteObject(screen);
        }
        protected void MoveCurvy(Form screen, Timer timer)
        {
            TimeElapsed += timer.Interval / second;
            icon.Left += (int)(moveDirection.X * TimeElapsed);
            icon.Top += (int)(Amplitude * Math.Sin(Frequency * TimeElapsed));
            DeleteObject(screen);
        }
        public virtual void Initialize(Form screen)
        {
            InitializePicBox();
            GetMoveDestination();
            GetMoveDirection(screen);
            AimForHero(screen);
            GetProjectileDirection();
            screen.Controls.Add(icon);
            moveDistance = GetDistance(moveDirection);
            shootDistance = GetDistance(projectileDirection);
            moveShifts = GetShifts(moveDistance, Speed, moveDirection);
            shootShifts = GetShifts(shootDistance, projectile.Speed, projectileDirection);
        }
        public abstract void InitializePicBox();
        public abstract void Move(Form screen, Timer timer);
    }
}