using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survivor.Data.Context;
using Survivor.Data.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitorController : ControllerBase
    {
        private readonly SurvivorDbContext _context;

        public CompetitorController(SurvivorDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompetitorEntity>>> GetAllCompetitors()
        {
            var competitors = await _context.Competitors.ToListAsync();

            if (competitors is null) return NotFound();

            return Ok(competitors);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompetitorEntity>> GetById([FromRoute(Name = "id")] int id)
        {
            var competitor = await _context.Competitors.FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (competitor is null) return NotFound();

            return Ok(competitor);
        }

        [HttpGet("categories/{categoryId:int}")]
        public async Task<ActionResult<List<CompetitorEntity>>> GetAllByCategoryId([FromRoute(Name = "categoryId")] int categoryId)
        {
            var competitors = await _context.Competitors.Where(c => c.CategoryId.Equals(categoryId)).ToListAsync();

            if (competitors is null) return NotFound();

            return Ok(competitors);
        }

        [HttpPost]
        public async Task<ActionResult<CompetitorEntity>> Create([FromBody] CompetitorEntity competitor)
        {
            var newCompetitor = new CompetitorEntity
            {
                FirstName = competitor.FirstName, 
                LastName = competitor.LastName, 
                CategoryId = competitor.CategoryId
            };

            _context.Competitors.Add(newCompetitor);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = competitor.Id }, competitor);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CompetitorEntity>> Update([FromRoute(Name = "id")] int id, [FromBody] CompetitorEntity competitor)
        {
            var entity = await _context.Competitors.FirstOrDefaultAsync(c => c.Id.Equals(competitor.Id));

            if (entity is null) return BadRequest();

            entity.ModifiedDate = DateTime.UtcNow;
            entity.FirstName = competitor.FirstName;
            entity.LastName = competitor.LastName;
            entity.CategoryId = competitor.CategoryId;

            _context.Competitors.Update(entity);

            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute(Name = "id")] int id)
        {
            var competitor = await _context.Competitors.FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (competitor is null) return NotFound();

            competitor.ModifiedDate = DateTime.UtcNow;
            competitor.IsDeleted = true;

            _context.Competitors.Update(competitor);

            await _context.SaveChangesAsync();

            return Ok(competitor);
        }
    }

}