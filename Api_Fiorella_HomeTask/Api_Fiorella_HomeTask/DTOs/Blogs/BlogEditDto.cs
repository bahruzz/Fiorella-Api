namespace Api_Fiorella_HomeTask.DTOs.Blogs
{
    public class BlogEditDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
       
        public IFormFile Images { get; set; }
    }
}
