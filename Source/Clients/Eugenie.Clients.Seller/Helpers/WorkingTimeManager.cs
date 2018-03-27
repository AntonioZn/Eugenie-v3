namespace Eugenie.Clients.Seller.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    using Models;

    public class WorkingTimeManager
    {
        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        public WorkingTimeManager()
        {
            
        }

        public async void Start()
        {
            while (true)
            {
                var settings = SettingsManager.Get();
                if (this.CheckIfOutsiteWorkingTime(settings))
                {
                    if (settings.Sleep)
                    {
                        SetSuspendState(false, true, true);
                    }
                    else if (settings.Shutdown)
                    {
                        var psi = new ProcessStartInfo("shutdown", "/s /t 0");
                        psi.CreateNoWindow = true;
                        psi.UseShellExecute = false;
                        Process.Start(psi);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(2));
            }
        }

        private bool CheckIfOutsiteWorkingTime(Settings settings)
        {
            var start = new TimeSpan(settings.OpenHours, settings.OpenMinutes, 0);
            var end = new TimeSpan(settings.CloseHours, settings.CloseMinutes, 0);
            var now = DateTime.Now.TimeOfDay;

            if (start <= end)
            {
                // start and stop times are in the same day
                if (now >= start && now <= end)
                {
                    return false;
                }

                return true;
            }
            else
            {
                // start and stop times are in different days
                if (now >= start || now <= end)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
