using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abstract.Entities;

namespace Irufushi.WebUI.Models
{
    public class UserModel
    {
        public IEnumerable<UserProfile> Users { get; set; }
    }
}