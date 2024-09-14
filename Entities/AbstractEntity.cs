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
    public record struct Vector2d(int X, int Y);
    public abstract class AbstractEntity
    {
        protected const int windowHeigth = 800;
        protected const int windowWidth = 1300;

        protected Vector2d size = new (77,72);
        protected Vector2d targetCoordinates = new Vector2d();
        protected Vector2d projectileDirection = new Vector2d();
        protected Projectile projectile = new Projectile();
        
        protected abstract int FirePeriod {  get; }
        protected virtual int Difficulty { get; set; } = 0;
        public virtual int Speed { get; set; } = 7;
        public virtual int Health { get; set; } = 1;
        public virtual int SpawnPeriod { get; set; }
        public virtual int XPos { get; set; }
        public virtual int YPos { get; set; }
        public virtual bool OnScreen { get; set; }
        // add spawntime
        public PictureBox icon = new PictureBox();

        public AbstractEntity()
        {
            XPos = 0;
            YPos = 0;
            OnScreen = false;
        }

        public AbstractEntity(int xPos, int yPos)
        {
            this.XPos = xPos;
            this.YPos = yPos;
        }

        public void SetCoordinates(int x, int y)
        {
            icon.Location = new Point(x, y);
        }

        public  bool GotHit(PictureBox _projectile)
        {
            return _projectile.Bounds.IntersectsWith(icon.Bounds);
        }

        public void DeleteObject(Form screen)
        {
            screen.Controls.Remove(icon);
        }

        public void SpawnEntity(Form screen)
        {
            screen.Controls.Add(icon);
        }
        private bool TargetIsHero(PictureBox pictureBox)
        {
            if (pictureBox.Tag != null && pictureBox.Tag.ToString() == "Hero")
            {
                return true;
            }
            return false;
        }
        protected void FindShootTarget(Form screen)
        {
            foreach (var control in screen.Controls)
            {
                if (control is PictureBox pictureBox && TargetIsHero(pictureBox))
                {
                    targetCoordinates.X = pictureBox.Location.X;
                    targetCoordinates.Y = pictureBox.Location.Y;
                    return;
                }
            }
        }
        protected void GetDirectionVector()
        {
            projectileDirection.X = targetCoordinates.X - projectile.icon.Location.X;
            projectileDirection.Y = targetCoordinates.Y - projectile.icon.Location.Y;
        }
        protected virtual void InitializeProjectile(Form screen)
        {
            Vector2d spawnCoordinates = new((icon.Left + icon.Width) / 2, icon.Top);
            projectile.Spawn(screen, spawnCoordinates);
            projectile.projectileSpread.Interval = FirePeriod;
        }
        public virtual void Projectile_Tick(object sender, EventArgs e)
        {
            projectile.icon.Left += projectileDirection.X / projectile.speed;
            projectile.icon.Top += projectileDirection.Y / projectile.speed;
            projectile.deleteOfScreen();
        }
        public virtual void Shoot(Form screen)
        {
            InitializeProjectile(screen);
            FindShootTarget(screen);
            projectile.projectileSpread.Tick += Projectile_Tick;
        }
        public abstract void InitializePicBox();
        public abstract void Move();
    }
}
