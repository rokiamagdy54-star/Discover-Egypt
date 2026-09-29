using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class GuideLanguageConfiguration : IEntityTypeConfiguration<GuideLanguage>
    {
        public void Configure(EntityTypeBuilder<GuideLanguage> builder)
        {
            builder.HasKey(guideLanguage => new { guideLanguage.GuideId, guideLanguage.LanguageId });

            builder.HasOne(guideLanguage => guideLanguage.Guide)
                   .WithMany(guide => guide.GuideLanguages)
                   .HasForeignKey(guideLanguage => guideLanguage.GuideId);

            builder.HasOne(guideLanguage => guideLanguage.Language)
                   .WithMany(language => language.GuideLanguages)
                   .HasForeignKey(guideLanguage => guideLanguage.LanguageId);
        }
    }
}
