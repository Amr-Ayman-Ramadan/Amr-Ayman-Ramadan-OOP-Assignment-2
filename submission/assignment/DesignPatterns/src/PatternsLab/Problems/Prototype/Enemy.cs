namespace PatternsLab.Problems.Prototype;

public class Weapon
{
    public string Name { get; set; } = "";
    public int Damage { get; set; }

    public Weapon Clone() => new Weapon { Name = Name, Damage = Damage };
}

/// <summary>
/// Prototype: every enemy knows how to copy itself through Clone().
/// The slow model loading only runs in the normal constructor; Clone() does not call it.
/// </summary>
public abstract class Enemy
{
    private string _modelData;

    public string Name { get; set; } = "";
    public int Health { get; set; }
    public Weapon Weapon { get; set; } = new();
    public List<string> Abilities { get; set; } = new();
    public string ModelId => _modelData;

    protected Enemy()
    {
        Console.WriteLine("   ...loading 3D model (slow)...");
        Thread.Sleep(500);
        _modelData = "MODEL_" + Guid.NewGuid().ToString("N")[..6];
    }

    /// <summary>
    /// Returns a deep copy of this enemy.
    /// MemberwiseClone copies ALL fields (also the private _modelData) without running the
    /// constructor, so the 3D model is not loaded again and the real type (Orc/Elf) is kept.
    /// MemberwiseClone is only a shallow copy, so the reference-type members
    /// (Weapon and the Abilities list) are copied by hand to make the clone independent.
    /// </summary>
    public Enemy Clone()
    {
        var copy = (Enemy)MemberwiseClone();
        copy.Weapon = Weapon.Clone();
        copy.Abilities = new List<string>(Abilities);
        return copy;
    }
}

public class Orc : Enemy
{
    public Orc()
    {
        Name = "Orc";
        Health = 100;
        Weapon = new Weapon { Name = "Axe", Damage = 25 };
        Abilities.Add("Rage");
    }
}

public class Elf : Enemy
{
    public Elf()
    {
        Name = "Elf";
        Health = 70;
        Weapon = new Weapon { Name = "Bow", Damage = 18 };
        Abilities.Add("Stealth");
    }
}

/// <summary>Client copy logic: works only with the base type, no "if (e is Orc)".</summary>
public static class EnemyCopier
{
    public static Enemy CopyEnemy(Enemy e) => e.Clone();
}

/// <summary>(Bonus R5) Stores named prototypes and hands out clones of them.</summary>
public class EnemyRegistry
{
    private readonly Dictionary<string, Enemy> _prototypes = new(StringComparer.OrdinalIgnoreCase);

    public void Register(string key, Enemy prototype) => _prototypes[key] = prototype;

    public Enemy Create(string key)
    {
        if (!_prototypes.TryGetValue(key, out var prototype))
            throw new KeyNotFoundException($"No prototype registered with the name '{key}'.");
        return prototype.Clone();
    }
}
