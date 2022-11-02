using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SignalrDemo.DbEntity
{
    [Table("DeliveryReport")]
    public partial class DeliveryReport
    {
        [Key]
        public long Id { get; set; }
        [StringLength(150)]
        public string? Name { get; set; }
        public bool? IsNifty50 { get; set; }
        [Column("IsFNO")]
        public bool? IsFno { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PrevClose { get; set; }
        [Column("LTP", TypeName = "decimal(18, 2)")]
        public decimal? Ltp { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ChangePercentage { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? DeliveryPercentage { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ReportDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDateTime { get; set; }
    }
}
