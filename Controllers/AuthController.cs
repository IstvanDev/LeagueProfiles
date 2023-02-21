using AutoMapper;
using LeagueProfiles.Dtos;
using LeagueProfiles.Helper;
using LeagueProfiles.Interfaces;
using LeagueProfiles.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace LeagueProfiles.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IPlayerRepository _playerRepository;

        public AuthController(IConfiguration configuration, IPlayerRepository playerRepository)
        {
            _configuration = configuration;
            _playerRepository = playerRepository;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetCurrentLoggedPlayer()
        {
            var userName = _playerRepository.GetPlayerNameTokenBased();
            return Ok(userName);
        }

        [HttpPost("register")]
        public ActionResult<Player> Register(LoginDto regRequest)
        {
            if (_playerRepository.PlayerExists(regRequest.Username))
                return BadRequest("Username taken.");

            CreatePasswordHash(regRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var player = new Player
            {
                Username = regRequest.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            if (!_playerRepository.CreatePlayer(player))
                return BadRequest("Failed to create player during registration.");

            return Ok(player);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginRequest)
        {
            if (!_playerRepository.PlayerExists(loginRequest.Username))
            {
                return BadRequest("User not found.");
            }

            var Player = await _playerRepository.GetPlayerByUserName(loginRequest.Username);

            if (!VerifyPasswordHash(loginRequest.Password, Player.PasswordHash, Player.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(Player);

            var refreshToken = GenerateRefreshToken();

            SetRefreshToken(refreshToken, ref Player);

            if (!_playerRepository.UpdatePlayer(Player))
                return BadRequest("Failed to update database.");

            return Ok(token);
        }

        [HttpPost("refresh-token"), Authorize]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var curPlayerName = _playerRepository.GetPlayerNameTokenBased();

            var currentPlayer = await _playerRepository.GetPlayerByUserName(curPlayerName);

            if (!currentPlayer.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (currentPlayer.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(currentPlayer);

            var newRefreshToken = GenerateRefreshToken();

            SetRefreshToken(newRefreshToken, ref currentPlayer);

            if (!_playerRepository.UpdatePlayer(currentPlayer))
                return BadRequest("Failed to update database.");

            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken, ref Player player)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            player.RefreshToken = newRefreshToken.Token;
            player.TokenCreated = newRefreshToken.Created;
            player.TokenExpires = newRefreshToken.Expires;
        }

        private string CreateToken(Player player)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, player.Username),
                new Claim(ClaimTypes.Role, player.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
