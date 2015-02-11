using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abstract.Entities
{
    public class Location
    {
        [Key, ForeignKey("UserProfile")]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }

}
