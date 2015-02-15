using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abstract.Entities
{
    public class Contacts
    {
        [Key, ForeignKey("UserProfile")]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string PhoneNum { get; set; }

        [MaxLength(50)]
        public string MobPhoneNum { get; set; }

        [MaxLength(50)]
        public string Skype { get; set; }

        public string ContEmail { get; set; }

        [MaxLength(50)]
        public string ICQ { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
