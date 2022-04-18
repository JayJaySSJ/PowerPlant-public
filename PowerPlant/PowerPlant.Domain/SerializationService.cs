using PowerPlant.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace PowerPlant.Domain
{
    public class SerializationService
    {
        private readonly ISerializationRepository _serializationRepository;
        private readonly IReadingsRepository _readingsRepository;

        public SerializationService(
            ISerializationRepository serializationRepository,
            IReadingsRepository readingsRepository)
        {
            _serializationRepository = serializationRepository;
            _readingsRepository = readingsRepository;
        }

        public async Task<bool> SerializeReadingsAsync(DateTime floorValue, DateTime ceilingValue, string filePath)
        {
            var dataToSerialize = await _readingsRepository.GetCriticalReadingsAsync(floorValue, ceilingValue);

            filePath += ".json";

            return await _serializationRepository.ExecuteAsync(dataToSerialize, filePath);
        }
    }
}