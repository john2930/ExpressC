using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
namespace Expenses.Model {
    public class LocalExpenseRepository : IExpenseRepository {
        List<Charge> Charges {get; set; }
        List<ExpenseReport> ExpenseReports { get; set; }
        List<Employee> Employees { get; set; }
        public LocalExpenseRepository() {
            Charges = new List<Charge>();
            ExpenseReports = new List<ExpenseReport>();
            Employees = new List<Employee>();
        }
        public void Save(Stream stream) {
            using(BinaryWriter bw = new BinaryWriter(stream)) {

                bw.Write(Employees.Count);
                foreach(Employee e in Employees) {
                    bw.Write(e.Alias);
                    bw.Write(e.EmployeeId);
                    bw.Write(e.Manager);
                    bw.Write(e.Name);
                }

                bw.Write(ExpenseReports.Count);
                foreach(ExpenseReport er in ExpenseReports) {

                    bw.Write((long)er.Amount);
                    bw.Write(er.Approver);
                    bw.Write(er.CostCenter);
                    
                    bw.Write(er.DateResolved == null ? 0L : er.DateResolved.Value.Ticks);
                    bw.Write(er.DateSaved == null ? 0L : er.DateSaved.Value.Ticks);
                    bw.Write(er.DateSubmitted == null ? 0L : er.DateSubmitted.Value.Ticks);
                    bw.Write(er.EmployeeId);
                    bw.Write(er.ExpenseReportId);
                    bw.Write(er.Notes);
                    bw.Write((long)er.OwedToCreditCard);
                    bw.Write((long)er.OwedToEmployee);
                    bw.Write(er.Purpose);
                    bw.Write((int)er.Status);                    
                }

                bw.Write(Charges.Count);
                
                foreach(Charge charge in Charges) {
                    bw.Write(charge.AccountNumber);
                    bw.Write((long)charge.BilledAmount);
                    bw.Write((int)charge.Category);
                    bw.Write(charge.ChargeId);
                    bw.Write(charge.Description);
                    bw.Write(charge.EmployeeId);
                    bw.Write(charge.ExpenseDate.Ticks);
                    bw.Write(charge.ExpenseReportId == null ? 0 : charge.ExpenseReportId.Value);
                    bw.Write((int)charge.ExpenseType);
                    bw.Write(charge.Location);
                    bw.Write(charge.Merchant);
                    bw.Write(charge.Notes);
                    bw.Write(charge.ReceiptRequired);
                    bw.Write((long)charge.TransactionAmount);
                }
            }
        }
        public void Load(Stream stream) {
            using(BinaryReader bw = new BinaryReader(stream)) {
                int count = bw.ReadInt32();
                for(int i = 0; i < count; i++) {
                    Employee employee = new Employee();
                    employee.Alias = bw.ReadString();
                    employee.EmployeeId = bw.ReadInt32();
                    employee.Manager = bw.ReadString();
                    employee.Name = bw.ReadString();
                    Employees.Add(employee);
                }

                count = bw.ReadInt32();
                for(int i = 0; i < count; i++) {
                    ExpenseReport er = new ExpenseReport();
                    er.Amount = bw.ReadInt64();
                    er.Approver = bw.ReadString();
                    er.CostCenter = bw.ReadInt32();
                    long tickCount = bw.ReadInt64();
                    if(tickCount != 0)
                        er.DateResolved = new DateTime(tickCount);
                    tickCount = bw.ReadInt64();
                    if(tickCount != 0)
                        er.DateSaved = new DateTime(tickCount);
                    tickCount = bw.ReadInt64();
                    if(tickCount != 0)
                        er.DateSubmitted = new DateTime(tickCount);
                    er.EmployeeId = bw.ReadInt32();
                    er.ExpenseReportId = bw.ReadInt32();
                    er.Notes = bw.ReadString();    
                    er.OwedToCreditCard = bw.ReadInt64();
                    er.OwedToEmployee = bw.ReadInt64();
                    er.Purpose = bw.ReadString();
                    er.Status = (ExpenseReportStatus)bw.ReadInt32();                   
                    ExpenseReports.Add(er);
                }
                count = bw.ReadInt32();

                for(int i = 0; i < count; i++) {
                    Charge charge = new Charge();
                    charge.AccountNumber = bw.ReadInt32();
                    charge.BilledAmount = bw.ReadInt64();
                    charge.Category = (CategoryType)bw.ReadInt32();
                    charge.ChargeId = bw.ReadInt32();
                    charge.Description = bw.ReadString();
                    charge.EmployeeId = bw.ReadInt32();
                    charge.ExpenseDate = new DateTime(bw.ReadInt64());
                    int value = bw.ReadInt32();
                    if(value != 0)
                        charge.ExpenseReportId = value;
                    charge.ExpenseType = (ChargeType)bw.ReadInt32();
                    charge.Location = bw.ReadString();
                    charge.Merchant = bw.ReadString();
                    charge.Notes = bw.ReadString();
                    charge.ReceiptRequired = bw.ReadBoolean();
                    charge.TransactionAmount = bw.ReadInt64();
                    Charges.Add(charge);
                }
            }
        }

        public void Create() {
            CreateEmployee("user", "manager", 1);
            CreateEmployee("manager", "user", 2);
        }
        void GenerateLocalDataBase() {

        }
        #region data generator
        Random random = new Random();

        public void CreateEmployee(string alias, string managerAlias, int id) {
            Employee employee = new Employee() {
                EmployeeId = id,
                Alias = alias,                
                Manager = managerAlias,
                Name = "New Employee",
            };
            AddFreeCharges(employee);
            AddSavedReports(managerAlias, employee);
            AddPendingReports(managerAlias, employee);
            AddApprovedReports(managerAlias, employee);
            Employees.Add(employee);
        }
        void AddPendingReports(string managerAlias, Employee employee) {
            for(int month = 0; month < 9; ++month) {
                int pendingReportsCount = 7 - month / 2;
                for(int reportIndex = 0; reportIndex < pendingReportsCount; ++reportIndex) {
                    string purpose = random.Next(2) == 1 ? "Visit to Tailspin Toys" : "Visit to Blue Yonder Airlines";
                    ExpenseReport expenseReport = new ExpenseReport() {
                        ExpenseReportId = ExpenseReports.Count + 1,
                        EmployeeId = employee.EmployeeId,
                        Amount = 450 + random.Next(640 - 450),
                        Approver = managerAlias,
                        CostCenter = 50992,
                        DateSubmitted = DateTime.Today.AddDays(-month * 30 - random.Next(30)),
                        Notes = (managerAlias == "user") ? "Kim Akers" : purpose,
                        OwedToCreditCard = 450M,
                        OwedToEmployee = 0M,
                        Purpose = purpose,
                        Status = ExpenseReportStatus.Pending,
                    };
                    ExpenseReports.Add(expenseReport);
                    AddCharges(month, expenseReport, employee);
                }
            }
        }
        void AddSavedReports(string managerAlias, Employee employee) {
            for(int month = 0; month < 6; ++month) {
                int pendingReportsCount = 3 - month / 2;
                for(int reportIndex = 0; reportIndex < pendingReportsCount; ++reportIndex) {
                    string purpose = random.Next(2) == 1 ? "Visit to Tailspin Toys" : "Visit to Blue Yonder Airlines";
                    ExpenseReport expenseReport = new ExpenseReport() {
                        ExpenseReportId = ExpenseReports.Count + 1,
                        EmployeeId = employee.EmployeeId,
                        Amount = 450 + random.Next(640 - 450),
                        Approver = managerAlias,
                        CostCenter = 50992,
                        DateSaved = DateTime.Today.AddDays(-month * 30 - random.Next(30)),
                        Notes = (managerAlias == "user") ? "Kim Akers" : purpose,
                        OwedToCreditCard = 450M,
                        OwedToEmployee = 0M,
                        Purpose = purpose,
                        Status = (int)ExpenseReportStatus.Saved,
                    };
                    ExpenseReports.Add(expenseReport);
                    AddCharges(month, expenseReport, employee);
                }
            }
        }
        void AddApprovedReports(string managerAlias, Employee employee) {
            DateTime today = DateTime.Today;
            DateTime currentMonthStart = new DateTime(today.Year, today.Month, 1);
            for(DateTime monthStart = currentMonthStart.AddMonths(-2); monthStart > today.AddYears(-3); monthStart = monthStart.AddMonths(-1)) {
                int daysInMonth = (int)(monthStart.AddMonths(1) - monthStart).TotalDays;
                int b1 = 250 + random.Next(100);
                int b2 = 100 + random.Next(20);
                int b3 = 100 + random.Next(10);
                int b4 = 310 + random.Next(90);
                ExpenseReport expenseReport = new ExpenseReport() {
                    ExpenseReportId = ExpenseReports.Count + 1,
                    EmployeeId = employee.EmployeeId,
                    Amount = b1 + b2 + b3 + b4,
                    Approver = managerAlias,
                    CostCenter = 50992,
                    DateSubmitted = monthStart.AddDays((today.Day - 5 + daysInMonth - 1) % (daysInMonth - 1) + 1),
                    DateResolved = monthStart.AddDays((today.Day + daysInMonth - 1) % (daysInMonth - 1) + 1),
                    Notes = "Visit to Tailspin Toys",
                    OwedToCreditCard = 850M,
                    OwedToEmployee = 0M,
                    Purpose = "Customer visit",
                    Status = ExpenseReportStatus.Approved
                };
                ExpenseReports.Add(expenseReport);
                Charges.Add(new Charge() {
                    ChargeId = Charges.Count + 1,
                    AccountNumber = 723000,
                    BilledAmount = b1,
                    Category = (int)CategoryType.AirFare,
                    Description = "Airfare to Chicago",
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = expenseReport.ExpenseReportId,
                    ExpenseDate = monthStart.AddDays((today.Day - 15 + daysInMonth - 1) % (daysInMonth - 1) + 1),
                    ExpenseType = ChargeType.Business,
                    Location = "Chicago, IL",
                    Merchant = "Blue Yonder Airlines",
                    Notes = string.Empty,
                    ReceiptRequired = true,
                    TransactionAmount = 350M,
                });
                Charges.Add(new Charge() {
                    ChargeId = Charges.Count + 1,
                    AccountNumber = 723000,
                    BilledAmount = b2,
                    Category = CategoryType.OtherTravelAndLodging,
                    Description = "Cab from airport",
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = expenseReport.ExpenseReportId,
                    ExpenseDate = monthStart.AddDays((today.Day - 5 + daysInMonth - 1) % (daysInMonth - 1) + 1),
                    ExpenseType = ChargeType.Business,
                    Location = "Chicago, IL",
                    Merchant = "Contoso Taxi",
                    Notes = string.Empty,
                    ReceiptRequired = false,
                    TransactionAmount = 50M,
                });
                Charges.Add(new Charge() {
                    ChargeId = Charges.Count + 1,
                    AccountNumber = 723000,
                    BilledAmount = b3,
                    Category = CategoryType.TEMeals,
                    Description = "Cab to airport",
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = expenseReport.ExpenseReportId,
                    ExpenseDate = monthStart.AddDays((today.Day - 3 + daysInMonth - 1) % (daysInMonth - 1) + 1),
                    ExpenseType = ChargeType.Business,
                    Location = "Chicago, IL",
                    Merchant = "Contoso Taxi",
                    Notes = string.Empty,
                    ReceiptRequired = false,
                    TransactionAmount = 50M,
                });
                Charges.Add(new Charge() {
                    ChargeId = Charges.Count + 1,
                    AccountNumber = 723000,
                    BilledAmount = b4,
                    Category = CategoryType.Hotel,
                    Description = "2 nights hotel",
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = expenseReport.ExpenseReportId,
                    ExpenseDate = monthStart.AddDays((today.Day - 3 + daysInMonth - 1) % (daysInMonth - 1) + 1),
                    ExpenseType = ChargeType.Business,
                    Location = "Chicago, IL",
                    Merchant = "Northwind Inn",
                    Notes = string.Empty,
                    ReceiptRequired = true,
                    TransactionAmount = 400M,
                });
                int c1 = 330 + random.Next(120);
                int c2 = 120 + random.Next(10);
                expenseReport = new ExpenseReport() {
                    ExpenseReportId = ExpenseReports.Count + 1,
                    Amount = c1,
                    Approver = managerAlias,
                    CostCenter = 50992,
                    DateSubmitted = monthStart.AddDays((today.Day - 5 + daysInMonth - 1) % (daysInMonth - 1) + 1),
                    DateResolved = monthStart.AddDays((today.Day + daysInMonth - 1) % (daysInMonth - 1) + 1),
                    Notes = "",
                    OwedToCreditCard = 0,
                    OwedToEmployee = 50M,
                    Purpose = "Last month's cell phone",
                    Status = ExpenseReportStatus.Approved,
                };
                ExpenseReports.Add(expenseReport);
                Charges.Add(new Charge() {
                    ChargeId = Charges.Count + 1,
                    AccountNumber = 742000,
                    BilledAmount = b3,
                    Category = CategoryType.Entertainment,
                    Description = "Cell phone bill",
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = expenseReport.ExpenseReportId,
                    ExpenseDate = monthStart.AddDays((today.Day - 10 + daysInMonth - 1) % (daysInMonth - 1) + 1),
                    ExpenseType = ChargeType.Personal,
                    Location = "Seattle, WA",
                    Merchant = "The Phone Company",
                    Notes = string.Empty,
                    ReceiptRequired = true,
                    TransactionAmount = 50M,
                });
                Charges.Add(new Charge() {
                    ChargeId = Charges.Count + 1,
                    AccountNumber = 742000,
                    BilledAmount = c2,
                    Category = CategoryType.Mileage,
                    Description = "Cell phone bill",
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = expenseReport.ExpenseReportId,
                    ExpenseDate = monthStart.AddDays((today.Day - 10 + daysInMonth - 1) % (daysInMonth - 1) + 1),
                    ExpenseType = ChargeType.Personal,
                    Location = "Seattle, WA",
                    Merchant = "The Phone Company",
                    Notes = string.Empty,
                    ReceiptRequired = true,
                    TransactionAmount = 50M,
                });
                Charges.Add(new Charge() {
                    ChargeId = Charges.Count + 1,
                    AccountNumber = 742000,
                    BilledAmount = b4,
                    Category = CategoryType.Hotel,
                    Description = "Cell phone bill",
                    EmployeeId = employee.EmployeeId,
                    ExpenseReportId = expenseReport.ExpenseReportId,
                    ExpenseDate = monthStart.AddDays((today.Day - 10 + daysInMonth - 1) % (daysInMonth - 1) + 1),
                    ExpenseType = ChargeType.Personal,
                    Location = "Seattle, WA",
                    Merchant = "The Phone Company",
                    Notes = string.Empty,
                    ReceiptRequired = true,
                    TransactionAmount = 50M,
                });
            }
        }
        void AddFreeCharges(Employee employee) {
            for(int month = 0; month < 9; ++month) {
                int chargesCount = (int)(13 - 1.3 * month) - 2;
                for(int chargeIndex = 0; chargeIndex < chargesCount; ++chargeIndex) {
                    int descRef = random.Next() % 100000000;
                    CategoryType category = (CategoryType)random.Next(10);
                    if(category == CategoryType.Other)
                        category = CategoryType.AirFare;
                    int amountBase;
                    switch(category) {
                        case CategoryType.AirFare: amountBase = 300; break;
                        case CategoryType.CarRental: amountBase = 400; break;
                        case CategoryType.ConferenceSeminar: amountBase = 200; break;
                        case CategoryType.Entertainment: amountBase = 120; break;
                        case CategoryType.Gifts: amountBase = 300; break;
                        case CategoryType.Hotel: amountBase = 500; break;
                        case CategoryType.Mileage: amountBase = 300; break;
                        case CategoryType.OtherTravelAndLodging: amountBase = 300; break;
                        case CategoryType.TEMeals: amountBase = 300; break;
                        default: amountBase = 80; break;
                    }
                    int amount = amountBase + random.Next(30);
                    Charges.Add(new Charge() {
                        ChargeId = Charges.Count + 1,
                        AccountNumber = 723000,
                        BilledAmount = amount,
                        Category = category,
                        Description = "REF# " + descRef.ToString(),
                        EmployeeId = employee.EmployeeId,
                        ExpenseDate = DateTime.Today.AddDays(-30 * month - random.Next(30)),
                        ExpenseType = ChargeType.Business,
                        Location = random.Next(2) == 1 ? "San Francisco, CA" : random.Next(2) == 1 ? "Seattle, WA" : "Chicago, IL",
                        Merchant = random.Next(5) != 4 ? "Northwind Inn" : "Contoso Taxi",
                        Notes = string.Empty,
                        ReceiptRequired = true,
                        TransactionAmount = (decimal)(0.75 * amount),
                    });
                }
            }
        }
        void AddCharges(int month, ExpenseReport expenseReport, Employee employee) {
            int amount1 = month == 1 ? 0 : month == 2 ? 40 : month == 3 ? 20 : month == 4 ? 40 : 80;
            int amount2 = month == 1 ? 10 : month == 2 ? 120 : month == 3 ? 40 : month == 4 ? 80 : 30;
            int amount3 = month == 1 ? 20 : month == 2 ? 40 : month == 3 ? 50 : month == 4 ? 22 : 90;
            int amount4 = month == 1 ? 120 : month == 2 ? 30 : month == 3 ? 40 : month == 4 ? 28 : 50;
            int amount5 = month == 1 ? 80 : month == 2 ? 20 : month == 3 ? 30 : month == 4 ? 14 : 13;
            int amount6 = month == 1 ? 40 : month == 2 ? 120 : month == 3 ? 200 : month == 4 ? 56 : 88;
            Charges.Add(new Charge() {
                ChargeId = Charges.Count + 1,
                AccountNumber = 723000,
                BilledAmount = amount1 + random.Next(10),
                Category = (int)CategoryType.AirFare,
                Description = "Airfare to San Francisco",
                EmployeeId = employee.EmployeeId,
                ExpenseReportId = expenseReport.ExpenseReportId,
                ExpenseDate = DateTime.Today.AddDays(-60),
                ExpenseType = ChargeType.Business,
                Location = "Chicago, IL",
                Merchant = "Blue Yonder Airlines",
                Notes = string.Empty,
                ReceiptRequired = true,
                TransactionAmount = 350M,
            });
            Charges.Add(new Charge() {
                ChargeId = Charges.Count + 1,
                AccountNumber = 723000,
                BilledAmount = amount2 + random.Next(10),
                Category = CategoryType.OtherTravelAndLodging,
                Description = "Cab from airport",
                EmployeeId = employee.EmployeeId,
                ExpenseReportId = expenseReport.ExpenseReportId,
                ExpenseDate = DateTime.Today.AddDays(-45),
                ExpenseType = ChargeType.Business,
                Location = "San Francisco, CA",
                Merchant = "Contoso Taxi",
                Notes = string.Empty,
                ReceiptRequired = false,
                TransactionAmount = 50,
            });
            Charges.Add(new Charge() {
                ChargeId = Charges.Count + 1,
                AccountNumber = 723000,
                BilledAmount = amount3 + random.Next(10),
                Category = CategoryType.Mileage,
                Description = "Cab to airport",
                EmployeeId = employee.EmployeeId,
                ExpenseReportId = expenseReport.ExpenseReportId,
                ExpenseDate = DateTime.Today.AddDays(-45),
                ExpenseType = ChargeType.Business,
                Location = "San Francisco, CA",
                Merchant = "Contoso Taxi",
                Notes = string.Empty,
                ReceiptRequired = false,
                TransactionAmount = 50,
            });
            Charges.Add(new Charge() {
                ChargeId = Charges.Count + 1,
                AccountNumber = 723000,
                BilledAmount = amount4 + random.Next(10),
                Category = CategoryType.Hotel,
                Description = "Cab to airport",
                EmployeeId = employee.EmployeeId,
                ExpenseReportId = expenseReport.ExpenseReportId,
                ExpenseDate = DateTime.Today.AddDays(-45),
                ExpenseType = ChargeType.Business,
                Location = "San Francisco, CA",
                Merchant = "Contoso Taxi",
                Notes = string.Empty,
                ReceiptRequired = false,
                TransactionAmount = 50,
            });
            Charges.Add(new Charge() {
                ChargeId = Charges.Count + 1,
                AccountNumber = 723000,
                BilledAmount = amount5 + random.Next(10),
                Category = CategoryType.CarRental,
                Description = "Cab to airport",
                EmployeeId = employee.EmployeeId,
                ExpenseReportId = expenseReport.ExpenseReportId,
                ExpenseDate = DateTime.Today.AddDays(-45),
                ExpenseType = ChargeType.Business,
                Location = "San Francisco, CA",
                Merchant = "Contoso Taxi",
                Notes = string.Empty,
                ReceiptRequired = false,
                TransactionAmount = 50,
            });
            Charges.Add(new Charge() {
                ChargeId = Charges.Count + 1,
                AccountNumber = 723000,
                BilledAmount = amount5 + random.Next(10),
                Category = CategoryType.ConferenceSeminar,
                Description = "Cab to airport",
                EmployeeId = employee.EmployeeId,
                ExpenseReportId = expenseReport.ExpenseReportId,
                ExpenseDate = DateTime.Today.AddDays(-45),
                ExpenseType = ChargeType.Business,
                Location = "San Francisco, CA",
                Merchant = "Contoso Taxi",
                Notes = string.Empty,
                ReceiptRequired = false,
                TransactionAmount = 50,
            });
            Charges.Add(new Charge() {
                ChargeId = Charges.Count + 1,
                AccountNumber = 723000,
                BilledAmount = amount6 + random.Next(10),
                Category = CategoryType.Entertainment,
                Description = "Cab to airport",
                EmployeeId = employee.EmployeeId,
                ExpenseReportId = expenseReport.ExpenseReportId,
                ExpenseDate = DateTime.Today.AddDays(-45),
                ExpenseType = ChargeType.Business,
                Location = "San Francisco, CA",
                Merchant = "Contoso Taxi",
                Notes = string.Empty,
                ReceiptRequired = false,
                TransactionAmount = 50,
            });
            Charges.Add(new Charge() {
                ChargeId = Charges.Count + 1,
                AccountNumber = 723000,
                BilledAmount = amount6 + random.Next(10),
                Category = CategoryType.Gifts,
                Description = "Cab to airport",
                EmployeeId = employee.EmployeeId,
                ExpenseReportId = expenseReport.ExpenseReportId,
                ExpenseDate = DateTime.Today.AddDays(-45),
                ExpenseType = ChargeType.Business,
                Location = "San Francisco, CA",
                Merchant = "Contoso Taxi",
                Notes = string.Empty,
                ReceiptRequired = false,
                TransactionAmount = 50,
            });
        }
        #endregion

        Employee GetEmployee(int employeeId) {
            return Employees.FirstOrDefault(item => item.EmployeeId == employeeId);
        }
        #region IExpenseRepository
        public Employee GetEmployee(string employeeAlias) {
            return Employees.FirstOrDefault(item => item.Alias == employeeAlias);
        }

        public int SaveEmployee(Employee employee) {            
            Employee dbEmployee;                
            dbEmployee = Employees.Single(item => item.EmployeeId == employee.EmployeeId);
            dbEmployee.Alias = employee.Alias;
            dbEmployee.Manager = employee.Manager;
            dbEmployee.Name = employee.Name;                
            return dbEmployee.EmployeeId;                            
        }

        public List<SummaryItem> GetSummaryItems(int employeeId) {
            
            Employee employee = Employees.FirstOrDefault(item => item.EmployeeId == employeeId);
            if (employee == null) {
                throw new ArgumentException("Employee not found");
            }

            List<SummaryItem> summaryItems = new List<SummaryItem>();

            summaryItems.AddRange(
                GetOutstandingCharges(employeeId).Select(
                    item =>
                        new SummaryItem()
                        {
                            Amount = item.TransactionAmount,
                            Date = item.ExpenseDate,
                            Description = (!string.IsNullOrWhiteSpace(item.Location)) ? string.Format("{0} ({1})", item.Merchant, item.Location) : item.Merchant,
                            Id = item.ChargeId,
                            ItemType = ItemType.Charge,
                            Submitter = employee.Name,
                        }));

            summaryItems.AddRange(
                GetExpenseReportsByStatus(employeeId, ExpenseReportStatus.Saved).Select(
                    item =>
                        new SummaryItem()
                        {
                            Amount = item.Amount,
                            Date = item.DateSaved,
                            Description = item.Purpose,
                            Id = item.ExpenseReportId,
                            ItemType = ItemType.SavedReport,
                            Submitter = employee.Name,
                        }));

            summaryItems.AddRange(
                GetExpenseReportsByStatus(employeeId, ExpenseReportStatus.Pending).Select(
                    item =>
                        new SummaryItem()
                        {
                            Amount = item.Amount,
                            Date = item.DateSubmitted,
                            Description = item.Purpose,
                            Id = item.ExpenseReportId,
                            ItemType = ItemType.PendingReport,
                            Submitter = employee.Name,
                        }));

            summaryItems.AddRange(
                        GetExpenseReportsForApproval(employee.Alias).Select(
                            item =>
                                new SummaryItem()
                                {
                                    Amount = item.Amount,
                                    Date = item.DateSubmitted,
                                    Description = string.Format("{0} ({1})", item.Purpose, this.GetEmployee(item.EmployeeId).Name),
                                    Id = item.ExpenseReportId,
                                    ItemType = ItemType.UnresolvedReport,
                                    Submitter = employee.Name,
                                }));

            return summaryItems;
        }

        public ExpenseReport GetExpenseReport(int expenseReportId) {            
            return ExpenseReports.FirstOrDefault(item => item.ExpenseReportId == expenseReportId);            
        }

        public List<ExpenseReport> GetExpenseReports(int employeeId) {    
            return ExpenseReports.Where(item => item.EmployeeId == employeeId).ToList();
        }

        public List<ExpenseReport> GetExpenseReportsByStatus(int employeeId, ExpenseReportStatus status) {
            var filteredReports = ExpenseReports.Where(item => ((item.EmployeeId == employeeId) && (item.Status == status)));
            return filteredReports.ToList();
        }

        public List<ExpenseReport> GetExpenseReportsForApproval(string employeeAlias) {    
            return ExpenseReports.Where(item => ((item.Approver == employeeAlias) && (item.Status == ExpenseReportStatus.Pending))).ToList();
        }

        public List<Charge> GetCharges(int expenseReportId) {
            return Charges.Where(item => item.ExpenseReportId == expenseReportId).ToList();
        }

        public Charge GetCharge(int chargeId) {
            return Charges.FirstOrDefault(item => item.ChargeId == chargeId);
        }

        public List<Charge> GetOutstandingCharges(int employeeId) {    
            return Charges.Where(item => ((item.EmployeeId == employeeId) && (item.ExpenseReportId == null))).ToList();
        }

        public decimal GetAmountOwedToCreditCard(int employeeId) {    
            return GetExpenseReportsByStatus(employeeId, ExpenseReportStatus.Pending).Sum(item => item.OwedToCreditCard);
        }

        public decimal GetAmountOwedToEmployee(int employeeId) {            
            return this.GetExpenseReportsByStatus(employeeId, ExpenseReportStatus.Pending).Sum(item => item.OwedToEmployee);            
        }

        public int SaveCharge(Charge charge) {         
            Charge dbCharge;
            if(charge.ChargeId == 0)
                dbCharge = new Charge() { ChargeId = Charges.Count };
            else
                dbCharge = Charges.Single(item => item.ChargeId == charge.ChargeId);
            dbCharge.AccountNumber = charge.AccountNumber;
            dbCharge.BilledAmount = charge.BilledAmount;
            dbCharge.Category = charge.Category;
            dbCharge.Description = charge.Description;
            dbCharge.EmployeeId = charge.EmployeeId;
            dbCharge.ExpenseDate = charge.ExpenseDate;
            dbCharge.ExpenseReportId = charge.ExpenseReportId;
            dbCharge.ExpenseType = charge.ExpenseType;
            dbCharge.Location = charge.Location;
            dbCharge.Merchant = charge.Merchant;
            dbCharge.Notes = charge.Notes;
            dbCharge.ReceiptRequired = charge.ReceiptRequired;
            dbCharge.TransactionAmount = charge.TransactionAmount;
            if(charge.ChargeId == 0)
                Charges.Add(dbCharge);
            return dbCharge.ChargeId;
        }

        public void DeleteCharge(int chargeId) {
            Charge dbCharge;
            dbCharge = Charges.Single(item => item.ChargeId == chargeId);
            Charges.Remove(dbCharge);
        }

        public int SaveExpenseReport(ExpenseReport expenseReport) {
            ExpenseReport dbExpenseReport;
            if(expenseReport.ExpenseReportId == 0)
                dbExpenseReport = new ExpenseReport() { ExpenseReportId = ExpenseReports.Count + 1 };
            else
                dbExpenseReport = ExpenseReports.Single(item => item.ExpenseReportId == expenseReport.ExpenseReportId);
            dbExpenseReport.Amount = expenseReport.Amount;
            dbExpenseReport.Approver = expenseReport.Approver;
            dbExpenseReport.CostCenter = expenseReport.CostCenter;
            dbExpenseReport.DateResolved = expenseReport.DateResolved;
            dbExpenseReport.DateSaved = expenseReport.DateSaved;
            dbExpenseReport.DateSubmitted = expenseReport.DateSubmitted;
            dbExpenseReport.EmployeeId = expenseReport.EmployeeId;
            dbExpenseReport.Notes = expenseReport.Notes;
            dbExpenseReport.OwedToCreditCard = expenseReport.OwedToCreditCard;
            dbExpenseReport.OwedToEmployee = expenseReport.OwedToEmployee;
            dbExpenseReport.Purpose = expenseReport.Purpose;
            dbExpenseReport.Status = expenseReport.Status;
            if(expenseReport.ExpenseReportId == 0)
                ExpenseReports.Add(dbExpenseReport);
            return dbExpenseReport.ExpenseReportId;
        }

        public void DeleteExpenseReport(int expenseReportId) {
            ExpenseReport dbExpenseReport;
            dbExpenseReport = ExpenseReports.Single(item => item.ExpenseReportId == expenseReportId);
            ExpenseReports.Remove(dbExpenseReport);
        }

        public void ResetData() {
            Employees.Clear();
            ExpenseReports.Clear();
            Charges.Clear();
        }
        #endregion
    }
}