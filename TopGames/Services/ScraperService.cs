using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using TopGames.Core.Models;
using TopGames.Core.Services;
using TopGames.Infrastructure;

namespace TopGames.Services
{
    public class ScraperService : IScraperService
    {
        IConfiguration _configuration;

        public ScraperService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Game> GetTopGames()
        {
            var topGames = new List<Game>();
            var date = DateTime.Now;

            var baseUrl = _configuration.GetValue<string>("WebScraper:BaseUrl");
            var countTopGames = _configuration.GetValue<int>("WebScraper:Count");

            var web = new HtmlWeb();

            var htmlDoc = web.Load(_configuration.GetValue<string>("WebScraper:TopGamesUrl"));

            var nodeList = htmlDoc.DocumentNode.QuerySelectorAll("div.b8cIId.ReQCgd.Q9MA7b > a");
            Game game;
            foreach (var node in nodeList)
            {
                var href = node.Attributes["href"].Value;
                href = $"{baseUrl}{href}";

                htmlDoc = web.Load(href);

                var addInfos = GetAdditionalInformation(htmlDoc.DocumentNode.QuerySelector("div.IxB2fe").ChildNodes);

                game = new Game();
                game.TrackId = HttpUtility.ParseQueryString(new Uri(href).Query).Get("id");
                game.Title = HttpUtility.HtmlDecode(htmlDoc.DocumentNode.QuerySelector("h1.AHFaub > span").InnerText);
                game.Description = HttpUtility.HtmlDecode(htmlDoc.DocumentNode.QuerySelector("div[jsname=\"sngebd\"]").InnerText);
                game.ReviewCount = Helpers.ExtractIntNumber(htmlDoc.DocumentNode.QuerySelector("span.AYi5wd.TBRnV > span").InnerText);
                game.InstallCount = Helpers.ExtractLongNumber(addInfos["Installs"]);
                game.CurrentVersion = addInfos["Current Version"];
                game.LastUpdateDate = DateTime.ParseExact(addInfos["Updated"], "MMMM d, yyyy", CultureInfo.InvariantCulture);
                game.Size = Helpers.ExtractIntNumber(addInfos["Size"]);
                game.Author = addInfos["Offered By"];
                game.CreatedDate = date;

                topGames.Add(game);

                if (topGames.Count >= countTopGames) break;
            }

            return topGames;
        }

        private Dictionary<string, string> GetAdditionalInformation(HtmlNodeCollection htmlNodes)
        {
            Dictionary<string, string> infos = new Dictionary<string, string>();
            foreach (var node in htmlNodes)
            {
                if (node.ChildNodes.Count > 1)
                    infos.Add(node.ChildNodes[0].InnerText, node.ChildNodes[1].InnerText);
            }
            return infos;
        }
    }
}
