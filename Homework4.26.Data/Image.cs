using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework4._26.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateUploaded{ get; set; }
        public string ImagePath { get; set; }
        public int Likes { get; set; }
    }
}
