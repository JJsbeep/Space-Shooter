using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace zap_program2024
{
    public class HealthBar
    {
        GameWindow screen;
        public HealthBar(GameWindow form)
        {
            screen = form;
        }
        public Label healthLabel = new Label();
        public void Initialize()
        {
            healthLabel.Size = new Size(87, 30);
            healthLabel.Location = new Point(50, 92);
            healthLabel.Font = new Font("Calibri", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            healthLabel.Text = $"♥: {screen.hero.Health}";
            healthLabel.ForeColor = Color.Maroon;
            healthLabel.BackColor = Color.Transparent;
            healthLabel.Visible = true;
            screen.Controls.Add(healthLabel);
        }
        public void Update()
        {
            healthLabel.Text = $"♥: {screen.hero.Health}";
        }
    }
}
