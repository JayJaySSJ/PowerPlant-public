using System;

namespace PowerPlant.WebApi.Client.Models
{
    public enum Status { 
        Open = 0,
        InProgress = 1,
        Closed = 2,
        IncorrectState = 3
    }

    public class InspectionTicket
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string ItemName { get; set; }
        public string Comment { get; set; }
        public Status Status { get; set; }
        public int? Assignment { get; set; }
    }
}