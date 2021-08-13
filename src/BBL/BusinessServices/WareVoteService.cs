using Application.BBLInterfaces.BusinessServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Application.EntitiesModels.Models;
using Application.DAL;
using System.Linq;

namespace Application.BBL.BusinessServices
{
    class WareVoteService : IWareVoteService
    {
        private readonly IDbContextFactory _dbContextFactory;

        public WareVoteService(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public double GetAverageRating(int wareId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var wareVotes = context.WareVotes.Where(wv => wv.WareId == wareId);

                var wareVotesQuantity = wareVotes.Count() == 0 ? 0 : Math.Round((double)wareVotes.Select(r => r.Rate).Sum() / (double)wareVotes.Count(), 2);

                return wareVotesQuantity;
            }

        }
    }
}
