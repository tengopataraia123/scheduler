//using System.Data;
//using Microsoft.Extensions.Logging;
//using Microsoft.EntityFrameworkCore;
//using ProgramServer.Domain.Roles;
//using ProgramServer.Domain.Locations;
//using ProgramServer.Domain.Subjects;
//using Newtonsoft.Json;
//using ProgramServer.Domain.Users;
//using ProgramServer.Domain.Events;
//using ProgramServer.Domain.Attendances;
//using ProgramServer.Domain.SubjectUsers;
//using ProgramServer.Domain.Surveys;

//namespace ProgramServer.Persistence.Extensions
//{
//    public static class ModelBuilderExtension
//    {
//        public static void Seed(this ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Role>().HasData(
//                new Role { Id = 1, Name = "Coordinator", IsCreator = true, RoleName = "Admin" },
//                new Role { Id = 2, Name = "Student", IsReceiver = true, RoleName = "User"},
//                new Role { Id = 3, Name = "Teacher", IsBroadcaster = true, RoleName = "Admin"}
//            );

//            modelBuilder.Entity<Location>().HasData(new Location { Id = 1, Latitude = "41.7151° N", Longitude = "44.8271° E", Address = "Tbilisi, Georgia" });

//            modelBuilder.Entity<Subject>().HasData(
//                new Subject
//                {
//                    Id = 1,
//                    Name = "სტატისტიკა",
//                    Code = "A932",
//                    LocationId = 1,
//                    DescriptionStr = JsonConvert.SerializeObject(new
//                    {
//                        Step = "master",
//                        Semester = 1,
//                        Credit = 6,
//                        IsMandatory = true,
//                        StartDate = new DateTime(2022, 09, 15),
//                        EndDate = new DateTime(2022, 12, 23)
//                    }),
//                },
//                new Subject
//                {
//                    Id = 2,
//                    Name = "NodeJS ტექნოლოგია",
//                    Code = "U074",
//                    LocationId = 1,
//                    DescriptionStr = JsonConvert.SerializeObject(new
//                    {
//                        Step = "master",
//                        Semester = 1,
//                        Credit = 6,
//                        IsMandatory = true,
//                        StartDate = new DateTime(2022, 09, 15),
//                        EndDate = new DateTime(2022, 12, 23)
//                    })
//                },
//                new Subject
//                {
//                    Id = 3,
//                    Name = "კიბერსივრცის სამართლებრივი რეგულირება",
//                    Code = "T279",
//                    LocationId = 1,
//                    DescriptionStr = JsonConvert.SerializeObject(new
//                    {
//                        Step = "master",
//                        Semester = 1,
//                        Credit = 6,
//                        IsMandatory = true,
//                        StartDate = new DateTime(2022, 09, 15),
//                        EndDate = new DateTime(2022, 12, 23)
//                    })
//                },
//                new Subject
//                {
//                    Id = 4,
//                    Name = "მონაცემთა მოდელირება და კომპიუტერული დამუშავება",
//                    Code = "T280",
//                    LocationId = 1,
//                    DescriptionStr = JsonConvert.SerializeObject(new
//                    {
//                        Step = "master",
//                        Semester = 1,
//                        Credit = 6,
//                        IsMandatory = true,
//                        StartDate = new DateTime(2022, 09, 15),
//                        EndDate = new DateTime(2022, 12, 23)
//                    })
//                },
//                new Subject
//                {
//                    Id = 5,
//                    Name = "გამოთვლადობის თეორიული საფუძვლები",
//                    Code = "T281",
//                    LocationId = 1,
//                    DescriptionStr = JsonConvert.SerializeObject(new
//                    {
//                        Step = "master",
//                        Semester = 1,
//                        Credit = 6,
//                        IsMandatory = true,
//                        StartDate = new DateTime(2022, 09, 15),
//                        EndDate = new DateTime(2022, 12, 23)
//                    })
//                });

//            modelBuilder.Entity<User>().HasData(
//                new User
//                {
//                    Id = 1,
//                    ProfileId = 123,
//                    FirstName = "ანა",
//                    LastName = "დვალი",
//                    Email = "ana.dvali.3@iliauni.edu.ge",
//                    RoleId = 2,
//                    MacAddressUser = "00-11-22-33-AA-BB",
//                    AttendanceId = 1,
//                    Password = "Test@123"
//                },
//                new User
//                {
//                    Id = 2,
//                    ProfileId = 234,
//                    FirstName = "ანი",
//                    LastName = "თხელიძე",
//                    Email = "ani.tkhelidze.1@iliauni.edu.ge",
//                    RoleId = 2,
//                    MacAddressUser = "66-77-88-99-AA-BB",
//                    AttendanceId = 1,
//                    Password = "Test@123"
//                },
//                new User
//                {
//                    Id = 3,
//                    ProfileId = 567,
//                    FirstName = "თენგიზ",
//                    LastName = "პატარაია",
//                    Email = "tengiz.pataraia.1@iliauni.edu.ge",
//                    RoleId = 2,
//                    MacAddressUser = "03-75-23-90-AA-BB",
//                    AttendanceId = 1,
//                    Password = "Test@123"
//                },
//                new User
//                {
//                    Id = 4,
//                    ProfileId = 890,
//                    FirstName = "ავთანდილ",
//                    LastName = "დიაკვნიშვილი",
//                    Email = "avtandil.diakvnishvili.1@iliauni.edu.ge",
//                    RoleId = 2,
//                    MacAddressUser = "43-04-43-12-AA-BB",
//                    AttendanceId = 1,
//                    Password = "Test@123"
//                },
//                new User
//                {
//                    Id = 5,
//                    ProfileId = 321,
//                    FirstName = "ზვიად",
//                    LastName = "ნოზაძე",
//                    Email = "zviad.nozadze.1@iliauni.edu.ge",
//                    RoleId = 2,
//                    MacAddressUser = "93-34-35-45-AA-BB",
//                    AttendanceId = 1,
//                    Password = "Test@123"
//                },
//                new User
//                {
//                    Id = 6,
//                    ProfileId = 543,
//                    FirstName = "მიხეილი",
//                    LastName = "ქაჯაია",
//                    Email = "mikheili.kajaia.1@iliauni.edu.ge",
//                    RoleId = 2,
//                    MacAddressUser = "07-06-94-76-AA-BB",
//                    AttendanceId = 1,
//                    Password = "Test@123"
//                },
//                new User
//                {
//                    Id = 7,
//                    ProfileId = 876,
//                    FirstName = "შოთა",
//                    LastName = "ცისკარიძე",
//                    Email = "shota.tsiskaridze@iliauni.edu.ge",
//                    RoleId = 1,
//                    MacAddressUser = "83-43-87-23-AA-BB",
//                    AttendanceId = 1,
//                    Password = "Test@123"
//                },
//                new User
//                {
//                    Id = 8,
//                    ProfileId = 256,
//                    FirstName = "მიხეილ",
//                    LastName = "თუთბერიძე",
//                    Email = "mikheil.tutberidze@iliauni.edu.ge",
//                    RoleId = 3,
//                    MacAddressUser = "89-23-02-07-AA-BB",
//                    AttendanceId = 1,
//                    Password = "Test@123"
//                }
//            );

//            modelBuilder.Entity<Event>().HasData(
//            // Events For Subject 1
//                new Event { Id = 1, SubjectId = 1, StartDate = new DateTime(2023, 3, 28, 14, 30, 0), EndDate = new DateTime(2023, 3, 28, 16, 50, 0) },
//                new Event { Id = 2, SubjectId = 1, StartDate = new DateTime(2023, 4, 4, 14, 30, 0), EndDate = new DateTime(2023, 4, 4, 16, 50, 0) },
//                new Event { Id = 3, SubjectId = 1, StartDate = new DateTime(2023, 4, 11, 14, 30, 0), EndDate = new DateTime(2023, 4, 11, 16, 50, 0) },
//                new Event { Id = 4, SubjectId = 1, StartDate = new DateTime(2023, 4, 18, 14, 30, 0), EndDate = new DateTime(2023, 4, 18, 16, 50, 0) },
//                new Event { Id = 5, SubjectId = 1, StartDate = new DateTime(2023, 4, 25, 14, 30, 0), EndDate = new DateTime(2023, 4, 25, 16, 50, 0) },

//            //Events For Subject 2
//                new Event { Id = 6, SubjectId = 2, StartDate = new DateTime(2023, 3, 27, 11, 00, 0), EndDate = new DateTime(2023, 3, 27, 13, 00, 0) },
//                new Event { Id = 7, SubjectId = 2, StartDate = new DateTime(2023, 4, 3, 11, 00, 0), EndDate = new DateTime(2023, 4, 3, 13, 00, 0) },
//                new Event { Id = 8, SubjectId = 2, StartDate = new DateTime(2023, 4, 10, 11, 00, 0), EndDate = new DateTime(2023, 4, 11, 13, 00, 0) },
//                new Event { Id = 9, SubjectId = 2, StartDate = new DateTime(2023, 4, 17, 11, 00, 0), EndDate = new DateTime(2023, 4, 17, 13, 00, 0) },
//                new Event { Id = 10, SubjectId = 2, StartDate = new DateTime(2023, 4, 24, 11, 00, 0), EndDate = new DateTime(2023, 4, 24, 13, 00, 0) },

//            // Events For Subject 3
//                new Event { Id = 11, SubjectId = 3, StartDate = new DateTime(2023, 3, 26, 19, 00, 0), EndDate = new DateTime(2023, 3, 26, 21, 50, 0) },
//                new Event { Id = 12, SubjectId = 3, StartDate = new DateTime(2023, 4, 3, 19, 00, 0), EndDate = new DateTime(2023, 4, 3, 21, 50, 0) },
//                new Event { Id = 13, SubjectId = 3, StartDate = new DateTime(2023, 4, 9, 19, 00, 0), EndDate = new DateTime(2023, 4, 16, 21, 50, 0) },
//                new Event { Id = 14, SubjectId = 3, StartDate = new DateTime(2023, 4, 16, 19, 00, 0), EndDate = new DateTime(2023, 4, 26, 21, 50, 0) },
//                new Event { Id = 15, SubjectId = 3, StartDate = new DateTime(2023, 4, 23, 19, 00, 0), EndDate = new DateTime(2023, 4, 23, 21, 50, 0) },

//            // Events For Subject 4
//                new Event { Id = 16, SubjectId = 4, StartDate = new DateTime(2023, 3, 25, 17, 30, 0), EndDate = new DateTime(2023, 3, 25, 20, 50, 0) },
//                new Event { Id = 17, SubjectId = 4, StartDate = new DateTime(2023, 4, 2, 17, 30, 0), EndDate = new DateTime(2023, 4, 2, 20, 50, 0) },
//                new Event { Id = 18, SubjectId = 4, StartDate = new DateTime(2023, 4, 8, 17, 30, 0), EndDate = new DateTime(2023, 4, 8, 20, 50, 0) },
//                new Event { Id = 19, SubjectId = 4, StartDate = new DateTime(2023, 4, 15, 17, 30, 0), EndDate = new DateTime(2023, 4, 15, 20, 50, 0) },
//                new Event { Id = 20, SubjectId = 4, StartDate = new DateTime(2023, 4, 22, 17, 30, 0), EndDate = new DateTime(2023, 4, 22, 20, 50, 0) },

//            // Events For Subject 1
//                new Event { Id = 21, SubjectId = 5, StartDate = new DateTime(2023, 3, 24, 18, 50, 0), EndDate = new DateTime(2023, 3, 24, 22, 30, 0) },
//                new Event { Id = 22, SubjectId = 5, StartDate = new DateTime(2023, 4, 1, 18, 50, 0), EndDate = new DateTime(2023, 4, 1, 22, 30, 0) },
//                new Event { Id = 23, SubjectId = 5, StartDate = new DateTime(2023, 4, 7, 18, 50, 0), EndDate = new DateTime(2023, 4, 7, 22, 30, 0) },
//                new Event { Id = 24, SubjectId = 5, StartDate = new DateTime(2023, 4, 14, 18, 50, 0), EndDate = new DateTime(2023, 4, 14, 22, 30, 0) },
//                new Event { Id = 25, SubjectId = 5, StartDate = new DateTime(2023, 4, 21, 18, 50, 0), EndDate = new DateTime(2023, 4, 21, 22, 30, 0) }
//            );

//            modelBuilder.Entity<Attendance>().HasData(
//                new Attendance { Id = 1, EventId = 1, EventCode = "STA001", Attended = true },
//                new Attendance { Id = 2, EventId = 2, EventCode = "STA001", Attended = true },
//                new Attendance { Id = 3, EventId = 3, EventCode = "STA001", Attended = true },
//                new Attendance { Id = 4, EventId = 4, EventCode = "STA001", Attended = false },
//                new Attendance { Id = 5, EventId = 5, EventCode = "STA001", Attended = true },
//                new Attendance { Id = 6, EventId = 6, EventCode = "NOD123", Attended = false },
//                new Attendance { Id = 7, EventId = 7, EventCode = "NOD123", Attended = true },
//                new Attendance { Id = 8, EventId = 8, EventCode = "NOD123", Attended = true },
//                new Attendance { Id = 9, EventId = 9, EventCode = "NOD123", Attended = false },
//                new Attendance { Id = 10, EventId = 10, EventCode = "NOD123", Attended = true },
//                new Attendance { Id = 11, EventId = 11, EventCode = "SCI008", Attended = true },
//                new Attendance { Id = 12, EventId = 12, EventCode = "SCI008", Attended = true },
//                new Attendance { Id = 13, EventId = 13, EventCode = "SCI008", Attended = true },
//                new Attendance { Id = 14, EventId = 14, EventCode = "SCI008", Attended = false },
//                new Attendance { Id = 15, EventId = 15, EventCode = "SCI008", Attended = false },
//                new Attendance { Id = 16, EventId = 16, EventCode = "MOD445", Attended = true },
//                new Attendance { Id = 17, EventId = 17, EventCode = "MOD445", Attended = true },
//                new Attendance { Id = 18, EventId = 18, EventCode = "MOD445", Attended = true },
//                new Attendance { Id = 19, EventId = 19, EventCode = "MOD445", Attended = true },
//                new Attendance { Id = 20, EventId = 20, EventCode = "MOD445", Attended = true },
//                new Attendance { Id = 21, EventId = 21, EventCode = "DIS003", Attended = true },
//                new Attendance { Id = 22, EventId = 22, EventCode = "DIS003", Attended = true },
//                new Attendance { Id = 23, EventId = 23, EventCode = "DIS003", Attended = true },
//                new Attendance { Id = 24, EventId = 24, EventCode = "DIS003", Attended = false },
//                new Attendance { Id = 25, EventId = 25, EventCode = "DIS003", Attended = false }

//            );

//            modelBuilder.Entity<SubjectUser>().HasData(
//                new SubjectUser { SubjectId = 1, UserId = 1 },
//                new SubjectUser { SubjectId = 1, UserId = 2 },
//                new SubjectUser { SubjectId = 1, UserId = 3 },
//                new SubjectUser { SubjectId = 1, UserId = 4 },
//                new SubjectUser { SubjectId = 1, UserId = 5 },
//                new SubjectUser { SubjectId = 1, UserId = 6 },
//                new SubjectUser { SubjectId = 1, UserId = 7 },
//                new SubjectUser { SubjectId = 1, UserId = 8 },
//                new SubjectUser { SubjectId = 2, UserId = 1 },
//                new SubjectUser { SubjectId = 2, UserId = 2 },
//                new SubjectUser { SubjectId = 2, UserId = 3 },
//                new SubjectUser { SubjectId = 2, UserId = 4 },
//                new SubjectUser { SubjectId = 2, UserId = 5 },
//                new SubjectUser { SubjectId = 2, UserId = 6 },
//                new SubjectUser { SubjectId = 2, UserId = 7 },
//                new SubjectUser { SubjectId = 2, UserId = 8 },
//                new SubjectUser { SubjectId = 3, UserId = 1 },
//                new SubjectUser { SubjectId = 3, UserId = 2 },
//                new SubjectUser { SubjectId = 3, UserId = 3 },
//                new SubjectUser { SubjectId = 3, UserId = 4 },
//                new SubjectUser { SubjectId = 3, UserId = 5 },
//                new SubjectUser { SubjectId = 3, UserId = 6 },
//                new SubjectUser { SubjectId = 3, UserId = 7 },
//                new SubjectUser { SubjectId = 3, UserId = 8 },
//                new SubjectUser { SubjectId = 4, UserId = 1 },
//                new SubjectUser { SubjectId = 4, UserId = 2 },
//                new SubjectUser { SubjectId = 4, UserId = 3 },
//                new SubjectUser { SubjectId = 4, UserId = 4 },
//                new SubjectUser { SubjectId = 4, UserId = 5 },
//                new SubjectUser { SubjectId = 4, UserId = 6 },
//                new SubjectUser { SubjectId = 4, UserId = 7 },
//                new SubjectUser { SubjectId = 4, UserId = 8 },
//                new SubjectUser { SubjectId = 5, UserId = 1 },
//                new SubjectUser { SubjectId = 5, UserId = 2 },
//                new SubjectUser { SubjectId = 5, UserId = 3 },
//                new SubjectUser { SubjectId = 5, UserId = 4 },
//                new SubjectUser { SubjectId = 5, UserId = 5 },
//                new SubjectUser { SubjectId = 5, UserId = 6 },
//                new SubjectUser { SubjectId = 5, UserId = 7 },
//                new SubjectUser { SubjectId = 5, UserId = 8 }
//            );

//            modelBuilder.Entity<Survey>().HasData(
//                new Survey { Id = -1, Subject = "Other", Question= "Clinical training facilities are adequate, infrastructural resources are appropriate." },
//                new Survey { Id = -2, Subject = "Other", Question = "Number of patients is sufficient, access to patient is facilitated." },
//                new Survey { Id = -3, Subject = "Other", Question = "Teaching process is well organized and students involve in patient management procedures." }
//            );
//        }
//    }
//}

