namespace SrpLab.Kitchen;

/// <summary>Expo routing rule: which pass lane the finished order goes to.</summary>
public sealed class ExpoLaneRouter
{
    public string LaneFor(bool hasAllergens, int readyMinutes) =>
        hasAllergens ? "LANE-ALLERGY" : readyMinutes > 20 ? "LANE-SLOW" : "LANE-FAST";
}
