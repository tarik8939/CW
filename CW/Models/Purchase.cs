using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CW.Models
{
    public partial class Purchase
    {
        public int PurchaseId { get; set; }
        public int WorkerId { get; set; }
        public int? ClientId { get; set; }
        public int TicketId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int TicketCount { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        [NotMapped] public string ClientFullName => $"{Client.FirstName} {Client.LastName}";

        public virtual Client Client { get; set; }
        public virtual Department Department { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual Worker Worker { get; set; }
    }
}
