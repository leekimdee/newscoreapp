using NewsCoreApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Data.Entities
{
    public class Image: DomainEntity<int>
    {
        [StringLength(255)]
        [Required]
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int ImageAlbumId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public Status Status { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [ForeignKey("ImageAlbumId")]
        public virtual ImageAlbum ImageAlbum { get; set; }
    }
}
