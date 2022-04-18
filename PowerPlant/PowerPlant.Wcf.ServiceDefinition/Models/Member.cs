using System.Runtime.Serialization;

namespace PowerPlant.Wcf.ServiceDefinition.Models
{
    public enum MemberFunction
    {
        Admin = 0,
        User = 1,
        Engineer = 2
    }

    [DataContract]
    public class Member
    {
        [DataMember]
        public int Id;

        [DataMember]
        public string Login;

        [DataMember]
        public string Password;

        [DataMember]
        public MemberFunction Function;
    }
}