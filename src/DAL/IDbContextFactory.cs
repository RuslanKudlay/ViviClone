using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DAL
{
    public interface IDbContextFactory
    {
        IApplicationDbContext Create();
    }
}
