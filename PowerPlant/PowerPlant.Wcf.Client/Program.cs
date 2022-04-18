namespace PowerPlant.Wcf.Client
{
    internal class Program
    {
        private static readonly MembersHandler _membersHandler = new MembersHandler();
        private static readonly ActionHandler _actionsHandler = new ActionHandler();

        static void Main()
        {
            var loggedMember = _membersHandler.LoginLoop();

            if (!string.IsNullOrEmpty(loggedMember))
            {
                _actionsHandler.ProgramLoop(loggedMember);
            }
        }
    }
}