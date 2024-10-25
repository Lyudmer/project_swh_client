﻿using ClientSWH.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ClientSWH.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);
            builder
                .HasMany(p => p.Packages)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Property(u => u.Id)
              .HasColumnType("uuid")
              .ValueGeneratedOnAdd()
              .IsRequired();
            builder.Property(u => u.UserName)
                .IsRequired();
            builder.Property(u => u.PasswordHash)
                .IsRequired();
            builder.Property(u => u.Email)
                .IsRequired();

        }
    }
}
