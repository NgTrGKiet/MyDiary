﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyDiary.Infrastructure.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "cac43a6e-f7bb-4448-baaf-1add431ccbbf",
                Name = "User",
                NormalizedName = "USER"
            },
            new IdentityRole
            {
                Id = "cbc43a8e-f7bb-4445-baaf-1add431ffbbf",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            }
        );
    }
}