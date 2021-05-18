using System;
using System.Linq;
using System.Threading.Tasks;
using hrmApp.Core.Models;
using Serilog;

// Seeding data to Database for code checking processes...


namespace hrmApp.Data.Seed
{
    public static class TestData
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any Project
            if (context.Projects.Any())
            {
                Log.Information("Database already seeded.");
                return;   // DB has been seeded
            }

            await InitProjects(context);
            Log.Information("Projects seeded.");

            await InitOrganizations(context);
            Log.Information("Organizations seeded.");

            await InitJobs(context);
            Log.Information("Jobs seeded.");

            await InitProcessStatuses(context);
            Log.Information("DocTypes seeded.");

            await InitDocTypes(context);
            Log.Information("DocTypes seeded.");

            await InitEmployees(context);
            Log.Information("Employees seeded.");

            // await InitProjectOrganizations(context);
            // Log.Information("Projects-Organizations relation seeded.");

            // await InitPOEmployees(context);
            // Log.Information("Projects-Organizations-Employees relation seeded.");

        }

        public static async Task InitProjects(ApplicationDbContext context)
        {
            if (context.Projects.Any())
            {
                return;   // DB has been seeded
            }
            context.Projects.AddRange(
                new Project
                {
                    ProjectName = "2014 KDKP MaNDA",
                    NumberOfEmployee = 120,
                    StartDate = DateTime.Parse("2014.03.10"),
                    EndDate = DateTime.Parse("2015.02.28"),
                    Description = "MaNDA - 2014/2015. évi Kulturális Közfoglalkoztatási MintaProgram",
                    IsActive = false
                },
                new Project
                {
                    ProjectName = "2015 KDKP MaNDA",
                    NumberOfEmployee = 120,
                    StartDate = DateTime.Parse("2015.03.10"),
                    EndDate = DateTime.Parse("2016.02.28"),
                    Description = "MaNDA - 2015/2016. évi Kulturális Közfoglalkoztatási Mintaprogram",
                    IsActive = false
                },
                new Project
                {
                    ProjectName = "2016 KDKP MaNDA",
                    NumberOfEmployee = 120,
                    StartDate = DateTime.Parse("2016.03.10"),
                    EndDate = DateTime.Parse("2017.02.28"),
                    Description = "MaNDA - 2016/2017. évi Kulturális Közfoglalkoztatási Program",
                    IsActive = false
                },
                new Project
                {
                    ProjectName = "2017 KDKP FH",
                    NumberOfEmployee = 120,
                    StartDate = DateTime.Parse("2017.03.10"),
                    EndDate = DateTime.Parse("2018.02.28"),
                    Description = "Forum Hungaricum - 2017/2018. évi Kulturális Közfoglalkoztatási Program",
                    IsActive = false
                },
                new Project
                {
                    ProjectName = "2018 KDKP FH",
                    NumberOfEmployee = 120,
                    StartDate = DateTime.Parse("2018.06.12"),
                    EndDate = DateTime.Parse("2019.02.28"),
                    Description = "Forum Hungaricum - 2018/2019. évi Kulturális Közfoglalkoztatási Program",
                    IsActive = false
                },
                new Project
                {
                    ProjectName = "2019 KDKP FH",
                    NumberOfEmployee = 120,
                    StartDate = DateTime.Parse("2019.03.10"),
                    EndDate = DateTime.Parse("2020.02.28"),
                    Description = "Forum Hungaricum - 2019/2020. évi Kulturális Közfoglalkoztatási Program",
                    IsActive = true
                }
            );
            await context.SaveChangesAsync();
        }

        public static async Task InitOrganizations(ApplicationDbContext context)
        {
            if (context.Organizations.Any())
            {
                return;   // DB has been seeded
            }
            context.Organizations.AddRange(
             new Organization
             {
                 OrganizationName = "Forum Hungaricum Nonprofit Kft.",
                 City = "Budapest",
                 Address = "Márvány utca 18.",
                 ContactName = "Bartók Gabriella",
                 ContactEmail = "b.gabi@forumhungaricum.hu",
                 ContactPhone = "06-30-123-4567",
                 Description = "Központi ügyintézés helye...",
                 IsActive = true,
                 PreferOrder = 10
             },
                new Organization
                {
                    OrganizationName = "OSZK",
                    City = "Budapest",
                    Address = "Vár u. 1.",
                    ContactName = "Kiss István",
                    ContactEmail = "kissistvan@oszk.hu",
                    ContactPhone = "06-30-123-4567",
                    Description = "Országos Széchényi Könyvtár - Tüske László főigazgató",
                    IsActive = true,
                    PreferOrder = 20
                },
                new Organization
                {
                    OrganizationName = "Kereskedelmi Múzeum",
                    City = "Budapest",
                    Address = "Közker tér 2.",
                    ContactName = "Juhász Klára",
                    ContactEmail = "juhasz.klara@kermuz.hu",
                    ContactPhone = "06-30-123-4857",
                    Description = "...",
                    IsActive = true,
                    PreferOrder = 30
                },
                new Organization
                {
                    OrganizationName = "Szolnoki Városi Könyvtár",
                    City = "Szolnok",
                    Address = "Könyves Kálmán u. 100.",
                    ContactName = "Oros Tibor",
                    ContactEmail = "otibi@gmail.hu",
                    ContactPhone = "06-70-153-4567",
                    Description = "",
                    IsActive = true,
                    PreferOrder = 40
                },
                new Organization
                {
                    OrganizationName = "Országgyűlési Könyvtár",
                    City = "Budapest",
                    Address = "Fő tér 1.",
                    ContactName = "Baán Viktoria",
                    ContactEmail = "baan.viktory@parlament.hu",
                    ContactPhone = "06-20-333-6895",
                    Description = "",
                    IsActive = true,
                    PreferOrder = 50
                },
                new Organization
                {
                    OrganizationName = "Mezőkövesdi Múzeum",
                    City = "Mezőkövesd",
                    Address = "Nagyrét u. 21.",
                    ContactName = "Field Eszter",
                    ContactEmail = "feszer@stoneoffice.com",
                    ContactPhone = "06-30-123-4567",
                    Description = "12 db munkaállomást kértnek",
                    IsActive = true,
                    PreferOrder = 60
                },
                new Organization
                {
                    OrganizationName = "RAAB Institute",
                    City = "Győr",
                    Address = "Gyorskocsi u. 1.",
                    ContactName = "Snell Irén",
                    ContactEmail = "s.irene@audi.de",
                    ContactPhone = "06-80-923-4563",
                    Description = "",
                    IsActive = true,
                    PreferOrder = 70
                },
                new Organization
                {
                    OrganizationName = "Digitális Erőmű",
                    City = "Ózd",
                    Address = "Gyár utca 1.",
                    ContactName = "Acél Áron",
                    ContactEmail = "aaron@karbontanya.hu",
                    ContactPhone = "06-60-656-4267",
                    Description = "50 fő elhelyezése megoldható.",
                    IsActive = true,
                    PreferOrder = 80
                },
                new Organization
                {
                    OrganizationName = "MTA BTK",
                    City = "Budapest",
                    Address = "Irányi tér 1.",
                    ContactName = "Csikós Miklós",
                    ContactEmail = "csimi@science.eu",
                    ContactPhone = "06-99-555-4567",
                    Description = "",
                    IsActive = true,
                    PreferOrder = 90
                },
                new Organization
                {
                    OrganizationName = "Miskolci Megyei Könyvtár",
                    City = "Miskolc",
                    Address = "Keleti u. 3.",
                    ContactName = "Király Kolos",
                    ContactEmail = "kolos.king@miskolc.hu",
                    ContactPhone = "06-30-123-4567",
                    Description = "",
                    IsActive = true,
                    PreferOrder = 100
                },
                new Organization
                {
                    OrganizationName = "Janus Pannonius Múzeum",
                    City = "Pécs",
                    Address = "Művészetek útja 1.",
                    ContactName = "Magyar Julianna",
                    ContactEmail = "magyar.juli@jpmuseum.hu",
                    ContactPhone = "06-70-543-4537",
                    Description = "",
                    IsActive = true,
                    PreferOrder = 110
                },
                new Organization
                {
                    OrganizationName = "Déri Múzeum",
                    City = "Debrecen",
                    Address = "Déri tér 1.",
                    ContactName = "Botlik Béla",
                    ContactEmail = "bebela@debmuz.hu",
                    ContactPhone = "06-20-987-4567",
                    Description = "",
                    IsActive = true,
                    PreferOrder = 120
                }
            );
            await context.SaveChangesAsync();
        }

        public static async Task InitJobs(ApplicationDbContext context)
        {
            if (context.Jobs.Any())
            {
                return;   // DB has been seeded
            }
            context.Jobs.AddRange(
                new Job
                {
                    JobName = "Archivátor",
                    Description = "Archivátori feladatok ellátása...",
                    PreferOrder = 10,
                    IsActive = true
                },
                new Job
                {
                    JobName = "Adminisztrátor",
                    Description = "Adminisztrátor feladatok ellátása...",
                    PreferOrder = 20,
                    IsActive = true
                },
                new Job
                {
                    JobName = "Vezető (Archivátor)",
                    Description = "Vezetői feladatok ellátása...",
                    PreferOrder = 30,
                    IsActive = true
                }
            );
            await context.SaveChangesAsync();
        }

        public static async Task InitProcessStatuses(ApplicationDbContext context)
        {
            if (context.ProcessStatuses.Any())
            {
                return;   // DB has been seeded
            }
            context.ProcessStatuses.AddRange(
                new ProcessStatus
                {
                    StatusName = "Regisztrált álláskereső",
                    Description = "Közvetítőlappal rendelkező álláskereső",
                    PreferOrder = 10,
                    IsActive = true
                },
                new ProcessStatus
                {
                    StatusName = "Adatfelvétel folyamatban",
                    Description = "Személyes adatok felvétele folyamatban",
                    PreferOrder = 20,
                    IsActive = true
                },
                new ProcessStatus
                {
                    StatusName = "Intézményi elhelyezés",
                    Description = "Személyes adatok felvétele megtörtént, papírok rendben. Intézmény kiközvetítés folyamatban",
                    PreferOrder = 30,
                    IsActive = true
                },
                new ProcessStatus
                {
                    StatusName = "Felvehető",
                    Description = "Munkakör - OK, Intézményi elhelyezés - OK, papírok - OK, stb.. ",
                    PreferOrder = 40,
                    IsActive = true
                },
                new ProcessStatus
                {
                    StatusName = "Felvett",
                    Description = "Bejelentett munkavállaló",
                    PreferOrder = 50,
                    IsActive = true
                },
                new ProcessStatus
                {
                    StatusName = "Kilépett",
                    Description = "Kilépett munkavállaló.",
                    PreferOrder = 60,
                    IsActive = true
                }
            );
            await context.SaveChangesAsync();
        }

        public static async Task InitDocTypes(ApplicationDbContext context)
        {
            if (context.DocTypes.Any())
            {
                return;   // DB has been seeded
            }
            context.DocTypes.AddRange(
              new DocType
              {
                  TypeName = "Közvetítólap",
                  Description = "Közvetítólap...",
                  MandatoryElement = true,
                  IsActive = true,
                  PreferOrder = 10
              },
                new DocType
                {
                    TypeName = "Személyi okmány(ok) másolata(i)",
                    Description = "Szem. ig., TAJ, ADÓ dokumentumok...",
                    MandatoryElement = true,
                    IsActive = true,
                    PreferOrder = 20
                },
                new DocType
                {
                    TypeName = "EÜ. alkalmassági igazolás",
                    Description = "Orvosi alkalmassági igazolás...",
                    MandatoryElement = true,
                    IsActive = true,
                    PreferOrder = 30
                },
                new DocType
                {
                    TypeName = "Bankszámla nyilatkozat",
                    Description = "Nyilatkozat a bankszámlaszámról",
                    MandatoryElement = false,
                    IsActive = true,
                    PreferOrder = 40
                },
                new DocType
                {
                    TypeName = "Munkavállalói nyilatkozatok",
                    Description = "Családi adókedvezmény, szabadság stb.",
                    MandatoryElement = false,
                    IsActive = true,
                    PreferOrder = 50
                },
                new DocType
                {
                    TypeName = "Munkaszerződés",
                    Description = "Aláírt munkaszerződés",
                    MandatoryElement = false,
                    IsActive = true,
                    PreferOrder = 60
                }
            );
            await context.SaveChangesAsync();
        }

        public static async Task InitEmployees(ApplicationDbContext context)
        {
            if (context.Employees.Any())
            {
                return;   // DB has been seeded
            }
            var rnd = new Random();
            context.Employees.AddRange(
               new Employee
               {
                   SurName = "Nagy",
                   ForeName = "István",
                   Birthplace = "Budapest",
                   DateOfBirth = DateTime.Parse(rnd.Next(1970, 2000).ToString() + "." + rnd.Next(1, 12).ToString() + "." + rnd.Next(1, 28).ToString()),
                   PermPostCode = "1011",
                   PermCity = "Budapest",
                   PermAddress = "Mészáros u. 12.",
                   PhoneNumber = "06 30 " + rnd.Next(235, 788).ToString() + " " + rnd.Next(35, 78).ToString() + " " + rnd.Next(15, 88).ToString(),
                   Email = "nagy.istvan" + "@example.com",
                   Description = "Korábban fényképészként dolgozott",
                   SSNumber = "83268388",
                   ProcessStatusId = rnd.Next(1, 6),    // 1-6
                   JobId = rnd.Next(1, 2)              // 1-3                   
               },
                new Employee
                {
                    SurName = "Jánosi",
                    ForeName = "Jolán",
                    Birthplace = "Budapest",
                    DateOfBirth = DateTime.Parse(rnd.Next(1970, 2000).ToString() + "." + rnd.Next(1, 12).ToString() + "." + rnd.Next(1, 28).ToString()),
                    PermPostCode = "1023",
                    PermCity = "Budapest",
                    PermAddress = "Bem tér 1.",
                    PhoneNumber = "06 30 " + rnd.Next(235, 788).ToString() + " " + rnd.Next(35, 78).ToString() + " " + rnd.Next(15, 88).ToString(),
                    Email = "jolan2001" + "@gmail.com",
                    Description = "OSZK-ban dolgozott.2 éve munkanélküli.",
                    SSNumber = "087501889",
                    ProcessStatusId = rnd.Next(1, 6),
                    JobId = rnd.Next(1, 2)
                },
                new Employee
                {
                    SurName = "Kercza",
                    ForeName = "Zsófia",
                    Birthplace = "Budapest",
                    DateOfBirth = DateTime.Parse(rnd.Next(1970, 2000).ToString() + "." + rnd.Next(1, 12).ToString() + "." + rnd.Next(1, 28).ToString()),
                    PermPostCode = "9022",
                    PermCity = "Győr",
                    PermAddress = "Szent István u. 8.",
                    PhoneNumber = "06 70 " + rnd.Next(235, 788).ToString() + " " + rnd.Next(35, 78).ToString() + " " + rnd.Next(15, 88).ToString(),
                    Email = "kerczazso" + "@t-online.hu",
                    Description = "-",
                    SSNumber = "29992036",
                    ProcessStatusId = rnd.Next(1, 6),
                    JobId = rnd.Next(1, 2)
                },
                new Employee
                {
                    SurName = "Kövesdi",
                    ForeName = "Ádám",
                    Birthplace = "Budapest",
                    DateOfBirth = DateTime.Parse(rnd.Next(1970, 2000).ToString() + "." + rnd.Next(1, 12).ToString() + "." + rnd.Next(1, 28).ToString()),
                    PermPostCode = "3600",
                    PermCity = "Ózd",
                    PermAddress = "Gyár u. 77.",
                    PhoneNumber = "06 20 " + rnd.Next(235, 788).ToString() + " " + rnd.Next(35, 78).ToString() + " " + rnd.Next(15, 88).ToString(),
                    Email = "kadam" + "@example.com",
                    Description = "Miskolci egetemen végzett 2016-ban.",
                    SSNumber = "092761931",
                    ProcessStatusId = rnd.Next(1, 6),
                    JobId = rnd.Next(1, 2)
                },
                new Employee
                {
                    SurName = "Juhász",
                    ForeName = "Tibor",
                    Birthplace = "Budapest",
                    DateOfBirth = DateTime.Parse(rnd.Next(1970, 2000).ToString() + "." + rnd.Next(1, 12).ToString() + "." + rnd.Next(1, 28).ToString()),
                    PermPostCode = "3600",
                    PermCity = "Ózd",
                    PermAddress = "Kossuth utca 20.",
                    PhoneNumber = "06 30 " + rnd.Next(235, 788).ToString() + " " + rnd.Next(35, 78).ToString() + " " + rnd.Next(15, 88).ToString(),
                    Email = "t.juhasz" + "@ozdtv.net",
                    Description = "TV-s előképzettség.",
                    SSNumber = "084195313",
                    ProcessStatusId = rnd.Next(1, 6),
                    JobId = rnd.Next(1, 2)
                },
                new Employee
                {
                    SurName = "Katona",
                    ForeName = "Gabriella",
                    Birthplace = "Budapest",
                    DateOfBirth = DateTime.Parse(rnd.Next(1970, 2000).ToString() + "." + rnd.Next(1, 12).ToString() + "." + rnd.Next(1, 28).ToString()),
                    PermPostCode = "1201",
                    PermCity = "Budapest",
                    PermAddress = "Királyok tere 1.",
                    PhoneNumber = "06 70 " + rnd.Next(235, 788).ToString() + " " + rnd.Next(35, 78).ToString() + " " + rnd.Next(15, 88).ToString(),
                    Email = "gabibaba" + "@example.com",
                    Description = "GYES-en volt 6 évig.",
                    SSNumber = "072959972",
                    ProcessStatusId = rnd.Next(1, 6),
                    JobId = rnd.Next(1, 2)
                },
                new Employee
                {
                    SurName = "Répási",
                    ForeName = "Kázmér",
                    Birthplace = "Budapest",
                    DateOfBirth = DateTime.Parse(rnd.Next(1970, 2000).ToString() + "." + rnd.Next(1, 12).ToString() + "." + rnd.Next(1, 28).ToString()),
                    PermPostCode = "4220",
                    PermCity = "Szolnok",
                    PermAddress = "Tavasz u. 12",
                    PhoneNumber = "06 22 " + rnd.Next(235, 788).ToString() + " " + rnd.Next(35, 78).ToString() + " " + rnd.Next(15, 88).ToString(),
                    Email = "karotta11" + "@example.com",
                    Description = "???",
                    SSNumber = "084535409",
                    ProcessStatusId = rnd.Next(1, 6),
                    JobId = rnd.Next(1, 2)
                },
                new Employee
                {
                    SurName = "Tóth",
                    ForeName = "Jakab",
                    Birthplace = "Budapest",
                    DateOfBirth = DateTime.Parse(rnd.Next(1970, 2000).ToString() + "." + rnd.Next(1, 12).ToString() + "." + rnd.Next(1, 28).ToString()),
                    PermPostCode = "8532",
                    PermCity = "Nagykanizsa",
                    PermAddress = "Kinitsi tér 6.",
                    PhoneNumber = "06 70 " + rnd.Next(235, 788).ToString() + " " + rnd.Next(35, 78).ToString() + " " + rnd.Next(15, 88).ToString(),
                    Email = "kabaost" + "@example.com",
                    Description = "Könyvtárban dolgozott...",
                    SSNumber = "075473561",
                    ProcessStatusId = rnd.Next(1, 6),
                    JobId = rnd.Next(1, 2)
                },
                new Employee
                {
                    SurName = "Lovas",
                    ForeName = "Tibor",
                    Birthplace = "Budapest",
                    DateOfBirth = DateTime.Parse(rnd.Next(1970, 2000).ToString() + "." + rnd.Next(1, 12).ToString() + "." + rnd.Next(1, 28).ToString()),
                    PermPostCode = "1101",
                    PermCity = "Budapest",
                    PermAddress = "Közraktár u. 12",
                    PhoneNumber = "06 20 " + rnd.Next(235, 788).ToString() + " " + rnd.Next(35, 78).ToString() + " " + rnd.Next(15, 88).ToString(),
                    Email = "tibor.lovas" + "@example.com",
                    Description = "-",
                    SSNumber = "091575935",
                    ProcessStatusId = rnd.Next(1, 6),
                    JobId = rnd.Next(1, 2)
                },
                new Employee
                {
                    SurName = "Dallos",
                    ForeName = "Szilvia",
                    Birthplace = "Budapest",
                    DateOfBirth = DateTime.Parse(rnd.Next(1970, 2000).ToString() + "." + rnd.Next(1, 12).ToString() + "." + rnd.Next(1, 28).ToString()),
                    PermPostCode = "4221",
                    PermCity = "Szolnok",
                    PermAddress = "Blaha Lujza u. 112",
                    PhoneNumber = "06 30 " + rnd.Next(235, 788).ToString() + " " + rnd.Next(35, 78).ToString() + " " + rnd.Next(15, 88).ToString(),
                    Email = "dalala" + "@example.com",
                    Description = "-",
                    SSNumber = "72677306",
                    ProcessStatusId = rnd.Next(1, 6),
                    JobId = rnd.Next(1, 2)
                },
                new Employee
                {
                    SurName = "Oravetz",
                    ForeName = "Nikoletta",
                    Birthplace = "Budapest",
                    DateOfBirth = DateTime.Parse(rnd.Next(1970, 2000).ToString() + "." + rnd.Next(1, 12).ToString() + "." + rnd.Next(1, 28).ToString()),
                    PermPostCode = "3600",
                    PermCity = "Ózd",
                    PermAddress = "József A. u. 17.",
                    PhoneNumber = "06 70 " + rnd.Next(235, 788).ToString() + " " + rnd.Next(35, 78).ToString() + " " + rnd.Next(15, 88).ToString(),
                    Email = "nikol.ori11" + "@gmail.com",
                    Description = "2019. novembertől nyugdíjba megy",
                    SSNumber = "069640285",
                    ProcessStatusId = rnd.Next(1, 6),
                    JobId = rnd.Next(1, 2)
                }
            );
            await context.SaveChangesAsync();
        }

        public static async Task InitProjectOrganizations(ApplicationDbContext context)
        {
            if (context.ProjectOrganizations.Any())
            {
                return;   // DB has been seeded
            }
            context.ProjectOrganizations.AddRange(
                new ProjectOrganization
                {
                    ProjectId = 5, // 1-6
                    OrganizationId = 1 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 5, // 1-6
                    OrganizationId = 2 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 5, // 1-6
                    OrganizationId = 3 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 5, // 1-6
                    OrganizationId = 6 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 5, // 1-6
                    OrganizationId = 8 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 5, // 1-6
                    OrganizationId = 10 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 6, // 1-6
                    OrganizationId = 1 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 6, // 1-6
                    OrganizationId = 3 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 6, // 1-6
                    OrganizationId = 4 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 6, // 1-6
                    OrganizationId = 7 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 6, // 1-6
                    OrganizationId = 8 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 6, // 1-6
                    OrganizationId = 10 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 6, // 1-6
                    OrganizationId = 11 // 1-12
                },
                new ProjectOrganization
                {
                    ProjectId = 6, // 1-6
                    OrganizationId = 12 // 1-12
                }
            );
            await context.SaveChangesAsync();
        }

        public static async Task InitPOEmployees(ApplicationDbContext context)
        {
            if (context.POEmployees.Any())
            {
                return;   // DB has been seeded
            }
            context.POEmployees.AddRange(
                new POEmployee
                {
                    ProjectOrganizationId = 1, // 1-6
                    EmployeeId = 1 // 1-20
                },
                new POEmployee
                {
                    ProjectOrganizationId = 2, // 1-6
                    EmployeeId = 2 // 1-20
                },
                new POEmployee
                {
                    ProjectOrganizationId = 3, // 1-6
                    EmployeeId = 3 // 1-20
                },
                new POEmployee
                {
                    ProjectOrganizationId = 4, // 1-6
                    EmployeeId = 4 // 1-20
                },
                new POEmployee
                {
                    ProjectOrganizationId = 5, // 1-6
                    EmployeeId = 5 // 1-20
                },
                new POEmployee
                {
                    ProjectOrganizationId = 6, // 1-6
                    EmployeeId = 6 // 1-20
                },
                new POEmployee
                {
                    ProjectOrganizationId = 7, // 7-14
                    EmployeeId = 3 // 1-20
                },
                new POEmployee
                {
                    ProjectOrganizationId = 8, // 7-14
                    EmployeeId = 6 // 1-20
                },
                new POEmployee
                {
                    ProjectOrganizationId = 9, // 7-14
                    EmployeeId = 7 // 1-20
                },
                new POEmployee
                {
                    ProjectOrganizationId = 10, // 7-14
                    EmployeeId = 8 // 1-20
                }
            );
            await context.SaveChangesAsync();
        }

    }
}