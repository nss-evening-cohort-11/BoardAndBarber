using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;

namespace BoardAndBarber.Data
{
    public class CustomerRepository
    {
        static List<Customer> _customers = new List<Customer>();

        const string _connectionString = "Server=localhost;Database=BoardAndBarber;Trusted_Connection=True;";

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
            using var connection = new SqlConnection(_connectionString);

            connection.Open();

            var command = connection.CreateCommand();
            var sql = "select * from customers";

            command.CommandText = sql;

            var reader = command.ExecuteReader();
            var customers = new List<Customer>();
            
            while (reader.Read())
            {
                var customer = MapToCustomer(reader);
                customers.Add(customer);
            }

            return customers;

            //return _customers;
        }

        public Customer GetById(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            connection.Open();

            var command = connection.CreateCommand();
            var query = $@"select *
                          from Customers
                          where id = {id}";

            command.CommandText = query;

            //run this query, and i don't care about the results
            //command.ExecuteNonQuery()

            //run this query, and only return the top row's leftmost column
            //command.ExecuteScalar()

            //run this query and give me the results, one row at a time
            var reader = command.ExecuteReader();
            //sql server has executed the command and is waiting to give us results
            
            if (reader.Read())
            {
                return MapToCustomer(reader);
            }
            else
            {
                return null;
            }

            //return _customers.FirstOrDefault(c => c.Id == id);
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

        Customer MapToCustomer(SqlDataReader reader)
        {
            var customerFromDb = new Customer();

            //do something with the  result
            customerFromDb.Id = (int)reader["Id"]; //explicit conversion/cast, throws exception on failure
            customerFromDb.Name = reader["Name"] as string; // implicit cast/conversion, returns a null on failure 
            customerFromDb.Birthday = DateTime.Parse(reader["Birthday"].ToString()); //parsing
            customerFromDb.FavoriteBarber = reader["FavoriteBarber"].ToString(); //make it a string
            customerFromDb.Notes = reader["Notes"].ToString();

            return customerFromDb;
        }


    }
}
