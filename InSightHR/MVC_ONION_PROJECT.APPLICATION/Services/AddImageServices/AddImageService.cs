using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.AddImageServices
{
    public class AddImageService:IAddImageService
    {
       public async Task<string?> AddImage(IFormFile File)
        {
            if (File != null && File.Length > 0)
            {
                // Dosya adını ve uzantısını alın
                var dosyaAdi = Path.GetFileName(File.FileName);

                // Dosyayı sunucu üzerine kaydedin
                var yol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", dosyaAdi);

                using (var stream = new FileStream(yol, FileMode.Create))
                {
                    await File.CopyToAsync(stream);
                }
                return  yol;
            }
            return null;
        }
    }
}
