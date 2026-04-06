using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VgcCollege.Data;
using VgcCollege.Models;

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
            .Where(r => r.Exam.ResultsReleased == true)
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
            .Where(r => r.Exam.ResultsReleased == true)
            .ToList();

        Assert.Empty(hidden);
    }
}