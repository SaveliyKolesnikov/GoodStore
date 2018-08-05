using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using GoodStoreEntity;

namespace GoodStore
{
    class ConsignmentRepository : IDisposable
    {
        private readonly SqlConnection _connection;
        private const string TableName = "Consignment";

        public ConsignmentRepository(string conStr)
            : this(new SqlConnection(conStr ?? throw new ArgumentNullException(nameof(conStr))))
        { }

        public ConsignmentRepository(SqlConnection connection) =>
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));

        public void Dispose() => _connection.Dispose();


        public async Task<List<Consignment>> GetConsignmentAsync()
        {
            var commandString = $@"SELECT ConsignmentId, ProductId, Amount, Date
                                   FROM {TableName}";

            var command = new SqlCommand(commandString, _connection);
            try
            {
                await _connection.OpenAsync();

                var result = new List<Consignment>();

                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Consignment
                        (
                            consignmentId: (int) reader["ConsignmentId"],
                            productId: (int) reader["productId"],
                            amount: (int) reader["Amount"],
                            date: (DateTime)reader["Date"]
                        ));
                    }
                }

                return result;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return new List<Consignment>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Consignment>();
            }
            finally
            {
                _connection.Close();
            }

        }

        public async Task CreateConsignmentAsync(Consignment consignment)
        {
            if (consignment is null) throw new ArgumentNullException(nameof(consignment));

            var query = $@"INSERT INTO {TableName} (ProductId, Amount, Date)
                           VALUES (@ProductId, @Amount, @Date)";

            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@ProductId", consignment.ProductId);
            command.Parameters.AddWithValue("@Amount", consignment.Amount);
            command.Parameters.AddWithValue("@Date", consignment.Date);
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

        public async Task DeleteConsignmentAsync(Consignment consignment)
        {
            if (consignment is null) throw new ArgumentNullException(nameof(consignment));

            var query = $"DELETE FROM {TableName} WHERE ConsignmentId = @ConsignmentId";
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@ConsignmentId", consignment.ConsignmentId);
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

        public async Task UpdateConsignmentAsync(Consignment consignment)
        {
            if (consignment is null) throw new ArgumentNullException(nameof(consignment));

            var query = $@"UPDATE {TableName} 
                        SET ProductId = @ProductId, Amount = @Amount, Date = @Date
                        WHERE ConsignmentId = @ConsignmentId";
            var command = new SqlCommand(query, _connection);
            command.Parameters.AddWithValue("@ProductId", consignment.ProductId);
            command.Parameters.AddWithValue("@Amount", consignment.Amount);
            command.Parameters.AddWithValue("@Date", consignment.Date);
            command.Parameters.AddWithValue("@ConsignmentId", consignment.ConsignmentId);

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
