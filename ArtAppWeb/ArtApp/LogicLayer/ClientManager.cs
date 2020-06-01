using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataObjects;

namespace LogicLayer
{
    public class ClientManager : IClientManager
    {
        private ClientAccessor _clientAccessor;
        public ClientManager()
        {
            _clientAccessor = new ClientAccessor();
        }

        public ClientManager(ClientAccessor clientAccessor)
        {
            _clientAccessor = clientAccessor;
        }

        public bool AddClient(Client client)
        {
            bool result;

            try
            {
                result = _clientAccessor.InsertClient(client) > 0;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data not found.", ex);
            }

            return result;
        }

        public bool EditClient(Client oldClient, Client newClient)
        {
            bool result;

            try
            {
                result = _clientAccessor.UpdateClient(oldClient, newClient) == 1;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data not found.", ex);
            }

            return result;
        }

        public Client GetClientByID(int clientID)
        {
            Client result = null;

            try
            {
                result = _clientAccessor.SelectClientByID(clientID);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data not found.", ex);
            }

            return result;
        }

        public List<Client> RetrieveAllClients()
        {
            List<Client> result = null;

            try
            {
                result = _clientAccessor.SelectAllClients();
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Data not found. ", ex);
            }

            return result;
        }
    }
}
