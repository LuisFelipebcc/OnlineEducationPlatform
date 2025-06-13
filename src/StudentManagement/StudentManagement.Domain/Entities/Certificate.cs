namespace StudentManagement.Domain.Entities
{
    public class Certificate
    {
        public Guid Id { get; private set; }
        public Guid CourseId { get; private set; }
        public DateTime IssueDate { get; private set; }
        public string ValidationCode { get; private set; }
        public decimal FinalGrade { get; private set; }

        protected Certificate() { }

        public Certificate(Guid courseId, decimal finalGrade)
        {
            if (finalGrade < 0 || finalGrade > 10)
                throw new ArgumentException("Grade must be between 0 and 10", nameof(finalGrade));

            Id = Guid.NewGuid();
            CourseId = courseId;
            IssueDate = DateTime.UtcNow;
            FinalGrade = finalGrade;
            ValidationCode = GenerateValidationCode();
        }

        private string GenerateValidationCode()
        {
            // Generates a unique code for certificate validation
            return $"{Id:N}-{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        public bool ValidateCode(string code)
        {
            return ValidationCode == code;
        }
    }
}