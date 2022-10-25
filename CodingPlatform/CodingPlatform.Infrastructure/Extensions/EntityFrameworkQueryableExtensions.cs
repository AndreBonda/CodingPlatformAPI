using System;
using CodingPlatform.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodingPlatform.Infrastructure.Extensions
{
    public static class EntityFrameworkQueryableExtensions
    {
        public static IQueryable<Tournament> StandardInclude(this DbSet<Tournament> set)
        {
            return set.Include(t => t.SubscribedUser)
                .ThenInclude(s => s.User)
                .Include(t => t.Admin)
                .Include(t => t.Submissions);
        }
    }
}

