using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopGames.Core.Models;
using TopGames.Core.Services;
using TopGames.EntityFramework;

namespace TopGames.Services
{
    public class GameService : IGameService
    {
        private readonly TopGamesDbContext _context;
        private readonly ILogger<GameService> _logger;

        public GameService(TopGamesDbContext context,
                           ILogger<GameService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Game> games)
        {
            try
            {
                await _context.AddRangeAsync(games);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error received during AddRangeAsync operation. Message: {ex.Message} - Detail: {ex.StackTrace}");
            }
            return false;
        }

        public async Task<IEnumerable<Game>> GetAll()
        {
            try
            {
                return await _context.Games.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error received during GetAll operation. Message: {ex.Message} - Detail: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<Game>> GetByDate(DateTime date)
        {
            try
            {
                return await _context.Games.Where(q => q.CreatedDate.Date == date).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error received during GetByDate operation. Message: {ex.Message} - Detail: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<Game>> GetByTrackId(string trackId)
        {
            try
            {
                return await _context.Games.Where(q => q.TrackId.Equals(trackId)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error received during GetByTrackId operation. Message: {ex.Message} - Detail: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Game> GetLatestByTrackId(string trackId)
        {
            try
            {
                return await _context.Games.Where(q => q.TrackId.Equals(trackId)).OrderByDescending(o => o.CreatedDate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error received during GetLatestByTrackId operation. Message: {ex.Message} - Detail: {ex.StackTrace}");
                throw;
            }
        }
    }
}
