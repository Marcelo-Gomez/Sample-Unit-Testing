using System;

namespace Sample_Unit_Testing.Application.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool Active { get; set; }

        public DateTime CreationDate { get; set; }
    }
}