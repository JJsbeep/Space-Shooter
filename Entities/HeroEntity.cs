﻿using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace zap_program2024.Entities
{
    public class HeroEntity : AbstractEntity
    {
        private bool moving;
        private bool movingLeft;
        private bool autoMode;

        //variables to keep track of scores
        public int score;
        public int scoreTime;

        public int _speed;
        public int _health;
        public int _xPos;
        public int _yPos;
        public bool _onScreen;
        public HeroEntity() 
        {
            _speed = 23;
            _health = 3;
            _xPos = 593;
            _yPos = 638;
            size = (115, 130);
            _onScreen = true;
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
        public override void Shoot(PictureBox _projectile)
        {
            throw new NotImplementedException();
        }
        public override void InitializePicBox()
        {
            icon.Image = new Bitmap(@"images\HeroShip.png");
            icon.Location = new Point( XPos, YPos );
            icon.Size = new Size(size.Item1, size.Item2);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        public override void Move()
        {
            if (OnScreen)
            {
                if (autoMode)
                { 
                    if (moving && movingLeft)
                    {
                        icon.Location = new Point(XPos - Speed, YPos);
                    }
                    else if (moving && !movingLeft)
                    {
                        icon.Location = new Point(XPos + Speed, YPos);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
