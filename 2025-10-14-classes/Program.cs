namespace classes
{
    class Student
    {
        string? name;
        string? secondname;
        string? father;

        List<int> birthday = new List<int>();
        List<string> address = new List<string>();
        List<int> exams = new List<int>();
        List<int> homeworks = new List<int>();
        List<int> lessons = new List<int>();

        int number;
        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetSecondName(string secondname)
        {
            this.secondname = secondname;
        }
        public void SetFather(string father)
        {
            this.father = father;
        }
        public void SetBirthday(int day, int month, int year)
        {
            birthday.Clear();
            birthday.Add(day);
            birthday.Add(month);
            birthday.Add(year);
        }
        public void SetAddress(string street, string house)
        {
            birthday.Clear();
            address.Add(street);
            address.Add(house);
        }
        public void SetNumber(int number)
        {
            this.number = number;
        }
        public void SetExam(int exam)
        {
            exams.Add(exam);
        }
        public void SetHomework(int homework)
        {
            homeworks.Add(homework);
        }
        public void SetLesson(int lesson)
        {
            lessons.Add(lesson);
        }
        public string GetName()
        {
            return name!;
        }
        public string GetSecondName()
        {
            return secondname!;
        }
        public string GetFather()
        {
            return father!;
        }
        public int GetBirthday()
        {
            foreach (int number in birthday)
            {
                return number;
            }
            return 0;
        }
        public string GetAddress()
        {
            foreach (string number in address)
            {
                return number;
            }
            return "";
        }
        public int GetExam()
        {
            foreach (int number in exams)
            {
                return number;
            }
            return 0;
        }
        public int GetHomework()
        {
            foreach (int number in homeworks)
            {
                return number;
            }
            return 0;
        }
        public int GetLesson()
        {
            foreach (int number in lessons)
            {
                return number;
            }
            return 0;
        }
        public int GetNumber()
        {
            return number;
        }
        public Student(string name, string secondname, string father, int day, int month, int year, string street, string house, int number)
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
            for (int i = 0; i < count_lesson; i++)
            {
                SetLesson(lesson);
            }
            for (int i = 0; i < count_homework; i++)

            {
                SetHomework(homework);
            }
            for (int i = 0; i < count_exam; i++)
            {
                SetExam(exam);
            }
        }
        public Student() : this("NoName", "NoSecondName", "NoFather", 1, 1, 2000, "NoStreet", "NoHouse", 0)
        {

        } 
        
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Student student1 = new Student("John", "Doe", "Smith", 15, 6, 2002, "Main St", "123A", 1);
            Student student2 = new Student(5, 10, 3, 1, 90, 85);
            Student student3 = new Student();
            Console.WriteLine($"Student 1: {student1.GetName()} {student1.GetSecondName()}, Father: {student1.GetFather()}");
            Console.WriteLine($"Student 2 Exam Score: {student2.GetExam()}");
            Console.WriteLine($"Student 3 Name: {student3.GetName()}");
        }
    }
}
