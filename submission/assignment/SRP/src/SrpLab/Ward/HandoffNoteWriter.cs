namespace SrpLab.Ward;

/// <summary>Formats the nurse handoff note text.</summary>
public sealed class HandoffNoteWriter
{
    public string Write(WardBoard board, int bed)
    {
        var occupant = board.FindOccupant(bed);
        if (occupant is null) return $"Bed {bed}: empty";

        var tone = occupant.Acuity >= 8 ? "ESCALATE" : occupant.Acuity >= 4 ? "WATCH" : "STABLE";
        return $"[HANDOFF {DateTime.UtcNow:yyyy-MM-dd}] Bed {bed} · {occupant.PatientId} · acuity={occupant.Acuity} · {tone}";
    }
}
