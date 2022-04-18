using PowerPlant.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PowerPlant.Domain.Interfaces
{
    public interface ISerializationRepository
    {
        Task<bool> ExecuteAsync(List<CriticalReading> dataToSave, string filePath);
    }
}