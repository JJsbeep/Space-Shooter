using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
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
        private Dictionary<EnemyData, Vector2d> foundEnemies; //values will be vector that determines enenemy's movement
        private bool canShoot = true;
        private Vector2d moveShoot = new Vector2d(); // Item.1 - Amount of moves(+to the right, -to the left), Item.2 - Amount of moves after which heor shoots
        private int shortestDistance = 0;
        private const int stepsAhead = 100;
        private const string enemyTag = "Enemy";
        private const string enemyProjectileTag = "ProjectileEnemy";
        private readonly Vector2d zeroVector = new Vector2d(0, 0);
        private EnemyData closestEnemyData;


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
            _autoMode = false;
            //foundEnemies = new Dictionary<EnemyData, Vector2d>();
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
        private void CalculateNextMove()
        {
            var enemyToFocus = 0; // enemy that would take the least time to kill

        }
        private void FindClosestEnemy()
        {
            foreach (var control in screen.Controls)
            {
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
                        }*/
                        //Vector2d enemyLocation = (pictureBox)
                        SetClosestEnemyData(pictureBox);
                    }
                }
            }
        }
        private void SetClosestEnemyData(PictureBox pictureBox)
        {
            var newEnemyMoveShootVector = CalculateMoveShootDistance(pictureBox);
            var distanceNewEnemy = GetAbsoluteDistance(newEnemyMoveShootVector);
            if (closestEnemyData.Name == "no data")
            {
                closestEnemyData.Name = pictureBox.Name;
                closestEnemyData.moveShootVector = newEnemyMoveShootVector;
            }
            else
            {
                if (distanceNewEnemy < shortestDistance)
                {
                    closestEnemyData.moveShootVector = newEnemyMoveShootVector;
                    closestEnemyData.Name = pictureBox.Name;

                }
                else if (distanceNewEnemy == shortestDistance)
                {
                    if (closestEnemyData.Name != pictureBox.Name)
                    {
                        closestEnemyData.Name = CompareEnemies(closestEnemyData.Name, pictureBox.Name);
                    }
                }
            }
        }
        private string CompareEnemies(string name1,  string name2)
        {
            if (diffiCultyDict[name1] >= diffiCultyDict[name2])
            {
                return name1;
            }
            return name2;
        }
        /*private Vector2d DetermineShorterDistance(Vector2d vector1,  Vector2d vector2)
        {
            var distance1 = GetAbsoluteDistance(CalculateMoveShootDistance(vector1));
            var distance2 = GetAbsoluteDistance(CalculateMoveShootDistance(vector2));
            if (distance1 == distance2)
            {
                if (vector1.Y <= vector2.Y)
                {
                    return vector1;
                }
                else if (vector1.Y > vector2.Y)
                {
                    return vector1;
                }
            }
        }*/
        private Vector2d CalculateMoveShootDistance(PictureBox pictureBox)
        {
            Vector2d result = new Vector2d();
            if (pictureBox.Left >= icon.Right)
            {
                result.X = (pictureBox.Left - icon.Left) / Speed + 2;
            }
            else if (pictureBox.Right < icon.Left)
            {
                result.X = (pictureBox.Right - icon.Left) / Speed - 2;
            }
            result.Y = (pictureBox.Bottom - icon.Top) / ProjectileSpeed + 1;
            return result;
        }  
        private int GetAbsoluteDistance(Vector2d moveVector)
        {
            var result = 0;
            result = Math.Abs(moveVector.X) + Math.Abs(moveVector.Y);
            return result;
        }

    }
}
