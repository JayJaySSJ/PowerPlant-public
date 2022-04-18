using PowerPlant.Wcf.ServiceDefinition.Models;
using System;
using System.Globalization;

namespace PowerPlant.Wcf.Client
{
    public class CliHelper
    {
        private readonly ConsoleManager _consoleManager;

        public CliHelper()
        {
            _consoleManager = new ConsoleManager();
        }

        public string GetString(string message)
        {
            _consoleManager.Write($"{message}: ");
            var stringOut = _consoleManager.ReadLine();
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

            var success = false;
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

        public DateTime GetValidDateTime(string rangePoint)
        {
            DateTime output;

            while (!DateTime.TryParseExact(
                GetString($"Provide valid {rangePoint} [MM/dd/yyyy hh:mm tt]"),
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

        public ConsoleColor GetConsoleColor(bool switcher, ConsoleColor defaultColor)
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

        public bool GetBool(string message)
        {
            bool boolOut;

            while (!bool.TryParse(GetString(message), out boolOut))
            {
                _consoleManager.Clear();
                _consoleManager.WriteLine("(!) Write true/false\n");
            }

            return boolOut;
        }
    }
}