namespace Eugenie.Clients.Seller.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Timers;

    public class TeamViewerPopupBlocker
    {
        private const int BN_CLICKED = 245;
        private const int WM_CLOSE = 0x0010;

        private readonly Timer timer;

        private readonly List<string> windowNames = new List<string>
                                                    {
                                                        "Спонсорирана сесия",
                                                        "Commercial use",
                                                        "Sponsored session",
                                                        "Commercial use suspected",
                                                        "Commercial use detected",
                                                        "Unable to connect"
                                                    };

        public TeamViewerPopupBlocker()
        {
            this.timer = new Timer();
            this.timer.Interval = 2000;
            this.timer.Elapsed += this.OnTimer;
        }

        public void Start()
        {
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            this.CheckTeamViewerMainWindowTitle();
        }

        private void CheckTeamViewerMainWindowTitle()
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

            foreach (var windowName in this.windowNames.Where(x => x == currentMainWindowTitle))
            {
                this.CloseWindowByName(windowName);
            }
        }

        private void CloseWindowByName(string windowName)
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