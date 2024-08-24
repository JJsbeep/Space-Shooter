using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zap_program2024.Entities
{
    public abstract class AbstractEntity
    {
        protected (int, int) size;
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

        public abstract void Shoot(PictureBox projectile);
        public abstract void InitializePicBox();
        public abstract void Move();
    }
}
