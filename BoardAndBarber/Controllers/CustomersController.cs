using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Data;
using BoardAndBarber.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardAndBarber.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            var repo = new CustomerRepository();
            repo.Add(customer);

            return Created($"/api/customers/{customer.Id}", customer);
        }


    }
}
