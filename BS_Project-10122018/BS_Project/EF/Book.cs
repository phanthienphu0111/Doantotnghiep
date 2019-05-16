namespace BS_Project.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            Comments = new HashSet<Comment>();
            OrdersDetails = new HashSet<OrdersDetail>();
            FilterPrices = new HashSet<FilterPrice>();
            Authors = new HashSet<Author>();
            Categories = new HashSet<Category>();
        }

        public int BookID { get; set; }

        [Required]
        public string Name { get; set; }

        public int? PublisherID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PublicationDate { get; set; }

        public int? ImageBoolID { get; set; }

        public string Overview { get; set; }

        public string Details { get; set; }

        public double? Price { get; set; }

        public int TotalQuantity { get; set; }

        public int ViewCount { get; set; }

        public bool? isDeleted { get; set; }

        public int? CurrentQuantity { get; set; }

        public virtual ImageBool ImageBool { get; set; }

        public virtual Publisher Publisher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdersDetail> OrdersDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FilterPrice> FilterPrices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Author> Authors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
    }
}
