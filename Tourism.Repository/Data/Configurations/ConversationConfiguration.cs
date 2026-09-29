using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.HasOne(conversation => conversation.Guide)
                   .WithMany(guide => guide.Conversations)
                   .HasForeignKey(conversation => conversation.GuideId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(conversation => conversation.Tourist)
                   .WithMany(tourist => tourist.Conversations)
                   .HasForeignKey(conversation => conversation.TouristId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
