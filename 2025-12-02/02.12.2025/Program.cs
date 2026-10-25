using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// 1️ - ПІБ
class Person
{
    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public string? Lastname { get; set; }
}

// 2️ - Адреса
class Address
{
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public int HouseNumber { get; set; }
    public short PostalCode { get; set; }
}

// 3 - Спеціалізація (окремий клас)
class Specialization
{
    public string? Name { get; set; }
}

// 4 - Курс
class KursInfo
{
    public int Kurs { get; set; }
}

// 5 - Група (окремо)
class GroupInfo
{
    public string? GroupName { get; set; }
}

// 6 - Відвідуваність
class Attendance
{
    public int LessonsVisited { get; set; }
    public int LessonsLate { get; set; }
}

// 7 - Предмет
class Subject
{
    public string? SubjectName { get; set; }
}

// 8 - Викладач
class Teacher : Person
{
}

// 9 - Оцінки
class Grades
{
    public List<int> DzRates { get; set; } = new List<int>();
    public List<int> PracticeRates { get; set; } = new List<int>();
    public List<int> ExamRates { get; set; } = new List<int>();
    public List<int> ZachetRates { get; set; } = new List<int>();

    public void PrintAverages()
    {
        double dzAvg = DzRates.Any() ? DzRates.Average() : 0;
        double practiceAvg = PracticeRates.Any() ? PracticeRates.Average() : 0;
        double examAvg = ExamRates.Any() ? ExamRates.Average() : 0;
        double zachetAvg = ZachetRates.Any() ? ZachetRates.Average() : 0;

        double totalAverage = (dzAvg + practiceAvg + examAvg + zachetAvg) / 4;

        Console.WriteLine("— Оцiнки студента:");
        Console.WriteLine($"  ДЗ: {string.Join(", ", DzRates)} (Середній: {dzAvg:F2})");
        Console.WriteLine($"  Практики: {string.Join(", ", PracticeRates)} (Середній: {practiceAvg:F2})");
        Console.WriteLine($"  Екзамени: {string.Join(", ", ExamRates)} (Середній: {examAvg:F2})");
        Console.WriteLine($"  Заліки: {string.Join(", ", ZachetRates)} (Середній: {zachetAvg:F2})");
        Console.WriteLine($"  Загальний середній бал: {totalAverage:F2}");
    }
}

//-----------------------------------------------
class Student
{
    public Person Name { get; set; } = new Person();
    public Address Address { get; set; } = new Address();

    public DateTime BirthDate { get; set; } = new DateTime();
    public DateTime StudyStartDate { get; set; } = new DateTime();

    public KursInfo KursInfo { get; set; } = new KursInfo();
    public Specialization Specialization { get; set; } = new Specialization();
    public GroupInfo GroupInfo { get; set; } = new GroupInfo();
    public Attendance Attendance { get; set; } = new Attendance();
    public Subject Subject { get; set; } = new Subject();
    public Teacher Teacher { get; set; } = new Teacher();
    public Grades Grades { get; set; } = new Grades();

    public Student()
    {
        /*Name = new Person();
        Address = new Address();
        KursInfo = new KursInfo();
        Specialization = new Specialization();
        GroupInfo = new GroupInfo();
        Attendance = new Attendance();
        Subject = new Subject();
        Teacher = new Teacher();
        Grades = new Grades();*/
    }

    public void PrintInfo()
    {
        Console.WriteLine("===== ІНФОРМАЦІЯ ПРО СТУДЕНТА =====\n");

        Console.WriteLine("— ПІБ:");
        Console.WriteLine($"  Ім'я: {Name.FirstName}");
        Console.WriteLine($"  Прізвище: {Name.Surname}");
        Console.WriteLine($"  По батькові: {Name.Lastname}\n");

        Console.WriteLine("— Адреса:");
        Console.WriteLine($"  Країна: {Address.Country}");
        Console.WriteLine($"  Регіон: {Address.Region}");
        Console.WriteLine($"  Місто: {Address.City}");
        Console.WriteLine($"  Вулиця: {Address.Street}");
        Console.WriteLine($"  Будинок: {Address.HouseNumber}");
        Console.WriteLine($"  Поштовий індекс: {Address.PostalCode}\n");

        Console.WriteLine("— Дата народження:");
        Console.WriteLine($"  {BirthDate:dd.MM.yyyy}\n");

        Console.WriteLine("— Період навчання:");
        Console.WriteLine($"  Дата початку навчання: {StudyStartDate:dd.MM.yyyy}\n");

        Console.WriteLine("— Курс:");
        Console.WriteLine($"  Курс: {KursInfo.Kurs}");

        Console.WriteLine("— Спеціалізація:");
        Console.WriteLine($"  {Specialization.Name}\n");

        Console.WriteLine("— Група:");
        Console.WriteLine($"  Група: {GroupInfo.GroupName}\n");

        Console.WriteLine("— Відвідуваність:");
        Console.WriteLine($"  Відвідано занять: {Attendance.LessonsVisited}");
        Console.WriteLine($"  Запізнень: {Attendance.LessonsLate}\n");

        Console.WriteLine("— Предмет:");
        Console.WriteLine($"  {Subject.SubjectName}\n");

        Console.WriteLine("— Викладач:");
        Console.WriteLine($"  {Teacher.FirstName} {Teacher.Surname} {Teacher.Lastname}\n");

        Grades.PrintAverages();
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Student student = new Student();

        student.Name.FirstName = "Іван";
        student.Name.Surname = "Петренко";
        student.Name.Lastname = "Олександрович";

        student.Address.Country = "Україна";
        student.Address.Region = "Київська область";
        student.Address.City = "Київ";
        student.Address.Street = "Хрещатик";
        student.Address.HouseNumber = 10;
        student.Address.PostalCode = 12345;

        student.BirthDate = new DateTime(2002, 6, 15);
        student.StudyStartDate = new DateTime(2020, 9, 1);

        student.KursInfo.Kurs = 2;

        student.Specialization.Name = "Програмування";

        student.GroupInfo.GroupName = "CS-22";

        student.Attendance.LessonsVisited = 42;
        student.Attendance.LessonsLate = 3;

        student.Subject.SubjectName = "Алгоритми і Структури Даних";

        student.Teacher.FirstName = "Олег";
        student.Teacher.Surname = "Павленко";
        student.Teacher.Lastname = "Олександрович";

        student.Grades.DzRates.AddRange(new[] { 10, 11, 12 });
        student.Grades.PracticeRates.AddRange(new[] { 9, 10, 12 });
        student.Grades.ExamRates.AddRange(new[] { 12 });
        student.Grades.ZachetRates.AddRange(new[] { 10, 10, 11 });

        student.PrintInfo();
    }
}