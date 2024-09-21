using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Timer = System.Windows.Forms.Timer;

namespace zap_program2024.Entities
{
    public record struct EnemyData(string Name, Vector2d moveShootVector);

    public class HeroEntity : AbstractEntity
    {
        private DateTime lastShotTime = DateTime.MinValue;
        private TimeSpan shotCooldown = TimeSpan.FromSeconds(0.5);
        

        public bool moving;
        public bool movingLeft;
        private bool upgradeReady;
        private bool _autoMode;

        private int _firePeriod;
        private int _projectileSpeed;

        //variables to keep track of score
        private int score;
        private int scoreTime;

        private int _speed;
        private int _health;
        private int _xPos;
        private int _yPos;
        private bool _onScreen;
        private bool _dead;

        //Automode variables
        private Dictionary<int, PictureBox> foundEnemies; //values will be vector that determines enenemy's movement
        private bool canShoot = true;
        private Vector2d moveShoot = new Vector2d(); // Item.1 - Amount of moves(+to the right, -to the left), Item.2 - Amount of moves after which heor shoots
        private int shortestDistance = 0;
        private int coinXCoord = -1;
        private const int stepsAhead = 100;
        private const string enemyTag = "Enemy";
        private const string enemyProjectileTag = "ProjectileEnemy";
        private static readonly List<string> dangerTags = new List<string>() { enemyTag, enemyProjectileTag};

        private const int maxEnemyProjectileSpeed = 9;
        private const int shootBound = 5;
        private readonly Vector2d zeroVector = new Vector2d(0, 0);
        private EnemyData closestEnemyData;
        private int lastEvenlyUpgrade = 0;


        public HeroEntity(GameWindow form) : base(form)
        {
            _firePeriod = 500;
            _projectileSpeed = 12;
            _speed = 15;
            _health = 3;
            _xPos = 593;
            _yPos = 638;
            _dead = false;
            size.X = 115;
            size.Y = 130;
            _onScreen = true;
            _autoMode = true;
            foundEnemies = new Dictionary<int, PictureBox>();
            closestEnemyData = new EnemyData("no data", zeroVector);
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
        protected override int ProjectileSpeed
        {
            get => _projectileSpeed;
            set => _projectileSpeed = value;
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
                    Action();
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
            projectileToSet.Spawn(spawnCoordinates, ProjectileSize, ProjectileSpeed);
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
            if (GotHit("ProjectileEnemy") || GotHit("Enemy"))
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
        private void UpgradeSpeed()
        {
            Speed++;
        }
        private void UpgradeHealth()
        {
            Health++;
        }
        private void UpgradeProjectileSpeed()
        {
            ProjectileSpeed++;
        }
        public void PerformUpgrade(int upgradeCode)
        {
            switch(upgradeCode)
            {
                case 1:
                    UpgradeSpeed();
                    break;
                case 2:
                    UpgradeHealth();
                    break;
                case 3:
                    UpgradeProjectileSpeed();
                    break;
                default:
                    throw new Exception("No upgrade for given code");
            }
        }

        //automode
        private void Action()
        {
            if (coinAvailable())
            {
                MoveForCoin();
            }
            else
            {
                MoveForEnemy();
            }
            if (CanShoot() && FireReady())
            {
                HeroShoot();
            }
        }
        private bool CanShoot()
        {
            if ((closestEnemyData.moveShootVector.X * Speed / FirePeriod) >= 1 || closestEnemyData.moveShootVector.X < shootBound)
            {
                return true;
            }
            return false;
        }
        private int CalculateNextMove()
        {
            var nextMove = 0;
            FindClosestEnemy(); // enemy that would take the least time to kill

            if (closestEnemyData.moveShootVector.X > 0)
            {
                nextMove = Speed;
                moveShoot.X = closestEnemyData.moveShootVector.X - 1;
            }
            else if (closestEnemyData.moveShootVector.X < 0)
            {
                nextMove = -Speed;
                moveShoot.X = closestEnemyData.moveShootVector.X + 1;

            }
            moveShoot.Y = closestEnemyData.moveShootVector.Y;
            closestEnemyData.moveShootVector = moveShoot;
            return nextMove;
        }
        private void MoveForEnemy()
        {
            var nextMove = CalculateNextMove();
            if (!AreaSafe(nextMove)) 
            {
                nextMove = FindSafeMove();
            }
            DoMove(nextMove);
            //icon.Left += nextMove;
        }
        private void DoMove(int move)
        {
            if (move > 0)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
        }
        private void MoveForCoin()
        {
            var nextMove = 0;
            if (coinXCoord < icon.Left)
            {
                if (!AreaSafe(-Speed))
                {
                    nextMove = FindSafeMove();
                    DoMove(nextMove);
                }
                else { icon.Left -= Speed; }
                
            }
            else
            {
                if (!AreaSafe(Speed))
                {
                    nextMove = FindSafeMove();
                    DoMove(nextMove);
                }
                else { icon.Left += Speed; }
            }
        }
        private void FindClosestEnemy()
        {
            foreach (var control in screen.Controls)
            {
                var closest = 4000;
                if (control is PictureBox pictureBox)
                {
                    if (pictureBox.Tag is not null && pictureBox.Tag.ToString() == enemyTag)
                    {
                        /*EnemyData enemyData = new EnemyData
                        {
                            Tag = pictureBox.Tag?.ToString(),
                            Location = new Vector2d(pictureBox.Location.X, pictureBox.Location.Y),
                        };
                        if (foundEnemies.Keys.Contains(enemyData) && foundEnemies[key: enemyData] == zeroVector)
                        {
                            enemyData.
                        }
                        //Vector2d enemyLocation = (pictureBox)*/
                        SetClosestEnemyData(pictureBox);
                        Console.WriteLine("EnemyFound");
                    }
                }
            }
        }
        private void SetClosestEnemyData(PictureBox pictureBox)
        {
            var newEnemyMoveShootVector = CalculateMoveShootDistance(pictureBox);
            var distanceNewEnemy = GetAbsoluteDistance(newEnemyMoveShootVector);
            /*if (!foundEnemies.ContainsKey(distanceNewEnemy))
            {
                foundEnemies.Add(distanceNewEnemy, pictureBox);
            }*/
            /*if (shortestDistance != 0 && foundEnemies[shortestDistance] ==  null)
            {
                foundEnemies.Remove(shortestDistance);
                shortestDistance = 4000;
            }*/
            
            if (closestEnemyData.Name == "no data")
            {
                closestEnemyData.Name = pictureBox.Name;
                closestEnemyData.moveShootVector = newEnemyMoveShootVector;
                shortestDistance = distanceNewEnemy;
            }
            else
            {
                DislocateDead();
                if (distanceNewEnemy < GetAbsoluteDistance(closestEnemyData.moveShootVector))
                {
                    closestEnemyData.moveShootVector = newEnemyMoveShootVector;
                    closestEnemyData.Name = pictureBox.Name;
                    shortestDistance = distanceNewEnemy;
                }
                else if (distanceNewEnemy == GetAbsoluteDistance(closestEnemyData.moveShootVector))
                {
                    if (closestEnemyData.Name != pictureBox.Name)
                    {
                        closestEnemyData.Name = CompareEnemies(closestEnemyData.Name, pictureBox.Name);
                        if (closestEnemyData.Name == pictureBox.Name)
                        {
                            closestEnemyData.moveShootVector = newEnemyMoveShootVector;
                        }
                    }
                }
            }
        }
        private string CompareEnemies(string name1,  string name2)
        {
            if (!diffiCultyDict.ContainsKey(name1) || !diffiCultyDict.ContainsKey(name2))
            {
                return null;
            }
            if (diffiCultyDict[name1] >= diffiCultyDict[name2])
            {
                return name1;
            }
            return name2;
        }
        private Vector2d CalculateMoveShootDistance(PictureBox pictureBox)
        {
            var result = new Vector2d();
            if (pictureBox.Left >= icon.Right)
            {
                result.X = ((pictureBox.Left - icon.Right) / Speed) + 2; //+ icon.Width / 3;
                Console.WriteLine(result.X);
            }
            else if (pictureBox.Right <= icon.Left)
            {
                result.X = ((pictureBox.Right - icon.Left ) / Speed) - 2;// - icon.Width / 3;
                Console.WriteLine(result.X);
            }
            else if (pictureBox.Right <= icon.Right)
            {
                result.X = (pictureBox.Right - icon.Right) / Speed;
            }
            else if (pictureBox.Left >= icon.Left)
            {
                result.X = (pictureBox.Left - icon.Left) / Speed;
            }
            result.Y = (icon.Top - pictureBox.Bottom) / ProjectileSpeed + 2;
            Console.WriteLine("ygkhfkythcgt");
            return result;
        }  
        private int GetAbsoluteDistance(Vector2d moveVector)
        {
            var result = 0;
            result = Math.Abs(moveVector.X) + Math.Abs(moveVector.Y);
            return result;
        }
        private void DislocateDead()
        {
            var focusPoint = new Point(icon.Left + closestEnemyData.moveShootVector.X, icon.Top - closestEnemyData.moveShootVector.Y - 2);
            foreach (var control in screen.Controls)
            {
                if (control != null && control is PictureBox pictureBox && pictureBox.Name == "Enemy")
                {
                    if (pictureBox.Bounds.Contains(focusPoint))
                    {
                        return;
                    }
                }
            }
            closestEnemyData.Name = "no data";
        }
        private bool AreaSafe(int moveToMake)
        {
            Point newPoint = new Point();
            PictureBox newIcon = new PictureBox();
            newIcon.Size = new Size(icon.Size.Width + (Math.Abs(2 * moveToMake)), icon.Size.Height + maxEnemyProjectileSpeed);
            newIcon.Left = icon.Left + moveToMake;
            newIcon.Top = icon.Top + maxEnemyProjectileSpeed;
            if (!DangerInWay(newIcon))
            {
                return true;
            }
            return false;
        }
        private bool DangerInWay(PictureBox Area)
        {
            foreach (var control in screen.Controls)
            {
                if (control != null && control is PictureBox pictureBox && dangerTags.Contains(pictureBox.Tag?.ToString()))
                {
                    if (pictureBox.Bounds.IntersectsWith(Area.Bounds))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool coinAvailable()
        {
            foreach (var control in screen.Controls)
            {
                if (control != null && control is PictureBox pictureBox && pictureBox.Tag?.ToString() == "Coin")
                {
                    coinXCoord = pictureBox.Bounds.X;
                    return true;
                }
            }
            return false;
        }
        private int FindSafeMove()
        {
            var moveVector = zeroVector;
            var move = 0;
            /*for (int i = 0; i < 30; i++)
            {*/
                if (AreaSafe(Speed))
                {
                    move = Speed;
                }
                else if (AreaSafe(-Speed))
                {
                    move = -Speed;
                }
                return move;
            /*}

            closestEnemyData.moveShootVector = moveVector;*/
        }
        public void AutoUpgrade()
        {
            if (Health <= 3)
            {
                UpgradeHealth();
            }
            else
            {
                UpgradeEvenly();
            }
            screen.SetAfterCoinPicked();
        }
        private void UpgradeEvenly()
        {
            if (lastEvenlyUpgrade == 0  || lastEvenlyUpgrade == 1)
            {
                UpgradeProjectileSpeed();
                return;
            }
            UpgradeSpeed();
        }
    }
}
