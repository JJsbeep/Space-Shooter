using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace zap_program2024.Entities
{
    public class BasicEnemyEntity : AbstractEntity
    {
        private int _speed;
        private int _health;
        private int _xPos;
        private int _yPos;
        private bool _onScreen;
        private bool _dead;

        
        public BasicEnemyEntity(GameWindow form) : base(form)
        {
            _speed = 4;
            _health = 1;
            _xPos = 0;
            _yPos = 0;
            _onScreen = false;
            _dead = false;
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
            icon.Name = "BasicEnemyPicbox";
            icon.Tag = "Enemy";
            icon.Image = Image.FromFile(@"..\..\..\images\BasicEnemyShip.png"); ;
            base.InitializePicBox();
        }
        protected override void GetMoveDirection()
        {
            base.GetMoveDirection();
        }
        public override void Move_Tick(object sender, EventArgs e)
        {
            if (IsAlive())
            {
                base.MoveStraight(icon, moveShifts);
            }
        }
        //no shooting since basic type of enemy does not shoot
        public override void StartOperating()
        {
            InitializeMovingTimer();
        }
    }
}
