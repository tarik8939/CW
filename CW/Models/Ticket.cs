using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CW.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            Purchases = new HashSet<Purchase>();
        }

        public int TicketId { get; set; }
        public int ScheduleId { get; set; }
        public decimal Price { get; set; }
        public int RouteStopFrom { get; set; }
        public int RouteStopTo { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        [NotMapped]
        [Display(Name = "Initial/End city")]
        public string asd => $"{RouteStopFromNavigation.City.City1}-{RouteStopToNavigation.City.City1}";

        public virtual RouteStop RouteStopFromNavigation { get; set; }
        public virtual RouteStop RouteStopToNavigation { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
