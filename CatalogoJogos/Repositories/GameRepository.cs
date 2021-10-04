using CatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogos.Repositories
{
    public class GameRepository : IGameRepository
    {
        private static Dictionary<Guid, Game> Games = new Dictionary<Guid, Game>();
        public void Dispose()
        {

        }

        public Task Insert(Game game)
        {
            Games.Add(game.Id, game);
            return Task.CompletedTask;
        }

        public Task<List<Game>> Obter(int pag, int q)
        {
            return Task.FromResult(Games.Values.Skip((pag - 1) * q).Take(q).ToList());
        }

        public Task<Game> Obter(Guid idGame)
        {
            if (!Games.ContainsKey(idGame))
                return null;
            return Task.FromResult(Games[idGame]);
        }

        public Task<List<Game>> Obter(string name, string productor)
        {
            return Task.FromResult(Games.Values.Where(game => game.Name.Equals(name) && game.Productor.Equals(productor)).ToList());
        }

        public Task Remove(Guid idGame)
        {
            Games.Remove(idGame);
            return Task.CompletedTask;
        }

        public Task Update(Game game)
        {
            Games[game.Id] = game;
            return Task.CompletedTask;
        }
    }
}
