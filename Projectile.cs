using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using zap_program2024.Entities;
using System.Diagnostics.Metrics;
using System.Diagnostics.Eventing.Reader;

namespace zap_program2024
{
    public class Projectile
    {
        Form screen;
        public Projectile(Form form)
        {
            screen = form;
        }
        const int windowHeight = 800;
        const int windowWidth = 1300;
        const string heroTag = "Hero";
        const string enemyTag = "Enenmy";

        private int counter = 1;
        private int sign = 1;
        private bool hit = false;
        public int Speed { get; set; } = 6;
        public PictureBox icon = new PictureBox();
        public Timer projectileSpread = new Timer();
        public Vector2d travelShifts = new Vector2d();
        public Vector2d curveCheckpoints = new(1, 12);
        public Vector2d targetCoords = new Vector2d();
        public Vector2d directionVector = new Vector2d();
        public bool Off { get; set; } = false;

        private bool GotRightTarget(string targetTag, PictureBox target)
        {
            if (target.Tag != null && target.Tag.ToString() == targetTag)
            {
                return true;
            }
            return false;
        }
        private bool GotHit(string targetTag)
        {
            foreach(var control in screen.Controls)
                    {
                if (control is PictureBox pictureBox && GotRightTarget(targetTag, pictureBox))
                {
                    if (pictureBox.Bounds.IntersectsWith(icon.Bounds))
                    {
                        Off = true;
                        return true;
                    }
                }
            }
            return false;
        }
        private bool CheckHit(string targetTag)
        {
            if (icon is not null)
            {
                if (!Off)
                {
                    return GotHit(targetTag);
                }
                else { return true; }
            }
            else 
            {
                projectileSpread?.Stop();
                return true;
            }
        }
        public bool IsOnScreen(PictureBox pictureBox)
        {
            if (windowHeight >= pictureBox.Top && pictureBox.Bottom >= 0 && windowWidth >= pictureBox.Left && pictureBox.Right >= 0)
            {
                return true;
            }
            return false;
        }
        public void Spawn(Vector2d coordinates, Vector2d size, int projectileSpeed)
        {
            Speed = projectileSpeed;
            icon.Location = new Point(coordinates.X, coordinates.Y);
            icon.Size = new Size(size.X, size.Y);
            //icon.Tag = "Projectile";
            icon.SizeMode = PictureBoxSizeMode.StretchImage;
            icon.BringToFront();
            screen.Controls.Add(icon);
            projectileSpread.Interval = projectileSpeed;
        }
        public void Delete(bool force = false)
        {
            Off = !IsOnScreen(icon);
            if (force || Off)
            {
                projectileSpread.Stop();
                projectileSpread.Dispose();
                icon.Dispose();
                screen.Controls.Remove(icon);
                projectileSpread = null;
                icon = null;
            }
        }
        public void SetShifts(Vector2d shifts)
        {
            travelShifts.X = shifts.X;
            travelShifts.Y = shifts.Y;
        }
        public void TravelStraight(object sender, EventArgs e)
        {
            if (!Off)
            {
                hit = CheckHit("Hero");
                if (hit)
                {
                    Delete(true);
                }
                else
                {
                    icon.Left += travelShifts.X;
                    icon.Top += travelShifts.Y;
                    Delete();
                }
            }
            else
            {
                projectileSpread?.Stop();
            }
        }
        private void MoveStraight()
        {
            icon.Left += travelShifts.X;
            icon.Top += travelShifts.Y;
            Delete();
        }
        private void MoveCurve()
        {
            if (curveCheckpoints.X <= counter && counter < curveCheckpoints.Y / 2)
            {
                icon.Left += sign * travelShifts.X;
                counter++;
            }
            else if (curveCheckpoints.Y / 2 <= counter && counter < curveCheckpoints.Y)
            {
                icon.Top += travelShifts.Y;
                counter++;
            }
            else { counter = 1; }
            Delete();
        }
        public void TravelCurvy(object sender, EventArgs e)
        {
            if (!Off)
            {
                hit = CheckHit(heroTag);
                if (hit)
                {
                    Delete(true);
                }
                else
                {
                    MoveCurve();
                }
            }
            else { counter = 1; }
        }
        public void TravelUp(object sender, EventArgs e)
        {
            hit = CheckHit(enemyTag);
            if (hit)
            {
                Delete(true);
            }
            else
            {
                icon.Top -= Speed;
            }
        }
    }
}
