using System;
namespace CodingPlatform.Domain
{
    public class Subscription : BaseEntity
    {
        public User User { get; private set; }

        private Subscription() { }

        public Subscription(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            User = user;
        }
    }
}

