using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopGames.Core.Models;

namespace TopGames.Core.Services
{
    public interface IGameService
    {
        Task<bool> AddRangeAsync(IEnumerable<Game> games);
        Task<IEnumerable<Game>> GetAll();
        Task<IEnumerable<Game>> GetByDate(DateTime date);
        Task<IEnumerable<Game>> GetByTrackId(string trackId);
        Task<Game> GetLatestByTrackId(string trackId);
    }
}
