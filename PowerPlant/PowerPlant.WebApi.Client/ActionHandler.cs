using PowerPlant.WebApi.Client.Clients;
using System;

namespace PowerPlant.WebApi.Client
{
    internal class ActionHandler
    {
        private readonly MembersWebApiClient _membersWebApiClient;

        private readonly MembersHandler _membersHandler;
        private readonly CliHelper _cliHelper;
        private readonly ReadingsHandler _readingsHandler;
        private readonly ConsoleManager _consoleManager;
        private readonly SerializationHandler _serializationHandler;
        private readonly InspectionsHandler _inspectionsHandler;

        public ActionHandler()
        {
            _membersWebApiClient = new MembersWebApiClient();

            _cliHelper = new CliHelper();
            _readingsHandler = new ReadingsHandler();
            _consoleManager = new ConsoleManager();
            _serializationHandler = new SerializationHandler();
            _membersHandler = new MembersHandler();
            _inspectionsHandler = new InspectionsHandler();
        }

        internal void ProgramLoop(string login)
        {
            _membersWebApiClient.UpdateLoggedMemberAsync(login);
            var loggedMember = _membersWebApiClient.GetAsync(login).Result;

            try
            {
                var exit = false;
                while (!exit)
                {
                    _consoleManager.WriteLine("\nPick number to choose action:");
                    _consoleManager.WriteLine("" +
                        " 0 - exit\n" +
                        " 1 - print readings\n" +
                        " 2 - print power readings\n" +
                        " 3 - create new member \t[admins only]\n" +
                        " 4 - delete member \t[admins only]\n" +
                        " 5 - serialize data to JSON\n" +
                        " 6 - print chosen critical readings\n" +
                        " 7 - print chosen critical statistics\n" +
                        " 8 - create new inspection ticket \t[admins and users only]\n" +
                        " 9 - print all inspection tickets\n" +
                        " 10 - assign inspection tickets \t[engineers only]\n");

                    var switcher = _cliHelper.GetInt("Your pick");
                    switch (switcher)
                    {
                        case 0:
                            _consoleManager.WriteLine("Adios");
                            exit = true;
                            break;
                        case 1:
                            _readingsHandler.RunReadings();
                            break;
                        case 2:
                            _readingsHandler.RunPowerReadings();
                            break;
                        case 3:
                            _membersHandler.CreateNew(loggedMember);
                            break;
                        case 4:
                            _membersHandler.Delete(loggedMember);
                            break;
                        case 5:
                            _serializationHandler.SerializeToJsonAsync();
                            break;
                        case 6:
                            _readingsHandler.PrintChosenCriticalReadings();
                            break;
                        case 7:
                            _readingsHandler.PrintChosenCriticalStatistics();
                            break;
                        case 8:
                            _inspectionsHandler.CreateNewInspectionTicet(loggedMember);
                            break;
                        case 9:
                            _inspectionsHandler.PrintAllInspectionTickets(loggedMember);
                            break;
                        case 10:
                            _inspectionsHandler.AssignInspectionTicketAsync(loggedMember);
                            break;
                        default:
                            _consoleManager.Clear();
                            _consoleManager.WriteLine("(!) Please write crorrect number from menu");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _consoleManager.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}