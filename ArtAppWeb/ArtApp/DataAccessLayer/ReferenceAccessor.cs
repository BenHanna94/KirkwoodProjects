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
    public class ReferenceAccessor : IReferenceAccessor
    {
        public int DeleteReference(Reference reference)
        {
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_delete_reference", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReferenceID", reference.ReferenceID);

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

        public List<Reference> GetAllReferences()
        {

            {
                List<Reference> references = new List<Reference>();


                var conn = DBConnection.GetConnection();
                var cmd = new SqlCommand("sp_select_all_references");
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();


                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var reference = new Reference();
                            reference.ReferenceID = (int)reader.GetInt32(0);
                            reference.ReferenceName = reader.GetString(1);
                            reference.ClientID = (int)reader.GetInt32(2);
                            reference.Description = reader.GetString(3);
                            reference.FileLocation = reader.GetString(4);

                            references.Add(reference);
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

                return references;
            }
        }

        public List<Reference> GetReferencesByClient(int clientID)
        {
            List<Reference> references = new List<Reference>();


            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_select_references_by_clientid");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ClientID", SqlDbType.Int);
            cmd.Parameters["@ClientID"].Value = clientID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reference = new Reference();
                        reference.ReferenceID = (int)reader.GetInt32(0);
                        reference.ReferenceName = reader.GetString(1);
                        reference.ClientID = clientID;
                        reference.Description = reader.GetString(2);
                        reference.FileLocation = reader.GetString(3);

                        references.Add(reference);
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

            return references;
        }

        public List<Reference> GetReferencesByPiece(int pieceID)
        {
            List<Reference> references = new List<Reference>();


            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_select_references_by_pieceid");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PieceID", SqlDbType.Int);
            cmd.Parameters["@PieceID"].Value = pieceID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var reference = new Reference();
                        reference.ReferenceID = reader.GetInt32(0);
                        reference.ReferenceName = reader.GetString(1);
                        reference.ClientID = reader.GetInt32(2);
                        reference.Description = reader.GetString(3);
                        reference.FileLocation = reader.GetString(4);

                        references.Add(reference);
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

            return references;
        }

        public int InsertOrDeletePieceReference(int referenceID, int pieceID, bool delete = false)
        {
            int rows = 0;

            string cmdText = delete ? "sp_delete_piece_reference" : "sp_insert_piece_reference";

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ReferenceID", referenceID);
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

        public int InsertReference(Reference reference)
        {
            int referenceIDCount = 0;

            var conn = DBConnection.GetConnection();

            // cmd
            var cmd = new SqlCommand("sp_insert_reference");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Automatically assumes the value is an int32. Decimal and money are more ambiguous

            cmd.Parameters.AddWithValue("@ReferenceName", reference.ReferenceName);
            cmd.Parameters.AddWithValue("@ClientID", reference.ClientID);
            cmd.Parameters.AddWithValue("@Description", reference.Description);
            cmd.Parameters.AddWithValue("@FileLocation", reference.FileLocation);

            try
            {
                conn.Open();
                referenceIDCount = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            
            return referenceIDCount;
        }

        public Reference SelectReferenceByID(int id)

        {
            Reference reference = null;

            // connection
            var conn = DBConnection.GetConnection();

            // command objects
            var cmd = new SqlCommand("sp_select_references_by_id");

            cmd.Connection = conn;

            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@ReferenceID", SqlDbType.NVarChar, 250);
            cmd.Parameters["@ReferenceID"].Value = id;
            // we cannot set the value of this parameter yet

            try
            {
                // open connection
                conn.Open();

                // execute the first command
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reference = new Reference();

                    reference.ReferenceID = id;
                    reference.ReferenceName = reader.GetString(0);
                    reference.ClientID = reader.GetInt32(1);
                    reference.Description = reader.GetString(2);
                    reference.FileLocation = reader.GetString(3);
                    
                }
                else
                {
                    throw new ApplicationException("Reference Not found.");
                }
                reader.Close(); // this is no longer needed
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reference;
        }

        public Reference SelectReferenceByName(string name)
        {
            Reference reference = null;

            // connection
            var conn = DBConnection.GetConnection();

            // command objects
            var cmd = new SqlCommand("sp_select_references_by_name");

            cmd.Connection = conn;

            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@ReferenceName", SqlDbType.NVarChar, 250);
            cmd.Parameters["@ReferenceName"].Value = name;
            // we cannot set the value of this parameter yet

            try
            {
                // open connection
                conn.Open();

                // execute the first command
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    reference = new Reference();

                    reference.ReferenceID = reader.GetInt32(0);
                    reference.ClientID = reader.GetInt32(1);
                    reference.Description = reader.GetString(2);
                    reference.FileLocation = reader.GetString(3);
                    reference.ReferenceName = name;
                }
                else
                {
                    throw new ApplicationException("Reference Not found.");
                }
                reader.Close(); // this is no longer needed
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return reference;
        }

        public int UpdateReference(Reference oldReference, Reference newReference)
        {
            {
                int rows = 0;

                // connecttion
                var conn = DBConnection.GetConnection();

                // cmd
                var cmd = new SqlCommand("sp_update_reference");
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                //Automatically assumes the value is an int32. Decimal and money are more ambiguous
                cmd.Parameters.AddWithValue("@ReferenceID", oldReference.ReferenceID);

                cmd.Parameters.AddWithValue("@NewReferenceName", newReference.ReferenceName);
                cmd.Parameters.AddWithValue("@NewClientID", newReference.ClientID);
                cmd.Parameters.AddWithValue("@NewDescription", newReference.Description);
                cmd.Parameters.AddWithValue("@NewFileLocation", newReference.FileLocation);

                cmd.Parameters.AddWithValue("@oldReferenceName", oldReference.ReferenceName);
                cmd.Parameters.AddWithValue("@oldClientID", oldReference.ClientID);
                cmd.Parameters.AddWithValue("@oldDescription", oldReference.Description);
                cmd.Parameters.AddWithValue("@oldFileLocation", oldReference.FileLocation);

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
}
