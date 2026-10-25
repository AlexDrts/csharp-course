using System;
using System.Collections;
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

        public string Name
        {
            get => name!;
            set => SetName(value);
        }

        public string Lastname
        {
            get => secondname!;
            set => SetSecondName(value);
        }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - Year;
                if (today.Month < Month || (today.Month == Month && today.Day < Day))
                    age--;
                return age;
            }
        }

        public double AverageGrade => (GetExam() + GetHomework() + GetLesson()) / 3.0;

        public void SetName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidStudentDataException("The name cannot be empty");
            name = value;
        }

        public void SetSecondName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidStudentDataException("The surname cannot be empty");
            secondname = value;
        }

        public void SetFather(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidStudentDataException("The father's name cannot be left blank");
            father = value;
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

        public double GetExam() => exams.Count > 0 ? exams.Average() : 0;
        public double GetHomework() => homeworks.Count > 0 ? homeworks.Average() : 0;
        public double GetLesson() => lessons.Count > 0 ? lessons.Average() : 0;

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

        public Student() : this("Alex", "Alexeivich", "Vladimir", 1, 1, 2000, "Abrikosovaia", "18", 0) { }

        public static bool operator ==(Student s1, Student s2)
        {
            if (ReferenceEquals(s1, s2)) return true;
            if (s1 is null || s2 is null) return false;
            return s1.AverageGrade == s2.AverageGrade;
        }

        public static bool operator !=(Student s1, Student s2) => !(s1 == s2);

        public static bool operator true(Student s) => s.AverageGrade >= 7;
        public static bool operator false(Student s) => s.AverageGrade < 7;

        public override bool Equals(object? obj)
        {
            if (obj is Student s) return this == s;
            return false;
        }

        public override int GetHashCode() => AverageGrade.GetHashCode();


        //---------------------------------------------------------------------
        public class AverageGradeComparer : IComparer<Student>
        {
            public int Compare(Student? x, Student? y)
            {
                if (x is null || y is null)
                    throw new ArgumentNullException("Student argument is null");

                int result = x.AverageGrade.CompareTo(y.AverageGrade);
                if (result == 0)
                {
                    return string.Compare(x.Lastname, y.Lastname, true);
                }

                return result;
            }
        }

        public class FullNameComparer : IComparer<Student>
        {
            public int Compare(Student? x, Student? y)
            {
                if (x is null || y is null)
                    throw new ArgumentNullException("Student argument is null");

                int nameCmp = string.Compare(x.Lastname, y.Lastname, true);
                if (nameCmp != 0) return nameCmp;

                nameCmp = string.Compare(x.Name, y.Name, true);
                if (nameCmp != 0) return nameCmp;

                return y.AverageGrade.CompareTo(x.AverageGrade);
            }
        }
    }


    class Group : IEnumerable<Student>
    {
        List<Student> students;
        string groupName;
        string specialization;
        int course;
        const int MaxStudents = 10;

        public int Count => students.Count;
        public string Specialization { get => specialization; set => specialization = value; }
        public int Course { get => course; set => course = value; }

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

        public Student this[int index]
        {
            get
            {
                if (index < 0 || index >= students.Count)
                    throw new IndexOutOfRangeException("Student index out of range");
                return students[index];
            }
            set
            {
                if (index < 0 || index >= students.Count)
                    throw new IndexOutOfRangeException("Student index out of range");
                students[index] = value;
            }
        }

        public void ShowGroup()
        {
            Console.WriteLine($"Group: {groupName}, Specialization: {specialization}, Course: {course}");
            Console.WriteLine("Students:");

            var sorted = students
                .OrderBy(s => s.Lastname)
                .ThenBy(s => s.Name)
                .ToList();

            int i = 1;
            foreach (var student in sorted)
            {
                Console.WriteLine($"{i}. {student.Lastname} {student.Name}, Age: {student.Age}, AvgGrade: {student.AverageGrade:F2}");
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


        //---------------------------------------------------------------------
        private class GroupEnumerator : IEnumerator<Student>
        {
            private readonly List<Student> _students;
            private int index = -1;

            public GroupEnumerator(List<Student> students)
            {
                _students = students;
            }

            public Student Current => _students[index];

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                index++;
                return index < _students.Count;
            }

            public void Reset()
            {
                index = -1;
            }

            public void Dispose() { }
        }

        public IEnumerator<Student> GetEnumerator()
        {
            return new GroupEnumerator(students);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Student s1 = new Student("John", "Doe", "Smith", 15, 6, 2002, "Main St", "123A", 1);
            Student s2 = new Student();
            s2.SetHomework(8);
            s2.SetLesson(9);
            s2.SetExam(7);

            Group g1 = new Group();
            g1.AddStudent(s1);
            g1.AddStudent(s2);

            Console.WriteLine($"First student name: {g1[0].Name}, Age: {g1[0].Age}, AvgGrade: {g1[0].AverageGrade:F2}");
            Console.WriteLine($"Second student name: {g1[1].Name}, Age: {g1[1].Age}, AvgGrade: {g1[1].AverageGrade:F2}");

            g1[1] = new Student("Alice", "Smith", "Johnson", 10, 5, 2001, "Oak St", "12", 2);

            Console.WriteLine("\nGroup info:");
            g1.ShowGroup();

            Console.WriteLine("\nIterating:");
            foreach (var st in g1)
                Console.WriteLine(st.Name);
        }
    }
}