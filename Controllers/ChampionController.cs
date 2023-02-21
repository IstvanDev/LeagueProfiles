using AutoMapper;
using LeagueProfiles.Dtos;
using LeagueProfiles.Interfaces;
using LeagueProfiles.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeagueProfiles.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ChampionController : Controller
    {
        private readonly IChampionRepository _championRepository;
        private readonly IMapper _mapper;

        public ChampionController(IChampionRepository championRepository, IMapper mapper)
        {
            _championRepository = championRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<ICollection<Champion>> GetChampions()
        {
            var champions = _championRepository.GetChampions();

            return Ok(champions);
        }

        [HttpGet("{championId}")]
        public async Task<ActionResult<Champion>> GetChampion(int championId)
        {
            if (!_championRepository.ChampionExists(championId))
                return NotFound("No chamption exists with that ID.");

            var champion = await _championRepository.GetChampionById(championId);

            return Ok(champion);
        }

        [HttpGet("{championId}/Players")]
        public ActionResult<ICollection<Player>> GetChampionPlayers(int championId)
        {
            if (!_championRepository.ChampionExists(championId))
                return NotFound("No chamption exists with that ID.");

            var players = _championRepository.GetChampionPlayers(championId);

            return Ok(players);
        }

        [HttpPost]
        public ActionResult<string> CreateChampion(ChampionDto championDto)
        {
            var champion = _mapper.Map<Champion>(championDto);

            if(!_championRepository.CreateChampion(champion))
                return BadRequest("Something went wrong while trying to put the object into the database.");

            return Ok("Created the following champion object: " + champion.ToString());
        }
    }
}
