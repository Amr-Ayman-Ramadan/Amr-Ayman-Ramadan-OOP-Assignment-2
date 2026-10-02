namespace SrpLab.Ward;

/// <summary>Runs the admission workflow: score the vitals, assign the bed, page if needed.</summary>
public sealed class BedAdmission
{
    private readonly WardBoard _board;
    private readonly AcuityScorer _scorer;
    private readonly PagerPolicy _pagerPolicy;
    private readonly PagerLog _pagerLog;

    public BedAdmission(WardBoard board, AcuityScorer scorer, PagerPolicy pagerPolicy, PagerLog pagerLog)
    {
        _board = board;
        _scorer = scorer;
        _pagerPolicy = pagerPolicy;
        _pagerLog = pagerLog;
    }

    public BedOccupant Admit(int bed, string patientId, int heartRate, int spo2)
    {
        var acuity = _scorer.Score(heartRate, spo2);
        var occupant = _board.AssignBed(bed, patientId, acuity);

        if (_pagerPolicy.ShouldPage(acuity))
            _pagerLog.RecordCodeYellow(bed, DateTime.UtcNow);

        return occupant;
    }
}
