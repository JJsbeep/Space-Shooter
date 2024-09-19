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
        protected Vector2d curveCheckpoints = new (1, 23);
        protected Vector2d shootTargetCoordinates = new Vector2d();
        protected Vector2d moveDestination = new Vector2d();
        protected Vector2d heroLocation = new Vector2d();
        protected Vector2d projectileDirection = new Vector2d();
        protected Vector2d moveDirection = new Vector2d();
        protected Vector2d moveShifts = new Vector2d();
        protected Vector2d shootShifts = new Vector2d();
        
        public Vector2d spawnCoordinates = new Vector2d();
        
        protected Random rnd = new Random();

        protected int moveDistance;
        protected int shootDistance;

        private int counter = 1;
        private int sign = 1;
        private int aliveCheckPeriod = 1;

        protected virtual int FirePeriod { get; set; } = 2000;
        protected virtual int ProjectileSpeed { get; set; }
        protected virtual int Difficulty { get; set; } = 0;
        public virtual int Speed { get; set; } = 1;
        public virtual int Health { get; set; } = 1;
        public virtual int SpawnPeriod { get; set; }
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
        public List<Projectile> firedProjectiles = new List<Projectile>();
        const int one = 1;

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

        public bool IsOnScreen(PictureBox pictureBox)
        {
            if (!Dead)
            { 
                if (windowHeight >= pictureBox.Top && pictureBox.Bottom >= 0 && windowWidth >= pictureBox.Left && pictureBox.Right >= 0)
                {
                    return true;
                }
            }
            return false;
            
        }
        public void SetCoordinates(int x, int y)
        {
            icon.Location = new Point(x, y);
        }
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
        public bool GotHit(string hitObjectTag)
        {
            if (!Dead)
            {
                foreach (var control in screen.Controls)
                {
                    if (control is PictureBox pictureBox && CausedByEnemy(hitObjectTag, pictureBox))
                    {
                        if (pictureBox.Bounds.IntersectsWith(icon.Bounds))
                        {
                            if (hitObjectTag == "ProjectileEnemy")
                            {
                                Console.WriteLine();
                            }
                            screen.Controls.Remove(pictureBox);                            
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public void DeleteObject(bool force = false)
        {
            if (!force) 
            { 
                OnScreen = IsOnScreen(icon); 
            }
            if (force || !OnScreen)
            {
                shootTimer.Stop();
                shootTimer.Dispose();
                moveTimer.Stop();
                moveTimer.Dispose();
                screen.Controls.Remove(icon);
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
        public virtual void GetProjectileDirection(Projectile projectile)
        {
            projectileDirection.X = shootTargetCoordinates.X - projectile.icon.Location.X;
            projectileDirection.Y = shootTargetCoordinates.Y - projectile.icon.Location.Y;
        }
        public virtual void GetMoveDirection()
        {
            if (moveDestination.X < icon.Location.X) { sign = -1; }
            else { sign = 1; }
            moveDirection.X = moveDestination.X - icon.Location.X;
            moveDirection.Y = moveDestination.Y - icon.Location.Y;
        }
        public virtual void GetRandomMoveDestination()
        {
            moveDestination.X = rnd.Next(0, windowWidth);
            moveDestination.Y = windowHeight;
        }

        protected virtual void PrepareMovingToHero()
        {
            LocateHero();
            moveDestination.X = heroLocation.X;
            moveDestination.Y+= heroLocation.Y;
            GetMoveDirection();
            GetDistance(moveDirection);
            moveShifts = GetShifts(moveDistance, Speed, moveDirection);
        }
        protected int GetDistance(Vector2d directionVector)
        {
            var distance = 0;
            distance = (int)Math.Sqrt(Math.Pow(directionVector.X, 2) + Math.Pow(directionVector.Y, 2));
            return distance;
        }
        protected Vector2d GetShifts(int distance, int speed, Vector2d directionVector)
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
        public virtual void Shoot(object sender, EventArgs e)
        {
            if (!Dead)
            {
                Projectile projectile = new Projectile(screen);
                firedProjectiles.Add(projectile);
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
                DeleteObject();
            }
            /*else
            {
                foreach(var control in screen.Controls)
                {
                    if (control is Label label)
                    {
                        label.Text = 
                    }
                }
            }*/
        }
        protected virtual void MoveCurvy(PictureBox pictureBox, Vector2d shifts)
        {
            if (IsAlive())
            {
                //GetMoveDirection();
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
        public void InitializeLifeTimer()
        {
            aliveTimer.Interval = aliveCheckPeriod;
            aliveTimer.Tick += AliveCheck_Tick;
            aliveTimer.Start();
        }
        public virtual void StartOperating()
        {
            //InitializeLifeTimer();
            InitializeMovingTimer();
            InitializeShootingTimer();
        }
        protected void AliveCheck_Tick(object sender, EventArgs e)
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
                    DeleteObject(true);
                    return false;
                }
                return true;
            }
            else { return true; }
        }
        public virtual void InitializePicBox()
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