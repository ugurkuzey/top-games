using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TopGames.Core.Services;

namespace TopGames.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GameController> _logger;

        public GameController(IGameService gameService,
                              ILogger<GameController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _gameService.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in GetAll action: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetByDate/{date}")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            try
            {
                return Ok(await _gameService.GetByDate(date));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in GetByDate action: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetByTrackId/{trackId}")]
        public async Task<IActionResult> GetByTrackId(string trackId)
        {
            try
            {
                return Ok(await _gameService.GetByTrackId(trackId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in GetByDate action: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetByTrackId/{trackId}/Latest")]
        public async Task<IActionResult> GetLatestByTrackId(string trackId)
        {
            try
            {
                return Ok(await _gameService.GetLatestByTrackId(trackId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in GetByDate action: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
