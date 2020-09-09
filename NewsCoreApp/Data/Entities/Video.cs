using NewsCoreApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Data.Entities
{
    public class Video: DomainEntity<int>
    {
        public Video()
        {

        }

        public Video(string title, string videoUrl, Status status)
        {
            Title = title;
            VideoUrl = videoUrl;
            Status = status;
        }

        [StringLength(255)]
        [Required]
        public string Title { get; set; }

        public string VideoUrl { get; set; }

        public Status Status { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}
