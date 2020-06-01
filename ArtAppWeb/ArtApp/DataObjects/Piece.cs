using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Piece
    {
        public int PieceID { set; get; }

        public int ProjectID { set; get; }

        public int UserID { set; get; }

        public string Description { set; get; }

        public string Stage { set; get; }

        public bool Complete { set; get; }

        public decimal Compensation { set; get; }

        public string CompensatedStatus { set; get; }

        public List<Reference> PieceReferences { set; get; }
    }
}
