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

        public class MSSQLProductDal : IProductDal
        {

            private SqlConnection GetMSSqlConnection()
            {
                string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=SSPI;";
                return new SqlConnection(connectionString);
            }

            public void Create(Product product)
            {
                throw new NotImplementedException();
            }

            public void Delete(int productId)
            {
                throw new NotImplementedException();
            }

            public List<Product> GetAllProducts()
            {
                List<Product> products = null;
                //provider
                using (var connection = GetMSSqlConnection())
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

            public Product GetProductById(int id)
            {
                throw new NotImplementedException();
            }

            public void Update(Product product)
            {
                throw new NotImplementedException();
            }
        }
        static void Main(string[] args)
        {
           var productDal = new MSSQLProductDal();
           var products = productDal.GetAllProducts();

            foreach (var item in products)
            {
                if (item.Price > 50)
                {
                    Console.WriteLine($"Id: {item.ProductId} Name = {item.Name} - Price = {item.Price}");
                }
            }
        }
    }
}
