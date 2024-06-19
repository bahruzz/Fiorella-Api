using Api_Fiorella_HomeTask.Data;
using Api_Fiorella_HomeTask.DTOs.Categories;
using Api_Fiorella_HomeTask.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Api_Fiorella_HomeTask.Controllers
{
   
    public class CategoryController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _context.Categories.AsNoTracking().ToListAsync();

            var mappedDatas = _mapper.Map<List<CategoryDto>>(response);
        
            return Ok(mappedDatas);
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CategoryCreateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _context.Categories.AddAsync(_mapper.Map<Category>(request));
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] CategoryEditDto request)
        {
            var entity = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();
            _mapper.Map(request, entity);
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var entity = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<CategoryDto>(entity));
        }

        [HttpDelete]

        public async Task<IActionResult> Delete([FromQuery] int? id)
        {
            if (id == null) return BadRequest();
            var entity = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? str)
        {
            return Ok(str == null ? await _context.Categories.ToListAsync() : await _context.Categories.Where(m => m.Name.Contains(str)).ToListAsync());
        }
    }
}
