namespace Api_Fiorella_HomeTask.DTOs.Sliders
{
    public class SliderCreateDto
    {
        public string? Image { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
