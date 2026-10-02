using LibrarySystem;

Console.WriteLine("=== City Library System ===");

// ------------------------------------------------------------------
// 1) Things that must NOT compile (left commented on purpose)
// ------------------------------------------------------------------
// Person p = new Person("P1", "Ali", "0100");                     // must NOT compile: Person constructor is protected
// Member m = new Member("M1", "Ali", "0100", 3, 0m);              // must NOT compile: Member constructor is protected
// Staff s = new Staff("S1", "Ali", "0100", DateTime.Today, 5000m, 0m); // must NOT compile: Staff constructor is protected
// LibraryItem i = new LibraryItem("C1", "X", 1m, 21, 1m);         // must NOT compile: LibraryItem constructor is protected
// student.FullName = "New Name";                                   // must NOT compile: FullName is get-only
// student.Loans.Add(someLoan);                                     // must NOT compile: IReadOnlyList<Loan> has no Add
// book1.IsOnLoan = true;                                           // must NOT compile: IsOnLoan has a private setter
// loan.Status = LoanStatus.Returned;                               // must NOT compile: Status has a private setter
// staff.MonthlySalary = 99999m;                                    // must NOT compile: MonthlySalary has a private setter

// ------------------------------------------------------------------
// Setup
// ------------------------------------------------------------------
DateTime today = new DateTime(2026, 10, 1);

Librarian librarian = new Librarian("S-01", "Mona Adel", "01011111111", new DateTime(2020, 3, 1), 9000m);
HeadLibrarian head = new HeadLibrarian("S-02", "Hany Samir", "01022222222", new DateTime(2015, 6, 15), 15000m);
Shelver shelver = new Shelver("S-03", "Omar Fathy", "01033333333", new DateTime(2023, 1, 10), 6000m, "Fiction");

StudentMember student = new StudentMember("M-01", "Sara Ali", "01044444444");
PremiumMember premium = new PremiumMember("M-02", "Youssef Nabil", "01055555555", 20m);

Book book1 = new Book("B-001", "Clean Code", 2m);
Book book2 = new Book("B-002", "The Pragmatic Programmer", 2m);
Book book3 = new Book("B-003", "Refactoring", 2m);
Book book4 = new Book("B-004", "Design Patterns", 2m);
Dvd dvd = new Dvd("D-001", "Inception", 10m);
Magazine magazine = new Magazine("G-001", "National Geographic", 4m);

// ------------------------------------------------------------------
// 2) Business rules that must be rejected
// ------------------------------------------------------------------
Console.WriteLine("\n--- Rule: a withdrawn item cannot be borrowed ---");
head.Withdraw(magazine);
try
{
    student.Borrow("L-100", magazine, today);
    Console.WriteLine("ERROR: this should have failed");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Rejected: " + ex.Message);
}
head.Restore(magazine);
Console.WriteLine($"After restore, withdrawn = {magazine.IsWithdrawn}");

Console.WriteLine("\n--- Rule: an item already on loan cannot be borrowed by someone else ---");
student.Borrow("L-001", book1, today);
try
{
    premium.Borrow("L-002", book1, today);
    Console.WriteLine("ERROR: this should have failed");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Rejected: " + ex.Message);
}

Console.WriteLine("\n--- Rule: a Student member can have at most 3 active loans ---");
student.Borrow("L-003", book2, today);
student.Borrow("L-004", book3, today);
Console.WriteLine($"{student.FullName} has {student.CountActiveLoans()} active loans (max {student.MaxActiveLoans}).");
try
{
    student.Borrow("L-005", book4, today);
    Console.WriteLine("ERROR: this should have failed");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Rejected: " + ex.Message);
}
Console.WriteLine($"Is '{book4.Title}' on loan? {book4.IsOnLoan}  (the failed borrow changed nothing)");

Console.WriteLine("\n--- Rule: identity fields cannot be empty ---");
try
{
    StudentMember noName = new StudentMember("M-99", "", "0100");
    Console.WriteLine("ERROR: this should have failed");
}
catch (ArgumentException ex)
{
    Console.WriteLine("Rejected: " + ex.Message);
}

Console.WriteLine("\n--- Rule: raises and late fees must be positive ---");
try
{
    librarian.GiveRaise(0m);
    Console.WriteLine("ERROR: this should have failed");
}
catch (ArgumentException ex)
{
    Console.WriteLine("Rejected: " + ex.Message);
}
try
{
    head.ChangeLateFee(book1, -1m);
    Console.WriteLine("ERROR: this should have failed");
}
catch (ArgumentException ex)
{
    Console.WriteLine("Rejected: " + ex.Message);
}

Console.WriteLine("\n--- Rule: return date cannot be before the borrow date ---");
Loan earlyLoan = premium.Borrow("L-006", book4, today);
try
{
    librarian.ProcessReturn(earlyLoan, today.AddDays(-2));
    Console.WriteLine("ERROR: this should have failed");
}
catch (ArgumentException ex)
{
    Console.WriteLine("Rejected: " + ex.Message);
}
Console.WriteLine($"Loan {earlyLoan.LoanId} status is still {earlyLoan.Status}.");

// ------------------------------------------------------------------
// 3) Staff polymorphism through the parent: one List<Staff>
// ------------------------------------------------------------------
Console.WriteLine("\n--- Monthly pay of every staff member ---");
librarian.GiveRaise(10m);
shelver.Reassign("Children");
List<Staff> staffList = new List<Staff>();
staffList.Add(librarian);
staffList.Add(head);
staffList.Add(shelver);
foreach (Staff member in staffList)
{
    Console.WriteLine($"{member.FullName,-15} salary {member.MonthlySalary,9:0.00}  ->  monthly pay {member.GetMonthlyPay(),9:0.00}");
}
Console.WriteLine($"{shelver.FullName} was reassigned to section: {shelver.Section}");

// ------------------------------------------------------------------
// 4) Items through the parent: one List<LibraryItem>
// ------------------------------------------------------------------
Console.WriteLine("\n--- Loan period and daily late fee of every item kind ---");
List<LibraryItem> items = new List<LibraryItem>();
items.Add(book2);
items.Add(dvd);
items.Add(magazine);
foreach (LibraryItem item in items)
{
    Console.WriteLine($"{item.Title,-26} base {item.BaseLateFee,5:0.00}  loan period {item.LoanPeriodDays,2} days  daily late fee {item.DailyLateFee,5:0.00}");
}

// ------------------------------------------------------------------
// 5) Premium member returns a DVD 5 days late
// ------------------------------------------------------------------
Console.WriteLine("\n--- Premium member returns a DVD 5 days late ---");
Loan dvdLoan = premium.Borrow("L-007", dvd, today);
DateTime lateReturn = dvdLoan.DueDate.AddDays(5);
librarian.ProcessReturn(dvdLoan, lateReturn);
Console.WriteLine($"Borrowed:  {dvdLoan.BorrowDate:yyyy-MM-dd}");
Console.WriteLine($"Due date:  {dvdLoan.DueDate:yyyy-MM-dd}");
Console.WriteLine($"Returned:  {dvdLoan.ReturnDate:yyyy-MM-dd}");
Console.WriteLine($"Late fee:  5 days x {dvd.DailyLateFee:0.00} = {5 * dvd.DailyLateFee:0.00}, minus {premium.DiscountPercent}% discount = {dvdLoan.LateFee:0.00}");
Console.WriteLine($"Reading points of {premium.FullName}: {premium.ReadingPoints}");
Console.WriteLine($"Is the DVD on loan now? {dvd.IsOnLoan}");
Console.WriteLine($"{premium.FullName}'s loan history has {premium.Loans.Count} loans.");

// ------------------------------------------------------------------
// 6) Illegal status changes
// ------------------------------------------------------------------
Console.WriteLine("\n--- Rule: legal status changes only ---");
try
{
    librarian.ProcessReturn(dvdLoan, lateReturn);
    Console.WriteLine("ERROR: this should have failed");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Rejected (return twice): " + ex.Message);
}
try
{
    librarian.MarkAsLost(dvdLoan);
    Console.WriteLine("ERROR: this should have failed");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("Rejected (lost after return): " + ex.Message);
}

Loan lostLoan = student.Loans[0];
librarian.MarkAsLost(lostLoan);
Console.WriteLine($"Loan {lostLoan.LoanId} ('{lostLoan.Item.Title}') is now {lostLoan.Status}; {student.FullName} has {student.CountActiveLoans()} active loans.");

Console.WriteLine("\nDone.");
