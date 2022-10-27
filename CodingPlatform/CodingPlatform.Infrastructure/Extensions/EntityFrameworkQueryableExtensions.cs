using System;
using CodingPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Extensions
{
    //TODO: non bellissimo
    public static class EntityFrameworkQueryableExtensions
    {
        public static IQueryable<Tournament> StandardInclude(this DbSet<Tournament> set)
        {
            return set
                .Include(t => t.Challenges)
                    .ThenInclude(c => c.Tips)
                .Include(t => t.SubscribedUser)
                    .ThenInclude(s => s.User)
                .Include(t => t.Admin)
                .Include(t => t.Submissions)
                    .ThenInclude(s => s.Admin)
                .Include(t => t.Submissions)
                    .ThenInclude(s => s.User)
                .Include(t => t.Submissions)
                    .ThenInclude(s => s.Challenge);
        }

        public static IQueryable<Submission> StandardInclude(this DbSet<Submission> set)
        {
            return set
                .Include(s => s.User)
                .Include(s => s.Admin)
                .Include(s => s.Challenge);
        }
    }
}

