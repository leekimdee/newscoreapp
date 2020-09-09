using NewsCoreApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Data.Entities
{
    public class ImageAlbum: DomainEntity<int>
    {
        public ImageAlbum()
        {
            Images = new List<Image>();
        }

        public ImageAlbum(string title, int sortOrder, Status status)
        {
            Title = title;
            SortOrder = sortOrder;
            Status = status;
        }

        [StringLength(255)]
        [Required]
        public string Title { get; set; }

        public int SortOrder { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public Status Status { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}
