namespace VdbAPI.Models
{
    public class UploadImagesDTO
    {
        public IFormFile Thumbnail { get; set; }
        public IFormFile Background { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
