using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayer
{
    public interface IReferenceAccessor
    {
        List<Reference> GetReferencesByClient(int clientID);

        List<Reference> GetReferencesByPiece(int pieceID);

        List<Reference> GetAllReferences();

        Reference SelectReferenceByName(string name);

        Reference SelectReferenceByID(int id);

        int InsertReference(Reference reference);

        int UpdateReference(Reference oldReference, Reference newReference);

        int DeleteReference(Reference reference);

        int InsertOrDeletePieceReference(int referenceID, int pieceID, bool delete = false);
    }
}
