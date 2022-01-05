using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AdoConsole
{
    class Program
    {
        public interface IProductDal
        {
            List<Product> GetAllProducts();
            Product GetProductById(int id);
            void Create(Product product);
            void Update(Product product);
            void Delete(int productId);
        }
        static void Main(string[] args)
        {
            var products = GetAllProducts();

            foreach (var item in products)
            {
                if (item.Price > 50)
                {
                    Console.WriteLine($"Id: {item.ProductId} Name = {item.Name} - Price = {item.Price}");
                }
            }
        }
        static List<Product> GetAllProducts()
        {
            List<Product> products = null;
            //provider
            using (var connection = GetConsoleApplication())
            {
                try
                {
                    connection.Open();

                    string sql = "select * from products";

                    SqlCommand command = new SqlCommand(sql, connection);

                    var reader = command.ExecuteReader();

                    products = new List<Product>();

                    while (reader.Read())
                    {
                        products.Add(
                            new Product
                            {
                                ProductId = int.Parse(reader["ProductID"].ToString()),
                                Name = reader["ProductName"].ToString(),
                                Price = double.Parse(reader["UnitPrice"].ToString())
                            });
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return products;
        }

        static SqlConnection GetConsoleApplication()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=SSPI;";
            return new SqlConnection(connectionString);
        }
    }
}
