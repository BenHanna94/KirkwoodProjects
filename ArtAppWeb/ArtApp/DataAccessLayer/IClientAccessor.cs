using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IClientAccessor
    {
        List<Client> SelectAllClients();

        Client SelectClientByID(int clientID);

        int UpdateClient(Client oldClient, Client newClient);

        int InsertClient(Client client);
    }
}
