using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abstract.Entities
{
    public class Photo
    {
        [Key, ForeignKey("UserProfile")]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public byte[] Data { get; set; }

        [ScaffoldColumn(false)]
        public string MimeType { get; set; }

        public string Description { get; set; }

        //[ForeignKey("UserProfile")]
        //[ScaffoldColumn(false)]
        //public int UserId { get; set; }
     
        public virtual UserProfile UserProfile { get; set; }
    }
}
