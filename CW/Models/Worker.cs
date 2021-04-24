using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CW.Models
{
    public partial class Worker
    {
        public Worker()
        {
            Purchases = new HashSet<Purchase>();
            Salaries = new HashSet<Salary>();
            Schedules = new HashSet<Schedule>();
        }

        public int WorkerId { get; set; }
        public int RoleId { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }
        [NotMapped]
        [Display(Name = "Worker")]
        public string WorkerFullName => $"{FirstName} {LastName}";

        public virtual Role Role { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
