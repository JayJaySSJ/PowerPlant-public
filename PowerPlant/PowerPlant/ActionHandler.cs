using PowerPlant.Domain;
using PowerPlant.Domain.Models;
using PowerPlant.Infrastructure;
using System;

namespace PowerPlant
{
    internal class ActionHandler
    {
        private readonly MembersService _memberService;

        private readonly MembersHandler _memberHandler;
        private readonly CliHelper _cliHelper;
        private readonly ReadingsHandler _readingsHandler;
        private readonly ConsoleManager _consoleManager;
        private readonly SerializationHandler _serializationHandler;

        public ActionHandler()
        {
            _memberService = new MembersService(new MembersRepository());
            
            _cliHelper = new CliHelper();
            _readingsHandler = new ReadingsHandler();
            _consoleManager = new ConsoleManager();
            _serializationHandler = new SerializationHandler();
            _memberHandler = new MembersHandler(_cliHelper, _memberService, _consoleManager);
        }

        internal void ProgramLoop(string login)
        {
            _memberService.UpdateLoggedMemberAsync(login);
            var loggedMember = _memberService.GetAsync(login).Result;

            try
            {
                bool exit = false;
                while (!exit)
                {
                    _consoleManager.WriteLine("\nPick number to choose action:");
                    _consoleManager.WriteLine(" 0 - exit \n 1 - print readings \n 2 - print power readings \n 3 - create new member \t[admins only]\n 4 - delete member \t[admins only]\n 5 - serialize data to JSON\n");

                    int switcher = _cliHelper.GetInt("Your pick");
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
                            _memberHandler.CreateNew(loggedMember);
                            break;
                        case 4:
                            _memberHandler.Delete(loggedMember);
                            break;
                        case 5:
                            _serializationHandler.ProvideSerializationAsync();
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