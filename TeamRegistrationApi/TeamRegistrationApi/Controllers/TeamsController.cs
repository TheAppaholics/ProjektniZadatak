using Microsoft.AspNetCore.Mvc;
using TeamRegistrationApi.Data;
using TeamRegistrationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TeamRegistrationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterTeam([FromBody] TeamRegistrationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Team name is required.");
            }

            var team = new Models.Team
            {
                Name = dto.Name,
                TournamentName = "Default Tournament",  
                TournamentDate = DateTime.Now,          
                Players = dto.Players
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return Ok(team);
        }


        // GET: api/teams
        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _context.Teams.Include(t => t.Players).ToListAsync();
            return Ok(teams);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Players.RemoveRange(team.Players); 
            _context.Teams.Remove(team);               
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] Models.Team updatedTeam)
        {
            var existingTeam = await _context.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == id);
            if (existingTeam == null)
            {
                return NotFound();
            }

            existingTeam.Name = updatedTeam.Name;

            _context.Players.RemoveRange(existingTeam.Players);

            existingTeam.Players = updatedTeam.Players;

            await _context.SaveChangesAsync();
            return Ok(existingTeam);
        }


    }
}
