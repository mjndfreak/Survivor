using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survivor.Data.Context;
using Survivor.Data.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly SurvivorDbContext _context;

        public CategoryController(SurvivorDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<CategoryEntity>> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();

            if (categories is null) return NotFound(); 

            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryEntity>> GetById([FromRoute(Name = "id")] int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (category is null) return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryEntity category)
        {
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryEntity>> Update([FromRoute(Name = "id")] int id, [FromBody] CategoryEntity category)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(c => c.Id.Equals(category.Id));

            if (entity is null) return BadRequest();

            entity.ModifiedDate = DateTime.UtcNow;
            entity.Name = category.Name;

            _context.Categories.Update(entity);

            await _context.SaveChangesAsync();

            return Ok(entity);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute(Name = "id")] int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (category is null) return NotFound();

            category.ModifiedDate = DateTime.UtcNow;
            category.IsDeleted = true;

            _context.Categories.Update(category);

            await _context.SaveChangesAsync();

            return Ok(category);
        }
    }
}