using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tourism.Core.Entities;

namespace Tourism.Repository.Data.DBContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity configurations are applied

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");

            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
     
        }
        public DbSet<TouristProfile> Tourists { get; set; }
        public DbSet<GuideProfile> Guides { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }       
        public DbSet<Language> Languages { get; set; }
        public DbSet<GuideLanguage> GuideLanguages { get; set; }       
        public DbSet<Place> Places { get; set; }
        public DbSet<PlacePhoto> PlacePhotos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BasePlan> BasePlans { get; set; }
        public DbSet<CustomPlan> CustomPlans { get; set; }
        public DbSet<ReadyPlan> ReadyPlans { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<PlaceReview> PlaceReviews { get; set; }
        public DbSet<GuideReview> GuideReviews { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Requset> Requests { get; set; }
        public DbSet<PlanPlace> PlanPlaces { get; set; }
        public DbSet<PasswordResetOtp> PasswordResetOtps { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CommunityPost> CommunityPosts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentImage> CommentImages { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
    }
}
