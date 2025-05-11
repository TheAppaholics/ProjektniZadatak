using System;
using LeaderboardAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaderboardAPI.Controllers
{
    [ApiController] // Označava da je klasa Web API kontroler
    [Route("api/[controller]")] // Definišem osnovni dio URL-a za ovaj kontroler
    public class LeaderboardController : ControllerBase
    {
        private readonly AppDbContext _context; // Veza prema bazi

        // Konstruktor u kojem se ubacuje AppDbContext
        public LeaderboardController(AppDbContext context)
        {
            _context = context; // Dodjeljujem proslijeđeni context ovoj klasi
        }

        // POST metoda za dodavanje rezultata u bazu
        [HttpPost("update")]
        public IActionResult Update([FromBody] List<MatchResult> results)
        {
            _context.MatchResults.AddRange(results); // Dodajem sve rezultate iz zahtjeva
            _context.SaveChanges(); // Spremam izmjene u bazi
            return Ok("Saved"); // Vraćam potvrdu
        }

        // GET metoda za prikaz svih rezultata iz baze
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            return Ok(_context.MatchResults.ToList()); // Vraćam sve rezultate iz baze
        }

        // GET metoda koja vraća tabelu timova sa obračunatim statistikama
        [HttpGet("standings")]
        public async Task<ActionResult<IEnumerable<LeaderboardEntry>>> GetLeaderboard()
        {
            var results = await _context.MatchResults.ToListAsync(); // Uzimam sve mečeve iz baze

            var leaderboard = new Dictionary<string, LeaderboardEntry>(); // Kreiram praznu tabelu za timove

            // Prolazim kroz sve mečeve i obrađujem statistike
            foreach (var match in results)
            {
                // Ako tim još nije u tabeli — dodajem ga
                if (!leaderboard.ContainsKey(match.TeamA))
                    leaderboard[match.TeamA] = new LeaderboardEntry { TeamName = match.TeamA };

                if (!leaderboard.ContainsKey(match.TeamB))
                    leaderboard[match.TeamB] = new LeaderboardEntry { TeamName = match.TeamB };

                var teamA = leaderboard[match.TeamA];
                var teamB = leaderboard[match.TeamB];

                // Povećavam broj odigranih utakmica
                teamA.GamesPlayed++;
                teamB.GamesPlayed++;

                // Ažuriram broj postignutih i primljenih golova
                teamA.GoalsFor += match.ScoreA;
                teamA.GoalsAgainst += match.ScoreB;

                teamB.GoalsFor += match.ScoreB;
                teamB.GoalsAgainst += match.ScoreA;

                // Provjeravam ishod meča i ažuriram bodove, pobjede, poraze, remije
                if (match.ScoreA > match.ScoreB)
                {
                    teamA.Wins++;
                    teamA.Points += 3;

                    teamB.Losses++;
                }
                else if (match.ScoreA < match.ScoreB)
                {
                    teamB.Wins++;
                    teamB.Points += 3;

                    teamA.Losses++;
                }
                else
                {
                    teamA.Draws++;
                    teamB.Draws++;

                    teamA.Points += 1;
                    teamB.Points += 1;
                }
            }

            // Sortiram tabelu: prvo po bodovima, pa po gol-razlici, pa po nazivu tima
            var sorted = leaderboard.Values
                .OrderByDescending(e => e.Points)
                .ThenByDescending(e => e.GoalsFor - e.GoalsAgainst)
                .ThenBy(e => e.TeamName)
                .ToList();

            return Ok(sorted); // Vraćam sortiranu tabelu
        }
    }
}
