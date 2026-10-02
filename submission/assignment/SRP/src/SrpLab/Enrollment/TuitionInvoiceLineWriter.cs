namespace SrpLab.Enrollment;

/// <summary>Finance export format of one tuition invoice line.</summary>
public sealed class TuitionInvoiceLineWriter
{
    private readonly VatCalculator _vat;

    public TuitionInvoiceLineWriter(VatCalculator vat) => _vat = vat;

    public string Write(CourseRoster roster, string studentEmail)
    {
        var course = roster.Course;
        if (!roster.IsSeated(studentEmail)) return $"{course.Code},WAITLIST,0.00";
        var vat = _vat.VatOn(course.Tuition);
        return $"{course.Code},TUITION,{course.Tuition:0.00},VAT,{vat:0.00},TOTAL,{(course.Tuition + vat):0.00}";
    }
}
