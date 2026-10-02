namespace SrpLab.Grading;

/// <summary>Combines the grade book with the academic policies into one standing per student.</summary>
public sealed class StandingCalculator
{
    private readonly LetterGradePolicy _letters;
    private readonly HonorRollPolicy _honor;

    public StandingCalculator(LetterGradePolicy letters, HonorRollPolicy honor)
    {
        _letters = letters;
        _honor = honor;
    }

    public StudentStanding For(GradeBook book, string studentId)
    {
        var avg = book.Average(studentId);
        var letter = _letters.LetterFor(avg);
        return new StudentStanding(studentId, avg, letter, _honor.Qualifies(avg, letter));
    }
}
