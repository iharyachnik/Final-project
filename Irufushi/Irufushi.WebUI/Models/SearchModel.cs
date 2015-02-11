using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abstract.Entities;

namespace Irufushi.WebUI.Models
{
    public class SearchModel
    {
        public IEnumerable<UserProfile> Users { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}