using System;
using System.Collections.Generic;
using System.Linq;

namespace classes
{
    class StudentManagementException : ApplicationException
    {
        public string? StudentName { get; set; }
        public StudentManagementException(string message) : base(message) { }
    }

    class InvalidGradeException : StudentManagementException
    {
        public int Grade { get; set; }
        public InvalidGradeException(string message, int grade) : base(message)
        {
            Grade = grade;
        }
    }

    class StudentNotFoundException : StudentManagementException
    {
        public StudentNotFoundException(string message) : base(message) { }
    }

    class InvalidStudentDataException : StudentManagementException
    {
        public InvalidStudentDataException(string message) : base(message) { }
    }

    class GroupManagementException : ApplicationException
    {
        public string? GroupName { get; set; }
        public GroupManagementException(string message) : base(message) { }
    }

    class GroupFullException : GroupManagementException
    {
        public int MaxSize { get; set; }
        public GroupFullException(string message, int maxSize) : base(message)
        {
            MaxSize = maxSize;
        }
    }

    class InvalidGroupDataException : GroupManagementException
    {
        public InvalidGroupDataException(string message) : base(message) { }
    }

    class TransferFailedException : GroupManagementException
    {
        public TransferFailedException(string message) : base(message) { }
    }

    class Student
    {
        int phonenumber;
        string? name;
        string? secondname;
        string? father;

        public int Day { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }

        public string Street { get; private set; } = "";
        public string House { get; private set; } = "";

        List<int> exams = new List<int>();
        List<int> homeworks = new List<int>();
        List<int> lessons = new List<int>();

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidStudentDataException("The name cannot be empty");
            this.name = name;
        }

        public void SetSecondName(string secondname)
        {
            if (string.IsNullOrWhiteSpace(secondname))
                throw new InvalidStudentDataException("The surname cannot be empty");
            this.secondname = secondname;
        }

        public void SetFather(string father)
        {
            if (string.IsNullOrWhiteSpace(father))
                throw new InvalidStudentDataException("The father's name cannot be left blank");
            this.father = father;
        }

        public void SetBirthday(int day, int month, int year)
        {
            if (day < 1 || day > 31 || month < 1 || month > 12 || year < 1900)
                throw new InvalidStudentDataException("Incorrect date of birth");
            Day = day;
            Month = month;
            Year = year;
        }

        public void SetAddress(string street, string house)
        {
            if (string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(house))
                throw new InvalidStudentDataException("Incorrect address details");
            Street = street;
            House = house;
        }

        public void SetNumber(int number) => phonenumber = number;

        public void SetExam(int exam)
        {
            if (exam < 0 || exam > 100)
                throw new InvalidGradeException("The rating should be between 0 and 100", exam);
            exams.Add(exam);
        }

        public void SetHomework(int homework)
        {
            if (homework < 0 || homework > 100)
                throw new InvalidGradeException("Homework assignments should be graded on a scale of 0 to 100", homework);
            homeworks.Add(homework);
        }

        public void SetLesson(int lesson)
        {
            if (lesson < 0 || lesson > 100)
                throw new InvalidGradeException("The grade for the lesson should be between 0 and 100", lesson);
            lessons.Add(lesson);
        }

        public string GetName() => name!;
        public string GetSecondName() => secondname!;
        public string GetFather() => father!;
        public string GetBirthday() => $"{Day}.{Month}.{Year}";
        public string GetAddress() => $"{Street}, {House}";
        public double GetExam() => exams.Count > 0 ? exams.Average() : 0;
        public double GetHomework() => homeworks.Count > 0 ? homeworks.Average() : 0;
        public double GetLesson() => lessons.Count > 0 ? lessons.Average() : 0;
        public int GetNumber() => phonenumber;

        public Student(string name, string secondname, string father,
                       int day, int month, int year,
                       string street, string house, int number)
        {
            SetName(name);
            SetSecondName(secondname);
            SetFather(father);
            SetBirthday(day, month, year);
            SetAddress(street, house);
            SetNumber(number);
        }

        public Student(int count_lesson, int count_homework, int count_exam, int lesson, int exam, int homework)
        {
            for (int i = 0; i < count_lesson; i++) SetLesson(lesson);
            for (int i = 0; i < count_homework; i++) SetHomework(homework);
            for (int i = 0; i < count_exam; i++) SetExam(exam);
        }

        public Student() : this("Alex", "Alexeivich", "Vladimir", 1, 1, 2000, "Abrikosovaia", "18", 0)
        { }

        public static bool operator ==(Student s1, Student s2)
        {
            if (ReferenceEquals(s1, s2)) return true;
            if (s1 is null || s2 is null) return false;
            return s1.GetHomework() == s2.GetHomework();
        }

        public static bool operator !=(Student s1, Student s2) => !(s1 == s2);

        public static bool operator true(Student s) => s.GetHomework() >= 7;
        public static bool operator false(Student s) => s.GetHomework() < 7;

        public override bool Equals(object? obj)
        {
            if (obj is Student s) return this == s;
            return false;
        }

        public override int GetHashCode() => GetHomework().GetHashCode();
    }

    class Group
    {
        List<Student> students;
        string groupName;
        string specialization;
        int course;
        const int MaxStudents = 10;

        public Group()
        {
            students = new List<Student>();
            groupName = "p45";
            specialization = "C#";
            course = 1;
        }

        public Group(List<Student> students)
        {
            if (students.Count > MaxStudents)
                throw new GroupFullException("The group exceeds the maximum size", MaxStudents);
            this.students = new List<Student>(students);
            groupName = "p87";
            specialization = "C++";
            course = 1;
        }

        public Group(Group group)
        {
            this.groupName = group.groupName;
            this.specialization = group.specialization;
            this.course = group.course;
            this.students = new List<Student>(group.students);
        }

        public void ShowGroup()
        {
            Console.WriteLine($"Group: {groupName}, Specialization: {specialization}, Course: {course}");
            Console.WriteLine("Students:");

            var sorted = students
                .OrderBy(s => s.GetSecondName())
                .ThenBy(s => s.GetName())
                .ToList();

            int i = 1;
            foreach (var student in sorted)
            {
                Console.WriteLine($"{i}. {student.GetSecondName()} {student.GetName()}");
                i++;
            }
        }

        public void AddStudent(Student student)
        {
            if (students.Count >= MaxStudents)
                throw new GroupFullException("The group is full", MaxStudents);
            students.Add(student);
        }

        public void TransferStudent(Group otherGroup, Student student)
        {
            if (!students.Contains(student))
                throw new TransferFailedException("No student found for transfer");
            if (otherGroup.students.Count >= MaxStudents)
                throw new GroupFullException("Target group is full", MaxStudents);

            students.Remove(student);
            otherGroup.students.Add(student);
        }

        public void ExpelAllFailed() => students.RemoveAll(s => s.GetExam() < 60);
        public void ExpelWorst()
        {
            if (students.Count == 0) return;
            var worst = students.OrderBy(s => s.GetExam()).First();
            students.Remove(worst);
        }

        public static bool operator ==(Group g1, Group g2)
        {
            if (ReferenceEquals(g1, g2)) return true;
            if (g1 is null || g2 is null) return false;
            return g1.students.Count == g2.students.Count;
        }

        public static bool operator !=(Group g1, Group g2) => !(g1 == g2);

        public override bool Equals(object? obj)
        {
            if (obj is Group g) return this == g;
            return false;
        }

        public override int GetHashCode() => students.Count.GetHashCode();
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Student student1 = new Student("John", "Jason", "Smith", 15, 6, 2002, "Main St", "123A", 1);
                Student student2 = new Student();
                Student student3 = new Student();

                student2.SetExam(105);
            }
            catch (StudentManagementException ex)
            {
                Console.WriteLine($"A student error occurred: {ex.Message}");
                if (ex is InvalidGradeException gradeEx)
                    Console.WriteLine($"Incorrect assessment: {gradeEx.Grade}");
            }
            finally
            {
                Console.WriteLine("Student verification completed.\n");
            }

            try
            {
                Group g1 = new Group();
                Group g2 = new Group();

                Student s1 = new Student("Alice", "Smith", "Johnson", 10, 5, 2001, "Oak St", "12", 2);
                Student s2 = new Student("Bob", "Brown", "Miller", 3, 3, 2000, "Pine St", "5", 3);

                g1.AddStudent(s1);
                g1.AddStudent(s2);

                g1.TransferStudent(g2, s2);
                g1.TransferStudent(g2, s2);
            }
            catch (GroupManagementException ex)
            {
                Console.WriteLine($"A group error has occurred: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Group verification complete.");
            }

            Student a = new Student();
            a.SetHomework(8);
            a.SetHomework(6);

            Student b = new Student();
            b.SetHomework(7);
            b.SetHomework(7);

            Console.WriteLine($"a == b? {a == b}");
            if (a) Console.WriteLine("a is successful");
        }
    }
}
