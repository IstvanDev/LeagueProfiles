using AutoMapper;
using LeagueProfiles.Dtos;
using LeagueProfiles.Interfaces;
using LeagueProfiles.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeagueProfiles.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class PlayerController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IMapper _mapper;

        public PlayerController(IPlayerRepository playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Player>))]

        public IActionResult GetPlayers()
        {
            var players = _playerRepository.GetPlayers();

            return Ok(players);
        }

        [HttpGet("{playerId}")]
        [ProducesResponseType(200, Type = typeof(Player))]
        [ProducesResponseType(400)]

        public async Task<IActionResult> GetPlayerById(int playerId)
        {
            if (!_playerRepository.PlayerExists(playerId))
                return NotFound("No player was found with that ID");

            var player = await _playerRepository.GetPlayerById(playerId);

            return Ok(player);
        }

        [HttpGet("search/{playerName}")]
        [ProducesResponseType(200, Type = typeof(Player))]
        [ProducesResponseType(400)]

        public async Task<IActionResult> GetPlayerByName(string playerName)
        {
            if (!_playerRepository.PlayerExists(playerName))
                return NotFound("No player was found with that name");

            var player = await _playerRepository.GetPlayerByName(playerName);

            return Ok(player);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreatePlayer([FromBody] PlayerDto playerdto)
        {
            if (playerdto == null)
                return BadRequest(ModelState);

            var playerMap = _mapper.Map<Player>(playerdto);

            if (!_playerRepository.CreatePlayer(playerMap))
            {
                ModelState.AddModelError("", "Something went wrong while trying to save to the database.");
                return BadRequest(ModelState);
            }

            return Ok("Succesfully created player: " + playerMap.ToString());
        }

        [HttpGet("{playerId}/Champions")]
        [ProducesResponseType(200, Type = typeof(ICollection<Champion>))]
        [ProducesResponseType(400)]
        public IActionResult GetPlayerChampions(int playerId)
        {
            if (!_playerRepository.PlayerExists(playerId))
                return NotFound("No player was found with that ID");

            var champions = _playerRepository.GetPlayerChampions(playerId);

            if (champions == null)
                return BadRequest("Player champion list instance is not set");

            return Ok(champions);
        }

        [HttpPost("{playerId}")]
        public IActionResult AddChampionToPlayer([FromQuery]int championId, int playerId)
        {
            if (!_playerRepository.AddChampionToPlayer(championId, playerId))
                return BadRequest("Something went wrong while trying to make the connection or save.");

            return Ok("Successfully made connection (?)");
        }
    }
}
