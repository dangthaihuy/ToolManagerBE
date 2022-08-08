using System;

namespace VnCompany.DataLayer
{
    public class IdentityRole
    {
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        public IdentityRole(string name)
            : this()
        {
            Name = name;
        }

        public IdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }
        public int AgencyId { get; set; }
    }
}
