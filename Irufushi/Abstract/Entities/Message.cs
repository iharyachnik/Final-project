using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abstract.Entities
{
    public class Message
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [ForeignKey("Sender")]
        [ScaffoldColumn(false)]
        public int SenderId { get; set; }

        [ForeignKey("Receiver")]
        [ScaffoldColumn(false)]
        public int ReceiverId { get; set; }

        public DateTime SendDateTime { get; set; }

        public virtual UserProfile Sender { get; set; }

        public virtual UserProfile Receiver { get; set; }
    }

}
