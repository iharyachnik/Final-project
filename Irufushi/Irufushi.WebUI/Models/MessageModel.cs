using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abstract.Entities;

namespace Irufushi.WebUI.Models
{
    public class MessageModel
    {
        public IEnumerable<AboutUser> Dialogs { get; set; }
    }
}