using System;
using System.Collections.Generic;

#nullable disable

namespace CW.Models
{
    public partial class Route
    {
        public Route()
        {
            RouteStops = new HashSet<RouteStop>();
            Schedules = new HashSet<Schedule>();
        }

        public int RouteId { get; set; }
        public int? Mileage { get; set; }
        public int? CityFrom { get; set; }
        public int? CityTo { get; set; }
        public string NumberOfRoute { get; set; }
        public int? StopCount { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual City CityFromNavigation { get; set; }
        public virtual City CityToNavigation { get; set; }
        public virtual ICollection<RouteStop> RouteStops { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
