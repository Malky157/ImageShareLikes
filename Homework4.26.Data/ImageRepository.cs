using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework4._26.Data
{
    public class ImageRepository
    {
        private readonly string _connectionString;
        public ImageRepository(string connection) 
        {
            _connectionString = connection;
        }

        public List<Image> GetImages()
        {
            using var context = new ImageDbContext(_connectionString);
            return context.Images.OrderByDescending(i => i.DateUploaded).ToList();
        }
        public void AddImage(Image image)
        {
            using var context = new ImageDbContext(_connectionString);
            context.Images.Add(image);
            context.SaveChanges();
        }
        public Image GetImageById(int id)
        {
            using var context = new ImageDbContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }
        public int GetLikesById(int id)
        {
            using var context = new ImageDbContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.Id == id).Likes;
        }
        public void IncrementLikes(int id)
        {
            
            using var context = new ImageDbContext(_connectionString);
            Image image = GetImageById(id);
            if (image != null)
            {
                image.Likes++;
                context.Images.Attach(image);
                context.Entry(image).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
