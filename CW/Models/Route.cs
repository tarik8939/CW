using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Distance")]
        public int? Mileage { get; set; }
        [Display(Name = "Initial city")]
        public int? CityFrom { get; set; }
        [Display(Name = "Final city")]
        public int? CityTo { get; set; }
        [Display(Name = "Number of route")]
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
