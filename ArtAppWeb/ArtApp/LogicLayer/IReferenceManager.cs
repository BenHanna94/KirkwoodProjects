using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayer
{
    public interface IReferenceManager
    {
        List<Reference> GetReferencesByClient(int clientID);

        List<string> GetReferenceNamesByPiece(int pieceID);

        List<string> GetAllReferenceNames();

        List<Reference> GetAllReferences();

        Reference GetReferenceByName(string name);

        Reference GetReferenceByID(int id);

        bool AddReference(Reference reference);

        bool EditReference(Reference oldReference, Reference newReference);

        bool DeleteReference(Reference reference);

        bool DeletePieceReference(int referenceID, int pieceID);

        bool AddPieceReference(int referenceID, int pieceID);
        bool EditReference(Reference reference);
    }
}