namespace SrpLab.Grading;

/// <summary>Academic policy: who makes the honor roll.</summary>
public sealed class HonorRollPolicy
{
    public bool Qualifies(decimal average, string letter) => average >= 85 && letter is "A" or "B";
}
