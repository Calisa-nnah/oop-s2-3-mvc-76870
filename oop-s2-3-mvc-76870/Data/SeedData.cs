using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Models;

namespace VgcCollege.Data
{
    // This class is used to seed (add) initial data into the database
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            // Get required services
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Apply database migrations
            await context.Database.MigrateAsync();

            // ROLES
            string[] roles = { "Admin", "Faculty", "Student" };

            // Create roles if they don't exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // USERS
            // Helper method to create users and assign roles
            async Task<ApplicationUser> CreateUser(string email, string password, string role)
            {
                var user = await userManager.FindByEmailAsync(email);

                // Create user if not found
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, password);

                    // Throw error if creation fails
                    if (!result.Succeeded)
                        throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                // Assign role if not already assigned
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }

                return user;
            }

            // Create default users
            var admin = await CreateUser("admin@vgc.com", "Admin123", "Admin");
            var faculty = await CreateUser("faculty1@vgc.com", "Faculty123", "Faculty");
            var student1 = await CreateUser("student1@vgc.com", "Student123", "Student");
            var student2 = await CreateUser("student2@vgc.com", "Student123", "Student");

            // STUDENTS
            if (!context.StudentProfiles.Any())
            {
                context.StudentProfiles.AddRange(
                    new StudentProfile
                    {
                        IdentityUserId = student1.Id,
                        Name = "Sandra Student",
                        Email = "student1@vgc.com",
                        Phone = "0851111111",
                        Address = "Dublin",
                        StudentNumber = "ST1001"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student2.Id,
                        Name = "Mary Student",
                        Email = "student2@vgc.com",
                        Phone = "0852222222",
                        Address = "Cork",
                        StudentNumber = "ST1002"
                    }
                );
            }

            // FACULTY
            if (!context.FacultyProfiles.Any())
            {
                context.FacultyProfiles.Add(new FacultyProfile
                {
                    IdentityUserId = faculty.Id,
                    Name = "John Faculty",
                    Email = "faculty1@vgc.com",
                    Phone = "0853333333"
                });
            }

            // BRANCHES
            if (!context.Branches.Any())
            {
                context.Branches.AddRange(
                    new Branch { Name = "Dublin Branch", Address = "Dublin City" },
                    new Branch { Name = "Cork Branch", Address = "Cork City" },
                    new Branch { Name = "Galway Branch", Address = "Galway City" }
                );
            }

            await context.SaveChangesAsync();

            // COURSES
            if (!context.Courses.Any())
            {
                var dublin = context.Branches.First(b => b.Name == "Dublin Branch");

                context.Courses.Add(new Course
                {
                    Name = "BSc Computing",
                    BranchId = dublin.Id,
                    StartDate = new DateTime(2026, 1, 10),
                    EndDate = new DateTime(2026, 12, 20)
                });

                await context.SaveChangesAsync();
            }

            // FACULTY ASSIGNMENT
            if (!context.FacultyCourseAssignments.Any())
            {
                var facultyProfile = context.FacultyProfiles.First();
                var course = context.Courses.First();

                context.FacultyCourseAssignments.Add(new FacultyCourseAssignment
                {
                    FacultyProfileId = facultyProfile.Id,
                    CourseId = course.Id
                });

                await context.SaveChangesAsync();
            }

            // ENROLMENTS
            if (!context.CourseEnrolments.Any())
            {
                var course = context.Courses.First();
                var s1 = context.StudentProfiles.First();
                var s2 = context.StudentProfiles.Skip(1).First();

                context.CourseEnrolments.AddRange(
                    new CourseEnrolment
                    {
                        StudentProfileId = s1.Id,
                        CourseId = course.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = s2.Id,
                        CourseId = course.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}