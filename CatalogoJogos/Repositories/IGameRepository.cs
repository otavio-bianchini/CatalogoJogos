using CatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogos.Repositories
{
    public interface IGameRepository : IDisposable
    {
        Task<List<Game>> Obter(int pag, int q);
        Task<Game> Obter(Guid idGame);
        Task<List<Game>> Obter(string name, string productor);
        Task Insert(Game game);
        Task Update(Game game);
        Task Remove(Guid idGame);
    }
}
