﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CW.Models
{
    public partial class Transport
    {
        public Transport()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int TransportId { get; set; }
        public int BrandId { get; set; }
        public int BusTypeId { get; set; }
        [Display(Name = "Number of seats")]
        public int SeatCount { get; set; }
        [Display(Name = "Name of model")]
        public string ModelName { get; set; }
        public string Description { get; set; }
        [Display(Name = "Price for 1 kilometers")]
        public decimal? PricePerKm { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual BusType BusType { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}