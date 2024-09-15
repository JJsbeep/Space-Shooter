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
        const int windowHeigth = 800;
        const int windowWidth = 1300;

        public int Speed { get; set; } = 8;
        public PictureBox icon = new PictureBox();
        public Timer projectileSpread = new Timer();
        public double Amplitude { get; set; } = 50; // Amplitude of the sine wave
        public double Frequency { get; set; } = 0.1; // Frequency of the sine wave
        public double TimeElapsed { get; set; } = 0; // Time elapsed since the projectile was spawned
        public bool IsOnScreen(PictureBox icon)
        {
            if (icon.Top < 0 || icon.Top > windowHeigth || icon.Left < 0 || icon.Left > windowWidth)
            {
                return true;
            }
            return false;
        }
        public void Spawn(Form screen, Vector2d coordinates)
        {
            icon.Location = new Point(coordinates.X, coordinates.Y);
            icon.Size = new Size(10, 10);
            icon.Tag = "projectile";
            icon.BringToFront();

            screen.Controls.Add(icon);
            projectileSpread.Interval = 16;
            projectileSpread.Start();
        }
        public void deleteOfScreen()
        {
            if (IsOnScreen(icon))
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
