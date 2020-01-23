using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TaskBarHidder
{
    static class Program
    {
        [STAThread]
        private static void Main()
        {
            KeyboardHook._hookId = WinAPIFunctions.SetHook(KeyboardHook.KeyboardLowLevelListenerProcess);

            KeyboardHook.onHotkeyPress += KeyPressHandler.HandleHotKeyPress;
            KeyboardHook.onKeyPress += KeyPressHandler.HandleKeyPress;
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (TaskBarHidder.CheckForRunningCopies())
                Application.Run(new TaskBarHidderForm());
            else
            {
                MessageBox.Show("Application is already running", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            WinAPIFunctions.UnhookWindowsHookEx(KeyboardHook._hookId);
        }
    }
}