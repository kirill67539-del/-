using System;
using System.Collections.Generic;
// Интерфейс задаёт методы, которые должен реализовать каждый студент
interface IStudent
{
    double Average();
    string CourseInfo();
}
class Student : IStudent
{
    // Имя доступно для чтения извне, но изменить его можно только внутри класса
    public string Name { get; private set; }
    // Массив хранит оценки студента
    private int[] grades;
    // Конструктор получает имя и оценки при создании объекта
    public Student(string name, int[] marks)
    {
        Name = name;
        grades = marks;
    }
    public double Average()
    {
        double sum = 0;
        // Перебираем все оценки и складываем их
        foreach (int mark in grades)
            sum += mark;
        // Если оценки есть — вычисляем среднее, иначе возвращаем 0.
        // Оператор ? позволяет выбрать результат в зависимости от условия
        return grades.Length > 0 ? sum / grades.Length : 0;
    }
    // virtual разрешает дочерним классам переопределить этот метод
    public virtual string CourseInfo() => "Курс не указан";
}
// Класс студента первого курса наследует свойства и методы Student
class FirstYearStudent : Student
{
    public FirstYearStudent(string name, int[] marks) : base(name, marks) { }
    // override заменяет метод родительского класса своей реализацией
    public override string CourseInfo() => "1 курс";
}
class SecondYearStudent : Student
{
    public SecondYearStudent(string name, int[] marks) : base(name, marks) { }
    public override string CourseInfo() => "2 курс";
}
class ThirdYearStudent : Student
{
    public ThirdYearStudent(string name, int[] marks) : base(name, marks) { }
    public override string CourseInfo() => "3 курс";
}
class Program
{
    static void Main()
    {
        // Создаём студентов разных курсов с их оценками.
        // Список типа IStudent может хранить объекты классов, которые реализуют этот интерфейс
        List<IStudent> students = new List<IStudent>
        {
            new FirstYearStudent("Анна", new int[] { 9, 7, 9, 10, 8 }),
            new SecondYearStudent("Иван", new int[] { 7, 6, 6, 6, 8 }),
            new ThirdYearStudent("Мария", new int[] { 10, 9, 7, 9, 7 })
        };
        Console.WriteLine("Список студентов");
        // Перебираем список и обрабатываем каждого студента
        foreach (IStudent student in students)
        {
            // Преобразуем IStudent в Student, чтобы получить доступ к свойству Name.
            Student s = (Student)student;
            Console.WriteLine($"Имя: {s.Name}");
            // Вызовется версия CourseInfo() из класса конкретного студента
            Console.WriteLine($"Курс: {student.CourseInfo()}");
            // Выводим средний балл с двумя знаками после запятой
            Console.WriteLine($"Средний балл: {student.Average():F2}");
            Console.WriteLine();
        }
    }
}