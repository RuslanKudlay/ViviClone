using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.BusinessServiceCommon;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Application.BBL.BusinessServices
{
    public class BlogService : IBlogService
    {
        private readonly IDbContextFactory _dbContextFactory;

        private readonly IModelMapper modelMapper;

        private readonly int BASE_COUNT_ROWS = 5;

        private readonly int BLOG_ID = 1;
        private readonly IPictureAttacherService pictureAttacherService;

        public BlogService(IDbContextFactory dbContextFactory, IModelMapper modelMapper, IPictureAttacherService pictureAttacherService)
        {
            _dbContextFactory = dbContextFactory;
            this.pictureAttacherService = pictureAttacherService;
            this.modelMapper = modelMapper;
        }

        public QueryModel<PostModel> GetPosts(QueryPostModel queryModel = null)
        {
            using (var context = _dbContextFactory.Create())
            {

                var query = context.Posts.Where(p => p.Blog.Id == BLOG_ID);

                queryModel.TotalCount = query.Count();

                if (!string.IsNullOrEmpty(queryModel.BodyContains))
                    query = query.Where(x => x.Body.Contains(queryModel.BodyContains));

                if (!string.IsNullOrEmpty(queryModel.TitleContains))
                    query = query.Where(x => x.Title.Contains(queryModel.TitleContains));

                if (!string.IsNullOrEmpty(queryModel.OrderBy))
                    query = query.OrderBy(x => queryModel.OrderBy);

                if (!string.IsNullOrEmpty(queryModel.OrderByDesc))
                    query = query.OrderByDescending(x => queryModel.OrderByDesc);

                query = query.Skip(queryModel.Skip ?? 0);

                //Uncomment when admin/Blog get pagination
                //query = query.Take(queryModel.Take ?? 10);

                queryModel.Result = query.ToList().Select(x => new PostModel
                {
                    Body = x.Body,
                    DateCreated = x.DateCreated,
                    ImageURL = x.ImageURL,
                    Id = x.Id,
                    IsEnable = x.IsEnable,
                    DateModificated = x.DateModificated,
                    MetaDescription = x.MetaDescription,
                    MetaKeywords = x.MetaKeywords,
                    SubUrl = x.SubUrl,
                    Title = x.Title
                }).ToList();

                return queryModel;
            }
        }

        public PostModel SaveOrUpdate(PostModel post)
        {
            using (var context = _dbContextFactory.Create())
            {
                Post postDb = post.Id > 0 ? context.Posts.FirstOrDefault(x => x.Id == post.Id) : new Post();

                postDb.Body = post.Body;
                postDb.DateCreated = post.DateCreated;
                postDb.DateModificated = post.DateModificated;
                postDb.ImageURL = post.ImageURL;
                postDb.IsEnable = post.IsEnable;
                postDb.MetaDescription = post.MetaDescription;
                postDb.MetaKeywords = post.MetaKeywords;
                postDb.Title = post.Title;
                postDb.SubUrl = post.SubUrl;

                postDb.BlogId = BLOG_ID;

                if (postDb.Id <= 0)
                {
                    if (context.Posts.FirstOrDefault(x => x.Title == post.Title) != null)
                        throw new Exception("There is the same post name");
                    if (context.Posts.FirstOrDefault(x => x.SubUrl == post.SubUrl) != null)
                        throw new Exception("There is the same post sub URL");

                    context.Posts.Add(postDb);
                }

                else
                {
                    if (context.Posts.FirstOrDefault(x => x.SubUrl == post.SubUrl && x.Id != postDb.Id) != null)
                        throw new Exception("There is the same post sub URL");
                }

                context.SaveChanges();
                return modelMapper.MapTo<Post, PostModel>(postDb);
            }
        }

        public PostModel GetPost(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var postEntity = context.Posts.FirstOrDefault(p => p.SubUrl == subUrl);
                var post = modelMapper.MapTo<Post, PostModel>(postEntity);

                // LogoImage is like '/api/getImage/97' string
                if (post.ImageURL != null)
                {
                    var imageId = int.Parse(post.ImageURL.Split('/')[3]);
                    var image = pictureAttacherService.GetPictureData(imageId);
                    var base64image = image != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image, 0, image.Length)) : "";
                    post.Base64Image = base64image;
                }

                return post;
            }
        }

        public PostModel GetPost(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var post = context.Posts.FirstOrDefault(p => p.Id == id);

                return modelMapper.MapTo<Post, PostModel>(post);
            }
        }

        public bool DeletePost(int id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var post = context.Posts.FirstOrDefault(p => p.Id == id);

                if (post == null)
                    throw new Exception("Post not found");

                context.Posts.Remove(post);

                context.SaveChanges();

                return true;
            }

        }
        public bool DeletePost(string subUrl)
        {
            using (var context = _dbContextFactory.Create())
            {
                var post = context.Posts.FirstOrDefault(p => p.SubUrl == subUrl);

                if (post == null)
                    throw new Exception("Post not found");

                context.Posts.Remove(post);

                context.SaveChanges();

                return true;
            }

        }

    }
}
