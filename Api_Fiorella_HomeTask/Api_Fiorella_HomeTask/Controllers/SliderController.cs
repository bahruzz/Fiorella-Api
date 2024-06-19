using Api_Fiorella_HomeTask.Data;
using Api_Fiorella_HomeTask.DTOs.Blogs;
using Api_Fiorella_HomeTask.DTOs.Sliders;
using Api_Fiorella_HomeTask.Helpers.Extensions;
using Api_Fiorella_HomeTask.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Fiorella_HomeTask.Controllers
{
    
    public class SliderController :BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SliderCreateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            foreach (var item in request.Images)
            {
                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "Input can accept only image format");
                    return BadRequest();
                }

                if (!item.CheckFileSize(200))
                {
                    ModelState.AddModelError("Image", "Image size must be max 200 KB");
                    return BadRequest();
                }

            }
            foreach (var item in request.Images)
            {
                string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;

                string path = _env.GenerateFilePath("img", fileName);

                await item.SaveFileToLocalAsync(path);
                request.Image = fileName;
            }
 
            await _context.Sliders.AddAsync(_mapper.Map<Slider>(request));
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Create), request);
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var response = await _context.Sliders.AsNoTracking().ToListAsync();

            var mappedDatas = _mapper.Map<List<SliderDto>>(response);

            return Ok(mappedDatas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var entity = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<SliderDto>(entity));
        }

        [HttpDelete]

        public async Task<IActionResult> Delete([FromQuery] int? id)
        {
            if (id == null) return BadRequest();
            var entity = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();
            string path = _env.GenerateFilePath("img", entity.Image);
            path.DeleteFileFromLocal();
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] SliderEditDto request)
        {
            if (id == null) return BadRequest();
            var entity = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return NotFound();


            if (request.NewImage is not null)
            {
                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "Input can accept only image format");
                    request.Image = entity.Image;
                    return BadRequest();
                }

                if (!request.NewImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 200 KB");
                    request.Image = entity.Image;
                    return BadRequest();
                }
            }
            if (request.NewImage is not null)
            {
                string oldPath = _env.GenerateFilePath("img", entity.Image);

                oldPath.DeleteFileFromLocal();

                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;

                string newPath = _env.GenerateFilePath("img", fileName);

                await request.NewImage.SaveFileToLocalAsync(newPath);

                request.Image = fileName;
            }

            _mapper.Map(request, entity);
            _context.Sliders.Update(entity);
            await _context.SaveChangesAsync();
            return Ok();

        }


    }
}
