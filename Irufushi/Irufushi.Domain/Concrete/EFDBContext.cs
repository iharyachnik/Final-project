using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Abstract.Entities;

namespace Irufushi.Domain.Concrete
{
    public class EFDBContext: DbContext
    {
                public EFDBContext()
            : base("IFDB")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<AboutUser> AboutUsers { get; set; }
        //public DbSet<Album> Albums { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<FriendShip> FriendShips { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<webpages_Roles> webpages_Roles { get; set; }
        public DbSet<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AboutUser>().HasRequired(x => x.UserProfile).WithOptional(x => x.AboutUser);
            //modelBuilder.Entity<Album>().HasRequired(x => x.UserProfile).WithMany(x => x.Albums)
            //    .WillCascadeOnDelete(false);
            //modelBuilder.Entity<Album>().HasMany(x => x.Photos).WithRequired(x => x.Album)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contacts>().HasRequired(x => x.UserProfile).WithOptional(x => x.Contacts);
            modelBuilder.Entity<Location>().HasRequired(x => x.UserProfile).WithOptional(x => x.Location);

            modelBuilder.Entity<Photo>().HasRequired(x => x.UserProfile).WithOptional(x => x.Photo);

            modelBuilder.Entity<Message>().HasRequired(x => x.Sender).WithMany(x => x.Senders)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>().HasRequired(x => x.Receiver).WithMany(x => x.Receivers)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Photo>().HasRequired(x => x.UserProfile).WithMany(x => x.Photos)
            //    .WillCascadeOnDelete(false);


            modelBuilder.Entity<FriendShip>().HasRequired(x => x.User).WithMany(x => x.FriendshipsOwn)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FriendShip>().HasRequired(x => x.Friend).WithMany(x => x.FriendshipsFor)
                .WillCascadeOnDelete(false);
        }
    }
}
