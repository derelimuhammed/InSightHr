using Microsoft.AspNetCore.Http;

namespace MVC_ONION_PROJECT.APPLICATION.Services.AddImageServices
{
    public interface IAddImageService
    {
        Task<string?> AddImage(IFormFile File);
    }
}