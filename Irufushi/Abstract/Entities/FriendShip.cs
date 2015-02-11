using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abstract.Entities
{
    public class FriendShip
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        [ScaffoldColumn(false)]
        public int UserId { get; set; }

        [ForeignKey("Friend")]
        [ScaffoldColumn(false)]
        public int FriendId { get; set; }

        public virtual UserProfile User { get; set; }

        public virtual UserProfile Friend { get; set; }
    }
}
