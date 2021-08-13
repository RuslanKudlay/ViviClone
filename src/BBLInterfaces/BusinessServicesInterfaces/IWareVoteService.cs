using Application.EntitiesModels.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IWareVoteService
    {
        double GetAverageRating(int wareId);
    }
}
