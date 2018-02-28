using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PersonApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Person")]
    [Authorize]
    public class PersonController : Controller
    {
        [HttpGet]
        public List<Person> GetPersons()
        {
            return new List<Person>()
            {
                new Person()
                {
                    Firstname = "Jakkrit",
                    Lastname = "Junrat",
                    Age = 27
                },
                new Person()
                {
                    Firstname = "Apinya",
                    Lastname = "Pimpisan",
                    Age = 27
                }
            };
        }
    }

    public class Person
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
    }
}