using Homework4._26.Data;
using Homework4._26.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace Homework4._26.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;

        private IWebHostEnvironment _environment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _environment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var ir = new ImageRepository(_connectionString);

            return View(new ImageListViewModel()
            {
                Images = ir.GetImages()
            });
        }
        public IActionResult ViewImage(int id)
        {
            var ir = new ImageRepository(_connectionString);
            var imagevm = new ImageViewModel()
            {
                Image = ir.GetImageById(id)
            };
            
                List<int> ids = HttpContext.Session.Get<List<int>>("liked");
                if (ids != null && ids.Contains(id))
                {
                    imagevm.IsEnabled = false;
                }
           

            return View(imagevm);
        }
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(Image image, IFormFile imageFile)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            using var stream = new FileStream(fullPath, FileMode.CreateNew);
            imageFile.CopyTo(stream);
            image.ImagePath = fileName;
            var ir = new ImageRepository(_connectionString);
            ir.AddImage(image);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public void IncrementLikes(int id)
        {
            if (id == 0)
            {
                return;
            }
            var ir = new ImageRepository(_connectionString);
            ir.IncrementLikes(id);
            
        }
        [HttpPost]
        public void SetSession(int id)
        {
            List<int> isLiked = HttpContext.Session.Get<List<int>>("liked") ?? new();
            isLiked.Add(id);
            HttpContext.Session.Set("liked", isLiked);
            ViewImage(id);
        }
        public IActionResult GetLikes(int id)
        {
            if (id == 0)
            {
                return Json(id);
            }
            var ir = new ImageRepository(_connectionString);
            return Json(ir.GetLikesById(id));
        }

    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }

    }
}