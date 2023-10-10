using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace fit_and_fuel.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedRole(modelBuilder, "Admin", "create", "update", "delete");

            SeedRole(modelBuilder, "Nutritionist");
            SeedRole(modelBuilder, "Patient");


            modelBuilder.Entity<AvailableTime>()
      .HasOne(at => at.nutritionist)
      .WithMany(n => n.AvaliableTimes)
      .HasForeignKey(at => at.NutritionistId)
      .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Nutritionist>()
                .HasMany(n => n.AvaliableTimes)
                .WithOne(at => at.nutritionist)
                .HasForeignKey(at => at.NutritionistId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Appoitment>()
         .HasOne(a => a.Patient)
         .WithMany(p => p.appoitments)
         .HasForeignKey(a => a.PatientId)
         .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Appoitment>()
                .HasOne(a => a.nutritionist)
                .WithMany(n => n.appoitments)
                .HasForeignKey(a => a.NutritionistId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DietPlan>()
                   .HasOne(p => p.Patient)
                   .WithOne(d => d.dietPlan)
                   .OnDelete(DeleteBehavior.Restrict);

            //    modelBuilder.Entity<Patient>()
            //.HasOne(p => p.healthRecord)
            //.WithOne(hr => hr.Patient)
            //.HasForeignKey<HealthRecord>(hr => hr.PatientId);


            modelBuilder.Entity<Nutritionist>().HasData(
                new Nutritionist { Id = 1, Name = "John Doe", Gender = "Male", Age = 30, PhoneNumber = "123-456-7890", CvURl = "cv_url_1", imgURl = "img_url_1",  },
                new Nutritionist { Id = 2, UserId = "2", Name = "Jane Smith", Gender = "Female", Age = 28, PhoneNumber = "987-654-3210", CvURl = "cv_url_2", imgURl = "img_url_2",  }
            );


            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 1, UserId = "3", Name = "Alice Johnson", Gender = "Female", Age = 25, PhoneNumber = "555-123-4567", NutritionistId = 1, imgURl = "img_url_1" },
                new Patient { Id = 2, Name = "Bob Williams", Gender = "Male", PhoneNumber = "555-987-6543", NutritionistId = 2, imgURl = "img_url_1" }
            );


            modelBuilder.Entity<Clinic>().HasData(
                new Clinic { Id = 1, Name = "Healthy Clinic", Address = "123 Main St", PhoneNumber = "5555555555", NutritionistId = 1 },
                new Clinic { Id = 2, Name = "Wellness Center", Address = "456 Elm St", PhoneNumber = "5551234567", NutritionistId = 2 }
            );


            modelBuilder.Entity<DietPlan>().HasData(
                new DietPlan { Id = 1, Duration = 5, StartDate = new DateTime(2023, 5, 10), EndDate = new DateTime(2023, 5, 15), PatientId = 1, NutritionistId = 1 },
                new DietPlan { Id = 2, Duration = 5, StartDate = new DateTime(2023, 5, 15), EndDate = new DateTime(2023, 5, 20), PatientId = 2, NutritionistId = 2 }
            );

            modelBuilder.Entity<Day>().HasData(


    new Day { Id = 1, DayName = "tusday", DietPlanId = 1 },
    new Day { Id = 2, DayName = "tusday", DietPlanId = 2 },
    new Day { Id = 3, DayName = "tusday", DietPlanId = 1 },
    new Day { Id = 4, DayName = "tusday", DietPlanId = 1 },
    new Day { Id = 5, DayName = "tusday", DietPlanId = 2 },
    new Day { Id = 6, DayName = "tusday", DietPlanId = 1 },
    new Day { Id = 7, DayName = "tusday", DietPlanId = 1 },
    new Day { Id = 8, DayName = "tusday", DietPlanId = 2 },
    new Day { Id = 9, DayName = "tusday", DietPlanId = 1 },
    new Day { Id = 10, DayName = "tusday", DietPlanId = 1 }



            );


            modelBuilder.Entity<Meal>().HasData(
                new Meal { Id = 1, Name = "Breakfast", Description = "Oatmeal with fruits", Calories = 300, Completion = false, DayId = 1 },
                new Meal { Id = 2, Name = "Lunch", Description = "Grilled chicken salad", Calories = 500, Completion = false, DayId = 1 }
            );


            modelBuilder.Entity<Post>().HasData(
                new Post { Id = 1, Title = "Healthy Eating Tips", Description = "Learn how to make healthier food choices.", ImageUrl = "image_url_1", Time = DateTime.Now, NutritionistId = 1 },
                new Post { Id = 2, Title = "Balanced Diet Importance", Description = "Understanding the importance of a balanced diet.", ImageUrl = "image_url_2", Time = DateTime.Now, NutritionistId = 2 }
            );

            modelBuilder.Entity<Appoitment>().HasData(
                new Appoitment { Id = 1, Time = DateTime.Now.AddHours(1), Status = "Scheduled", PatientId = 1, NutritionistId = 1 },
                new Appoitment { Id = 2, Time = DateTime.Now.AddHours(2), Status = "Scheduled", PatientId = 2, NutritionistId = 2 }
            );


            modelBuilder.Entity<HealthRecord>().HasData(
        new HealthRecord
        {
            Id = 1,
            PatientId = 1,
            Height = 175,
            Weight = 70,

            Illnesses = "None"
        },
        new HealthRecord
        {
            Id = 2,
            PatientId = 2,
            Height = 180,
            Weight = 80,

            Illnesses = "None"
        }
    );

            var hasher = new PasswordHasher<ApplicationUser>();
            var adminUser = new ApplicationUser
            {
                Id = "1",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "adminUser@example.com",
                PhoneNumber = "1234567890",
                NormalizedEmail = "adminUser@EXAMPLE.COM",
                EmailConfirmed = true,
                LockoutEnabled = false
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin!23");
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);


            var NutritionistadminUser = new ApplicationUser
            {
                Id = "2",
                UserName = "nutritionist",
                NormalizedUserName = "NUTRITIONIST",
                Email = "nutritionistUser@example.com",
                PhoneNumber = "1234567890",
                NormalizedEmail = "nutritionistUser@EXAMPLE.COM",
                EmailConfirmed = true,
                LockoutEnabled = false
            };
            NutritionistadminUser.PasswordHash = hasher.HashPassword(NutritionistadminUser, "Nutritionist!23");
            modelBuilder.Entity<ApplicationUser>().HasData(NutritionistadminUser);


            var patientUser = new ApplicationUser
            {
                Id = "3",
                UserName = "patient",
                NormalizedUserName = "PATIENT",
                Email = "patientUser@example.com",
                PhoneNumber = "1234567890",
                NormalizedEmail = "patientUser@EXAMPLE.COM",
                EmailConfirmed = true,
                LockoutEnabled = false
            };
            patientUser.PasswordHash = hasher.HashPassword(patientUser, "Patient!23");
            modelBuilder.Entity<ApplicationUser>().HasData(patientUser);


            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
    new IdentityUserRole<string>
    {
        RoleId = "admin",
        UserId = adminUser.Id
    },
    new IdentityUserRole<string>
    {
        RoleId = "nutritionist",
        UserId = NutritionistadminUser.Id
    },
    new IdentityUserRole<string>
    {
        RoleId = "patient",
        UserId = patientUser.Id
    }
    );
        }


        private int nextId = 1;
        private void SeedRole(ModelBuilder modelBuilder, string roleName, params string[] permissions)
        {
            var role = new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.Empty.ToString()
            };
            modelBuilder.Entity<IdentityRole>().HasData(role);

            var roleClaims = permissions.Select(permission =>
            new IdentityRoleClaim<string>
            {
                Id = nextId++,
                RoleId = role.Id,
                ClaimType = "permissions",
                ClaimValue = permission
            }).ToArray();
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(roleClaims);
        }



        public DbSet<Nutritionist> Nutritionists { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Clinic> Clinics { get; set; }

        public DbSet<Appoitment> Appoitments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<DietPlan> DietPlans { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<HealthRecord> healthRecords { get; set; }

        public DbSet<Payment> PaymentRecords { get; set; }

        public DbSet<fit_and_fuel.Model.AvailableTime> AvailableTime { get; set; } = default!;


        public DbSet<fit_and_fuel.Model.Like> Like { get; set; } = default!;

        public DbSet<fit_and_fuel.Model.Comment> Comment { get; set; } = default!;

        public DbSet<Rating> Ratings { get; set; }



    }
}
