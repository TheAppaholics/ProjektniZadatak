﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamRegistrationApi.Models;
using TeamRegistrationApi.Data;

namespace TeamRegistrationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerStatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerStatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerStatistics>>> GetStats()
        {
            return await _context.PlayerStatistics
                .Include(s => s.Player)
                .Include(s => s.Match)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerStatistics>> GetStat(int id)
        {
            var stat = await _context.PlayerStatistics
                .Include(s => s.Player)
                .Include(s => s.Match)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (stat == null) return NotFound();
            return stat;
        }

        [HttpPost]
        public async Task<ActionResult<PlayerStatistics>> CreateStat(PlayerStatistics stat)
        {
            _context.PlayerStatistics.Add(stat);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStat), new { id = stat.Id }, stat);
        }
    }
}
