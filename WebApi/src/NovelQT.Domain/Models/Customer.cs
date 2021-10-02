using NovelQT.Domain.Core.Models;
using System;

namespace NovelQT.Domain.Models
{
    public class Customer : EntityTrackable
    {
        public Customer(Guid id, string name, string email, DateTime birthDate)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
        }

        // Empty constructor for EF
        protected Customer(System.Guid guid) { }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }
    }
}
