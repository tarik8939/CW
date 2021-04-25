using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CW.Models
{
    public partial class City
    {
        public City()
        {
            Addresses = new HashSet<Address>();
            Departments = new HashSet<Department>();
            RouteCityFromNavigations = new HashSet<Route>();
            RouteCityToNavigations = new HashSet<Route>();
            RouteStops = new HashSet<RouteStop>();
        }

        public int CityId { get; set; }
        [Display(Name = "City")]
        public string City1 { get; set; }
        public double? latitude { get; set; }//широта
        public double? longitude { get; set; }//довгота

        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Route> RouteCityFromNavigations { get; set; }
        public virtual ICollection<Route> RouteCityToNavigations { get; set; }
        public virtual ICollection<RouteStop> RouteStops { get; set; }
    }
}
