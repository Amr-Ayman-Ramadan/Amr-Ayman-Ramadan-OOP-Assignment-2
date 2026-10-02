using System.Diagnostics;
using PatternsLab.Problems.Builder;
using PatternsLab.Problems.Prototype;
using PatternsLab.Problems.Singleton;

Console.WriteLine("=== SINGLETON: AFTER ===\n");

// R4: 10 threads ask for the config at the same moment -> still loaded only once.
var configs = new AppConfig[10];
Parallel.For(0, configs.Length, i => configs[i] = AppConfig.Instance);
Console.WriteLine($"10 threads got the same object? {configs.All(c => ReferenceEquals(c, configs[0]))}");

var db = new DatabaseService();
var ui = new UiService();

Console.WriteLine();

db.Config.Theme = "Dark";
Console.WriteLine("Admin changed theme to Dark.");

ui.Render();

Console.WriteLine($"\nSame config object? {ReferenceEquals(db.Config, ui.Config)}");
Console.WriteLine($"Times config was loaded from disk: {AppConfig.LoadCount}");
// var config = new AppConfig();   // does NOT compile: 'AppConfig.AppConfig()' is inaccessible due to its protection level (R5)

Console.WriteLine("\n=== PROTOTYPE: AFTER ===\n");

Enemy orcPrototype = new Orc();       // the slow load happens once, here
var sw = Stopwatch.StartNew();
var army = new List<Enemy>();
for (int i = 1; i <= 5; i++)
{
    var orc = orcPrototype.Clone();   // fast: no model loading
    orc.Name = $"Orc-{i}";
    army.Add(orc);
}
Console.WriteLine($"Cloned 5 orcs in {sw.ElapsedMilliseconds} ms\n");

Enemy original = new Orc();
original.Name = "Boss Orc";
Enemy copy = EnemyCopier.CopyEnemy(original);   // client only knows "Enemy"

Console.WriteLine($"\nCopy is still an {copy.GetType().Name}, name = {copy.Name}");
Console.WriteLine($"Original model id: {original.ModelId}");
Console.WriteLine($"Copy model id:     {copy.ModelId}");

copy.Weapon.Damage = 999;
Console.WriteLine($"\nWe changed the COPY's weapon damage to 999.");
Console.WriteLine($"Original's weapon damage is still: {original.Weapon.Damage}");

copy.Abilities.Add("Fire Breath");
Console.WriteLine($"Original abilities: {string.Join(", ", original.Abilities)}");
Console.WriteLine($"Copy abilities:     {string.Join(", ", copy.Abilities)}");

Console.WriteLine("\n-- Bonus: prototype registry --");
var registry = new EnemyRegistry();
registry.Register("orc", orcPrototype);
registry.Register("elf", new Elf());
sw.Restart();
var e1 = registry.Create("elf");
var e2 = registry.Create("orc");
Console.WriteLine($"Created {e1.Name} + {e2.Name} from the registry in {sw.ElapsedMilliseconds} ms");

Console.WriteLine("\n=== BUILDER: AFTER ===\n");
Console.WriteLine(RegistrationCallSites.CreateLiveStudent());
Console.WriteLine(RegistrationCallSites.CreateVideosOnly());

Console.WriteLine("\nInvalid combinations fail at Build():");
try
{
    new CourseRegistration.Builder("omar@mail.com", "SEF-101", AccessMode.LiveGroup).Build();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"  Rejected: {ex.Message}");
}

try
{
    new CourseRegistration.Builder("mona@mail.com", "SEF-101", AccessMode.VideosOnly).InGroup("G2").Build();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"  Rejected: {ex.Message}");
}
