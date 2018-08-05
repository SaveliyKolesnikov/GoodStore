using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using GoodStoreEntity;

namespace GoodStore
{
    class ProductsRepository : IDisposable
    {
        private readonly SqlConnection _connection;
        private const string TableName = "Products";

        public ProductsRepository(string conStr)
            : this(new SqlConnection(conStr ?? throw new ArgumentNullException(nameof(conStr))))
        { }

        public ProductsRepository(SqlConnection connection) =>
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));

        public void Dispose() => _connection.Dispose();


        public async Task<List<Product>> GetProductsAsync()
        {
            var commandString = $@"SELECT ProductId, Name, Price, Unit, Amount
                                   FROM {TableName}";

            var command = new SqlCommand(commandString, _connection);
            try
            {
                await _connection.OpenAsync();

                var result = new List<Product>();

                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Product
                        (
                            productId: (int)reader["ProductId"],
                            name: (string)reader["Name"],
                            price: (double)reader["Price"],
                            unit: (string)reader["Unit"],
                            amount: (double)reader["Amount"]
                        ));
                    }
                }

                return result;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return new List<Product>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Product>();
            }
            finally
            {
                _connection.Close();
            }

        }

        public async Task CreateProductAsync(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            var query = $@"INSERT INTO {TableName} (Name, Price, Unit, Amount)
                           VALUES (@Name, @Price, @Unit, @Amount)";

            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Unit", product.Unit);
            command.Parameters.AddWithValue("@Amount", product.Amount);
            try
            {
                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task DeleteProductAsync(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            var query = $"DELETE FROM {TableName} WHERE ProductId = @ProductId";
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@ProductId", product.ProductId);

            try
            {
                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _connection.Close();
            }

            var consignmentRepository = new ConsignmentRepository(_connection);
            foreach (var productConsignment in product.Consignments)
            {
                await consignmentRepository.DeleteConsignmentAsync(productConsignment);
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            var query = $@"UPDATE {TableName} 
                        SET Name = @Name, Price = @Price, Unit = @Unit, Amount = @Amount
                        WHERE ProductId = @ProductId";
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Unit", product.Unit);
            command.Parameters.AddWithValue("@Amount", product.Amount);
            command.Parameters.AddWithValue("@ProductId", product.ProductId);

            try
            {
                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}

