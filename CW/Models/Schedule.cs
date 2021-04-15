using System;
using System.Collections.Generic;

#nullable disable

namespace CW.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int ScheduleId { get; set; }
        public int WorkerId { get; set; }
        public int RouteId { get; set; }
        public int TransportId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual Route Route { get; set; }
        public virtual Transport Transport { get; set; }
        public virtual Worker Worker { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
