namespace BS_Project.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Image
    {
        public int ImageID { get; set; }

        public int ImageBoolID { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public virtual ImageBool ImageBool { get; set; }
    }
}
