using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc.ViewModels
{
    public class List<Projects>
    {
        public bool Selected { get; set; }
        public int Id { get; set; }
        public string name { get; set; }
    }
}