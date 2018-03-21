namespace Eugenie.Clients.Seller.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Timers;

    public static class TeamViewerPopupBlocker
    {
        private const int BN_CLICKED = 245;
        private const int WM_CLOSE = 0x0010;

        private static readonly Timer timer;

        private static readonly List<string> windowNames = new List<string>
                                                    {
                                                        "Спонсорирана сесия",
                                                        "Commercial use",
                                                        "Sponsored session",
                                                        "Commercial use suspected",
                                                        "Commercial use detected",
                                                        "Unable to connect"
                                                    };

        static TeamViewerPopupBlocker()
        {
            timer = new Timer();
            timer.Interval = 2000;
            timer.Elapsed += (sender, args) => CheckTeamViewerMainWindowTitle();
        }

        public static void Start()
        {
            timer.Start();
        }

        public static void Stop()
        {
            timer.Stop();
        }

        private static void CheckTeamViewerMainWindowTitle()
        {
            var processes = Process.GetProcessesByName("TeamViewer");

            if (processes.Length == 0)
            {
                return;
            }

            var currentMainWindowTitle = processes[0].MainWindowTitle;

            if (string.IsNullOrEmpty(currentMainWindowTitle))
            {
                return;
            }

            foreach (var windowName in windowNames.Where(x => x == currentMainWindowTitle))
            {
                CloseWindowByName(windowName);
            }
        }

        private static void CloseWindowByName(string windowName)
        {
            if (string.IsNullOrEmpty(windowName))
            {
                return;
            }

            var windowPtr = FindWindowByCaption(IntPtr.Zero, windowName);

            if (windowPtr == IntPtr.Zero)
            {
                return;
            }

            var sendMsgPtr = SendMessage(windowPtr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

            if (sendMsgPtr == (IntPtr) 1)
            {
                return;
            }

            var butn = FindWindowEx(windowPtr, IntPtr.Zero, "Button", "OK");

            if (butn != IntPtr.Zero)
            {
                SendMessage(butn, BN_CLICKED, IntPtr.Zero, IntPtr.Zero);
            }
        }

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindowByCaption(IntPtr zeroOnly, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);
    }
}