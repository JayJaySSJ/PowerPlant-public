namespace PowerPlant.Domain.Models
{
    public enum MemberFunction
    {
        Admin = 0,
        User = 1,
        Engineer = 2
    }

    public class Member
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public MemberFunction Function { get; set; }
    }
}