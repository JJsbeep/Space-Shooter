using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace zap_program2024.Entities
{
    public class HeroEntity : AbstractEntity
    {

        public bool moving;
        public bool movingLeft;
        public bool _autoMode;

        public int _firePeriod;
        
        //variables to keep track of score
        public int score;
        public int scoreTime;

        public int _speed;
        public int _health;
        public int _xPos;
        public int _yPos;
        public bool _onScreen;
        public HeroEntity()
        {
            _firePeriod = 500;
            _speed = 23;
            _health = 3;
            _xPos = 593;
            _yPos = 638;
            size.X = 115;
            size.Y = 130;
            _onScreen = true;
            _autoMode = false;
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
        public override  int YPos
        {
            get => _yPos;
        }
        public override bool OnScreen 
        { 
            get => _onScreen; 
            set => _onScreen = value; 
        }
        public bool AutoMode
        {
            get => _autoMode;
            set => _autoMode = value;
        }
        public override void Projectile_Tick(object sender, EventArgs e)
        {
            projectile.icon.Top -= projectile.Speed;
            projectile.deleteOfScreen();    
        }
        public override void Shoot(Form screen)
        {
            InitializeProjectile(screen);
            projectile.projectileSpread.Tick += Projectile_Tick;
        }
        public override void InitializePicBox()
        {
            icon.Name = "HeroPicbox";
            icon.Tag = "Hero";
            icon.Image = Image.FromFile(@"..\..\..\images\HeroShip.png");
            icon.Size = new Size(size.X, size.Y);
            icon.Location = new Point(XPos, YPos);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.Visible = true;
            icon.BackColor = Color.Transparent;
        }
        public override void Move(Form screen, Timer timer)
        {
            if (!AutoMode)
            {
                if (moving)
                {
                    if (movingLeft)
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
                    else
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
                }
            }
            else
            {
                throw new NotImplementedException();
            }

        }
        public override void Initialize(Form screen)
        {
            InitializePicBox();
            screen.Controls.Add(icon);
        }
    }
}
