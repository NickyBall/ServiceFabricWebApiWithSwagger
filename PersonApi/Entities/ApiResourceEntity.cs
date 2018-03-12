using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi.Entities
{
    public class ApiResourceEntity : TableEntity
    {
        // PK = "ResourceData"
        // RK = {Scope}
        public string ApiDescription { get; set; }
    }
}
