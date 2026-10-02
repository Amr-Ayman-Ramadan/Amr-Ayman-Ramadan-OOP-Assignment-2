# Assignment 4 â€” SRP, Design Patterns & Inheritance

- **Name:** Amr Ayman Fathy Ramadan
- **Email:** amraymanramadan37@gmail.com

## Parts
| Folder | Task | Run |
|---|---|---|
| `submission/assignment/SRP/` | Part 01 â€” 10 classes refactored + `Responsibilities.md` | `dotnet run --project submission/assignment/SRP/src/SrpLab.Runner` |
| `submission/assignment/DesignPatterns/` | Part 02 â€” Singleton, Prototype, Builder + `linked.md` | `dotnet run --project submission/assignment/DesignPatterns/src/PatternsLab.Runner` |
| `submission/assignment/Inheritance/` | Part 03 â€” `ClassDiagram.png` + library system | `dotnet run --project submission/assignment/Inheritance/src` |
| `submission/assignment/LeetCode/1456_MaxVowelsInSubstring/` | Part 04 â€” sliding window solution | submitted on LeetCode |

## Repository layout
The website requires everything under `submission/`, so the folder tree from the PDF lives in `submission/assignment/`:
- `submission/assignment/` â€” SRP, DesignPatterns, Inheritance, LeetCode (same structure as the PDF)
- `submission/leetcode/account.md` â€” my LeetCode account
- `submission/linkedin/posts.md` â€” links to my 3 LinkedIn posts (also in `submission/assignment/DesignPatterns/linked.md`)

## Notes & assumptions

### SRP
- Each original class was split into small types inside its own folder/namespace (e.g. `SrpLab/Ward`, `SrpLab/Billing`). No interfaces were used.
- The runner prints the same results as the original. `AuthorizePaymentStub` output differs between runs in both versions because `string.GetHashCode()` is randomized per process.
- `SubscriptionBilling`: writing the dunning email no longer consumes an invoice number as a hidden side effect. The number is minted once with `InvoiceNumberSequence.Next()` and passed to the email and ledger writers.

### Design Patterns
- **Singleton:** `sealed` class, private constructor, `Lazy<AppConfig>` (thread-safe, loads once). `LoadCount` can only be read from outside.
- **Prototype:** `Enemy.Clone()` uses `MemberwiseClone()` (copies the private `_modelData` without running the slow constructor and keeps the real type), then deep-copies `Weapon` and `Abilities`. Bonus `EnemyRegistry` stores named prototypes.
- **Builder:** required values (email, course, access mode) are the builder's constructor parameters; optional values are fluent methods. `Build()` validates the LiveGroup / VideosOnly rules.

### Inheritance (library system)
- Only taught features are used: no `abstract`, `virtual/override`, interfaces, lambdas or LINQ. Non-instantiable parents use `protected` constructors.
- Per-kind values (max loans, discount, allowance, loan period, late-fee multiplier) are passed to the parent through `: base(...)`, so `GetMonthlyPay()` and `DailyLateFee` are written once with no type checks.
- Methods that change item / loan state (`MarkBorrowed`, `Return`, `ChangeBaseLateFee`, `Withdraw`...) are `internal`; the public entry points are `Member.Borrow`, `Librarian` and `HeadLibrarian` actions.
- Assumption: marking a loan as **lost** does not set `IsOnLoan` back to false (the manager said it changes only through borrowing and returning), so a lost item cannot be borrowed again.
- Assumption: a loan that is not returned yet (or is lost) has a late fee of 0 â€” the fee is calculated when the item comes back.
- Late fee = days late Ã— item daily late fee âˆ’ member discount %. Example: DVD base fee 10 â†’ 20/day, 5 days late â†’ 100, Premium 20% discount â†’ **80**.
