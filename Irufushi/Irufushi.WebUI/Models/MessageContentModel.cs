using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abstract.Entities;

namespace Irufushi.WebUI.Models
{
    public class MessageContentModel
    {
        public IEnumerable<Message> Messages { get; set; }
        public string Content { get; set; }
        public Message NewMessage { get; set; }
    }
}