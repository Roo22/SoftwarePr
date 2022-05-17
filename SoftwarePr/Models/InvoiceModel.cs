using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SoftwarePr.Models
{
    public class InvoiceModel
    {
        [Key]
        public int InvoiceId { get; set; }

        public DateTime? DateInvoice { get; set; }
        public float TotalBill { get; set; }

        public int? FKUserID { get; set; }
        [ForeignKey("FKUserID")]
        public virtual UserLoginSignUp user { get; set; }


    }
}