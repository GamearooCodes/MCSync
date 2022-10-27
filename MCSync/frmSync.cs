﻿using MesaSuite.Common;
using MesaSuite.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MCSync
{
    public partial class frmSync : Form
    {
        private Dictionary<Task, DataGridViewRow> rowsByTask = new Dictionary<Task, DataGridViewRow>();
        private NotifyIcon trayIcon = new NotifyIcon();
        private Boolean showBalloonTips;

        public frmSync()
        {
            InitializeComponent();
        }

        private void frmSync_Load(object sender, EventArgs e)
        {
            formFadeTimer.Start();
            Dock = DockStyle.Fill;
            Dictionary<string, object> configValues = UserPreferences.Get().Sections.GetOrSetDefault("mcsync", () => new Dictionary<string, object>());
            showBalloonTips = configValues.GetOrDefault("showBalloonTips", true).Cast<bool>();

            Syncer syncer = new Syncer();
            syncer.TaskAdded += Syncer_TaskAdded;
            syncer.SyncComplete += Syncer_SyncComplete;

            trayIcon.Icon = Properties.Resources.icn_mcsync;
            trayIcon.Text = "MCSync";
            trayIcon.Visible = true;
            trayIcon.Click += new EventHandler(trayIcon_Clicked);

            if(showBalloonTips)
            {
                trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                trayIcon.BalloonTipTitle = "Beginning Sync";
                trayIcon.BalloonTipText = "Initializing" + "\n" + "Sync will begin shortly";
                trayIcon.ShowBalloonTip(3000);
            }

            syncer.BeginSync();
        }

        private void Syncer_SyncComplete(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                foreach(string error in Task.Errors)
                {
                    if(showBalloonTips)
                    {
                        trayIcon.BalloonTipIcon = ToolTipIcon.Error;
                        trayIcon.BalloonTipTitle = "Sync Error";
                        trayIcon.BalloonTipText = error;
                        trayIcon.ShowBalloonTip(3000);
                    }
                    else
                    {
                        MessageBox.Show("An error occurred during sync: \n\n" + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                foreach(string info in Task.Informations)
                {
                    if(showBalloonTips)
                    {
                        trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                        trayIcon.BalloonTipTitle = "Sync Information";
                        trayIcon.BalloonTipText = info;
                        trayIcon.ShowBalloonTip(3000);
                    }
                    else
                    {
                        MessageBox.Show(info, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                pbarOverall.Value = pbarOverall.Maximum;

                if(showBalloonTips)
                {
                    trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                    trayIcon.BalloonTipTitle = "Sync Complete";
                    trayIcon.BalloonTipText = "Syncing is complete. Make sure your resource packs are enabled!";
                    trayIcon.ShowBalloonTip(3000);
                }
                else
                {
                    MessageBox.Show("Sync Completed" + "\n" + "Make sure all resource packs are enabled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Application.ExitThread();
            });
        }

        private void Syncer_TaskAdded(object sender, Task e)
        {
            Invoke((MethodInvoker)delegate
            {
                int index = dgvTasks.Rows.Add();
                DataGridViewRow row = dgvTasks.Rows[index];

                row.Cells[0].Value = e.TaskDescription;
                row.Cells[1].Value = e.Status;

                rowsByTask[e] = row;

                e.StatusUpdate += Task_StatusUpdate;
                pbarOverall.Maximum = dgvTasks.Rows.Count;
            });
        }

        private void Task_StatusUpdate(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                Task task = (Task)sender;
                DataGridViewRow row = rowsByTask[task];

                row.Cells[1].Value = task.Status;
                lblItem.Text = "Current Task: \n" + task.TaskDescription;

                pbarOverall.Value = dgvTasks.Rows.OfType<DataGridViewRow>().Where(dgvr => !string.Equals("Waiting", dgvr.Cells[1].Value as string, StringComparison.OrdinalIgnoreCase)).Count();
            });
        }

        private void frmSync_FormClosing(object sender, FormClosingEventArgs e)
        {
            trayIcon.Dispose();
            Opacity = 0;
            Application.ExitThread();
        }

        private void trayIcon_Clicked(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            BringToFront();
        }

        private void formFadeTimer_Tick(object sender, EventArgs e)
        {
            Opacity += 0.1;
            if(Opacity > 0.85)
            {
                formFadeTimer.Stop();
            }
        }
    }
}
