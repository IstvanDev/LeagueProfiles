using LeagueProfiles.Data;
using LeagueProfiles.Dtos;
using LeagueProfiles.Interfaces;
using LeagueProfiles.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace LeagueProfiles.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DataContext _dataContext;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlayerRepository(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool CreatePlayer(Player player)
        {
            if (_dataContext.Players.Any(p => p.Username == player.Username))
            {
                return false;
            }

            _dataContext.Add(player);

            return Save();
        }

        public bool AddChampionToPlayer(int championId, int playerId)
        {
            var player = _dataContext.Players.Where(p => p.Id == playerId).Include(p => p.Champions).FirstOrDefault();

            var champion = _dataContext.Champions.Where(c => c.Id == championId).FirstOrDefault();

            if (player == null || champion == null)
                return false;

            player.Champions.Add(champion);

            return Save();
        }

        public bool Save()
        {
            return _dataContext.SaveChanges() > 0 ? true : false;
        }

        public async Task<Player> GetPlayerById(int id)
        {
            var playerById = await _dataContext.Players.Where(p => p.Id == id).FirstOrDefaultAsync();

            return playerById;
        }

        public async Task<ICollection<Player>> GetPlayerByName(string name)
        {
            var playersByName = await _dataContext.Players.Where(p => p.Name == name).ToListAsync();

            return playersByName;
        }
        public async Task<Player> GetPlayerByUserName(string userName)
        {
            var playerByUserName = await _dataContext.Players.Where(p => p.Username == userName).FirstOrDefaultAsync();

            return playerByUserName;
        }

        public ICollection<Player> GetPlayers()
        {
            return _dataContext.Players.ToList();
        }

        public bool PlayerExists(int id)
        {
            return _dataContext.Players.Any(p => p.Id == id);
        }
        
        public bool PlayerExists(string name)
        {
            return _dataContext.Players.Any(p => p.Username == name);
        }

        public ICollection<Champion> GetPlayerChampions(int id)
        {
            return _dataContext.Players
                .Where(p => p.Id == id)
                .Select(p => p.Champions)
                .FirstOrDefault();
        }

        public string GetPlayerNameTokenBased()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }

        public bool UpdatePlayer(Player player)
        {
            if (!PlayerExists(player.Id))
                return false;

            _dataContext.Update(player);

            return Save();
        }

        public bool DeletePlayer(Player player)
        {
            if (!PlayerExists(player.Id))
                return false;

            _dataContext.Remove(player);

            return Save();
        }
    }
}
