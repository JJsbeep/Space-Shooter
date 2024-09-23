using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zap_program2024
{
    public partial class MainMenu : Form
    {
        private Button ManualModeButton;
        private Button AutoModeButton;
        private Label GameName;
        public GameWindow gameWindow;
        public SettingsForm settingsForm;

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainMenu));
            ManualModeButton = new Button();
            AutoModeButton = new Button();
            GameName = new Label();
            SuspendLayout();
            // 
            // ManualModeButton
            // 
            ManualModeButton.BackColor = Color.Transparent;
            ManualModeButton.BackgroundImage = (Image)resources.GetObject("ManualModeButton.BackgroundImage");
            ManualModeButton.Font = new Font("Broadway", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ManualModeButton.ForeColor = Color.Maroon;
            ManualModeButton.Location = new Point(268, 344);
            ManualModeButton.Name = "ManualModeButton";
            ManualModeButton.Size = new Size(683, 130);
            ManualModeButton.TabIndex = 0;
            ManualModeButton.Text = "Manual Mode";
            ManualModeButton.UseVisualStyleBackColor = true;
            ManualModeButton.Click += ManualModeButton_Click;
            // 
            // AutoModeButton
            // 
            AutoModeButton.BackColor = Color.Transparent;
            AutoModeButton.BackgroundImage = (Image)resources.GetObject("AutoModeButton.BackgroundImage");
            AutoModeButton.Font = new Font("Broadway", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            AutoModeButton.ForeColor = Color.Maroon;
            AutoModeButton.Location = new Point(268, 554);
            AutoModeButton.Name = "AutoModeButton";
            AutoModeButton.Size = new Size(683, 130);
            AutoModeButton.TabIndex = 1;
            AutoModeButton.Text = "Auto Mode";
            AutoModeButton.UseVisualStyleBackColor = true;
            AutoModeButton.Click += AutoModeButton_Click;
            // 
            // GameName
            // 
            GameName.BackColor = Color.Transparent;
            GameName.Font = new Font("Broadway", 48F, FontStyle.Regular, GraphicsUnit.Point, 0);
            GameName.ForeColor = Color.Maroon;
            GameName.Location = new Point(186, 131);
            GameName.Name = "GameName";
            GameName.Size = new Size(853, 179);
            GameName.TabIndex = 2;
            GameName.Text = "SPACESHIP \r\nSHOOTER";
            GameName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // MainMenu
            // 
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1282, 753);
            Controls.Add(GameName);
            Controls.Add(AutoModeButton);
            Controls.Add(ManualModeButton);
            Name = "MainMenu";
            ResumeLayout(false);
            //
            // Game Window
            //
            gameWindow = new GameWindow();
            //
            // Settings Window
            //
            
        }

        public MainMenu()
        {
            InitializeComponent();
            
        }

        private void ManualModeButton_Click(object sender, EventArgs e)
        {
            Hide();
            gameWindow.SetAutoMode(false);
            gameWindow.ShowDialog();
            Close();
        }

        private void AutoModeButton_Click(object sender, EventArgs e)
        {
            Hide();
            gameWindow.SetAutoMode(true);
            gameWindow.ShowDialog();
            Close();
        }
    }
}
