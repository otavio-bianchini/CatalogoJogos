using CatalogoJogos.Exeptions;
using CatalogoJogos.InputModel;
using CatalogoJogos.Services;
using CatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogos.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }
        /// <summary>
        /// Retorna a lista de jogos paginada
        /// </summary>
        /// <remarks>Não é possível retornar jogo sem paginação</remarks>
        /// <param name="pag">Página a ser consultada</param>
        /// <param name="q">Quantidade de parametros por pagina</param>
        /// <response code = "200">Retorna lista de jogos</response>
        /// <response code = "204">Caso não haja jogos</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<GameViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)]int pag = 1,[FromQuery, Range(1,50)]int q = 5)
        {
            var game = await _gameService.Obter(pag, q);
            if (game.Count() == 0) { return NoContent(); }
            return Ok(game);
        }
        /// <summary>
        /// Retorna jogo pelo seu Id
        /// </summary>
        /// <param name="idGame">Id do jogo desejado</param>
        /// <response code = "200">Retorna o jogo uscado</response>
        /// <response code = "204">caso não haja jogo com o Id requisitado</response>
        /// <returns></returns>
        [HttpGet("{idGame:guid}")]
        public async Task<ActionResult<GameViewModel>> Obter([FromRoute] Guid idGame)
        {
            var game = await _gameService.Obter(idGame);
            if (game == null)
                return NoContent();
            return Ok(game);
        }
        /// <summary>
        /// Insere novo Jogo na lista
        /// </summary>
        /// <param name="gameInputModel">
        /// Informações referentes ao jogo a ser acrecentado na lista onde:
        ///  O nome deve conter entre 3 e 100 caracteres;
        ///  A produtora deve conter entre 1 e 100 caracteres;
        ///  O preço deve estar em um valor entre 0 e R$10.000,00
        /// </param>
        /// <response code = "200">Jogo inserido na lista</response>
        /// <response code = "400">Dado do jogo inválido</response>
        /// <response code = "422">Caso já haja este mesmo jogo feito pela mesma produtora na lista</response>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<GameViewModel>> InsertGame([FromBody] GameInputModel gameInputModel)
        {
            try
            {
                var game = await _gameService.Insert(gameInputModel);
                return Ok();
            }
            catch (GameRegisterException ex)
            {
                return UnprocessableEntity("Já existe um jogo com esse nome nesta produtora");
            }
        }
        /// <summary>
        /// Atualiza informações de um jogo
        /// </summary>
        /// <param name="idGame">Id do jogo que se deseja alterar</param>
        /// <param name="gameInputModel">Novos parâmetros para o jogo</param>
        /// <response code = "200">Atualização de jogo concluida</response>
        /// <response code = "400">Dado do jogo invalido</response>
        /// <returns></returns>
        [HttpPut("{idGame:guid}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid idGame, [FromBody] GameInputModel gameInputModel)
        {
            try
            {
                await _gameService.Update(idGame,gameInputModel);
                return Ok();
            }
            catch (GameNotRegisteredException ex)
            {
                return NotFound("Jogo não cadastrado");
            }
        }
        [HttpPatch("{idGame:guid}/preco/{cost:double}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid idGame, [FromRoute] double Price)
        {
            try
            {
                await _gameService.Update(idGame, Price);
                return Ok();
            }
            catch (GameNotRegisteredException ex)
            {
                return NotFound("Jogo não cadastrado");
            }
        }
        [HttpDelete("{idGame:guid}")]
        public async Task<ActionResult> DeleteGame([FromRoute] Guid idGame)
        {
            try
            {
                await _gameService.Remove(idGame);
                return Ok();
            }
            catch (GameNotRegisteredException ex)
            {
                return NotFound("Jogo não cadastrado");
            }
        }
    }
}
