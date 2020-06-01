using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IPieceAccessor
    {
        List<Piece> GetPiecesByProject(int projectID, bool complete = false);

        List<string> SelectAllCompensatedStatuses();

        Piece GetPieceByID(int pieceID);

        int InsertPiece(Piece piece);

        int UpdatePiece(Piece oldPiece, Piece newPiece);

        int CompletePiece(int pieceID);

        int DecompletePiece(int pieceID);

        Piece SelectPieceByID(int id);
    }
}
