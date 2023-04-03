namespace Bloggie.Web.Repositories
{
    public interface IImageRepository
    {
        Task<String> UploadAsync(IFormFile file);
    }
}
