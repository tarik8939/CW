using System;
using System.Collections.Generic;

#nullable disable

namespace CW.Models
{
    public partial class Salary
    {
        public int SalaryId { get; set; }
        public int WorkerId { get; set; }
        public int Salary1 { get; set; }
        public DateTime DataOfPayment { get; set; }
        public int ForMounth { get; set; }
        public int ForYear { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual Worker Worker { get; set; }
    }
}
