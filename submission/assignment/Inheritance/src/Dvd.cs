namespace LibrarySystem;

public class Dvd : LibraryItem
{
    public Dvd(string catalogNumber, string title, decimal baseLateFee)
        : base(catalogNumber, title, baseLateFee, 7, 2.0m)
    {
    }
}
