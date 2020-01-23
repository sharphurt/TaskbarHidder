namespace TaskBarHidder
{
    public static class KeyPressHandler
    {
        public static void HandleHotKeyPress()
        {
            TaskBarHidder.SwitchTaskBarState();
            Icons.UpdateIcon();
        }

        public static void HandleKeyPress(int keyCode)
        {
            if (TaskBarHidder.IsHotKeySettingMode)
            {
                TaskBarHidder.NewHotKeyCodes[TaskBarHidder.CurrentChangingKeyIndex] = keyCode;
                TaskBarHidderForm.KeyButtons[TaskBarHidder.CurrentChangingKeyIndex].Text = keyCode.ToString();
            }
            if (++TaskBarHidder.CurrentChangingKeyIndex > 1)
                TaskBarHidder.IsHotKeySettingMode = false;
        }
    }
}