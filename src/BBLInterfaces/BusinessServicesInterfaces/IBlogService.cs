using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using System.Collections.Generic;



namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IBlogService
    {
        QueryModel<PostModel> GetPosts(QueryPostModel queryModel);

        PostModel GetPost(string subUrl);

        PostModel GetPost(int id);

        PostModel SaveOrUpdate(PostModel post);

        bool DeletePost(int id);

        bool DeletePost(string subUrl);


    }
}
