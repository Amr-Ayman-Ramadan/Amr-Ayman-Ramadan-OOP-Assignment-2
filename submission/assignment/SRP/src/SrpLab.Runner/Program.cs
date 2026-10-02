using SrpLab.Appointments;
using SrpLab.Billing;
using SrpLab.Checkout;
using SrpLab.Enrollment;
using SrpLab.Grading;
using SrpLab.Kitchen;
using SrpLab.Lending;
using SrpLab.Support;
using SrpLab.Ward;
using SrpLab.Warehouse;

Console.WriteLine("SrpLab — 10 classes after SRP refactoring");
Console.WriteLine("=========================================");

// 1) WardBoard
var ward = new WardBoard();
var pagerLog = new PagerLog();
var admission = new BedAdmission(ward, new AcuityScorer(), new PagerPolicy(), pagerLog);
admission.Admit(1, "p-88", heartRate: 130, spo2: 89);
Console.WriteLine(new HandoffNoteWriter().Write(ward, 1));
Console.WriteLine(string.Join(" | ", pagerLog.Drain()));
Console.WriteLine(new CensusCsvExporter().Export(ward));

// 2) CheckoutBasket
var basket = new CheckoutBasket();
basket.AddLine("SKU-1", 40m, 2);
basket.ApplyCouponText("SAVE10");
basket.EnableGiftWrap();
var totals = new CheckoutTotalCalculator(new CouponDiscountCalculator(), new GiftWrapPolicy());
var grandTotal = totals.GrandTotal(basket);
var auth = new PaymentAuthorizer().Authorize(grandTotal, "4242", basket.Lines.Count);
Console.WriteLine($"basket total={grandTotal} auth={auth}");
Console.Write(new GiftMessageCardWriter().Write(basket, grandTotal, "Mona"));

// 3) SupportTicket
var intake = new TicketIntake(new TicketPriorityClassifier());
var sla = new SlaPolicy();
var ticket = intake.Open("T-1", "cannot login", "prod is down for me", DateTimeOffset.UtcNow);
Console.WriteLine(new PublicReplyWriter().Write(ticket, sla.Deadline(ticket), "Nora"));
Console.WriteLine(new EscalationBlurbWriter().Write(ticket, sla.Deadline(ticket)));

// 4) LoanDesk
var riskModel = new LoanRiskModel();
var loanDesk = new LoanDesk(riskModel, new LoanEligibilityPolicy(riskModel), new RequiredDocumentsChecklist());
var decision = loanDesk.Evaluate(new LoanApplication(60_000m, 640, 4, HasCollateral: false));
Console.WriteLine(new DecisionLetterWriter().Write(decision, "Omar"));
Console.WriteLine(new UnderwriterCsvExporter().Row("APP-1", decision));

// 5) CourseEnrollmentDesk
var roster = new CourseRoster(new Course("SEF-101", Capacity: 1, Tuition: 3000m));
Console.WriteLine(roster.Register("a@mail.com"));
Console.WriteLine(roster.Register("b@mail.com"));
Console.WriteLine(new WelcomePacketWriter().Write(roster, "b@mail.com", "Bea"));
Console.WriteLine(new TuitionInvoiceLineWriter(new VatCalculator()).Write(roster, "a@mail.com"));

// 6) KitchenTicket
var order = new KitchenOrder();
order.AddItem("Pasta", new[] { "wheat", "milk" }, 12);
var allergens = new AllergenDetector().Detect(order);
var eta = new PrepTimeEstimator().EstimateReadyMinutes(order, openStations: 2, hasAllergens: allergens.Count > 0);
Console.WriteLine(new ThermalTicketPrinter().Render(order, 42, eta, allergens));
Console.WriteLine(new ExpoLaneRouter().LaneFor(allergens.Count > 0, eta));

// 7) SubscriptionBilling
var sub = new Subscription("c-9", 99m, new DateOnly(2026, 9, 1), new DateOnly(2026, 10, 1));
sub.RegisterFailedPayment();
var proration = new ProrationCalculator();
var invoices = new InvoiceNumberSequence();
var invoiceNo = invoices.Next(sub.PeriodStart); // minting a number is now an explicit step
var balance = proration.Prorate(sub, sub.PeriodStart);
Console.WriteLine(new DunningEmailWriter(new DunningSeverityPolicy()).Write(sub, "Sara", new DateOnly(2026, 9, 20), balance, invoiceNo));
Console.WriteLine(new LedgerJournalExporter().Line(sub, invoiceNo, balance));

// 8) WarehousePickList
var pick = new WarehousePickList();
pick.AddNeed("BOLT", "A", 3, 10, 7);
pick.AddNeed("NUT", "B", 1, 5, 5);
var allocations = new StockAllocator().Allocate(pick);
var route = new PickPathPlanner().WalkingOrder(pick);
Console.WriteLine(new PickerScriptWriter().Write(route, allocations));
Console.WriteLine(new WmsBatchXmlExporter().Export("B-1", allocations));

// 9) GradeBook
var grades = new GradeBook();
grades.Record("s1", 92);
grades.Record("s1", 88);
var standings = new StandingCalculator(new LetterGradePolicy(), new HonorRollPolicy());
Console.WriteLine(new TranscriptWriter().Write(standings.For(grades, "s1"), "Ali"));
Console.WriteLine(new GradeCsvExporter().Export(grades.StudentIds.Select(id => standings.For(grades, id))));

// 10) AppointmentDesk
var hours = new ClinicHours(new TimeOnly(9, 0), new TimeOnly(17, 0), 30);
var book = new AppointmentBook(hours);
var slot = new SlotFinder(hours, book).FindNextSlot(DateTimeOffset.Parse("2026-09-21T08:00:00Z"), 48);
if (slot is null) throw new InvalidOperationException("no slot");
book.TryBook(slot.Value);
Console.WriteLine(new SmsReminderWriter().Write(slot.Value, "0100"));
Console.Write(new IcsCalendarWriter().Write(slot.Value, hours.SlotMinutes, "Ali", "Dr. Hany"));

Console.WriteLine("Done. Every class now has one reason to change.");
