using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abstract.Entities
{
    public class AboutUser
    {
        [Key, ForeignKey("UserProfile")]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BirthDate { get; set; }

        public bool? Gender { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
