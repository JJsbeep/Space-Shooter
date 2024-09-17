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
        public int _difficulty;
        public int _firePeriod;
        public int _speed;
        public int _health;
        public int _xPos;
        public int _yPos;
        public int _spawnPeriod;
        public bool _onScreen;
        public bool _dead;

        public MidEnemyEntity(Form form) : base(form)
        {
            _difficulty = 2;
            _firePeriod = 1000;
            _speed = 4;
            _health = 3;
            _xPos = 0;
            _yPos = 0;
            _dead = false;
            _onScreen = false;
        }
        protected override int Difficulty
        {
            get => _difficulty;
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
        public override int YPos
        {
            get => _yPos; 
            set => _yPos = value;
        }
        public override int SpawnPeriod
        {
            get => _spawnPeriod;
            set => _spawnPeriod = value;
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
        public override void InitializePicBox()
        {
            icon.Name = "MidEnemyPicbox";
            icon.Image = Image.FromFile(@"..\..\..\images\MidEnemyShip.png"); ;
            icon.Size = new Size(size.X, size.Y);
            icon.Location = new Point(XPos, YPos);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.Visible = true;
            icon.BackColor = Color.Transparent;
        }
        public override void GetProjectileDirection()
        {
            GetRandomTarget();
            base.GetProjectileDirection();
        }
        public override void Shoot(object sender, EventArgs e)
        {
            if (projectile == null || projectile.icon == null)
            {
                // Handle the null case, possibly reinitialize or log an error
                return;
            }
            InitializeProjectile();
            GetRandomTarget();
            GetProjectileDirection();
            projectile.projectileSpread.Tick += Projectile_Tick;
        }
        public override void Move_Tick(object sender, EventArgs e)
        {
            base.MoveStraight(icon, moveShifts);
        }
    }
}
