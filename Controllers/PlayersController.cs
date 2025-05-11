using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballStatsAPI.Data;
using FootballStatsAPI.Models;

namespace FootballStatsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly FootballContext _context;

    public PlayersController(FootballContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        return await _context.Players.Include(p => p.Statistics).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(int id)
    {
        var player = await _context.Players.Include(p => p.Statistics).FirstOrDefaultAsync(p => p.Id == id);
        if (player == null) return NotFound();
        return player;
    }

    [HttpPost]
    public async Task<ActionResult<Player>> CreatePlayer(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
    }
}
