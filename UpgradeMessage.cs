using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace zap_program2024
{
    public class UpgradeMessage
    {
        Label messageLabel = new Label();
        GameWindow screen;
        public UpgradeMessage(GameWindow form)
        {
            screen = form;
        }
        public void Initialize()
        {
            messageLabel.Location = new Point(1, 664);
            messageLabel.Size = new Size(252, 120);
            messageLabel.Text = "UPGRADE READY\nPRESS 1 FOR SPEED\nPRESS 2 FOR PROJECTILE SPEED\nPRESS 3 FOR HEALTH";
            messageLabel.Font = new Font("Calibri", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            messageLabel.ForeColor = Color.Maroon;
            messageLabel.BackColor = Color.Transparent;
            messageLabel.Visible = false;
            screen.Controls.Add(messageLabel);
        }
        public void Show()
        {
            messageLabel.Visible = true;
        }
        public void Hide()
        {
            messageLabel.Visible = false;
        }
    }
}
