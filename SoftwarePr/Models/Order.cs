using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SoftwarePr.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int Qty { get; set; }
        public int UnitPrice { get; set; }
        public float OrderBill { get; set; }
        public DateTime? OrderDate { get; set; }

        public int? FkProdId { get; set; }
        [ForeignKey("FkProdId")]
        public virtual Products prodcts { get; set; }

        public int? FkInvoiceID { get; set; }
        [ForeignKey("FkInvoiceID")]
        public virtual InvoiceModel invoices { get; set; }


    }
}