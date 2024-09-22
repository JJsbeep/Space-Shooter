using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace zap_program2024.Entities
{
    public record struct Vector2d(int X, int Y);
    public abstract class AbstractEntity
    {
        protected const int windowHeight = 800;
        protected const int windowWidth = 1300;

        protected Vector2d size = new (77,72);
        protected Vector2d curveCheckpoints = new (1, 23); // size of vertical/horizontal movements
        protected Vector2d shootTargetCoordinates = new Vector2d();
        protected Vector2d moveDestination = new Vector2d();
        protected Vector2d heroLocation = new Vector2d();
        protected Vector2d projectileDirection = new Vector2d();
        protected Vector2d moveDirection = new Vector2d();
        protected Vector2d moveShifts = new Vector2d();
        protected Vector2d shootShifts = new Vector2d();
        
        protected Random rnd = new Random();

        protected int moveDistance;
        protected int shootDistance;

        private int counter = 1;//counts horizontal and vertical movements
        private int sign = 1;
        private int aliveCheckPeriod = 1;
        private const int maxShootPos = windowHeight * 3 / 4; // closest position to hero, when it is possible to fire
        protected virtual int FirePeriod { get; set; } = 2000;
        protected virtual int ProjectileSpeed { get; set; }
        public virtual int Speed { get; set; } = 1;
        public virtual int Health { get; set; } = 1;
        public virtual int XPos { get; set; } = 0;
        public virtual int YPos { get; set; } = 0;
        public virtual bool OnScreen { get; set; }
        public virtual bool Dead {  get; set; }
        protected virtual Vector2d ProjectileSize { get; set; } = new Vector2d(15, 15);

        public PictureBox icon = new PictureBox();
        public Timer shootTimer = new Timer();
        public Timer moveTimer = new Timer();
        public Timer aliveTimer = new Timer();
        public GameWindow screen;
        protected Dictionary<string, int> diffiCultyDict = new Dictionary<string, int>()
        {
            {"BasicEnemyPicbox", 1 },
            {"MidEnemyPicbox", 2 },
            {"HardEnemyPicbox", 3 },
            {"BossEnemyPicbox", 4 },
        };
        public AbstractEntity(GameWindow form)
        {
            screen = form;
        }

        private bool IsOnScreen(PictureBox pictureBox)
        {
            if (!Dead)
            { 
                if (windowHeight >= pictureBox.Top && pictureBox.Bottom >= 0 &&
                    windowWidth >= pictureBox.Left && pictureBox.Right >= 0) //chceckif picturebox fits on a screen
                {
                    return true;
                }
            }
            return false;
        }

        //gets projectile back to the top of a screen if it falls out of it
        private void GetBackUp()
        {
            if (!IsOnScreen(icon))
            {
                icon.Top = 0;
                icon.Left = windowWidth - icon.Left;
            }
        }
        
        //checks if given picturebox is hit by an enemy based on enemy tag
        private bool CausedByEnemy(string hitObjectTag, PictureBox pictureBox)
        {
            var tag  = hitObjectTag;
            var thing = pictureBox;
            if (tag == "ProjectileEnemy")
            {
                Console.WriteLine();
            }
            if (pictureBox.Tag != null && pictureBox.Tag.ToString() == hitObjectTag)
            {
                
                return true;
            }
            return false;
        }
        //searches for all pictureboxes and checks if any of them was hit by an enemy based on given tag
        protected bool GotHit(string hitObjectTag)
        {
            if (!Dead)
            {
                foreach (var control in screen.Controls)
                {
                    if (control is PictureBox pictureBox && CausedByEnemy(hitObjectTag, pictureBox))
                    {
                        if (pictureBox.Bounds.IntersectsWith(icon.Bounds))
                        {
                            screen.Controls.Remove(pictureBox);                            
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        protected void DeleteObject()
        {
            shootTimer.Stop();
            shootTimer.Dispose();
            moveTimer.Stop();
            moveTimer.Dispose();
            screen.Controls.Remove(icon);
            Dead = true;
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
        private void AimForHero()
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
        private void GetProjectileDirection(Projectile projectile)
        {
            projectileDirection.X = shootTargetCoordinates.X - projectile.icon.Location.X;
            projectileDirection.Y = shootTargetCoordinates.Y - projectile.icon.Location.Y;
        }
        protected virtual void GetMoveDirection()
        {
            if (moveDestination.X < icon.Location.X) { sign = -1; }
            else { sign = 1; }
            moveDirection.X = moveDestination.X - icon.Location.X;
            moveDirection.Y = moveDestination.Y - icon.Location.Y;
        }
        private void GetRandomMoveDestination()
        {
            moveDestination.X = rnd.Next(0, windowWidth);
            moveDestination.Y = windowHeight;
        }

        protected void PrepareMovingToHero()
        {
            LocateHero();
            moveDestination.X = heroLocation.X;
            moveDestination.Y+= heroLocation.Y;
            GetMoveDirection();
            GetDistance(moveDirection);
            moveShifts = GetShifts(moveDistance, Speed, moveDirection);
        }
        private int GetDistance(Vector2d directionVector)
        {
            var distance = 0;
            distance = (int)Math.Sqrt(Math.Pow(directionVector.X, 2) + Math.Pow(directionVector.Y, 2));
            return distance;
        }
        //returns X and Y shifts of movement
        private Vector2d GetShifts(int distance, int speed, Vector2d directionVector)
        {
            Vector2d shifts = new Vector2d();
            var movesAmount = 0;
            movesAmount = distance / speed;
            shifts.X = directionVector.X / movesAmount;
            shifts.Y = directionVector.Y / movesAmount;
            return shifts;
        }
        protected virtual void InitializeProjectile(Projectile projectileToSet)
        {
            Vector2d spawnCoordinates = new((icon.Left + icon.Width / 2), icon.Bottom);
            projectileToSet.icon.Image = Image.FromFile(@"..\..\..\images\enemy_projectile.png");
            projectileToSet.Spawn(spawnCoordinates, ProjectileSize, ProjectileSpeed);
            projectileToSet.icon.Tag = "ProjectileEnemy";
            GetProjectileDirection(projectileToSet);
            shootDistance = GetDistance(projectileDirection);
            shootShifts = GetShifts(shootDistance, projectileToSet.Speed, projectileDirection);
            projectileToSet.SetShifts(shootShifts);
            projectileToSet.projectileSpread.Tick += projectileToSet.MoveStraight;
            projectileToSet.projectileSpread.Start();
        }
        protected bool IsPositionToShoot()
        {
            if (icon.Bottom <= maxShootPos)
            {
                return false;
            }
            return true;
        }
        public virtual void Shoot(object sender, EventArgs e)
        {
            if (!Dead && IsPositionToShoot())
            {
                Projectile projectile = new Projectile(screen);
                AimForHero();
                InitializeProjectile(projectile);
                DeleteObject();
            }
        }

        protected void MoveStraight(PictureBox pictureBox, Vector2d shifts)
        {
            if (IsAlive())
            {
                GotHit("ProjectileHero");
                pictureBox.Left += shifts.X;
                pictureBox.Top += shifts.Y;
                GetBackUp();
                DeleteObject();
            }
        }
        protected virtual void MoveCurvy(PictureBox pictureBox, Vector2d shifts)
        {
            if (IsAlive())
            {
                if (curveCheckpoints.X <= counter && counter < curveCheckpoints.Y / 2)
                {
                    pictureBox.Left += sign * shifts.X;
                    counter++;
                }
                else if (curveCheckpoints.Y / 2 <= counter && counter < curveCheckpoints.Y)
                {
                    pictureBox.Top += shifts.Y;
                    counter++;
                }
                else { counter = 1; }
                GetBackUp();
                DeleteObject();

            }
            else { counter = 1; }
        }
        public virtual void Initialize()
        {
            InitializePicBox();
            GetRandomMoveDestination();
            GetMoveDirection();
            screen.Controls.Add(icon);
            moveDistance = GetDistance(moveDirection);
            moveShifts = GetShifts(moveDistance, Speed, moveDirection);
        }
        private void InitializeShootingTimer()
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
        public void InitializeLifeTimer()
        {
            aliveTimer.Interval = aliveCheckPeriod;
            aliveTimer.Tick += AliveCheck_Tick;
            aliveTimer.Start();
        }
        public virtual void StartOperating()
        {
            InitializeMovingTimer();
            InitializeShootingTimer();
        }
        private void AliveCheck_Tick(object sender, EventArgs e)
        {
            IsAlive();
        }
        protected virtual bool IsAlive()
        {
            if (GotHit("ProjectileHero"))
            {
                Health--;
                if(Health == 0)
                {
                    screen.scoreBar.Update();
                    Dead = true;
                    DeleteObject();
                    return false;
                }
                return true;
            }
            else { return true; }
        }
        protected virtual void InitializePicBox()
        {
            icon.Location = new Point(XPos, YPos);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.Visible = true;
            icon.BackColor = Color.Transparent;
            icon.Size = new Size(size.X, size.Y);
            screen.Controls.Add(icon);
        }
        public virtual void Move_Tick(object sender, EventArgs e) { }
    }
}