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
            var sql = @"INSERT INTO [dbo].[Customers]
                               ([Name]
                               ,[Birthday]
                               ,[FavoriteBarber]
                               ,[Notes])
                        Output inserted.notes
                        VALUES
                               (@name,@birthday,@favoritebarber,@notes)";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("name", customerToAdd.Name);
            cmd.Parameters.AddWithValue("birthday", customerToAdd.Birthday);
            cmd.Parameters.AddWithValue("favoritebarber", customerToAdd.FavoriteBarber);
            cmd.Parameters.AddWithValue("notes", customerToAdd.Notes);

            //run this query, and only return the top row's leftmost column
            var newId = (int) cmd.ExecuteScalar();

            customerToAdd.Id = newId;
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
        }

        public Customer Update(int id, Customer customer)
        {
            var sql = @"UPDATE [dbo].[Customers]
                          SET [Name] = @name
                             ,[Birthday] = @birthday
                             ,[FavoriteBarber] = @favoriteBarber
                             ,[Notes] = @notes
                        output inserted.*
                        WHERE id = @id";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("name", customer.Name);
            cmd.Parameters.AddWithValue("birthday", customer.Birthday);
            cmd.Parameters.AddWithValue("favoriteBarber", customer.FavoriteBarber);
            cmd.Parameters.AddWithValue("notes", customer.Notes);
            cmd.Parameters.AddWithValue("id", id);

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return MapToCustomer(reader);
            }

            return null;
        }

        public void Remove(int customerId)
        {
            var sql = @"DELETE 
                        FROM [dbo].[Customers]
                        WHERE Id = @id";

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("id", customerId);

            //run this query, and i don't care about the results
            var rows = cmd.ExecuteNonQuery();

            if (rows != 1)
            {
                //do something because that is bad
            }
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
