using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Reference
    {
        public int ReferenceID { get; set; }

        public string ReferenceName { get; set; }

        public int ClientID { get; set; }

        public string Description { get; set; }

        public string FileLocation { get; set; }

        public List<int> PieceList { get; set; }
    }
}
