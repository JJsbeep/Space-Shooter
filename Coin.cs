using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;
using zap_program2024.Entities;

namespace zap_program2024
{
    public class Coin
    {
        private const int floorLocation = 657;
        private const string heroTag = "Hero";
        private Random rnd = new Random();
        private GameWindow screen;
        private PictureBox icon = new PictureBox();
        public Vector2d location = new Vector2d();
        public Vector2d size = new Vector2d(50, 50);
        public Timer pickTimer = new Timer();
        private const int timeToPick = 3200;
        public bool Availablable { get; set; } = false;
        public Coin(GameWindow form) 
        {
            screen = form;
        }
        public void GetRandomLocation()
        {
            location.Y = floorLocation;
            location.X = 1000;//rnd.Next(0, 1000);
        }
        public void Initilaize()
        {
            GetRandomLocation();
            icon.Image = icon.Image = Image.FromFile(@"..\..\..\images\coin.gif");
            icon.Location = new Point(location.X, location.Y);
            icon.SizeMode = PictureBoxSizeMode.AutoSize;
            icon.Visible = false;
            icon.BackColor = Color.Transparent;
            icon.Size = new Size(size.X, size.Y);
            //screen.Controls.Add(icon);
        }
        public void Appear()
        {
            GetRandomLocation();
            screen.Controls.Add(icon);
            icon.Visible = true;
            pickTimer.Interval = timeToPick;
            pickTimer.Tick += Dissappear;
            pickTimer.Start();
            Availablable = true;
        }
        private void Dissappear(object sender, EventArgs e)
        {
            icon.Visible = false;
            screen.Controls.Remove(icon);
            pickTimer.Stop();
            Availablable = false;
            screen.coinAppeared = false;
            screen.scoreBar.UpgradeReady = false;
        }
        private void Take()
        {
            screen.Controls.Remove(icon);
            icon.Visible = false;
            pickTimer.Stop();
            Availablable = false;
        }
        public bool IsHero(PictureBox pictureBox)
        {
            if (pictureBox is not null && pictureBox.Tag?.ToString() == heroTag)
            {
                return true;
            }
            return false;
        }
        public bool PickedUp()
        {
            foreach (var control in screen.Controls)
            {
                if (control is PictureBox picturebox && IsHero(picturebox))
                {
                    if (picturebox.Bounds.IntersectsWith(icon.Bounds))
                    {
                        Take();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
