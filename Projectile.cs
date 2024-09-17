using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using zap_program2024.Entities;

namespace zap_program2024
{
    public class Projectile
    {
        const int windowHeight = 800;
        const int windowWidth = 1300;

        public int Speed { get; set; } = 4;
        public PictureBox icon = new PictureBox();
        public Timer projectileSpread = new Timer();
        public bool IsOnScreen(PictureBox pictureBox)
        {
            if (windowHeight >= pictureBox.Top && pictureBox.Bottom >= 0 && windowWidth >= pictureBox.Left && pictureBox.Right >= 0)
            {
                return true;
            }
            return false;
        }
        public void Spawn(Form screen, Vector2d coordinates)
        {
            icon.Location = new Point(coordinates.X, coordinates.Y);
            icon.Size = new Size(15, 15);
            icon.Tag = "projectile";
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.BringToFront();

            screen.Controls.Add(icon);
            projectileSpread.Interval = Speed;
            projectileSpread.Start();
        }
        public void deleteOfScreen()
        {
            if (!IsOnScreen(icon))
            {
                projectileSpread.Stop();
                projectileSpread.Dispose();
                icon.Dispose();
                projectileSpread = null;
                icon = null;
            }
        }
    }
}
