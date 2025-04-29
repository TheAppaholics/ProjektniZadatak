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

        // POST: api/teams/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterTeam([FromBody] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
                return Ok(team);
            }
            return BadRequest("Invalid data.");
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

            _context.Players.RemoveRange(team.Players); // prvo brišemo igrače
            _context.Teams.Remove(team);                // zatim tim
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] Team updatedTeam)
        {
            var existingTeam = await _context.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == id);
            if (existingTeam == null)
            {
                return NotFound();
            }

            existingTeam.Name = updatedTeam.Name;

            // Obriši stare igrače
            _context.Players.RemoveRange(existingTeam.Players);

            // Dodaj nove
            existingTeam.Players = updatedTeam.Players;

            await _context.SaveChangesAsync();
            return Ok(existingTeam);
        }


    }
}
