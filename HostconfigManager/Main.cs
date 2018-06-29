using HostconfigManager.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HostconfigManager
{
    public partial class Main : Form
    {
        private bool _hidden = false;
        private EnvironmentConfig environment;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Hide();
            notifyIcon.Visible = true;
            notifyIcon.BalloonTipTitle = "Current environmnet";
            notifyIcon.BalloonTipText = "this is the text";
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;

            _hidden = true;

            environment = new EnvironmentConfig();
            foreach (var env in environment.Environments)
            {
                var toolStrip = new ToolStripMenuItem
                {
                    Name = env.Hostname,
                    Text = env.Hostname
                };
                toolStrip.Click += ToolStrip_Click;
                contextMenuStrip.Items.Add(toolStrip);
            }
        }

        private void ToolStrip_Click(object sender, EventArgs e)
        {
            var targetEnv = (sender as ToolStripMenuItem).Name;
            var result = environment.SetEnvironment(targetEnv);
            foreach (var toolStripItem in contextMenuStrip.Items)
            {
                if (toolStripItem is ToolStripMenuItem)
                {
                    (toolStripItem as ToolStripMenuItem).Font = new Font((toolStripItem as ToolStripMenuItem).Font, FontStyle.Regular);
                }
            }

            // set selected to black
            if (result)
            {
                (sender as ToolStripMenuItem).Font = new Font((sender as ToolStripMenuItem).Font, FontStyle.Bold);

            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ToggleMain();
        }

        private void ToggleMain()
        {
            if (_hidden)
            {
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Maximized;
                this.Show();
                _hidden = false;
            }
            else
            {
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                _hidden = true;
            }

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            ToggleMain();
        }
    }
}
