namespace BS_Project.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public int CommentID { get; set; }

        public int UserID { get; set; }

        public int BookID { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool? isDeleted { get; set; }

        public int? isLike { get; set; }

        public int? FolderID { get; set; }

        public virtual Book Book { get; set; }

        public virtual User User { get; set; }
    }
}
