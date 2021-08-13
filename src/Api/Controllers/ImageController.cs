using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.EntitiesModels.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Api.Controllers
{
    public class ImageController : ApplicationApiController
    {
        private IPictureAttacherService _pictureAttacherService;

        public ImageController(IPictureAttacherService pictureAttacherService, UserManager<ApplicationUser> manager) : base(manager)
        {
            _pictureAttacherService = pictureAttacherService;
        }

        [HttpPost]
        [Route("api/uploadImage")]
        public JsonResult UploadImage(IFormFile file)
        {
            try
            {
                if (file == null) throw new Exception("File is null");
                if (file.Length == 0) throw new Exception("File is empty");

                byte[] fileData = null;
                using (var memoryStream = file.OpenReadStream())
                {
                    using (var binaryReader = new BinaryReader(memoryStream))
                    {
                        fileData = binaryReader.ReadBytes((int)file.Length);                       
                    }                  
                }
                var result = _pictureAttacherService.AddPicture(fileData);

                if (result <= 0)
                    throw new Exception("Image was not found");


                return new JsonResult(new { link  = string.Format("/api/getImage/" + result) });

            }
            catch (Exception e)
            {
                throw new Exception("InternalServerError" + e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/getImage/{id:int}")]
        public IActionResult GetImage(int id)
        {
            var image = _pictureAttacherService.GetPictureData(id);

            var result = image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image, 0, image.Length)) : "";

            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/getImage/{id:int}/Dimension")]
        public IActionResult GetImageWithDimension(int id)
        {
            return InvokeMethod(_pictureAttacherService.GetPictureWithDimension, id);
        }

        [HttpDelete]
        [Route("api/deleteImage/{id:int}")]
        public IActionResult DeleteImage(int id)
        {
            return InvokeMethod(_pictureAttacherService.DeletePicture, id);           
        }
    }
}
