namespace BS_Project.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FilterPrice")]
    public partial class FilterPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FilterID { get; set; }

        public double? PriceFrom { get; set; }

        public double? PriceTo { get; set; }

        public int BookID { get; set; }

        public virtual Book Book { get; set; }
    }
}
