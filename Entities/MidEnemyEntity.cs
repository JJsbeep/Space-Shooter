﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;


namespace zap_program2024.Entities
{
    
    public class MidEnemyEntity : AbstractEntity
    {
        private int _health;
        private int _firePeriod;
        private int _projectileSpeed;
        private int _speed;
        private int _xPos;
        private int _yPos;
        private bool _onScreen;
        private bool _dead;

        public MidEnemyEntity(GameWindow form) : base(form)
        {
            _projectileSpeed = 6;
            _firePeriod = 2750;
            _speed = 3;
            _health = 2;
            _xPos = 0;
            _yPos = 0;
            _dead = false;
            _onScreen = false;
        }
        public override int FirePeriod
        {
            get => _firePeriod;
            set => _firePeriod = value;
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
        public override int YPos
        {
            get => _yPos; 
            set => _yPos = value;
        }
        public override bool OnScreen
        {
            get => _onScreen;
            set => _onScreen = value;
        }
        public override bool Dead
        {
            get => _dead;
            set => _dead = value;
        }
        protected override void InitializePicBox()
        {
            icon.Name = "MidEnemyPicbox";
            icon.Tag = "Enemy"; 
            icon.Image = Image.FromFile(@"..\..\..\images\MidEnemyShip.png"); ;
            base.InitializePicBox();
        }
        public override void Shoot(object sender, EventArgs e)
        {
            if (!Dead && IsPositionToShoot())
            {
                Projectile projectile = new Projectile(screen);
                GetRandomTarget();
                InitializeProjectile(projectile);
            }
        }
        public override void Move_Tick(object sender, EventArgs e)
        {
            if (IsAlive())
            {
                base.MoveStraight(icon, moveShifts);
            }
        }
    }
}
