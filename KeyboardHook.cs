using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TaskBarHidder
{
    public class KeyboardHook
    {
        public const int HookKeyboardLl = 0x0d;
        private const int KeyDownEventPointer = 0x0100;
        public static IntPtr _hookId = IntPtr.Zero;

        public delegate void HotkeyHandlerMethodContainer(); 
        public static event HotkeyHandlerMethodContainer onHotkeyPress;

        public delegate void KeyPressHandlerMethodContainer(int keyCode);
        public static event KeyPressHandlerMethodContainer onKeyPress;
        
        private static readonly List<int> PressedKeyCodes = new List<int>();
        
        public static readonly WinAPIFunctions.LowLevelKeyboardProc KeyboardLowLevelListenerProcess = HookCallback;
        
        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var keyCode = Marshal.ReadInt32(lParam);

            if (nCode < 0 || wParam != (IntPtr) KeyDownEventPointer)
            {
                PressedKeyCodes.Clear();
                return WinAPIFunctions.CallNextHookEx(_hookId, nCode, wParam, lParam);
            }
            //MessageBox.Show(keyCode.ToString());
            if (onKeyPress != null) onKeyPress(keyCode);
            
            CheckForHotkey(keyCode, wParam);
            return WinAPIFunctions.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        private static void CheckForHotkey(int keyCode, IntPtr keyEvent)
        {
            if (keyEvent != (IntPtr) KeyDownEventPointer)
            {
                PressedKeyCodes.Clear();
                return;
            }

            if (keyCode == TaskBarHidder.settings.keyCodes[0] || keyCode == TaskBarHidder.settings.keyCodes[1] && !PressedKeyCodes.Contains(keyCode))
                PressedKeyCodes.Add(keyCode);
            else
                PressedKeyCodes.Clear();

            if (PressedKeyCodes.Contains(TaskBarHidder.settings.keyCodes[0]) && PressedKeyCodes.Contains(TaskBarHidder.settings.keyCodes[1]))
            {
                if (onHotkeyPress != null) onHotkeyPress();
                //MessageBox.Show("it works!!!");
                PressedKeyCodes.Clear();                
            }
        }
    }
}