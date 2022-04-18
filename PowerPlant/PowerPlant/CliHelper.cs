using PowerPlant.Domain.Models;
using System;

namespace PowerPlant
{
    public interface ICliHelper
    {
        int GetInt(string message);
        string GetString(string message);
        MemberFunction GetMemberFunction();
    }

    public class CliHelper : ICliHelper
    {
        private readonly ConsoleManager _consoleManager;

        public CliHelper()
        {
            _consoleManager = new ConsoleManager();
        }

        public string GetString(string message)
        {
            _consoleManager.Write($"{message}: ");
            string stringOut = _consoleManager.ReadLine();
            while (string.IsNullOrEmpty(stringOut))
            {
                _consoleManager.Clear();
                _consoleManager.WriteLine($"(!) Invalid string, please try again..\n");
                _consoleManager.Write($"{message}: ");
                stringOut = _consoleManager.ReadLine();
            }
            return stringOut;
        }

        public int GetInt(string message)
        {
            int intOut;

            while (!int.TryParse(GetString(message), out intOut))
            {
                _consoleManager.Clear();
                _consoleManager.WriteLine("(!) Write number\n");
            }

            return intOut;
        }

        public MemberFunction GetMemberFunction()
        {
            var functions = string.Join(", ", Enum.GetNames(typeof(MemberFunction)));
            var input = default(string);
            var output = new MemberFunction();

            bool success = false;
            while (!success)
            {
                while (!Enum.TryParse(input, out output))
                {
                    _consoleManager.Clear();
                    input = GetString($"Pick function [{functions}]");
                }

                if (Enum.IsDefined(typeof(MemberFunction), input))
                {
                    success = true;
                }
                else
                {
                    _consoleManager.Clear();
                    input = GetString($"(!) Wrong input, try again...\nPick function [{functions}]");
                    continue;
                }
            }

            return output;
        }
    }
}