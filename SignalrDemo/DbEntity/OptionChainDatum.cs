using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SignalrDemo.DbEntity
{
    public partial class OptionChainDatum
    {
        [Key]
        public long Id { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        public int? StrikePrice { get; set; }
        public int? SumOfPut { get; set; }
        public int? SumOfCall { get; set; }
        public int? Difference { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ResponseDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDateTime { get; set; }
    }
}
