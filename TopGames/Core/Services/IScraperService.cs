using System.Collections.Generic;
using TopGames.Core.Models;

namespace TopGames.Core.Services
{
    public interface IScraperService
    {
        List<Game> GetTopGames();
    }
}
