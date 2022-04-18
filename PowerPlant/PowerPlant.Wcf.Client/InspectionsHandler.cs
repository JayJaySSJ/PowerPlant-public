using PowerPlant.Wcf.Client.Clients;
using PowerPlant.Wcf.ServiceDefinition.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerPlant.Wcf.Client
{
    internal class InspectionsHandler
    {
        private readonly CliHelper _cliHelper;
        private readonly ConsoleManager _consoleManager;

        private readonly InspectionsManagementClient _inspectionsManagementClient;

        public InspectionsHandler()
        {
            _cliHelper = new CliHelper();
            _consoleManager = new ConsoleManager();

            _inspectionsManagementClient = new InspectionsManagementClient();
        }

        public async void CreateNewInspectionTicketAsync(Member loggedMember)
        {
            _consoleManager.Clear();

            if (loggedMember.Function == MemberFunction.Engineer)
            {
                _consoleManager.WriteLine("(!) Only for admins and users");
                return;
            }

            var itemName = _cliHelper.GetString("Provide item name");

            while (!_inspectionsManagementClient.ItemExistsAsync(itemName).Result)
            {
                _consoleManager.Clear();
                _consoleManager.WriteLine("(!) Invalid item name, try again");
                itemName = _cliHelper.GetString("Provide item name");
            }

            if (_inspectionsManagementClient.OpenTicketExistsAsync(itemName).Result)
            {
                _consoleManager.Clear();
                _consoleManager.WriteLine($"(!) Open ticket for {itemName} already exists in Db");
                _consoleManager.WriteLine("Redirecting to menu...");
                return;
            }

            var result = await _inspectionsManagementClient.CreateAsync(new InspectionTicket
            {
                CreationDate = DateTime.Now,
                ItemName = itemName,
                Status = Status.Open
            });

            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = _cliHelper.GetConsoleColor(result, defaultColor);

            var message = result
                ? "Ticket created successfully"
                : "(!) Error while creating ticket";
            _consoleManager.WriteLine(message);

            Console.ForegroundColor = defaultColor;
        }

        public void PrintAllInspectionTickets(Member loggedMember)
        {
            _consoleManager.Clear();
            var tickets = _inspectionsManagementClient.GetAllAsync().Result;

            if(loggedMember.Function == MemberFunction.Engineer)
            {
                tickets = tickets
                    .Where(x => x.Assignment == loggedMember.Id)
                    .ToList();
            }

            foreach (var ticket in tickets)
            {
                _consoleManager.WriteLine($"Ticket ID:\t\t{ticket.Id}");
                _consoleManager.WriteLine($"Creation date:\t\t{ticket.CreationDate:MM/dd/yyyy hh:mm tt}");
                _consoleManager.WriteLine($"Assignment date:\t{ticket.AssignmentDate:MM/dd/yyyy hh:mm tt}");
                _consoleManager.WriteLine($"Termination date:\t{ticket.TerminationDate:MM/dd/yyyy hh:mm tt}");
                _consoleManager.WriteLine($"Item name:\t\t{ticket.ItemName}");
                _consoleManager.WriteLine($"Comment:\t\t{ticket.Comment}");
                _consoleManager.WriteLine($"TICKET STATUS:\t\t{ticket.Status}");
                _consoleManager.WriteLine($"Assignment:\t\t[Member Id] {ticket.Assignment}\n");
            }
        }

        public async void AssignInspectionTicketAsync(Member loggedMember)
        {
            _consoleManager.Clear();

            if (loggedMember.Function != MemberFunction.Engineer)
            {
                _consoleManager.WriteLine("(!) Only for engineers");
                return;
            }

            _consoleManager.WriteLine("Available tickets to assign listed below:");

            var tickets = _inspectionsManagementClient.GetAllAsync().Result;
            var ids = new List<int>();

            foreach (var ticket in tickets)
            {
                if (ticket.Assignment == null || ticket.AssignmentDate == null)
                {
                    ids.Add(ticket.Id);

                    _consoleManager.WriteLine($"Ticket ID:\t\t{ticket.Id}");
                    _consoleManager.WriteLine($"Creation date:\t\t{ticket.CreationDate:MM/dd/yyyy hh:mm tt}");
                    _consoleManager.WriteLine($"Item name:\t\t{ticket.ItemName}");
                    _consoleManager.WriteLine($"TICKET STATUS:\t\t{ticket.Status}");
                    _consoleManager.WriteLine($"Assignment:\t\t{ticket.Assignment}\n");
                }
            }

            if(ids.Count < 1)
            {
                _consoleManager.WriteLine("(!) No avilable tickets to be assigned, redirecting to menu...");
                return;
            }

            var id = 0;
            while(!ids.Contains(id))
            {
                id = _cliHelper.GetInt("Enter ticket's ID from above");
            }

            var pickedTicket = tickets.FirstOrDefault(x => x.Id == id);

            if(pickedTicket != null)
            {
                pickedTicket.AssignmentDate = GetAssignmentDateTime(pickedTicket.Id, pickedTicket.CreationDate);
                pickedTicket.Comment = "[" + loggedMember.Login + "] " + _cliHelper.GetString("Write comment");
                pickedTicket.Status = Status.InProgress;
                pickedTicket.Assignment = loggedMember.Id;
            }

            var result = await _inspectionsManagementClient.AssignAsync(pickedTicket);

            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = _cliHelper.GetConsoleColor(result, defaultColor);

            var message = result
                ? "Ticket assigned successfully"
                : "(!) Error while assignning ticket";
            _consoleManager.WriteLine(message);

            Console.ForegroundColor = defaultColor;
        }

        private DateTime GetAssignmentDateTime(int id, DateTime creationDate)
        {
            _consoleManager.Clear();

            _consoleManager.WriteLine($"Pick Assignment Date for ticket ID: {id}");
            var answer = _cliHelper.GetBool($"Today's date [{DateTime.Now:MM/dd/yyyy hh:mm tt}](true) or custom(false)?");

            if(answer)
            {
                return DateTime.Now;
            }

            var pickedDate = DateTime.MinValue;
            while(pickedDate <= creationDate)
            {
                pickedDate = _cliHelper.GetValidDateTime($"assignment date bigger than {creationDate:MM/dd/yyyy hh:mm tt}");
            }

            return pickedDate;
        }
    }
}