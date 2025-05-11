using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamRegistrationApi.Models;
using TeamRegistrationApi.Data;

namespace TeamRegistrationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MatchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Match>>> GetMatches()
        {
            var matches = await _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .ToListAsync();
            return Ok(matches);
        }

        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleMatches()
        {
            var teams = await _context.Teams.ToListAsync();

            if (teams.Count < 2)
            {
                return BadRequest("Nedovoljno timova za zakazivanje utakmica.");
            }

            var matches = new List<Match>();
            var random = new Random();
            var shuffledTeams = teams.OrderBy(t => random.Next()).ToList();

            for (int i = 0; i < shuffledTeams.Count - 1; i += 2)
            {
                var match = new Match
                {
                    HomeTeamId = shuffledTeams[i].Id,
                    AwayTeamId = shuffledTeams[i + 1].Id,
                    MatchDate = DateTime.Now.AddDays(i),
                    Round = "Round of " + teams.Count
                };
                matches.Add(match);
            }

            await _context.Matches.AddRangeAsync(matches);
            await _context.SaveChangesAsync();

            return Ok(matches);
        }

        [HttpPost("update-bracket")]
        public async Task<IActionResult> UpdateBracket()
        {
            var completedMatches = await _context.Matches
                .Where(m => m.HomeScore.HasValue && m.AwayScore.HasValue)
                .ToListAsync();

            var winners = new List<Team>();

            foreach (var match in completedMatches)
            {
                if (match.HomeScore > match.AwayScore)
                {
                    var homeTeam = await _context.Teams.FindAsync(match.HomeTeamId);
                    if (homeTeam != null) winners.Add(homeTeam);
                }
                else
                {
                    var awayTeam = await _context.Teams.FindAsync(match.AwayTeamId);
                    if (awayTeam != null) winners.Add(awayTeam);
                }
            }

            if (winners.Count < 2)
            {
                return Ok("Turnir je završen.");
            }

            var newMatches = new List<Match>();
            var random = new Random();
            var shuffledWinners = winners.OrderBy(t => random.Next()).ToList();

            for (int i = 0; i < shuffledWinners.Count - 1; i += 2)
            {
                var match = new Match
                {
                    HomeTeamId = shuffledWinners[i].Id,
                    AwayTeamId = shuffledWinners[i + 1].Id,
                    MatchDate = DateTime.Now.AddDays(i),
                    Round = "Next Round"
                };
                newMatches.Add(match);
            }

            await _context.Matches.AddRangeAsync(newMatches);
            await _context.SaveChangesAsync();

            return Ok(newMatches);
        }
    }
}
