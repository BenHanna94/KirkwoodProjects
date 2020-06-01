using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IPieceManager
    {
        List<Piece> GetPiecesByProject(int projectID, bool complete = false);

        List<string> GetAllCompensatedStatuses();

        bool AddPiece(Piece piece);

        bool EditPiece(Piece oldPiece, Piece newPiece);

        bool SetPieceCompleteStatus(bool complete, int pieceID);

        Piece GetPieceByID(int id);
    }
}

