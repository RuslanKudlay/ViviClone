using Application.EntitiesModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IPictureAttacherService
    {
        byte[] GetPictureData(int id);

        object GetPictureWithDimension(int id);

        int AddPicture(byte[] picture);

        bool DeletePicture(int id);
    }
}
