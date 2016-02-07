namespace Eugenie.Server.Host
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    using Api;

    using Microsoft.Owin.Hosting;

    public static class Host
    {
        public static void HostWebApi(string address)
        {
            try
            {
                WebApp.Start<Startup>(address);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.ToString().ToLower().Contains("denied"))
                {
                    throw new AccessDeniedException();
                }

                throw new PortInUseException();
            }
        }

        public static void HostWebApi(int port)
        {
            var localIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)?.ToString();
            var address = "http://" + localIp + ":" + port;

            HostWebApi(address);
        }
    }
}