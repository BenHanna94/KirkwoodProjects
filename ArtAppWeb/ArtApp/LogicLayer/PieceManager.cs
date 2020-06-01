using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataObjects;

namespace LogicLayer
{
    public class PieceManager : IPieceManager
    {
        private IPieceAccessor _pieceAccessor;

        public PieceManager()
        {
            _pieceAccessor = new PieceAccessor();
        }

        public PieceManager(PieceAccessor pieceAccessor)
        {
            _pieceAccessor = pieceAccessor;
        }

        public bool AddPiece(Piece piece)
        {
            try
            {
                return _pieceAccessor.InsertPiece(piece) > 0;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data Not Saved. ", ex);
            }
        }

        public bool EditPiece(Piece oldPiece, Piece newPiece)
        {
            try
            {
                return 1 == _pieceAccessor.UpdatePiece(oldPiece, newPiece);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data Not Edited. ", ex);
            }
        }

        public List<string> GetAllCompensatedStatuses()
        {
            List<string> compStatuses = null;
            try
            {
                compStatuses = _pieceAccessor.SelectAllCompensatedStatuses();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Statuses not found", ex);
            }

            return compStatuses;
        }

        public Piece GetPieceByID(int id)
        {
            try
            {
                return _pieceAccessor.SelectPieceByID(id);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found. ", ex);
            }
        }

        public List<Piece> GetPiecesByProject(int projectID, bool complete = false)
        {
            try
            {
                return _pieceAccessor.GetPiecesByProject(projectID, complete);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found. ", ex);
            }
        }

        public bool SetPieceCompleteStatus(bool complete, int pieceID)
        {
            bool result = false;
            try
            {
                if (complete)
                {
                    result = 1 == _pieceAccessor.CompletePiece(pieceID);
                }
                else
                {
                    result = 1 == _pieceAccessor.DecompletePiece(pieceID);
                }
                if (result == false)
                {
                    throw new ApplicationException("Project record not updated.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed!", ex);
            }
            return result;
        }
    }
}
