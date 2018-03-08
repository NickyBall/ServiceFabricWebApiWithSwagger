using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi
{
    public class ClientStore : IClientStore
    {
        private StorageClient Db { get; set; }
        public ClientStore(StorageClient Storage)
        {
            Db = Storage;
        }
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            TableResult RetrieveResult = await Db.ClientStore.ExecuteAsync(TableOperation.Retrieve<ClientEntity>("ClientData", clientId));
            var Entity = (ClientEntity)RetrieveResult.Result;
            return new Client()
            {
                ClientId = clientId,
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                    {
                        new Secret(Entity.Secret.Sha256())
                    },

                // scopes that client has access to
                AllowedScopes = { Entity.Scope }
            };
        }
    }
}
