using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Display(Name = "Start date")]
        public DateTime StartDateTime { get; set; }
        [Display(Name = "End date")]
        public DateTime EndDateTime { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        [NotMapped]
        [Display(Name = "Travel time")]
        public TimeSpan TimeInTravel => EndDateTime.Date.Subtract(StartDateTime.Date);

        public virtual Route Route { get; set; }
        public virtual Transport Transport { get; set; }
        public virtual Worker Worker { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
