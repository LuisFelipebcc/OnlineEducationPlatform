using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement.Domain.Entities
{
    public class LearningHistory
    {
        public IReadOnlyCollection<CourseProgress> InProgressCourses { get; }
        public IReadOnlyCollection<CourseProgress> CompletedCourses { get; }
        public decimal AverageGrade { get; }
        public int TotalCompletedCourses { get; }
        public int TotalStudyHours { get; }

        public LearningHistory()
        {
            InProgressCourses = new List<CourseProgress>().AsReadOnly();
            CompletedCourses = new List<CourseProgress>().AsReadOnly();
            AverageGrade = 0;
            TotalCompletedCourses = 0;
            TotalStudyHours = 0;
        }

        public LearningHistory(
            IEnumerable<CourseProgress> inProgressCourses,
            IEnumerable<CourseProgress> completedCourses)
        {
            if (inProgressCourses == null)
                throw new ArgumentNullException(nameof(inProgressCourses));
            if (completedCourses == null)
                throw new ArgumentNullException(nameof(completedCourses));

            InProgressCourses = inProgressCourses.ToList().AsReadOnly();
            CompletedCourses = completedCourses.ToList().AsReadOnly();
            TotalCompletedCourses = CompletedCourses.Count;
            TotalStudyHours = CompletedCourses.Sum(c => c.StudyHours);
            AverageGrade = CalculateAverageGrade();
        }

        private decimal CalculateAverageGrade()
        {
            if (!CompletedCourses.Any())
                return 0;

            return CompletedCourses.Average(c => c.FinalGrade);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not LearningHistory other)
                return false;

            return InProgressCourses.SequenceEqual(other.InProgressCourses) &&
                   CompletedCourses.SequenceEqual(other.CompletedCourses);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                InProgressCourses.GetHashCode(),
                CompletedCourses.GetHashCode()
            );
        }
    }

    public class CourseProgress
    {
        public Guid CourseId { get; }
        public string CourseTitle { get; }
        public decimal FinalGrade { get; }
        public int StudyHours { get; }
        public DateTime CompletionDate { get; }

        public CourseProgress(
            Guid courseId,
            string courseTitle,
            decimal finalGrade,
            int studyHours,
            DateTime completionDate)
        {
            CourseId = courseId;
            CourseTitle = courseTitle;
            FinalGrade = finalGrade;
            StudyHours = studyHours;
            CompletionDate = completionDate;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not CourseProgress other)
                return false;

            return CourseId == other.CourseId &&
                   CourseTitle == other.CourseTitle &&
                   FinalGrade == other.FinalGrade &&
                   StudyHours == other.StudyHours &&
                   CompletionDate == other.CompletionDate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                CourseId,
                CourseTitle,
                FinalGrade,
                StudyHours,
                CompletionDate
            );
        }
    }
}