using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Module6HW1.DB;
using Module6HW1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Module6HW1.Controllers
{
    [Route("api")]
    [ApiController]
    public class TeapotController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public TeapotController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("teapots")]
        public async Task<IActionResult> GetTeapots()
        {
            var teapots = await _dbContext.Teapots.ToListAsync();

            if (teapots.Capacity == 0)
            {
                return NotFound(new { ErrorMessage = "Db is empty!" });
            }

            return Ok(teapots);
        }

        [HttpGet("teapots/{id}")]
        public async Task<IActionResult> GetTeapotById([FromRoute] int id)
        {
            var teapot = await _dbContext.Teapots.FirstOrDefaultAsync(x => x.Id == id);

            if (teapot == null)
            {
                return NotFound(new {ErrorMessage = "Teapot with this id is absent!"});
            }

            return Ok(teapot);
        }

        [HttpPost("teapots")]
        public async Task<IActionResult> AddTeapot([FromBody] Teapot teapotFromBody)
        {
            var teapot = _dbContext.Teapots.FirstOrDefault(x => x.Id == teapotFromBody.Id);

            if (teapot != null)
            {
                return NotFound(new {ErrorMessage = "Teapot with this id exists!" });
            }

            await _dbContext.Teapots.AddAsync(teapotFromBody);
            await _dbContext.SaveChangesAsync();

            return Ok(new {SuccessMessage = "Teapot has been added!"});
        }


        [HttpPut("teapots")]
        public async Task<IActionResult> EditTeapotById([FromBody] Teapot teapotFromBody)
        {
            var teapotToEdit = _dbContext.Teapots.FirstOrDefault(x => x.Id == teapotFromBody.Id);

            if (teapotToEdit == null)
            {
                return NotFound(new { NotFound = "Teapot with this id is absent!" });
            }

            teapotToEdit.Title = teapotFromBody.Title;
            teapotToEdit.Price = teapotFromBody.Price;
            teapotToEdit.Description = teapotFromBody.Description;
            teapotToEdit.Capacity = teapotFromBody.Capacity;
            teapotToEdit.Quantity = teapotFromBody.Quantity;
            teapotToEdit.ImgUrl = teapotFromBody.ImgUrl;
            teapotToEdit.ManufacturerCountry = teapotFromBody.ManufacturerCountry;
            teapotToEdit.WarrantyInMonthes = teapotFromBody.WarrantyInMonthes;

            await _dbContext.SaveChangesAsync();

            return Ok(new { SuccessMessage = "The teapot has been changed!" });
        }

        [HttpDelete("teapots/{id}")]
        public async Task<IActionResult> DeleteTeapotById([FromRoute] int id)
        {
            var teapotToDelete = _dbContext.Teapots.FirstOrDefault(x => x.Id == id);

            if (teapotToDelete == null)
            {
                return NotFound(new { ErrorMessage = "Teapot with this id is absent!" });
            }

            _dbContext.Teapots.Remove(teapotToDelete);
            await _dbContext.SaveChangesAsync();

            return Ok(new { SuccessMessage = "The teapot has been removed!" });
        }
    }
}
