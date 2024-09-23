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
    public partial class SettingsForm : Form
    {
        public GameWindow gameWindow;
        public SettingsForm(GameWindow gameForm)
        {
            InitializeComponent();
            gameWindow = gameForm;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ResumeButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            gameWindow.Resume();
        }
    }
}
