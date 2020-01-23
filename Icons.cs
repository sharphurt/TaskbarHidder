using System.Drawing;

namespace TaskBarHidder
{
    public static class Icons
    {
        public static readonly Icon Unpinned = new Icon("unpinned.ico");
        public static readonly Icon Pinned = new Icon("pinned.ico");

        public static void UpdateIcon()
        {
            var currentState = TaskBarHidder.GetTaskBarState();
            TaskBarHidderForm.NotifyIcon.Icon =
                currentState == TaskBarHidder.AppBarStates.AutoHide ? Unpinned : Pinned;
        }
    }
}