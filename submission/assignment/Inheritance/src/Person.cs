namespace LibrarySystem;

// Nobody is "just a person": the constructor is protected, so only Member / Staff children can call it.
public class Person
{
    public string PersonId { get; }
    public string FullName { get; }
    public string Phone { get; }

    protected Person(string personId, string fullName, string phone)
    {
        if (string.IsNullOrWhiteSpace(personId))
            throw new ArgumentException("Person ID cannot be empty.");
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty.");
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Phone number cannot be empty.");

        PersonId = personId;
        FullName = fullName;
        Phone = phone;
    }
}
