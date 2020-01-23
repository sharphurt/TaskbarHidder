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
            Application.Run(new TaskBarHidderForm());

            WinAPIFunctions.UnhookWindowsHookEx(KeyboardHook._hookId);
        }
    }
}