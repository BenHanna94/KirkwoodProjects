using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IClientManager
    {
        List<Client> RetrieveAllClients();

        Client GetClientByID(int clientID);

        bool EditClient(Client oldClient, Client newClient);

        bool AddClient(Client client);
    }
}
