using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly IEmailHelper _emailHelper;

        public HomeController(IEmailHelper emailHelper)
        {
            _emailHelper = emailHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Gonder(string ad, string email, string subject, string comments)
        {
            string head = "İletişim Formundan Yeni Bir Mesajınız Var!";
            string content = $"Konu: {subject} <br>Email Adresi: {email} <br>Gönderen Kişi: {ad} <br>Mesajı: {comments}";
            string mailadresi = "info@insight-hr.online";
            var isSend= await _emailHelper.SendContactFormMessageAsync(head, content, mailadresi);
            if (!isSend)
            {
                //Task.Delay(3500).Wait();
                //return RedirectToAction("Index");
                //return Json(new { success = true });

                var resultFail = new
                {
                    success = false,
                    redirectAction = "/Index" // Yönlendirme yapılacak action sayfasının adı
                };

                // JSON verisini ve yönlendirme action bilgisini döndürme
                return Json(resultFail);
            }
            var sonuc = new
            {
                success = true,
                redirectAction = "/Index" // Yönlendirme yapılacak action sayfasının adı
            };

            // JSON verisini ve yönlendirme action bilgisini döndürme
            return Json(sonuc);
        }

            // GET: HomeController
            public ActionResult Index()
        {
            return View();
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
 

    }
}
