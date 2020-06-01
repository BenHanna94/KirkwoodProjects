using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Project
    {
        public int ProjectID{ set; get; }
         
        public string Name{ set; get; }
        
        public string Type{ set; get; }
       
        public string Description{ set; get; }
       
        public bool Complete{ set; get; }
         
        public List<Piece> ProjectPieces{ set; get; }
    }
}
