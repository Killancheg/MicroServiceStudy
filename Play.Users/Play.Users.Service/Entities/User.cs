using Play.Common;
using System;

namespace Play.Users.Service.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public DateTimeOffset CreatedDate { get; set; };
    }
}
