using CatalogoJogos.Entities;
using CatalogoJogos.Exeptions;
using CatalogoJogos.InputModel;
using CatalogoJogos.Repositories;
using CatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogos.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        public async Task<GameViewModel> Insert(GameInputModel game)
        {
            var gameEntity = await _gameRepository.Obter(game.Name, game.Productor);
            if (gameEntity.Count > 0)
                throw new GameRegisterException();
            var gameInsert = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Productor = game.Productor,
                Price = game.Price
            };
            await _gameRepository.Insert(gameInsert);

            return new GameViewModel
            {
                Id = gameInsert.Id,
                Name = game.Name,
                Productor = game.Productor,
                Price = game.Price
            };
        }

        public async Task<List<GameViewModel>> Obter(int pag, int q)
        {
            var games = await _gameRepository.Obter(pag, q);

            return games.Select(game => new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Productor = game.Productor,
                Price = game.Price
            }).ToList();
        }

        public  async Task<GameViewModel> Obter(Guid idGame)
        {
            var game = await _gameRepository.Obter(idGame);
            if (game==null)
            {
                return null;
            }
            return new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Productor = game.Productor,
                Price = game.Price
            };
        }

        public async Task Remove(Guid idGame)
        {
            var game = await _gameRepository.Obter(idGame);
            if (game == null)
                throw new GameNotRegisteredException();
            await _gameRepository.Remove(idGame);
        }

        public async Task Update(Guid idGame, GameInputModel game)
        {
            var GameEntity = await _gameRepository.Obter(idGame);
            if (GameEntity == null)
                throw new GameNotRegisteredException();
            GameEntity.Name = game.Name;
            GameEntity.Productor = game.Productor;
            GameEntity.Price = game.Price;
            await _gameRepository.Update(GameEntity);
        }

        public async Task Update(Guid idGame, double price)
        {
            var GameEntity = await _gameRepository.Obter(idGame);
            if (GameEntity == null)
                throw new GameNotRegisteredException();
            GameEntity.Price = price;
            await _gameRepository.Update(GameEntity);
        }
        public void Dispose()
        {
            _gameRepository?.Dispose();
        }
    }
}
