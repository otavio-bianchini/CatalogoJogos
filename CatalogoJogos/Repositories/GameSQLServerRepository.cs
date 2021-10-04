using CatalogoJogos.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogos.Repositories
{
    public class GameSQLServerRepository : IGameRepository
    {
        private readonly SqlConnection sqlConnection;
        public GameSQLServerRepository(IConfiguration configuration){
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }
        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }

        public async Task Insert(Game game)
        {
            var comando = $"insert Games (Id, Name, Productor, Price) values ('{game.Id}', '{game.Name}', '{game.Productor}', '{game.Price.ToString().Replace(".",",")}' )";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task<List<Game>> Obter(int pag, int q)
        {
            var games = new List<Game>();
            var comando = $"select * from Games order by id offset {((pag - 1) * q)} rows fetch next {q} rows only";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Productor = (string)sqlDataReader["Productor"],
                    Price = (double)sqlDataReader["Price"]
                });
            }
            await sqlConnection.CloseAsync();

            return games;
        }

        public async Task<Game> Obter(Guid idGame)
        {
            Game game = null;
            var comando = $"select * from Games where Id = '{idGame}'";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                game = new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Productor = (string)sqlDataReader["Productor"],
                    Price = (double)sqlDataReader["Price"]
                };
            }
            await sqlConnection.CloseAsync();

            return game;
        }

        public async Task<List<Game>> Obter(string name, string productor)
        {
            var games = new List<Game>();
            var comando = $"select * from Games where Name = '{name}' and Productor = '{productor}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                games.Add(new Game
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Name = (string)sqlDataReader["Name"],
                    Productor = (string)sqlDataReader["Productor"],
                    Price = (double)sqlDataReader["Price"]
                });
            }

            await sqlConnection.CloseAsync();

            return games;
        }

        public async Task Remove(Guid idGame)
        {
            var comando = $"from Games delete where Id = '{idGame}'";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Update(Game game)
        {
            var comando = $"update Games set Name = '{game.Name}', Productor = '{game.Productor}', Price = '{game.Price.ToString().Replace(".", ",")} where Id = '{game.Id}'";
            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }
    }
}
