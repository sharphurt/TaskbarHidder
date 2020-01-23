using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace TaskBarHidder
{
    public static class FormEventHandlers
    {
        private static Form _form;
        private static FormWindowState _oldFormState;

        public static Form Form
        {
            set { _form = value; }
        }

        public static void TaskBarHidderFormLoad(object sender, EventArgs e)
        {
            _form.Hide();
        }

        public static void ExitMenuItemClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public static void NotifyIconClick(object sender, EventArgs e)
        {
            if (_form.WindowState == FormWindowState.Normal || _form.WindowState == FormWindowState.Maximized)
            {
                _oldFormState = _form.WindowState;
                _form.WindowState = FormWindowState.Minimized;
            }
            else
            {
                _form.Show();
                _form.WindowState = _oldFormState;
            }
        }

        public static void WindowResize(object sender, EventArgs e)
        {
            if (_form.WindowState == FormWindowState.Minimized)
                _form.Hide();
        }

        public static void TimerTick(object sender, EventArgs e)
        {
            Icons.UpdateIcon();
            var key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", true);
            TaskBarHidder.settings.isAutorun = key.GetValue("TaskBarHidder") != null;
            TaskBarHidderForm.AutorunCheckBox.Checked = key.GetValue("TaskBarHidder") != null;
            TaskBarHidder.UpdateConfigFile();
        }

        public static void ChangeKey1ButtonClick(object sender, EventArgs e)
        {
            TaskBarHidder.IsHotKeySettingMode = true;
            TaskBarHidder.CurrentChangingKeyIndex = 0;
            TaskBarHidderForm.KeyButtons[0].Text = "...";
            if (TaskBarHidder.NewHotKeyCodes[1] == -1)
                TaskBarHidderForm.KeyButtons[1].Text = TaskBarHidder.settings.keyCodes[1].ToString();
            TaskBarHidderForm.SaveChangesButton.Visible = true;
        }

        public static void ChangeKey2ButtonClick(object sender, EventArgs e)
        {
            TaskBarHidder.IsHotKeySettingMode = true;
            TaskBarHidder.CurrentChangingKeyIndex = 1;
            TaskBarHidderForm.KeyButtons[1].Text = "...";
            if (TaskBarHidder.NewHotKeyCodes[0] == -1)
                TaskBarHidderForm.KeyButtons[0].Text = TaskBarHidder.settings.keyCodes[0].ToString();
            TaskBarHidderForm.SaveChangesButton.Visible = true;
        }

        public static void ChangeHotKeyModeButtonClick(object sender, EventArgs e)
        {
            TaskBarHidderForm.ChangeHotKeyPanel.Visible = true;
            TaskBarHidderForm.KeyButtons[0].Text = TaskBarHidder.settings.keyCodes[0].ToString();
            TaskBarHidderForm.KeyButtons[1].Text = TaskBarHidder.settings.keyCodes[1].ToString();
        }

        public static void SaveChangesButtonClick(object sender, EventArgs e)
        {
            TaskBarHidder.settings.keyCodes[0] = TaskBarHidder.NewHotKeyCodes[0];
            TaskBarHidder.settings.keyCodes[1] = TaskBarHidder.NewHotKeyCodes[1];
            TaskBarHidder.UpdateConfigFile();
            TaskBarHidderForm.ChangeHotKeyPanel.Visible = false;
            TaskBarHidderForm.SaveChangesButton.Visible = false;
        }

        public static void AutorunCheckboxChanged(object sender, EventArgs e)
        {
            var key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            if (TaskBarHidderForm.AutorunCheckBox.Checked)
                key.SetValue("TaskBarHidder", Application.ExecutablePath);
            else 
                key.DeleteValue("TaskBarHidder");
            TaskBarHidder.UpdateConfigFile();
        }

        public static void FormClosing(object sender, FormClosingEventArgs e)
        {
            _oldFormState = _form.WindowState;
            _form.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }
    }
}