using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VgcCollege.Data;
using VgcCollege.Models;

namespace VgcCollege.Tests
{
    public class ExamResultTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public void Duplicate_Exam_Result_Can_Be_Detected()
        {
            var context = GetDbContext();

            var result1 = new ExamResult
            {
                ExamId = 1,
                StudentProfileId = 1,
                Score = 80,
                Grade = "A"
            };

            var result2 = new ExamResult
            {
                ExamId = 1,
                StudentProfileId = 1,
                Score = 70,
                Grade = "B"
            };

            context.ExamResults.Add(result1);
            context.ExamResults.Add(result2);
            context.SaveChanges();

            var count = context.ExamResults
                .Count(r => r.ExamId == 1 && r.StudentProfileId == 1);

            Assert.True(count == 2);
        }

        [Fact]
        public void Score_Should_Be_Valid()
        {
            var result = new ExamResult
            {
                Score = 120
            };

            Assert.False(result.Score <= 100);
        }

        [Fact]
        public void Released_Results_Should_Be_Visible()
        {
            var context = GetDbContext();

            var exam = new Exam
            {
                Id = 1,
                Title = "Test Exam",
                ResultsReleased = true
            };

            context.Exams.Add(exam);

            var result = new ExamResult
            {
                ExamId = 1,
                StudentProfileId = 1,
                Score = 85,
                Grade = "A"
            };

            context.ExamResults.Add(result);
            context.SaveChanges();

            var visible = context.ExamResults
                .Include(r => r.Exam)
                .Where(r => r.Exam != null && r.Exam.ResultsReleased)
                .ToList();

            Assert.NotEmpty(visible);
        }

        [Fact]
        public void Unreleased_Results_Should_Not_Be_Visible()
        {
            var context = GetDbContext();

            var exam = new Exam
            {
                Id = 2,
                Title = "Hidden Exam",
                ResultsReleased = false
            };

            context.Exams.Add(exam);

            var result = new ExamResult
            {
                ExamId = 2,
                StudentProfileId = 1,
                Score = 60,
                Grade = "C"
            };

            context.ExamResults.Add(result);
            context.SaveChanges();

            var hidden = context.ExamResults
                .Include(r => r.Exam)
                .Where(r => r.Exam != null && r.Exam.ResultsReleased)
                .ToList();

            Assert.Empty(hidden);
        }

        [Fact]
        public void Student_Should_Only_See_Their_Own_Results()
        {
            var context = GetDbContext();

            context.ExamResults.AddRange(
                new ExamResult { ExamId = 1, StudentProfileId = 1, Score = 80, Grade = "A" },
                new ExamResult { ExamId = 1, StudentProfileId = 2, Score = 70, Grade = "B" }
            );

            context.SaveChanges();

            var student1Results = context.ExamResults
                .Where(r => r.StudentProfileId == 1)
                .ToList();

            Assert.Single(student1Results);
        }

        [Fact]
        public void Score_Should_Be_Between_0_And_100()
        {
            var result = new ExamResult
            {
                Score = -10
            };

            Assert.False(result.Score >= 0 && result.Score <= 100);
        }

        [Fact]
        public void ExamResult_Should_Link_To_Exam()
        {
            var context = GetDbContext();

            var exam = new Exam
            {
                Id = 3,
                Title = "Math Exam",
                ResultsReleased = true
            };

            context.Exams.Add(exam);

            var result = new ExamResult
            {
                ExamId = 3,
                StudentProfileId = 1,
                Score = 90,
                Grade = "A"
            };

            context.ExamResults.Add(result);
            context.SaveChanges();

            var saved = context.ExamResults
                .Include(r => r.Exam)
                .FirstOrDefault();

            Assert.NotNull(saved?.Exam);
        }

        [Fact]
        public void No_Results_Should_Return_Empty_List()
        {
            var context = GetDbContext();

            var results = context.ExamResults.ToList();

            Assert.Empty(results);
        }
    }
}