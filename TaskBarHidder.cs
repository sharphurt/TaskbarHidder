using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Security.Principal;
using System.Threading;

namespace TaskBarHidder
{
    public class TaskBarHidder
    {
        public static bool IsHotKeySettingMode = false;

        public struct AppSettings
        {
            public int[] keyCodes;
            public bool isAutorun;
        }

        public static AppSettings settings = ReadConfigFile();
        public static readonly int[] NewHotKeyCodes = {0, 0};
        public static int CurrentChangingKeyIndex = -1;
        private static string configHeader = "#Key codes for hidding taskbar";


        public enum AppBarStates
        {
            AutoHide = 0x01,
            AlwaysOnTop = 0x02
        }

        private enum AppBarMessages
        {
            New = 0x00000000,
            Remove = 0x00000001,
            QueryPos = 0x00000002,
            SetPos = 0x00000003,
            GetState = 0x00000004,
            GetTaskBarPos = 0x00000005,
            Activate = 0x00000006,
            GetAutoHideBar = 0x00000007,
            SetAutoHideBar = 0x00000008,
            WindowPosChanged = 0x00000009,
            SetState = 0x0000000a
        }

        private static AppSettings ReadConfigFile()
        {
            try
            {
                var lines = File.ReadAllLines("config.txt");
                var autorunValue = false;
                var keyCodesValue = new int[2];
                foreach (var line in lines)
                {
                    if (line.StartsWith("#"))
                        continue;
                    if (line.StartsWith("+Autorun"))
                        autorunValue = bool.Parse(line.Split(' ')[1]);
                    if (line.StartsWith("+FirstKeyCode"))
                        keyCodesValue[0] = int.Parse(line.Split(' ')[1]);
                    if (line.StartsWith("+SecondKeyCode"))
                        keyCodesValue[1] = int.Parse(line.Split(' ')[1]);
                }

                return new AppSettings
                {
                    isAutorun = autorunValue,
                    keyCodes = keyCodesValue
                };
            }
            catch
            {
                return new AppSettings
                {
                    isAutorun = false,
                    keyCodes = new[] {160, 27}
                };
            }
        }

        public static void UpdateConfigFile()
        {
            try
            {
                File.WriteAllText("config.txt", configHeader
                                                + "\n+FirstKeyCode " + settings.keyCodes[0]
                                                + "\n+SecondKeyCode " + settings.keyCodes[1]
                                                + "\n+Autorun " + settings.isAutorun);
            }
            catch
            {
                File.WriteAllText("config.txt", configHeader
                                                + "\n+FirstKeyCode " + 160
                                                + "\n+SecondKeyCode " + 27
                                                + "\n+Autorun " + false);
            }
        }

        private static void SetTaskBarState(AppBarStates option)
        {
            var msgData = new WinAPIFunctions.AppBarData();
            msgData.cbSize = (uint) Marshal.SizeOf(msgData);
            msgData.hWnd = WinAPIFunctions.FindWindow("System_TrayWnd", null);
            msgData.lParam = (int) option;
            WinAPIFunctions.SHAppBarMessage((uint) AppBarMessages.SetState, ref msgData);
        }

        public static AppBarStates GetTaskBarState()
        {
            var msgData = new WinAPIFunctions.AppBarData();
            msgData.cbSize = (uint) Marshal.SizeOf(msgData);
            msgData.hWnd = WinAPIFunctions.FindWindow("System_TrayWnd", null);
            return (AppBarStates) WinAPIFunctions.SHAppBarMessage((uint) AppBarMessages.GetState, ref msgData);
        }

        public static void SwitchTaskBarState()
        {
            var currentState = GetTaskBarState();
            if (currentState == AppBarStates.AutoHide)
                SetTaskBarState(AppBarStates.AlwaysOnTop);
            else
                SetTaskBarState(AppBarStates.AutoHide);
        }

        public static bool CheckForRunningCopies()
        {
            bool isOk;
            const string strMyAppName = "TaskBarHidder";
            var strMutex = WindowsIdentity.GetCurrent().Name;
            strMutex = strMutex.Split('\\')[1];
            strMutex += strMyAppName;
            var m = new Mutex(true, strMutex, out isOk);
            return isOk;
        }
    }
}