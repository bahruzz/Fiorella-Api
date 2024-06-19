namespace Api_Fiorella_HomeTask.DTOs.Blogs
{
    public class BlogCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        
        public IFormFile Images { get; set; }
    }
}
