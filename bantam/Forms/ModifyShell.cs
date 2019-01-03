﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bantam_php
{
    public partial class AddHost : Form
    {
        public static AddHost instance = null;
       
        /// <summary>
        /// 
        /// </summary>
        public static string g_CallingShellUrl = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, int> requestTypes = new Dictionary<string, int>() {
            { "cookie", 0 },
            { "post", 1 },
        };

        /// <summary>
        /// 
        /// </summary>
        public AddHost()
        {
            InitializeComponent();

            comboBoxVarType.SelectedIndex = 0;
        }

        /// <summary>
        /// Update / Edit Shell Routine
        /// </summary>
        /// <param name="shellUrl"></param>
        /// <param name="varName"></param>
        /// <param name="varType"></param>
        public AddHost(string shellUrl = "", string varName = "", string varType = "")
        {
            InitializeComponent();

            Text = "Update Shell";

            txtBoxShellUrl.Text = shellUrl;
            txtBoxArgName.Text = varName;
            g_CallingShellUrl = shellUrl;

            if (BantamMain.Shells.ContainsKey(shellUrl)) {
                checkBoxResponseEncryption.Checked = BantamMain.Shells[shellUrl].encryptResponse;
            }

            if (requestTypes.ContainsKey(varType)) {
                comboBoxVarType.SelectedIndex = requestTypes[varType];
            } else {
                comboBoxVarType.SelectedIndex = 0;
            }

            btnAddShell.Visible = false;
            btnUpdateShell.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnAddShell_Click(object sender, EventArgs e)
        {
            string shellURL = txtBoxShellUrl.Text;

            if (string.IsNullOrEmpty(shellURL)) {
                return;
            }

            if (BantamMain.Shells.ContainsKey(shellURL)) {
                Program.g_BantamMain.guiCallbackRemoveShellURL(shellURL);
                BantamMain.Shells.Remove(shellURL);
            }

            BantamMain.Shells.Add(shellURL, new ShellInfo());
            BantamMain.Shells[shellURL].requestArgName = txtBoxArgName.Text;

            if (comboBoxVarType.Text == "cookie") {
                BantamMain.Shells[shellURL].sendDataViaCookie = true;
            }

            if (checkBoxResponseEncryption.Checked == false) {
                BantamMain.Shells[shellURL].encryptResponse = false;
            }

            //MessageBox.Show("1");

            Program.g_BantamMain.InitializeShellData(shellURL);
            Program.g_BantamMain.addClientForm.Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddHost_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.g_BantamMain.addClientForm = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnUpdateShell_Click(object sender, EventArgs e)
        {
            string shellURL = txtBoxShellUrl.Text;

            if (string.IsNullOrEmpty(shellURL)) {
                return;
            }

            if (BantamMain.Shells.ContainsKey(g_CallingShellUrl)) {
                Program.g_BantamMain.guiCallbackRemoveShellURL(g_CallingShellUrl);
                BantamMain.Shells.Remove(g_CallingShellUrl);
            }

            if (BantamMain.Shells.ContainsKey(shellURL)) {
                Program.g_BantamMain.guiCallbackRemoveShellURL(shellURL);
                BantamMain.Shells.Remove(shellURL);
            }

            BantamMain.Shells.Add(shellURL, new ShellInfo());
            BantamMain.Shells[shellURL].requestArgName = txtBoxArgName.Text;

            if (comboBoxVarType.Text == "cookie") {
                BantamMain.Shells[shellURL].sendDataViaCookie = true;
            }

            if (checkBoxResponseEncryption.Checked == false) {
                BantamMain.Shells[shellURL].encryptResponse = false;
                BantamMain.Shells[shellURL].encryptResponse = false;
            }
            Program.g_BantamMain.InitializeShellData(shellURL);
            Program.g_BantamMain.updateHostForm.Hide();
        }

        private void AddHost_Shown(object sender, EventArgs e)
        {
            //MessageBox.Show("1");
        }
    }
}