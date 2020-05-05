using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

namespace CrewCloudRepository.Models
{
    public class CrewCloudDBContext : DbContext
    {


        

        public CrewCloudDBContext(DbContextOptions<CrewCloudDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserImage> UserImage { get; set; }
        public virtual DbSet<RestaurantImage> RestaurantImage { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<JobApplication> JobApplication { get; set; }
        public virtual DbSet<JobPost> JobPost { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<RestaurantRating> RestaurantRating { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRating> UserRating { get; set; }

        public virtual DbSet<Log> Log  { get; set; }
        public virtual DbSet<RestaurantUserNotifications> RestaurantUserNotifications { get; set; }


    }
}
