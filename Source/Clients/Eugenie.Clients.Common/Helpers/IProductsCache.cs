using System.Collections.Generic;
using Eugenie.Clients.Common.Models;

namespace Eugenie.Clients.Common.Helpers
{
    public interface IProductsCache
    {
        IEnumerable<SimplifiedProduct> SimplifiedProducts { get; set; }
    }
}