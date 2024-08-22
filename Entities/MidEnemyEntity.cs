﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zap_program2024.Entities
{
    internal class MidEnemyEntity : AbstractEntity
    {

        public int _speed;
        public int _health;
        public int _xPos;
        public int _yPos;
        public bool _onScreen;
        public MidEnemyEntity()
        {
            _speed = 12;
            _health = 3;
            _xPos = 0;
            _yPos = 0;
            _onScreen = false;
            size = (77, 72);
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
        }
        public override bool OnScreen
        {
            get => _onScreen;
            set => _onScreen = value;
        }
        public override void Shoot(PictureBox _projectile)
        {
            throw new NotImplementedException();
        }
        public override void InicializePicBox()
        {
            icon.Image = new Bitmap(@"images\MidEnemyShip.png");
            icon.Location = new Point(XPos, YPos);
            icon.Size = new Size(size.Item1, size.Item2);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        public override void Move()
        {
            throw new NotImplementedException();
        }
    }
}
