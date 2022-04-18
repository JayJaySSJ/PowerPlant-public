using PowerPlant.Domain;
using PowerPlant.Infrastructure;

namespace PowerPlant
{
    internal class Program
    {
        private static PlantAssetsConditionMonitoring _plantAssetsConditionMonitoring;

        private static MembersHandler _loginHandler;
        private static readonly ActionHandler _actionsHandler = new ActionHandler();

        static void Main()
        {
            var readingsRepository = new ReadingsRepository();
            var plantDataProvider = new PlantDataProvider();
            var dateProvider = new DateProvider();
            var cliHelper = new CliHelper();
            var memberService = new MembersService(new MembersRepository());
            var consoleManager = new ConsoleManager();

            _loginHandler = new MembersHandler(cliHelper, memberService, consoleManager);

            _plantAssetsConditionMonitoring = new PlantAssetsConditionMonitoring(readingsRepository, plantDataProvider, dateProvider, memberService);

            _plantAssetsConditionMonitoring.StartMonitoring();

            string loggedMember = _loginHandler.LoginLoop();

            if (!string.IsNullOrEmpty(loggedMember))
            {
                _actionsHandler.ProgramLoop(loggedMember);
            }

            _plantAssetsConditionMonitoring.StopMonitoring();
        }
    }
}