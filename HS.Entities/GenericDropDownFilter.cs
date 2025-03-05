using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities
{
    public class GenericDropDownFilter
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; } = false;
    }
}
