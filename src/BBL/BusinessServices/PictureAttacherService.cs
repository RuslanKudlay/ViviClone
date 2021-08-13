using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Application.BBL.BusinessServices
{
    public class PictureAttacherService : IPictureAttacherService
    {
        private readonly IDbContextFactory _dbContextFactory;

        public PictureAttacherService(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public int AddPicture(byte[] picture)
        {
            using (var context = _dbContextFactory.Create())
            {
                var image = new Image()
                {
                    Data = picture
                };
                context.Images.Add(image);
                context.SaveChanges();

                return image.Id;
            }
        }

        public bool DeletePicture(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var image = context.Images.FirstOrDefault(i => i.Id == id);
                if (image == null)
                    throw new Exception("Image was not found ");

                context.Images.Remove(image);

                context.SaveChanges();

                return true;
            }
        }

        public byte[] GetPictureData(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var image = context.Images.Where(i => i.Id == id).FirstOrDefault();

                if (image == null)
                    throw new Exception("Image was not found");

                return image.Data;
            }
        }

        public object GetPictureWithDimension(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var image = context.Images.Where(i => i.Id == id).FirstOrDefault();

                var data = image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image.Data, 0, image.Data.Length)) : "";

                int width, height;

                if (image == null)
                    throw new Exception("Image was not found");

                using (var ms = new MemoryStream(image.Data))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    width = img.Width;
                    height = img.Height;
                }

                return new { data, size = new { width, height} };
            }
        }

    }
}
