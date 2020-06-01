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
    public class ClientAccessor : IClientAccessor
    {
        
        public List<Client> SelectAllClients()
        {
            List<Client> result = new List<Client>();

            var conn = DBConnection.GetConnection();

            //Consider replacing this with a more restrictive procedure later
            var cmd = new SqlCommand("sp_select_all_clients", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            // No parameters

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var client = new Client();

                        client.ClientID = reader.GetInt32(0);
                        client.Name = reader.GetString(1);
                        client.Contact = reader.GetString(2);
                        client.Notes = reader.GetString(3);

                        result.Add(client);
                    }
                }

                reader.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public Client SelectClientByID(int clientID)
        {
            Client client = new Client();

            // connection
            var conn = DBConnection.GetConnection();

            // Command objects (2)
            var cmd1 = new SqlCommand("sp_select_client_by_id");

            cmd1.Connection = conn;

            cmd1.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd1.Parameters.AddWithValue("@ClientID", clientID);

            try
            {
                conn.Open();

                var reader1 = cmd1.ExecuteReader();

                if (reader1.Read())
                {
                    client.ClientID = clientID;
                    client.Name = reader1.GetString(1);
                    client.Contact = reader1.GetString(2);
                    client.Notes = reader1.GetString(3);
                }
                else
                {
                    throw new ApplicationException("Client Not Found.");
                }

            }
            catch (Exception ex)
            {

                throw ex; 
            }
            finally
            {
                conn.Close();
            }

            return client;
        }


        public int InsertClient(Client client)
        {
            int clientID = 0;

            var conn = DBConnection.GetConnection();

            //Consider replacing this with a more restrictive procedure later
            var cmd = new SqlCommand("sp_insert_client", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", client.Name);
            cmd.Parameters.AddWithValue("@Contact", client.Contact);
            cmd.Parameters.AddWithValue("@Notes", client.Notes);

            try
            {
                conn.Open();
                clientID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return clientID;
        }

        public int UpdateClient(Client oldClient, Client newClient)
        {

            int rows = 0;

            // connecttion
            var conn = DBConnection.GetConnection();

            // cmd
            var cmd = new SqlCommand("sp_update_client");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            //Automatically assumes the value is an int32. Decimal and money are more ambiguous
            cmd.Parameters.AddWithValue("@ClientID", oldClient.ClientID);

            
            cmd.Parameters.AddWithValue("@NewName", newClient.Name);
            cmd.Parameters.AddWithValue("@NewContact", newClient.Contact);
            cmd.Parameters.AddWithValue("@NewNotes", newClient.Notes);

            cmd.Parameters.AddWithValue("@OldName", oldClient.Name);
            cmd.Parameters.AddWithValue("@OldContact", oldClient.Contact);
            cmd.Parameters.AddWithValue("@OldNotes", oldClient.Notes);

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
