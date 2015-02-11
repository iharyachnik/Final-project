using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Abstract.Entities
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }
        public string UserName { get; set; }

        public virtual AboutUser AboutUser { get; set; }

        public virtual Contacts Contacts { get; set; }

        public virtual Location Location { get; set; }

        //public virtual ICollection<Album> Albums { get; set; }

        public virtual Photo Photo { get; set; }

        public virtual ICollection<FriendShip> FriendshipsOwn { get; set; }

        public virtual ICollection<FriendShip> FriendshipsFor { get; set; }

        public virtual ICollection<Message> Senders { get; set; }

        public virtual ICollection<Message> Receivers { get; set; }
    }

}
