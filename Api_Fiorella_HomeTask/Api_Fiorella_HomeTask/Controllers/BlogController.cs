using Api_Fiorella_HomeTask.Data;
using Api_Fiorella_HomeTask.DTOs.Blogs;
using Api_Fiorella_HomeTask.DTOs.Categories;
using Api_Fiorella_HomeTask.Helpers.Extensions;
using Api_Fiorella_HomeTask.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace Api_Fiorella_HomeTask.Controllers
{

    public class BlogController : BaseController
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BlogCreateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!request.Images.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input can accept only image format");
                return BadRequest();
            }

            if (!request.Images.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be max 200 KB");
                return BadRequest();
            }
            string fileName = Guid.NewGuid().ToString() + "-" + request.Images.FileName;

            string path = _env.GenerateFilePath("img", fileName);

            await request.Images.SaveFileToLocalAsync(path);
            request.Image = fileName;

            await _context.Blogs.AddAsync(_mapper.Map<Blog>(request));
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), request);
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var response = await _context.Blogs.AsNoTracking().ToListAsync();

            var mappedDatas = _mapper.Map<List<BlogDto>>(response);

            return Ok(mappedDatas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var entity = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<BlogDto>(entity));
        }

        [HttpDelete]

        public async Task<IActionResult> Delete([FromQuery] int? id)
        {
            if (id == null) return BadRequest();
            var entity = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();
            string path = _env.GenerateFilePath("img", entity.Image);
            path.DeleteFileFromLocal();
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? str)
        {
            return Ok(str == null ? await _context.Blogs.ToListAsync() : await _context.Blogs.Where(m => m.Title.Contains(str)).ToListAsync());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] BlogEditDto request)
        {
            if (id == null) return BadRequest();
            var entity = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();


            if (request.Images is not null)
            {
                if (!request.Images.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "Input can accept only image format");
                    request.Image = entity.Image;
                    return BadRequest();
                }

                if (!request.Images.CheckFileSize(200))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 200 KB");
                    request.Image = entity.Image;
                    return BadRequest();
                }
            }
            if (request.Images is not null)
            {
                string oldPath = _env.GenerateFilePath("img", entity.Image);

                oldPath.DeleteFileFromLocal();

                string fileName = Guid.NewGuid().ToString() + "-" + request.Images.FileName;

                string newPath = _env.GenerateFilePath("img", fileName);

                await request.Images.SaveFileToLocalAsync(newPath);

                request.Image = fileName;
            }

            _mapper.Map(request, entity);
            _context.Blogs.Update(entity);
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
