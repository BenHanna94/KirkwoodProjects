using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Client
    {
        public int ClientID { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Notes { get; set; }
        public List<Reference> References { get; set; }

    }
}
