using System;
using System.Runtime.Serialization;

namespace PowerPlant.Wcf.ServiceDefinition.Models
{
    public enum Status
    {
        Open = 0,
        InProgress = 1,
        Closed = 2,
        IncorrectState = 3
    }

    [DataContract]
    public class InspectionTicket
    {
        [DataMember]
        public int Id;

        [DataMember]
        public DateTime CreationDate;

        [DataMember]
        public DateTime? AssignmentDate;

        [DataMember]
        public DateTime? TerminationDate;

        [DataMember]
        public string ItemName;

        [DataMember]
        public string Comment;

        [DataMember]
        public Status Status;

        [DataMember]
        public int? Assignment;
    }
}