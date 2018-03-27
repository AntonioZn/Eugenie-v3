namespace Eugenie.Clients.Seller.ViewModels
{
    using Common.Contracts;

    using Helpers;

    using Sv.Wpf.Core.Helpers;
    using Sv.Wpf.Core.Mvvm;

    public class LotteryTicketCheckerViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly LotteryTicketChecker lotteryTicketChecker;
        private string result;
        private TaskManager.Task checkTicketTask;

        public LotteryTicketCheckerViewModel(TaskManager taskManager, LotteryTicketChecker lotteryTicketChecker)
        {
            this.TaskManager = taskManager;
            this.lotteryTicketChecker = lotteryTicketChecker;
        }

        public TaskManager TaskManager { get; }

        public string Result
        {
            get => this.result;
            set => this.Set(ref this.result, value);
        }

        public async void HandleBarcode(string barcode)
        {
            if (this.checkTicketTask != null)
            {
                this.TaskManager.CancelTask(this.checkTicketTask);
            }

            this.checkTicketTask = new TaskManager.Task("check", false);
            this.checkTicketTask.Function = async (cts, logger) =>
                                            {
                                                this.Result = string.Empty;
                                                this.Result = await this.lotteryTicketChecker.CheckAsync(barcode, cts.Token);
                                            };

            await this.TaskManager.Run(this.checkTicketTask);
        }
    }
}