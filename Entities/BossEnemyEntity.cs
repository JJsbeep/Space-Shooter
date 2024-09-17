using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace zap_program2024.Entities
{
    public class BossEnemyEntity : AbstractEntity
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


        public BossEnemyEntity(Form form) : base(form)
        {
            _firePeriod = 750;
            _difficulty = 4;
            _speed = 8;
            _health = 10;
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
        public override void Projectile_Tick(object sender, EventArgs e)
        {
            base.MoveCurvy(projectile.icon, shootShifts);
            projectile.deleteOfScreen();
        }
        public override void InitializePicBox()
        {
            icon.Name = "BossEnemyPicbox";
            icon.Image = Image.FromFile(@"..\..\..\images\BossShip.png"); ;
            icon.Size = new Size(size.X, size.Y);
            icon.Location = new Point(XPos, YPos);
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.Visible = true;
            icon.BackColor = Color.Transparent;
        }
        public override void Move_Tick(object sender, EventArgs e)
        {
            base.MoveCurvy(icon, moveShifts);
        }
    }
}
