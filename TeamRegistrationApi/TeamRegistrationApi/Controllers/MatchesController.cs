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
        public async Task<ActionResult<List<MatchDto>>> GetMatches()
        {
            var matches = await _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.RoundResults)
                .ToListAsync();

            var matchDtos = matches.Select(m => new MatchDto
            {
                MatchId = m.MatchId,
                HomeTeamName = m.HomeTeam.Name,
                AwayTeamName = m.AwayTeam.Name,
                MatchDate = m.MatchDate,
                Round = m.Round,
                HomeScore = m.RoundResults.Sum(r => r.HomeScore ?? 0),
                AwayScore = m.RoundResults.Sum(r => r.AwayScore ?? 0),
                RoundResults = m.RoundResults.Select(r => new MatchRoundResultDto
                {
                    RoundNumber = r.RoundNumber,
                    HomeScore = r.HomeScore,
                    AwayScore = r.AwayScore
                }).ToList()
            }).ToList();

            return Ok(matchDtos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<MatchDto>> GetMatch(int id)
        {
            var match = await _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.RoundResults)
                .FirstOrDefaultAsync(m => m.MatchId == id);

            if (match == null)
                return NotFound();

            var matchDto = new MatchDto
            {
                MatchId = match.MatchId,
                HomeTeamName = match.HomeTeam.Name,
                AwayTeamName = match.AwayTeam.Name,
                MatchDate = match.MatchDate,
                Round = match.Round,
                HomeScore = match.RoundResults.Sum(r => r.HomeScore ?? 0),
                AwayScore = match.RoundResults.Sum(r => r.AwayScore ?? 0),
                RoundResults = match.RoundResults.Select(r => new MatchRoundResultDto
                {
                    RoundNumber = r.RoundNumber,
                    HomeScore = r.HomeScore,
                    AwayScore = r.AwayScore
                }).ToList()
            };

            return Ok(matchDto);
        }


        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleMatches()
        {
            var teams = await _context.Teams.ToListAsync();

            if (teams.Count < 2)
            {
                return BadRequest("Not enough teams to schedule matches.");
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

            var winners = new List<Models.Team>();

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("schedule-single")]
        public async Task<IActionResult> ScheduleSingleMatch([FromBody] MatchDto dto)
        {
            if (dto.HomeTeamName == dto.AwayTeamName)
                return BadRequest("Teams cannot be the same.");

            var homeTeam = await _context.Teams.FirstOrDefaultAsync(t => t.Name == dto.HomeTeamName);
            var awayTeam = await _context.Teams.FirstOrDefaultAsync(t => t.Name == dto.AwayTeamName);

            if (homeTeam == null || awayTeam == null)
                return BadRequest("One of the teams does not exist.");

            var match = new Match
            {
                HomeTeamId = homeTeam.Id,
                AwayTeamId = awayTeam.Id,
                MatchDate = dto.MatchDate,
                Round = dto.Round ?? "Scheduled"
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{matchId}/rounds")]
        public async Task<IActionResult> AddOrUpdateRoundResult(int matchId, [FromBody] MatchRoundResultDto dto)
        {
            var match = await _context.Matches
                .Include(m => m.RoundResults)
                .FirstOrDefaultAsync(m => m.MatchId == matchId);

            if (match == null)
                return NotFound("Match not found.");

            var existingRoundResult = match.RoundResults.FirstOrDefault(r => r.RoundNumber == dto.RoundNumber);

            if (existingRoundResult != null)
            {
                existingRoundResult.HomeScore = dto.HomeScore;
                existingRoundResult.AwayScore = dto.AwayScore;
            }
            else
            {
                var roundResult = new MatchRoundResult
                {
                    MatchId = matchId,
                    RoundNumber = dto.RoundNumber,
                    HomeScore = dto.HomeScore,
                    AwayScore = dto.AwayScore
                };
                match.RoundResults.Add(roundResult);
            }

            await _context.SaveChangesAsync();
            return Ok(match.RoundResults);
        }
    }
}
