using System;
using CodingPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Extensions
{
    public static class EntityFrameworkQueryableExtensions
    {
        public static IQueryable<Tournament> StandardInclude(this DbSet<Tournament> set)
        {
            //TODO: non bellissimo
            return set
                .Include(t => t.Challenges)
                    .ThenInclude(c => c.Tips)
                .Include(t => t.SubscribedUser)
                    .ThenInclude(s => s.User)
                .Include(t => t.Admin)
                .Include(t => t.Submissions)
                    .ThenInclude(s => s.Admin)
                .Include(t => t.Submissions)
                    .ThenInclude(s => s.User);
        }
    }
}

