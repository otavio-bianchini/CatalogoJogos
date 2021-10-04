using CatalogoJogos.InputModel;
using CatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogos.Services
{
    public interface IGameService : IDisposable
    {
        Task<List<GameViewModel>> Obter(int pag, int q);
        Task<GameViewModel> Obter(Guid idGame);
        Task<GameViewModel> Insert(GameInputModel game);
        Task Update(Guid idGame, GameInputModel game);
        Task Update(Guid idGame, double price);
        Task Remove(Guid idGame);
    }
}
