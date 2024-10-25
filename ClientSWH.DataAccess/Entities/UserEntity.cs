﻿
namespace ClientSWH.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Hidden { get; set; }
        public ICollection<PackageEntity> Packages { get; set; } = [];

    }
}
