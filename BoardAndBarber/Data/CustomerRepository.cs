using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Models;

namespace BoardAndBarber.Data
{
    public class CustomerRepository
    {
        static List<Customer> _customers = new List<Customer>();

        public void Add(Customer customerToAdd)
        {
            var newId = 1;
            if (_customers.Count > 0)
            {
                //get the next id by finding the max current id
                newId = _customers.Select(p => p.Id).Max() + 1;
            }

            customerToAdd.Id = newId;

            _customers.Add(customerToAdd);
        }

        public List<Customer> GetAll()
        {
            return _customers;
        }

        public Customer GetById(int id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        public Customer Update(int id, Customer customer)
        {
            var customerToUpdate = GetById(id);

            customerToUpdate.Birthday = customer.Birthday;
            customerToUpdate.FavoriteBarber = customer.FavoriteBarber;
            customerToUpdate.Name = customer.Name;
            customerToUpdate.Notes = customer.Notes;

            return customerToUpdate;
        }

        public void Remove(int id)
        {
            var customerToDelete = GetById(id);

            _customers.Remove(customerToDelete);
        }

    }
}
