using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.Property(language => language.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(language => language.Code)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.HasIndex(language => language.Code)
                   .IsUnique();
        }
    }
}
