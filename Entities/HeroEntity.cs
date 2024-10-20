﻿using Microsoft.VisualBasic.ApplicationServices;
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
    //name is based on type of enemy and and entries of moveShootVector represents amount of moves to reach target X position and amount of moves of a projectile to reach the target
    public record struct EnemyData(string Name, Vector2d moveShootVector);

    public class HeroEntity : AbstractEntity
    {
        public DateTime lastShotTime = DateTime.MinValue;
        public TimeSpan shotCooldown = TimeSpan.FromSeconds(0.5);
        

        public bool moving;
        public bool movingLeft;


        private bool _autoMode;

        public int _firePeriod;
        private int _projectileSpeed;

        //variables to keep track of score

        private int _speed;
        private int _health;
        private int _xPos;
        private int _yPos;
        private bool _onScreen;
        private bool _dead;

        //Automode variables
        private Dictionary<int, PictureBox> foundEnemies; //values will be vector that determines enenemy's movement
        private Dictionary<string, int> upgradeCodes;
        private Vector2d moveShoot = new Vector2d(); // Item.1 - Amount of moves(+to the right, -to the left), Item.2 - Amount of moves after which heor shoots
        private int coinXCoord = -1; //negative value if it is not present on the screen initially
        private const int critHealthAmount = 3;
        private const string enemyTag = "Enemy";
        private const string enemyProjectileTag = "ProjectileEnemy";
        private static readonly List<string> dangerTags = new List<string>() { enemyTag, enemyProjectileTag};

        private const int maxEnemyProjectileSpeed = 9;
        private const int shootBound = 5;
        private readonly Vector2d zeroVector = new Vector2d(0, 0);
        private EnemyData closestEnemyData;
        private int lastEvenlyUpgrade;



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
            upgradeCodes = new Dictionary<string, int> {
                {"NoUpgrade", 0 },
                {"Speed", 1 },
                {"ProjectileSpeed", 2 },
                {"Health", 3 },
            };
            lastEvenlyUpgrade = upgradeCodes["NoUpgrade"];
        }
        public bool ShootingReady
        {
            get;
            set;
        } = true;
        public override int FirePeriod
        {
            get => _firePeriod;
        }
        public override int ProjectileSpeed
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
        protected override void InitializePicBox()
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
            if (!screen.gamePaused)
            {
                if (GotHit("ProjectileEnemy") || GotHit("Enemy"))
                {
                    return Death();
                }
                else
                {
                    return true;
                }
            }
            else {return true;}
        }

        private bool Death()
        {
            Health--;
            screen.healthBar.Update();
            if (Health == 0)
            {
                Dead = true;
                DeleteObject();
                screen.GameOver();
                return false;
            }
            return true;
        }
        public void HeroShoot()
        {
            if (!Dead)
            {
                Projectile projectile = new Projectile(screen);
                InitializeProjectile(projectile);

            }
            else
            {
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
            screen.healthBar.Update();
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
                    UpgradeProjectileSpeed();
                    break;
                case 3:
                    UpgradeHealth();
                    break;
                default:
                    throw new Exception("No upgrade for given code");
            }
        }

        //automode
        private void Action()
        {
            if (!screen.gamePaused)
            {
                if (CoinAvailable())
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
        }
        private bool CanShoot()
        {
            if ((closestEnemyData.moveShootVector.X * Speed / FirePeriod) >= 1 ||
                closestEnemyData.moveShootVector.X < shootBound) //checks if there is enought time to fire a projectile in order to be ready to fire when destination is reached
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
        }
        private void DoMove(int move)
        {
            if (move > 0)
            {
                MoveRight();
            }
            else if (move < 0)
            {
                MoveLeft();
            }
            else { icon.Left += 0; }
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
                if (control is PictureBox pictureBox)
                {
                    if (pictureBox.Tag is not null && pictureBox.Tag.ToString() == enemyTag)
                    {
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
            
            if (closestEnemyData.Name == "no data") // no focus
            {
                closestEnemyData.Name = pictureBox.Name;
                closestEnemyData.moveShootVector = newEnemyMoveShootVector;
            }
            else
            {
                DislocateDead();
                if (distanceNewEnemy < GetAbsoluteDistance(closestEnemyData.moveShootVector)) //closer enemy is found
                {
                    closestEnemyData.moveShootVector = newEnemyMoveShootVector;
                    closestEnemyData.Name = pictureBox.Name;
                }
                else if (distanceNewEnemy == GetAbsoluteDistance(closestEnemyData.moveShootVector)) //target as far as found object
                {
                    if (closestEnemyData.Name != pictureBox.Name) //sets the focus on a stronger enemy
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
        private string CompareEnemies(string name1,  string name2) //compares enemies based on strength
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
                result.X = ((pictureBox.Left - icon.Right) / Speed) + 2; 
                Console.WriteLine(result.X);
            }
            else if (pictureBox.Right <= icon.Left)
            {
                result.X = ((pictureBox.Right - icon.Left ) / Speed) - 2;
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
            return result;
        }  
        private int GetAbsoluteDistance(Vector2d moveVector)
        {
            var result = 0;
            result = Math.Abs(moveVector.X) + Math.Abs(moveVector.Y);
            return result;
        }
        private void DislocateDead() // sets no focus when focused enemy is shot down
        {
            var focusPoint = new Point(icon.Left + closestEnemyData.moveShootVector.X, icon.Top - closestEnemyData.moveShootVector.Y - 2);
            foreach (var control in screen.Controls)
            {
                if (control is PictureBox { Name: "Enemy" } pictureBox)
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
            var newIcon = new PictureBox();
            newIcon.Size = new Size(icon.Size.Width + Math.Abs(moveToMake) , icon.Size.Height + 2 * maxEnemyProjectileSpeed);
            newIcon.Left = icon.Left +  2 * moveToMake;
            newIcon.Top = icon.Top + 2 * maxEnemyProjectileSpeed;
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
                if (control is PictureBox pictureBox && dangerTags.Contains(pictureBox.Tag?.ToString()))
                {
                    if (pictureBox.Bounds.IntersectsWith(Area.Bounds))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool CoinAvailable()
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
        private int FindSafeMove() // check whether it is safe to move to right, left, or stay at place
        {
            var move = 0;
            if (AreaSafe( 3 * Speed))
            {
                move = Speed;
            }
            else if (AreaSafe(- 3 * Speed))
            {
                move = -Speed;
            }
            return move;
        }
        public void AutoUpgrade() //auto upgrades ship when a coin is picked up
        {
            if (Health <= critHealthAmount)
            {
                UpgradeHealth();
            }
            else
            {
                UpgradeEvenly();
            }
            screen.SetAfterCoinPicked();
        }
        private void UpgradeEvenly() // upgrades speed and projectile speed evenly
        {
            if (lastEvenlyUpgrade == upgradeCodes["NoUpgrade"] || lastEvenlyUpgrade == upgradeCodes["Speed"])
            {
                UpgradeProjectileSpeed();
                return;
            }
            UpgradeSpeed();
        }
    }
}
