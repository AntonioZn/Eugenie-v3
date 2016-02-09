namespace Eugenie.Clients.Admin.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.Models;

    public static class MeasureTypeMapper
    {
        public static List<MeasureType> GetTypes()
        {
            return Enum.GetValues(typeof (MeasureType)).Cast<MeasureType>().ToList();
        }
    }
}