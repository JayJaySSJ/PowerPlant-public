using PowerPlant.Domain.Models;
using System;
using System.Collections.Generic;

namespace PowerPlant.Domain.Interfaces
{
    public interface IPlantDataProvider
    {
        event EventHandler<NewDataSet> OnPlantDataArrival;
        List<string> GetItemNames();
    }
}