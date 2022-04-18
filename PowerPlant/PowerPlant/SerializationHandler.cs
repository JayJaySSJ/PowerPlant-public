using PowerPlant.Domain;
using PowerPlant.Infrastructure;
using System;
using System.Globalization;

namespace PowerPlant
{
    public class SerializationHandler
    {
        private readonly SerializationService _serializationService;

        private readonly CliHelper _cliHelper;

        public SerializationHandler()
        {
            var serializationRepository = new SerializationRepository();
            var readingsRepository = new ReadingsRepository();

            _serializationService = new SerializationService(
                serializationRepository, 
                readingsRepository);

            _cliHelper = new CliHelper();
        }

        public async void ProvideSerializationAsync()
        {
            var filePath = _cliHelper.GetString("Write file-name to save data to [without extension]");

            var startDate = GetValidDateTime("start-date");
            var endDate = GetValidDateTime("end-date");

            var result = await _serializationService.SerializeReadingsAsync(startDate, endDate, filePath);

            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = GetConsoleColor(result, defaultColor);

            var message = result
                ? "Serialized correctly"
                : "(!) Serialization failed";
            Console.WriteLine(message);

            Console.ForegroundColor = defaultColor;
        }

        public DateTime GetValidDateTime(string rangePoint)
        {
            DateTime output;

            while (!DateTime.TryParseExact(
                _cliHelper.GetString($"Provide valid {rangePoint} [MM/dd/yyyy hh:mm tt]"),
                "MM/dd/yyyy hh:mm tt",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out output))
            {
                Console.Clear();
                Console.WriteLine("(!) Invalid date provided, try again...\n");
            }

            return output;
        }

        private ConsoleColor GetConsoleColor(bool switcher, ConsoleColor defaultColor)
        {
            switch (switcher)
            {
                case true:
                    return ConsoleColor.Green;
                case false:
                    return ConsoleColor.Red;
                default:
                    return defaultColor;
            }
        }
    }
}