using PowerPlant.WebApi.Client.Clients;
using PowerPlant.WebApi.Client.Models;
using System;
using System.Collections.Generic;

namespace PowerPlant.WebApi.Client
{
    public class MembersHandler
    {
        private readonly CliHelper _cliHelper;
        private readonly ConsoleManager _consoleManager;

        private readonly MembersWebApiClient _membersWebApiClient;

        public MembersHandler()
        {
            _cliHelper = new CliHelper();
            _consoleManager = new ConsoleManager();

            _membersWebApiClient = new MembersWebApiClient();
        }

        public string LoginLoop()
        {
            var exit = false;
            var loggedMember = string.Empty;

            while (!exit)
            {
                _consoleManager.WriteLine("System under restricted protection. Login first. \n");

                var switcher = _cliHelper.GetInt(" 0 - exit \n 1 - login \npick");
                switch (switcher)
                {
                    case 0:
                        exit = true;
                        break;
                    case 1:
                        _consoleManager.Clear();
                        loggedMember = Login();
                        if (!string.IsNullOrEmpty(loggedMember))
                        {
                            exit = true;
                        }
                        break;
                    default:
                        _consoleManager.Clear();
                        _consoleManager.WriteLine("(!) Please write crorrect number from menu\n");
                        break;
                }
            }

            return loggedMember;
        }

        private string Login()
        {
            var login = _cliHelper.GetString("Login");
            var pw = _cliHelper.GetString("Password");

            var memberExists = _membersWebApiClient.MemberExists(new MemberCredentials { Login = login, Password = pw }).Result;
            _consoleManager.Clear();

            if (memberExists)
            {
                _consoleManager.WriteLine($"Login successful. Hello {login}");
            }
            else
            {
                _consoleManager.WriteLine($"(!) Login unsuccesful. Try again...");
                return null;
            }

            return login;
        }

        public async void CreateNew(Member loggedMember)
        {
            _consoleManager.Clear();

            if (loggedMember.Function != 0)
            {
                _consoleManager.WriteLine("(!) Only for admins");
                return;
            }

            _consoleManager.WriteLine("Enter new member's credentials:\n");

            Member member = new Member
            {
                Id = 0,
                Login = _cliHelper.GetString("Login"),
                Password = _cliHelper.GetString("Password"),
                Function = _cliHelper.GetMemberFunction()
            };

            _consoleManager.Clear();

            var result = await _membersWebApiClient.CreateAsync(member);

            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = _cliHelper.GetConsoleColor(result, defaultColor);

            var message = result
                ? "Member created successfully"
                : "(!) Error while adding member";
            _consoleManager.WriteLine(message);

            Console.ForegroundColor = defaultColor;
        }

        public async void Delete(Member loggedMember)
        {
            _consoleManager.Clear();

            if (loggedMember.Function != 0)
            {
                _consoleManager.WriteLine("(!) Only for admins");
                return;
            }

            if (_cliHelper.GetString("Password") != loggedMember.Password)
            {
                _consoleManager.Clear();
                _consoleManager.WriteLine("(!) Wrong password. \nRedirecting to menu...");
                return;
            }

            Dictionary<int, Member> members = _membersWebApiClient.GetAllAsync().Result;

            _consoleManager.Clear();
            _consoleManager.WriteLine("Pick member to delete from database:\n");
            foreach (KeyValuePair<int, Member> member in members)
            {
                if (member.Value.Id == loggedMember.Id)
                {
                    continue;
                }
                _consoleManager.WriteLine($"{member.Key}. {member.Value.Login} [{member.Value.Function}]");
            }

            var id = _cliHelper.GetInt("ID");
            if (members.ContainsKey(id) && id != loggedMember.Id)
            {
                _consoleManager.Clear();

                var result = await _membersWebApiClient.DeleteAsync(id);

                var defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = _cliHelper.GetConsoleColor(result, defaultColor);

                var message = result
                    ? "Member deleted successfully"
                    : "(!) Error while deleting Member";
                _consoleManager.WriteLine(message);

                Console.ForegroundColor = defaultColor;
            }
            else
            {
                _consoleManager.WriteLine($"(!) There is no member under given id [{id}], or it's yours. \nRedirecting to menu...");
            }
        }
    }
}