namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;

    using Autofac;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;

    public class ViewModelLocator
    {
        private IContainer container;

        public ViewModelLocator()
        {
            this.Register();
        }

        public ProductsEditorViewModel ProductsEditorViewModel => this.container.Resolve<ProductsEditorViewModel>();

        public ActiveServersViewModel ActiveServersViewModel => this.container.Resolve<ActiveServersViewModel>();

        private void Register()
        {
            var containerBuilder = new ContainerBuilder();
            var servers = new List<ServerInformation>
                          {
                              new ServerInformation("test 1", "http://localhost:9000", "asdasdasd"),
                              new ServerInformation("test 2", "http://localhost:9000", "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAghKFVRs7OE6YRBoflNwdbgAAAAACAAAAAAAQZgAAAAEAACAAAADcux-gBEcAR-f0csaFobLE8k_10RDytz9lvmB_3ynQ7gAAAAAOgAAAAAIAACAAAAATpHJNm48sAugHmhkK3ygZYbXCCQRAPbdtzviyi3aVwUABAABh6kZKpslmn1zFHhWSYnOzsQOS_k5PrKHTsEJMmmJuzIkUG52SDsyr8NZIdu3tE0fBZ9H9r9qGwNaO4HOZOEm0k47EBQ0XLl4YAgRFDkFmOEMqfQpU0HesVVnHtn0gvjpObEaH8FwrX_aZnTfTUf2sRjjgMzClq_m84ugAk_5WOq4rZoFT9QlpkGidl8R7gawy17t-ofb4mFoYWabKmomhsBVIbCXyzfoM4f_T2K0aUWplusogSC0GUiNwV4wY7U7yuXtgDH3cdlytL03nDGA3H8lQ5JAu5mbWrnW7ftxrT7DBogCcVb9L4AS4QLalcfC0vsHRpmOta0eNVtO02yYnAZRsl7jGiM2hYggJtPxdvr79ex-4GewXybw9qLJs_ngwX6KsNN3_YkTLNAJxYmcHaltHvS2Fr7n36VdFmStT6EAAAADH9Ntz_aHlCL1cEbrUN-AEhO-PcVhlDeM99J0xNuBLqlv4Vi5bohrOabxd85Osde4wrv1iNVM7RRyMe1n05W9B"),
                              new ServerInformation("test 3", "http://localhost:9000", "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAghKFVRs7OE6YRBoflNwdbgAAAAACAAAAAAAQZgAAAAEAACAAAADcux-gBEcAR-f0csaFobLE8k_10RDytz9lvmB_3ynQ7gAAAAAOgAAAAAIAACAAAAATpHJNm48sAugHmhkK3ygZYbXCCQRAPbdtzviyi3aVwUABAABh6kZKpslmn1zFHhWSYnOzsQOS_k5PrKHTsEJMmmJuzIkUG52SDsyr8NZIdu3tE0fBZ9H9r9qGwNaO4HOZOEm0k47EBQ0XLl4YAgRFDkFmOEMqfQpU0HesVVnHtn0gvjpObEaH8FwrX_aZnTfTUf2sRjjgMzClq_m84ugAk_5WOq4rZoFT9QlpkGidl8R7gawy17t-ofb4mFoYWabKmomhsBVIbCXyzfoM4f_T2K0aUWplusogSC0GUiNwV4wY7U7yuXtgDH3cdlytL03nDGA3H8lQ5JAu5mbWrnW7ftxrT7DBogCcVb9L4AS4QLalcfC0vsHRpmOta0eNVtO02yYnAZRsl7jGiM2hYggJtPxdvr79ex-4GewXybw9qLJs_ngwX6KsNN3_YkTLNAJxYmcHaltHvS2Fr7n36VdFmStT6EAAAADH9Ntz_aHlCL1cEbrUN-AEhO-PcVhlDeM99J0xNuBLqlv4Vi5bohrOabxd85Osde4wrv1iNVM7RRyMe1n05W9B"),
                              new ServerInformation("test 4", "http://localhost:9000", "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAghKFVRs7OE6YRBoflNwdbgAAAAACAAAAAAAQZgAAAAEAACAAAADcux-gBEcAR-f0csaFobLE8k_10RDytz9lvmB_3ynQ7gAAAAAOgAAAAAIAACAAAAATpHJNm48sAugHmhkK3ygZYbXCCQRAPbdtzviyi3aVwUABAABh6kZKpslmn1zFHhWSYnOzsQOS_k5PrKHTsEJMmmJuzIkUG52SDsyr8NZIdu3tE0fBZ9H9r9qGwNaO4HOZOEm0k47EBQ0XLl4YAgRFDkFmOEMqfQpU0HesVVnHtn0gvjpObEaH8FwrX_aZnTfTUf2sRjjgMzClq_m84ugAk_5WOq4rZoFT9QlpkGidl8R7gawy17t-ofb4mFoYWabKmomhsBVIbCXyzfoM4f_T2K0aUWplusogSC0GUiNwV4wY7U7yuXtgDH3cdlytL03nDGA3H8lQ5JAu5mbWrnW7ftxrT7DBogCcVb9L4AS4QLalcfC0vsHRpmOta0eNVtO02yYnAZRsl7jGiM2hYggJtPxdvr79ex-4GewXybw9qLJs_ngwX6KsNN3_YkTLNAJxYmcHaltHvS2Fr7n36VdFmStT6EAAAADH9Ntz_aHlCL1cEbrUN-AEhO-PcVhlDeM99J0xNuBLqlv4Vi5bohrOabxd85Osde4wrv1iNVM7RRyMe1n05W9B"),
                              new ServerInformation("test 5", "http://localhost:9000", "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAghKFVRs7OE6YRBoflNwdbgAAAAACAAAAAAAQZgAAAAEAACAAAADcux-gBEcAR-f0csaFobLE8k_10RDytz9lvmB_3ynQ7gAAAAAOgAAAAAIAACAAAAATpHJNm48sAugHmhkK3ygZYbXCCQRAPbdtzviyi3aVwUABAABh6kZKpslmn1zFHhWSYnOzsQOS_k5PrKHTsEJMmmJuzIkUG52SDsyr8NZIdu3tE0fBZ9H9r9qGwNaO4HOZOEm0k47EBQ0XLl4YAgRFDkFmOEMqfQpU0HesVVnHtn0gvjpObEaH8FwrX_aZnTfTUf2sRjjgMzClq_m84ugAk_5WOq4rZoFT9QlpkGidl8R7gawy17t-ofb4mFoYWabKmomhsBVIbCXyzfoM4f_T2K0aUWplusogSC0GUiNwV4wY7U7yuXtgDH3cdlytL03nDGA3H8lQ5JAu5mbWrnW7ftxrT7DBogCcVb9L4AS4QLalcfC0vsHRpmOta0eNVtO02yYnAZRsl7jGiM2hYggJtPxdvr79ex-4GewXybw9qLJs_ngwX6KsNN3_YkTLNAJxYmcHaltHvS2Fr7n36VdFmStT6EAAAADH9Ntz_aHlCL1cEbrUN-AEhO-PcVhlDeM99J0xNuBLqlv4Vi5bohrOabxd85Osde4wrv1iNVM7RRyMe1n05W9B"),
                              new ServerInformation("test 6", "http://localhost:9000", "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAghKFVRs7OE6YRBoflNwdbgAAAAACAAAAAAAQZgAAAAEAACAAAADcux-gBEcAR-f0csaFobLE8k_10RDytz9lvmB_3ynQ7gAAAAAOgAAAAAIAACAAAAATpHJNm48sAugHmhkK3ygZYbXCCQRAPbdtzviyi3aVwUABAABh6kZKpslmn1zFHhWSYnOzsQOS_k5PrKHTsEJMmmJuzIkUG52SDsyr8NZIdu3tE0fBZ9H9r9qGwNaO4HOZOEm0k47EBQ0XLl4YAgRFDkFmOEMqfQpU0HesVVnHtn0gvjpObEaH8FwrX_aZnTfTUf2sRjjgMzClq_m84ugAk_5WOq4rZoFT9QlpkGidl8R7gawy17t-ofb4mFoYWabKmomhsBVIbCXyzfoM4f_T2K0aUWplusogSC0GUiNwV4wY7U7yuXtgDH3cdlytL03nDGA3H8lQ5JAu5mbWrnW7ftxrT7DBogCcVb9L4AS4QLalcfC0vsHRpmOta0eNVtO02yYnAZRsl7jGiM2hYggJtPxdvr79ex-4GewXybw9qLJs_ngwX6KsNN3_YkTLNAJxYmcHaltHvS2Fr7n36VdFmStT6EAAAADH9Ntz_aHlCL1cEbrUN-AEhO-PcVhlDeM99J0xNuBLqlv4Vi5bohrOabxd85Osde4wrv1iNVM7RRyMe1n05W9B"),
                              new ServerInformation("test 7", "http://localhost:9000", "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAghKFVRs7OE6YRBoflNwdbgAAAAACAAAAAAAQZgAAAAEAACAAAADcux-gBEcAR-f0csaFobLE8k_10RDytz9lvmB_3ynQ7gAAAAAOgAAAAAIAACAAAAATpHJNm48sAugHmhkK3ygZYbXCCQRAPbdtzviyi3aVwUABAABh6kZKpslmn1zFHhWSYnOzsQOS_k5PrKHTsEJMmmJuzIkUG52SDsyr8NZIdu3tE0fBZ9H9r9qGwNaO4HOZOEm0k47EBQ0XLl4YAgRFDkFmOEMqfQpU0HesVVnHtn0gvjpObEaH8FwrX_aZnTfTUf2sRjjgMzClq_m84ugAk_5WOq4rZoFT9QlpkGidl8R7gawy17t-ofb4mFoYWabKmomhsBVIbCXyzfoM4f_T2K0aUWplusogSC0GUiNwV4wY7U7yuXtgDH3cdlytL03nDGA3H8lQ5JAu5mbWrnW7ftxrT7DBogCcVb9L4AS4QLalcfC0vsHRpmOta0eNVtO02yYnAZRsl7jGiM2hYggJtPxdvr79ex-4GewXybw9qLJs_ngwX6KsNN3_YkTLNAJxYmcHaltHvS2Fr7n36VdFmStT6EAAAADH9Ntz_aHlCL1cEbrUN-AEhO-PcVhlDeM99J0xNuBLqlv4Vi5bohrOabxd85Osde4wrv1iNVM7RRyMe1n05W9B"),
                              new ServerInformation("test 8", "http://localhost:9001", "asadsasd")
                          };

            var serversManager = new ServersManager(servers);
            var client = new WebApiServerClient(serversManager);

            containerBuilder.RegisterInstance(serversManager).As<IServersManager>();
            containerBuilder.RegisterInstance(client).As<IWebApiServerClient>();
            containerBuilder.RegisterType<ProductsEditorViewModel>();
            containerBuilder.RegisterType<ActiveServersViewModel>();

            this.container = containerBuilder.Build();
        }
    }
}