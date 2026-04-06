using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Models;

namespace VgcCollege.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            // ROLES
            string[] roles = { "Admin", "Faculty", "Student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // HELPER METHOD TO CREATE USERS
            async Task<ApplicationUser> CreateUser(string email, string password, string role)
            {
                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }

                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }

                return user;
            }

            // USERS
            var admin = await CreateUser("admin@vgc.com", "Admin123", "Admin");
            var faculty = await CreateUser("faculty1@vgc.com", "Faculty123", "Faculty");

            var student1 = await CreateUser("student1@vgc.com", "Student123", "Student");
            var student2 = await CreateUser("student2@vgc.com", "Student123", "Student");
            var student3 = await CreateUser("aisha.khan@vgc.com", "Student123", "Student");
            var student4 = await CreateUser("david.zhang@vgc.com", "Student123", "Student");
            var student5 = await CreateUser("maria.santos@vgc.com", "Student123", "Student");
            var student6 = await CreateUser("ahmed.hassan@vgc.com", "Student123", "Student");
            var student7 = await CreateUser("emma.oconnor@vgc.com", "Student123", "Student");
            var student8 = await CreateUser("liam.murphy@vgc.com", "Student123", "Student");
            var student9 = await CreateUser("aoife.kelly@vgc.com", "Student123", "Student");
            var student10 = await CreateUser("jack.byrne@vgc.com", "Student123", "Student");

            // STUDENTS
            if (!context.StudentProfiles.Any())
            {
                context.StudentProfiles.AddRange(
                    new StudentProfile
                    {
                        IdentityUserId = student1.Id,
                        Name = "Sandra Student",
                        Email = "student1@vgc.com",
                        Phone = "0851116758",
                        Address = "Dublin",
                        StudentNumber = "ST1001"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student2.Id,
                        Name = "Mary Student",
                        Email = "student2@vgc.com",
                        Phone = "0855637648",
                        Address = "Cork",
                        StudentNumber = "ST1002"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student3.Id,
                        Name = "Aisha Khan",
                        Email = "aisha.khan@vgc.com",
                        Phone = "0872233445",
                        Address = "Dublin",
                        StudentNumber = "ST1003"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student4.Id,
                        Name = "David Zhang",
                        Email = "david.zhang@vgc.com",
                        Phone = "0873344556",
                        Address = "Dublin",
                        StudentNumber = "ST1004"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student5.Id,
                        Name = "Maria Santos",
                        Email = "maria.santos@vgc.com",
                        Phone = "0874455667",
                        Address = "Dublin",
                        StudentNumber = "ST1005"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student6.Id,
                        Name = "Ahmed Hassan",
                        Email = "ahmed.hassan@vgc.com",
                        Phone = "0875566778",
                        Address = "Dublin",
                        StudentNumber = "ST1006"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student7.Id,
                        Name = "Emma O'Connor",
                        Email = "emma.oconnor@vgc.com",
                        Phone = "0871234567",
                        Address = "Limerick",
                        StudentNumber = "ST1007"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student8.Id,
                        Name = "Liam Murphy",
                        Email = "liam.murphy@vgc.com",
                        Phone = "0872345678",
                        Address = "Cork",
                        StudentNumber = "ST1008"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student9.Id,
                        Name = "Aoife Kelly",
                        Email = "aoife.kelly@vgc.com",
                        Phone = "0873456789",
                        Address = "Galway",
                        StudentNumber = "ST1009"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student10.Id,
                        Name = "Jack Byrne",
                        Email = "jack.byrne@vgc.com",
                        Phone = "0874567890",
                        Address = "Waterford",
                        StudentNumber = "ST1010"
                    }
                );

                await context.SaveChangesAsync();
            }

            // FACULTY
            if (!context.FacultyProfiles.Any())
            {
                context.FacultyProfiles.Add(new FacultyProfile
                {
                    IdentityUserId = faculty.Id,
                    Name = "John Faculty",
                    Email = "faculty1@vgc.com",
                    Phone = "0857650912"
                });

                await context.SaveChangesAsync();
            }

            // BRANCHES
            if (!context.Branches.Any())
            {
                context.Branches.AddRange(
                    new Branch { Name = "Dublin Branch", Address = "Dublin City" },
                    new Branch { Name = "Cork Branch", Address = "Cork City" },
                    new Branch { Name = "Galway Branch", Address = "Galway City" }
                );

                await context.SaveChangesAsync();
            }

            // COURSES
            if (!context.Courses.Any())
            {
                var dublin = context.Branches.First(b => b.Name == "Dublin Branch");
                var cork = context.Branches.First(b => b.Name == "Cork Branch");
                var galway = context.Branches.First(b => b.Name == "Galway Branch");

                context.Courses.AddRange(
                    new Course
                    {
                        Name = "BSc Computing",
                        BranchId = dublin.Id,
                        StartDate = new DateTime(2026, 1, 10),
                        EndDate = new DateTime(2026, 12, 20)
                    },
                    new Course
                    {
                        Name = "Business Studies",
                        BranchId = cork.Id,
                        StartDate = new DateTime(2026, 1, 10),
                        EndDate = new DateTime(2026, 12, 20)
                    },
                    new Course
                    {
                        Name = "Digital Media",
                        BranchId = galway.Id,
                        StartDate = new DateTime(2026, 1, 10),
                        EndDate = new DateTime(2026, 12, 20)
                    }
                );

                await context.SaveChangesAsync();
            }

            // FACULTY ASSIGNMENT
            if (!context.FacultyCourseAssignments.Any())
            {
                var facultyProfile = context.FacultyProfiles.First();
                var course = context.Courses.First(c => c.Name == "BSc Computing");

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
                var computing = context.Courses.First(c => c.Name == "BSc Computing");
                var business = context.Courses.First(c => c.Name == "Business Studies");
                var media = context.Courses.First(c => c.Name == "Digital Media");

                var students = context.StudentProfiles.OrderBy(s => s.StudentNumber).ToList();

                context.CourseEnrolments.AddRange(
                    new CourseEnrolment
                    {
                        StudentProfileId = students[0].Id,
                        CourseId = computing.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = students[1].Id,
                        CourseId = computing.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = students[2].Id,
                        CourseId = business.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = students[3].Id,
                        CourseId = business.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = students[4].Id,
                        CourseId = media.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = students[5].Id,
                        CourseId = media.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = students[6].Id,
                        CourseId = computing.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = students[7].Id,
                        CourseId = business.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = students[8].Id,
                        CourseId = media.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = students[9].Id,
                        CourseId = computing.Id,
                        EnrolDate = DateTime.Today,
                        Status = "Active"
                    }
                );

                await context.SaveChangesAsync();
            }

            // ATTENDANCE RECORDS
            if (!context.AttendanceRecords.Any())
            {
                var enrolments = context.CourseEnrolments.ToList();

                foreach (var enrolment in enrolments)
                {
                    context.AttendanceRecords.AddRange(
                        new AttendanceRecord
                        {
                            CourseEnrolmentId = enrolment.Id,
                            WeekNumber = 1,
                            Present = true
                        },
                        new AttendanceRecord
                        {
                            CourseEnrolmentId = enrolment.Id,
                            WeekNumber = 2,
                            Present = true
                        },
                        new AttendanceRecord
                        {
                            CourseEnrolmentId = enrolment.Id,
                            WeekNumber = 3,
                            Present = enrolment.Id % 2 == 0
                        }
                    );
                }

                await context.SaveChangesAsync();
            }

            // ASSIGNMENTS
            if (!context.Assignments.Any())
            {
                var computing = context.Courses.First(c => c.Name == "BSc Computing");
                var business = context.Courses.First(c => c.Name == "Business Studies");
                var media = context.Courses.First(c => c.Name == "Digital Media");

                context.Assignments.AddRange(
                    new Assignment
                    {
                        CourseId = computing.Id,
                        Title = "Programming Fundamentals CA",
                        MaxScore = 100,
                        DueDate = new DateTime(2026, 4, 30)
                    },
                    new Assignment
                    {
                        CourseId = computing.Id,
                        Title = "Web Development Project",
                        MaxScore = 100,
                        DueDate = new DateTime(2026, 5, 20)
                    },
                    new Assignment
                    {
                        CourseId = business.Id,
                        Title = "Business Report",
                        MaxScore = 100,
                        DueDate = new DateTime(2026, 4, 25)
                    },
                    new Assignment
                    {
                        CourseId = business.Id,
                        Title = "Marketing Presentation",
                        MaxScore = 100,
                        DueDate = new DateTime(2026, 5, 15)
                    },
                    new Assignment
                    {
                        CourseId = media.Id,
                        Title = "Digital Design Portfolio",
                        MaxScore = 100,
                        DueDate = new DateTime(2026, 4, 28)
                    },
                    new Assignment
                    {
                        CourseId = media.Id,
                        Title = "Video Editing Project",
                        MaxScore = 100,
                        DueDate = new DateTime(2026, 5, 18)
                    }
                );

                await context.SaveChangesAsync();
            }

            // ASSIGNMENT RESULTS
            if (!context.AssignmentResults.Any())
            {
                string GetFeedback(double score)
                {
                    if (score >= 70) return "Excellent work. Strong understanding shown.";
                    if (score >= 60) return "Very good work. Good effort overall.";
                    if (score >= 50) return "Good attempt. Some areas need improvement.";
                    if (score >= 40) return "Pass. More detail and accuracy needed.";
                    return "Needs improvement. Please review the topic and resubmit stronger work.";
                }

                var assignments = context.Assignments.Include(a => a.Course).ToList();

                var computingStudents = context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Include(e => e.Course)
                    .Where(e => e.Course!.Name == "BSc Computing")
                    .Select(e => e.StudentProfile!)
                    .Distinct()
                    .ToList();

                var businessStudents = context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Include(e => e.Course)
                    .Where(e => e.Course!.Name == "Business Studies")
                    .Select(e => e.StudentProfile!)
                    .Distinct()
                    .ToList();

                var mediaStudents = context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Include(e => e.Course)
                    .Where(e => e.Course!.Name == "Digital Media")
                    .Select(e => e.StudentProfile!)
                    .Distinct()
                    .ToList();

                var results = new List<AssignmentResult>();

                foreach (var assignment in assignments.Where(a => a.Course!.Name == "BSc Computing"))
                {
                    double[] scores = { 78, 65, 84, 59 };

                    for (int i = 0; i < computingStudents.Count && i < scores.Length; i++)
                    {
                        results.Add(new AssignmentResult
                        {
                            AssignmentId = assignment.Id,
                            StudentProfileId = computingStudents[i].Id,
                            Score = scores[i],
                            Feedback = GetFeedback(scores[i])
                        });
                    }
                }

                foreach (var assignment in assignments.Where(a => a.Course!.Name == "Business Studies"))
                {
                    double[] scores = { 72, 55, 68 };

                    for (int i = 0; i < businessStudents.Count && i < scores.Length; i++)
                    {
                        results.Add(new AssignmentResult
                        {
                            AssignmentId = assignment.Id,
                            StudentProfileId = businessStudents[i].Id,
                            Score = scores[i],
                            Feedback = GetFeedback(scores[i])
                        });
                    }
                }

                foreach (var assignment in assignments.Where(a => a.Course!.Name == "Digital Media"))
                {
                    double[] scores = { 81, 63, 49 };

                    for (int i = 0; i < mediaStudents.Count && i < scores.Length; i++)
                    {
                        results.Add(new AssignmentResult
                        {
                            AssignmentId = assignment.Id,
                            StudentProfileId = mediaStudents[i].Id,
                            Score = scores[i],
                            Feedback = GetFeedback(scores[i])
                        });
                    }
                }

                context.AssignmentResults.AddRange(results);
                await context.SaveChangesAsync();
            }

            // EXAMS
            if (!context.Exams.Any())
            {
                var computing = context.Courses.First(c => c.Name == "BSc Computing");
                var business = context.Courses.First(c => c.Name == "Business Studies");
                var media = context.Courses.First(c => c.Name == "Digital Media");

                context.Exams.AddRange(
                    new Exam
                    {
                        CourseId = computing.Id,
                        Title = "Computing Final Exam",
                        Date = new DateTime(2026, 6, 10),
                        MaxScore = 100,
                        ResultsReleased = true
                    },
                    new Exam
                    {
                        CourseId = business.Id,
                        Title = "Business Final Exam",
                        Date = new DateTime(2026, 6, 12),
                        MaxScore = 100,
                        ResultsReleased = false
                    },
                    new Exam
                    {
                        CourseId = media.Id,
                        Title = "Media Final Exam",
                        Date = new DateTime(2026, 6, 14),
                        MaxScore = 100,
                        ResultsReleased = true
                    }
                );

                await context.SaveChangesAsync();
            }

            // EXAM RESULTS
            if (!context.ExamResults.Any())
            {
                string GetGrade(double score)
                {
                    if (score >= 70) return "A";
                    if (score >= 60) return "B";
                    if (score >= 50) return "C";
                    if (score >= 40) return "D";
                    return "F";
                }

                var exams = context.Exams.Include(e => e.Course).ToList();

                var computingStudents = context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Include(e => e.Course)
                    .Where(e => e.Course!.Name == "BSc Computing")
                    .Select(e => e.StudentProfile!)
                    .Distinct()
                    .ToList();

                var businessStudents = context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Include(e => e.Course)
                    .Where(e => e.Course!.Name == "Business Studies")
                    .Select(e => e.StudentProfile!)
                    .Distinct()
                    .ToList();

                var mediaStudents = context.CourseEnrolments
                    .Include(e => e.StudentProfile)
                    .Include(e => e.Course)
                    .Where(e => e.Course!.Name == "Digital Media")
                    .Select(e => e.StudentProfile!)
                    .Distinct()
                    .ToList();

                var examResults = new List<ExamResult>();

                foreach (var exam in exams.Where(e => e.Course!.Name == "BSc Computing"))
                {
                    double[] scores = { 76, 62, 88, 54 };

                    for (int i = 0; i < computingStudents.Count && i < scores.Length; i++)
                    {
                        examResults.Add(new ExamResult
                        {
                            ExamId = exam.Id,
                            StudentProfileId = computingStudents[i].Id,
                            Score = scores[i],
                            Grade = GetGrade(scores[i])
                        });
                    }
                }

                foreach (var exam in exams.Where(e => e.Course!.Name == "Business Studies"))
                {
                    double[] scores = { 69, 51, 44 };

                    for (int i = 0; i < businessStudents.Count && i < scores.Length; i++)
                    {
                        examResults.Add(new ExamResult
                        {
                            ExamId = exam.Id,
                            StudentProfileId = businessStudents[i].Id,
                            Score = scores[i],
                            Grade = GetGrade(scores[i])
                        });
                    }
                }

                foreach (var exam in exams.Where(e => e.Course!.Name == "Digital Media"))
                {
                    double[] scores = { 83, 64, 47 };

                    for (int i = 0; i < mediaStudents.Count && i < scores.Length; i++)
                    {
                        examResults.Add(new ExamResult
                        {
                            ExamId = exam.Id,
                            StudentProfileId = mediaStudents[i].Id,
                            Score = scores[i],
                            Grade = GetGrade(scores[i])
                        });
                    }
                }

                context.ExamResults.AddRange(examResults);
                await context.SaveChangesAsync();
            }
        }
    }
}
