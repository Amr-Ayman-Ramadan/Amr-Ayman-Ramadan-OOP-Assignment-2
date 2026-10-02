# Task 1.1 — Responsibilities of the 10 classes

For every class I list the separate **reasons to change** I found (who would ask for the change), why keeping them together is a problem, and the types I split it into in Task 1.2 (one sentence each = its single responsibility).

---

## 1. WardBoard

**Responsibilities found**
1. **Bed occupancy** — which patient is in which bed (ward staff / bed management).
2. **Clinical acuity scoring** — turning heart rate + SpO2 into a score (clinical team).
3. **Paging policy** — "acuity ≥ 8 → CODE-YELLOW" threshold (hospital policy, changes separately from the scoring formula).
4. **Pager log side effect** — `AssignBed` silently writes to a pager log; assigning a bed should not secretly send pages.
5. **Handoff note wording** — text format + tone words ESCALATE / WATCH / STABLE (nursing documentation).
6. **Census CSV export** — export file layout (reporting / integration).

**Why it is a problem:** it looks like "one ward thing", but the clinical formula, the paging threshold and the note format are owned by different people. Changing the CSV columns means touching the same class that decides when a nurse gets paged. The hidden side effect in `AssignBed` also makes the method hard to test and to reuse (you cannot assign a bed without paging).

**After refactoring**
| Type | Single responsibility |
|---|---|
| `WardBoard` | Stores which patient occupies which bed and their acuity. |
| `AcuityScorer` | Calculates the clinical acuity score from vital signs. |
| `PagerPolicy` | Decides whether an acuity score must page the team. |
| `PagerLog` | Keeps pager messages until they are drained. |
| `BedAdmission` | Runs the admission workflow (score → assign → page) so the side effect is explicit. |
| `HandoffNoteWriter` | Formats the nurse handoff note. |
| `CensusCsvExporter` | Produces the census CSV. |

---

## 2. CheckoutBasket

**Responsibilities found**
1. **Basket contents** — lines, quantities, subtotal.
2. **Coupon parsing / discount rules** — understanding marketing strings like `SAVE10`, `WELCOME10`, `FREESHIP` (marketing).
3. **Packaging fee policy** — gift wrap costs 4.99 (operations).
4. **Grand total calculation** — combining subtotal, discount and fees.
5. **Gift card copy** — customer-facing text (marketing / UX).
6. **Payment authorization** — building a payload and an auth code (payment gateway integration).

**Why it is a problem:** a new coupon campaign, a new packaging price, a new gift-card text and a new payment gateway are four unrelated changes that all land in one class. The payment code especially should never be in the same type as cart math — a bug in a text template could break checkout.

**After refactoring**
| Type | Single responsibility |
|---|---|
| `CheckoutBasket` (+ `BasketLine`) | Holds the items and the customer's choices (coupon text, gift wrap). |
| `CouponDiscountCalculator` | Turns a coupon text into a discount amount. |
| `GiftWrapPolicy` | Knows the gift-wrap fee. |
| `CheckoutTotalCalculator` | Computes the grand total from subtotal, discount and fees. |
| `GiftMessageCardWriter` | Writes the gift card text. |
| `PaymentAuthorizer` | Gets an authorization code from the (fake) gateway. |

---

## 3. SupportTicket

**Responsibilities found**
1. **Ticket data** — id, subject, body, opened time.
2. **Priority classification** — keyword heuristics ("down", "urgent"...) (support playbook).
3. **Hidden re-classification** — the constructor and `AppendCustomerMessage` silently recalculate priority.
4. **SLA policy** — hours per priority and breach check (operations).
5. **Public reply template** — CX wording.
6. **Internal escalation message** — internal format.

**Why it is a problem:** the keyword list will change every few weeks, SLA hours change with contracts, and the reply wording changes with the CX team — all in one class. The automatic recalculation inside the constructor is a side effect that you cannot turn off or test separately.

**After refactoring**
| Type | Single responsibility |
|---|---|
| `SupportTicket` | Holds the ticket data and conversation body. |
| `TicketPriorityClassifier` | Guesses a priority from the ticket text. |
| `TicketIntake` | Opens tickets and re-prioritizes them when the customer writes again. |
| `SlaPolicy` | Calculates the SLA deadline and whether it is breached. |
| `PublicReplyWriter` | Writes the public reply to the customer. |
| `EscalationBlurbWriter` | Writes the internal escalation line. |

---

## 4. LoanDesk

**Responsibilities found**
1. **Application data** — amount, credit score, employment, collateral.
2. **Risk model** — the scoring formula (risk committee).
3. **Eligibility cut-offs** — risk ≥ 55 and credit ≥ 580 (lending policy).
4. **Required documents** — compliance checklist (regulation).
5. **Decision letter** — legal / communication wording.
6. **Underwriter CSV export** — analytics schema.

**Why it is a problem:** the risk committee, compliance, legal and analytics all change for different reasons, but they all edit the same class. Changing a sentence in the letter forces recompiling/retesting the risk formula.

**After refactoring**
| Type | Single responsibility |
|---|---|
| `LoanApplication` | Holds the applicant's facts. |
| `LoanRiskModel` | Calculates the risk score. |
| `LoanEligibilityPolicy` | Decides if an application is eligible. |
| `RequiredDocumentsChecklist` | Lists the documents the applicant must provide. |
| `LoanDesk` (+ `LoanDecision`) | Runs the evaluation and returns one decision object. |
| `DecisionLetterWriter` | Writes the letter to the applicant. |
| `UnderwriterCsvExporter` | Produces the analytics CSV row. |

---

## 5. CourseEnrollmentDesk

**Responsibilities found**
1. **Course information** — code, capacity, tuition.
2. **Seat allocation / waitlist** — register, seated or waitlisted, positions.
3. **Waitlist promotion policy** — how many students are promoted when seats open (operations).
4. **Welcome packet content** — markdown text (marketing).
5. **Tuition invoice line** — finance export format.
6. **VAT rule** — 14% tax, hidden inside the invoice formatting (tax law).

**Why it is a problem:** a change in the VAT rate or in the welcome text should not touch the seat algorithm. The tax rate was buried in a string-formatting method, so finance could not find or reuse it.

**After refactoring**
| Type | Single responsibility |
|---|---|
| `Course` | Holds the course code, capacity and tuition. |
| `CourseRoster` | Manages seated students and the waitlist. |
| `WaitlistPromoter` | Applies the promotion policy when seats open. |
| `WelcomePacketWriter` | Writes the welcome packet markdown. |
| `VatCalculator` | Knows the VAT rate and calculates VAT. |
| `TuitionInvoiceLineWriter` | Formats the tuition invoice line. |

---

## 6. KitchenTicket

**Responsibilities found**
1. **Order contents** — items, ingredients, prep minutes.
2. **Allergen detection** — ingredient → allergen dictionary (food regulation).
3. **ETA estimation** — stations, parallel cooking, +3 minutes allergy protocol (kitchen operations).
4. **Thermal ticket layout** — 32 columns, separators, upper-case (printer vendor).
5. **Expo lane routing** — which lane the order goes to.

**Why it is a problem:** buying a new printer (different width) means editing the class that contains the allergen rules — a food-safety risk. Also `RenderThermalTicket` hard-codes `EstimatedReadyMinutes(2)`, so the printing code makes an operations decision.

**After refactoring**
| Type | Single responsibility |
|---|---|
| `KitchenOrder` (+ `KitchenOrderItem`) | Holds the dishes of one order. |
| `AllergenDetector` | Finds allergens in the ingredients. |
| `PrepTimeEstimator` | Estimates the ready time. |
| `ThermalTicketPrinter` | Lays out the ticket for the thermal printer. |
| `ExpoLaneRouter` | Chooses the expo lane. |

---

## 7. SubscriptionBilling

**Responsibilities found**
1. **Subscription data** — customer, price, period, failed payments.
2. **Proration** — finance calendar math.
3. **Invoice numbering** — a global static counter + format (operations / accounting).
4. **Dunning severity policy** — 1 → friendly, 2 → second notice, 3+ → final (collections).
5. **Dunning email copy** — the wording.
6. **Ledger export** — accounting journal format.

**Why it is a problem:** the worst one is the **hidden side effect**: `DunningEmail` (and `LedgerJournalLine`) call `NextInvoiceNumber()`, so just *previewing* an email burns an invoice number. Composing text should never change accounting state. Also the static counter is shared by every object and is not thread-safe.

**After refactoring**
| Type | Single responsibility |
|---|---|
| `Subscription` | Holds subscription data and counts failed payments. |
| `ProrationCalculator` | Calculates the prorated amount. |
| `InvoiceNumberSequence` | Mints invoice numbers (thread-safe, explicit). |
| `DunningSeverityPolicy` | Chooses the reminder severity. |
| `DunningEmailWriter` | Writes the email — receives the invoice number, never creates one. |
| `LedgerJournalExporter` | Formats the ledger line. |

---

## 8. WarehousePickList

**Responsibilities found**
1. **Pick lines** — what needs to be picked.
2. **Stock allocation / backorder policy** — min(needed, on hand).
3. **Walking path** — aisle/bin ordering heuristic (warehouse layout).
4. **Picker script wording** — handheld device text + shortage warning (UX).
5. **WMS XML batch** — integration contract.

**Why it is a problem:** a new warehouse layout, a new handheld UI and a new WMS version are three different teams. `PickerScript` even re-runs allocation and searches the lines again to find shortages, so presentation code contains business logic.

**After refactoring**
| Type | Single responsibility |
|---|---|
| `WarehousePickList` (+ `PickLine`, `Allocation`, `PickStop`) | Holds the lines to pick. |
| `StockAllocator` | Decides how much of each line can be allocated. |
| `PickPathPlanner` | Orders the stops for walking. |
| `PickerScriptWriter` | Writes the handheld instructions. |
| `WmsBatchXmlExporter` | Produces the WMS XML batch. |

---

## 9. GradeBook

**Responsibilities found**
1. **Score storage + averaging**.
2. **Letter band policy** — A/B/C/D/F cut-offs (faculty senate).
3. **Honor roll rule** — another academic rule.
4. **Transcript format** — registrar document.
5. **CSV export** — export layout.

**Why it is a problem:** changing the A cut-off and changing the CSV column order are unrelated decisions from different owners. The policies were also impossible to reuse without a whole grade book.

**After refactoring**
| Type | Single responsibility |
|---|---|
| `GradeBook` | Stores scores and averages them. |
| `LetterGradePolicy` | Maps an average to a letter. |
| `HonorRollPolicy` | Decides honor-roll eligibility. |
| `StandingCalculator` (+ `StudentStanding`) | Combines the book and the policies into a student's standing. |
| `TranscriptWriter` | Writes the plain-text transcript. |
| `GradeCsvExporter` | Writes the CSV export. |

---

## 10. AppointmentDesk

**Responsibilities found**
1. **Opening-hours policy** — working days, open/close, slot length (clinic HR / ops).
2. **Booking state** — which slots are taken.
3. **Slot search algorithm** — find next free slot.
4. **ICS calendar format** — calendar interoperability.
5. **SMS reminder copy** — messaging channel.

**Why it is a problem:** the clinic changing its weekend days, a calendar client needing a new ICS field, and marketing changing the SMS text all modify the same scheduler class.

**After refactoring**
| Type | Single responsibility |
|---|---|
| `ClinicHours` | Knows when the clinic is open and how long a slot is. |
| `AppointmentBook` | Remembers booked slots and books new ones. |
| `SlotFinder` | Searches for the next free slot. |
| `IcsCalendarWriter` | Serializes an appointment to ICS. |
| `SmsReminderWriter` | Writes the SMS reminder. |

---

### Behavior note
The runner prints the same results as before (same handoff note, totals, letters, tickets, invoice number, pick script, transcript, reminder). The auth code differs between runs in both versions because .NET randomizes `string.GetHashCode()` per process. I also print a few extra outputs (CSV, ICS, XML) so every new class is exercised.
One intentional fix: the invoice number is now minted explicitly once and passed to the email and ledger writers, instead of being consumed as a hidden side effect of writing text.
