namespace Eugenie.Clients.AdminService
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Messaging;
    using System.Net;
    using System.ServiceProcess;
    using System.Threading.Tasks;

    using Eugenie.Clients.Common.Helpers;

    using Common.Contracts;
    using Common.Models;

    using Newtonsoft.Json;

    public partial class AdminService : ServiceBase
    {
        private MessageQueueManager manager;
        private IWebApiClient apiClient;

        public AdminService()
        {
            this.InitializeComponent();
            this.eventLog1 = new EventLog();
            if (!EventLog.SourceExists("EugenieSource"))
            {
                EventLog.CreateEventSource("EugenieSource", "EugenieLog");
            }

            this.eventLog1.Source = "EugenieSource";
            this.eventLog1.Log = "EugenieLog";
        }

        protected override void OnStart(string[] args)
        {
            this.eventLog1.WriteEntry("Eugenie Admin service starting");

            this.apiClient = new WebApiClient();

            this.manager = new MessageQueueManager();

            this.manager.MessageQueue.BeginPeek();
            this.manager.MessageQueue.PeekCompleted += this.ProcessMessages;
        }

        private void ProcessMessages(object sender, PeekCompletedEventArgs e)
        {
            var serverGroups = this.manager.MessageQueue.GetAllMessages().Select(x => new
            {
                Id = x.Id,
                Deserialized = JsonConvert.DeserializeObject<ServerAddProductPair>(x.Body.ToString())
            }).GroupBy(y => y.Deserialized.ServerInformation.Name);

            Parallel.ForEach(serverGroups, (group) =>
            //foreach (var group in serverGroups)
            {
                foreach (var pair in group)
                {
                    var client = ServerTester.TestServerAsync(pair.Deserialized.ServerInformation).Result;
                    if (client != null)
                    {
                        try
                        {
                            var status = this.apiClient.AddOrUpdateAsync(client, pair.Deserialized.AddProductModel).Result;
                            if (status == HttpStatusCode.OK)
                            {
                                this.manager.MessageQueue.ReceiveById(pair.Id);
                            }
                            else if (status == HttpStatusCode.BadRequest)
                            {
                                this.manager.MessageQueue.ReceiveById(pair.Id);
                            }
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        break;
                    }
                }
            });

            this.manager.MessageQueue.BeginPeek();
        }

        protected override void OnStop()
        {
            this.eventLog1.WriteEntry("In onStop.");
        }

        internal void TestStartupAndStop()
        {
            this.OnStart(new string[1]);
            Console.ReadLine();
            this.OnStop();
        }
    }
}