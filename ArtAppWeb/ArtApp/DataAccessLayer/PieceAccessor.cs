using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayer
{
    public class PieceAccessor : IPieceAccessor
    {
        public int CompletePiece(int pieceID)
        {
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_complete_piece", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PieceID", pieceID);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }

        public int DecompletePiece(int pieceID)
        {
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_decomplete_piece", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PieceID", pieceID);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }

        public Piece GetPieceByID(int pieceID)
        {
            throw new NotImplementedException();
        }

        public List<Piece> GetPiecesByProject(int projectID, bool complete = false)
        {
            List<Piece> pieces = new List<Piece>();

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_select_pieces_by_projectid");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ProjectID", SqlDbType.Int);
            cmd.Parameters["@ProjectID"].Value = projectID;

            cmd.Parameters.Add("@Complete", SqlDbType.Bit);
            cmd.Parameters["@Complete"].Value = complete;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var piece = new Piece();

                        piece.PieceID = reader.GetInt32(0);
                        piece.ProjectID = projectID;
                        piece.UserID = reader.GetInt32(1);
                        piece.Description = reader.GetString(2);
                        piece.Stage = reader.GetString(3);
                        piece.Complete = reader.GetBoolean(4);
                        piece.Compensation = reader.GetDecimal(5);
                        piece.CompensatedStatus = reader.GetString(6);

                        pieces.Add(piece);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return pieces;
        }

        public int InsertPiece(Piece piece)
        {
            int pieceID = 0;

            var conn = DBConnection.GetConnection();

            var cmd = new SqlCommand("sp_insert_piece", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProjectID", piece.ProjectID);
            cmd.Parameters.AddWithValue("@UserID", piece.UserID);
            cmd.Parameters.AddWithValue("@Description", piece.Description);
            cmd.Parameters.AddWithValue("@Stage", piece.Stage);

            try
            {
                conn.Open();
                pieceID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return pieceID;
        }

        public List<string> SelectAllCompensatedStatuses()
        {
            List<string> compStatuses = new List<string>();

            // connection
            var conn = DBConnection.GetConnection();

            // command objects
            var cmd = new SqlCommand("sp_select_all_compensated_status");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                // open connection
                conn.Open();

                // execute the first command

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string status = reader.GetString(0);
                    compStatuses.Add(status);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return compStatuses;
        }

        public Piece SelectPieceByID(int id)
        {
            Piece piece = null;

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_select_piece_by_id");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PieceID", SqlDbType.Int);
            cmd.Parameters["@PieceID"].Value = id;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        piece = new Piece();

                        piece.PieceID = id;
                        piece.ProjectID = reader.GetInt32(0);
                        piece.UserID = reader.GetInt32(1);
                        piece.Description = reader.GetString(2);
                        piece.Stage = reader.GetString(3);
                        piece.Complete = reader.GetBoolean(4);
                        piece.Compensation = reader.GetDecimal(5);
                        piece.CompensatedStatus = reader.GetString(6); 
                    }
                    
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return piece;
        }

        public int UpdatePiece(Piece oldPiece, Piece newPiece)
        {
            int rows = 0;

            var conn = DBConnection.GetConnection();

            var cmd = new SqlCommand("sp_update_piece", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PieceID", oldPiece.PieceID);
                                               
            cmd.Parameters.AddWithValue("@OldUserID", oldPiece.UserID);
            cmd.Parameters.AddWithValue("@OldDescription", oldPiece.Description);
            cmd.Parameters.AddWithValue("@OldStage", oldPiece.Stage);
            cmd.Parameters.AddWithValue("@OldCompensation", oldPiece.Compensation);
            cmd.Parameters.AddWithValue("@OldCompensatedStatusID", oldPiece.CompensatedStatus);

            cmd.Parameters.AddWithValue("@NewUserID", newPiece.UserID);
            cmd.Parameters.AddWithValue("@NewDescription", newPiece.Description);
            cmd.Parameters.AddWithValue("@NewStage", newPiece.Stage);
            cmd.Parameters.AddWithValue("@NewCompensation", newPiece.Compensation);
            cmd.Parameters.AddWithValue("@NewCompensatedStatusID", newPiece.CompensatedStatus);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }
    }
}
