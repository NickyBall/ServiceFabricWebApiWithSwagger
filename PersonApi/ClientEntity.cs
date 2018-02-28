using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi
{
    public class ClientEntity : TableEntity
    {
        // PK = "ClientData"
        // RK = {ClientId}

        public string Secret { get; set; }
        public string Scope { get; set; }
    }
}
