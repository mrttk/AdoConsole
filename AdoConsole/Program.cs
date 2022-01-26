using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AdoConsole
{
    class Program
    {
        public interface IProductDal
        {
            List<Product> GetAllProducts();
            Product GetProductById(int id);
            List<Product> Find(string productName);
            void Create(Product product);
            void Update(Product product);
            void Delete(int productId);
        }

        public class ProductManager : IProductDal
        {
            private IProductDal _productDal;

            public ProductManager(IProductDal productDal)
            {
                _productDal = productDal;
            }
            public void Create(Product product)
            {
                throw new NotImplementedException();
            }

            public void Delete(int productId)
            {
                throw new NotImplementedException();
            }

            public List<Product> Find(string productName)
            {
                return _productDal.Find(productName);
            }

            public List<Product> GetAllProducts()
            {
                return _productDal.GetAllProducts();
            }

            public Product GetProductById(int id)
            {
                return _productDal.GetProductById(id);
            }

            public void Update(Product product)
            {
                throw new NotImplementedException();
            }
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
                Product product = null;
                //provider
                using (var connection = GetMSSqlConnection())
                {

                    try
                    {
                        connection.Open();

                        string sql = "select * from products where ProductID=@productid";

                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.Add("@productid", SqlDbType.Int).Value = id;

                        var reader = command.ExecuteReader();
                        reader.Read();

                        if (reader.HasRows)
                        {
                            product = new Product()
                            {
                                ProductId = int.Parse(reader["ProductID"].ToString()),
                                Name = reader["ProductName"].ToString(),
                                Price = double.Parse(reader["UnitPrice"].ToString())
                            };
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
                return product;
            }

            public void Update(Product product)
            {
                throw new NotImplementedException();
            }

            public List<Product> Find(string productName)
            {
                List<Product> products = null;
                //provider
                using (var connection = GetMSSqlConnection())
                {
                    try
                    {
                        connection.Open();

                        string sql = "select * from products where ProductName like @name";

                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.Add("@name", SqlDbType.NVarChar).Value = "%" + productName + "%";

                        var reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
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
        }

        static void Main(string[] args)
        {
            var productDal = new ProductManager(new MSSQLProductDal());
            //var products = productDal.GetAllProducts();

            //foreach (var item in products)
            //{
            //    if (item.Price > 50)
            //    {
            //        Console.WriteLine($"Id: {item.ProductId} Name = {item.Name} - Price = {item.Price}");
            //    }
            //}

            var product = productDal.GetProductById(2);
            if (product != null)
            {
                Console.WriteLine($"Id: {product.ProductId} - Name: {product.Name} - Price: {product.Price}");
            }
            else
            {
                Console.WriteLine("Ürün Bulunamadı!");
            }


            Console.WriteLine("Result for search key");
            Console.WriteLine("------------------------------");

            var productList = productDal.Find("Ch");

            foreach (var item in productList)
            {
                Console.WriteLine($"Id: {item.ProductId} Name = {item.Name} - Price = {item.Price}");
            }
        }
    }
}
