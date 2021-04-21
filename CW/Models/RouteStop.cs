using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CW.Models
{
    public partial class RouteStop
    {
        public RouteStop()
        {
            TicketRouteStopFromNavigations = new HashSet<Ticket>();
            TicketRouteStopToNavigations = new HashSet<Ticket>();
        }

        public int StopId { get; set; }
        public int RouteId { get; set; }
        public int CityId { get; set; }
        public int StopNumber { get; set; }
        public int DistanceToStop { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual City City { get; set; }
        public virtual Route Route { get; set; }
        public virtual ICollection<Ticket> TicketRouteStopFromNavigations { get; set; }
        public virtual ICollection<Ticket> TicketRouteStopToNavigations { get; set; }
    }
}
