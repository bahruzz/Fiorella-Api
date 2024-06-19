using Api_Fiorella_HomeTask.Data;
using Api_Fiorella_HomeTask.DTOs.Products;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Fiorella_HomeTask.Controllers
{
    public class ProductController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _context.Products.AsNoTracking().Include(m => m.ProductImages).Include(m => m.Category).ToListAsync();

            var mappedDatas = _mapper.Map<List<ProductDto>>(response);
           
            return Ok(mappedDatas);
        }
    }
}
