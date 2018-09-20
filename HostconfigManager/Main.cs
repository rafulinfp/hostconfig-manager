using System;
using System.Drawing;
using System.Windows.Forms;
using HostconfigManager.Core.Services;

namespace HostconfigManager
{
    public partial class Main : Form
    {
        private bool _hidden = false;
        private EnvironmentConfig environment;
        private bool _enforceClosing = false;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Hide();
            notifyIcon.Visible = true;


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
                notifyIcon.BalloonTipTitle = $"Active Environment: {environment.CurrentEnvironment.Hostname}";
                notifyIcon.BalloonTipText = $"{environment.CurrentEnvironment.IpAddress}";
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.ShowBalloonTip(3000);
            }
            else
            {
                notifyIcon.BalloonTipTitle = $"Error";
                notifyIcon.BalloonTipText = $"Could not set environment configuration for: {targetEnv}";
                notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
                notifyIcon.ShowBalloonTip(3000);
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
                ShowInTaskbar = true;
                WindowState = FormWindowState.Maximized;
                Show();
                _hidden = false;
            }
            else
            {
                ShowInTaskbar = false;
                WindowState = FormWindowState.Minimized;
                Hide();
                _hidden = true;
            }

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_enforceClosing)
            {
                e.Cancel = true;
                ToggleMain();
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            notifyIcon.ShowBalloonTip(3000);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _enforceClosing = true;
            Application.Exit();
        }
    }
}
