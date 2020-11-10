﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Data;
using BoardAndBarber.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardAndBarber.Controllers
{
    public abstract class FirebaseEnabledController : ControllerBase
    {
        protected string UserId => User.FindFirst(x => x.Type == "user_id").Value;
    }


    [Route("api/customers")]
    [ApiController]
    [Authorize]
    public class CustomersController : FirebaseEnabledController
    {
        CustomerRepository _repo;

        public CustomersController(CustomerRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            _repo.Add(customer);

            return Created($"/api/customers/{customer.Id}", customer);
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var allCustomers = _repo.GetAll();

            return Ok(allCustomers);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var customer = _repo.GetById(id);

            if (customer == null) return NotFound("No customer with that Id was found");

            return Ok(customer);
        }


        //api/customers/{id}
        //api/customers/2
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, Customer customer)
        {
            var updatedCustomer = _repo.Update(id, customer);

            return Ok(updatedCustomer);
        }
        
        //api/customers/{id}
        //api/customers/2
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            if (_repo.GetById(id) == null)
            {
                return NotFound();
            }

            _repo.Remove(id);

            return Ok();
        }
    }
}
