using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Array
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Завантажуємо збірку
            Assembly asml = Assembly.LoadFrom("./SampleLibrary.dll");

            // Виводимо список типів даних, оголошених у поточному модулі
            Console.WriteLine("Оголошені типи даних:");
            foreach (Type t in asml.GetTypes())
            {
                Console.WriteLine(t.FullName);
            }

            // Отримуємо тип даних зі збірки
            Type type = asml.GetType("SampleLibrary.Employee");

            // Пізнє зв’язування дозволяє створювати екземпляри певного типу,
            // а також використовувати їх під час виконання програми.

            // Використання пізнього зв’язування менш безпечне, оскільки при жорсткому кодуванні
            // всіх типів (раннє зв’язування) на етапі компіляції можна виявити багато помилок.
            // Водночас пізнє зв’язування дозволяє створювати розширювані застосунки,
            // коли додаткова функціональність програми невідома та може бути розроблена
            // і підключена сторонніми розробниками.

            // Ключову роль у пізньому зв’язуванні відіграє клас System.Activator.
            // За допомогою його статичного методу Activator.CreateInstance()
            // можна створювати екземпляри заданого типу.

            // SampleLibrary.Employee emp = new("Іван", "Іванов", 30, "Директор", 50000M);
            // emp.Print();

            // Створимо об'єкт класу Employee
            object person = Activator.CreateInstance(type,
                new object[] { "Іван", "Іванов", 30, "Директор", 50000M });

            Console.WriteLine();

            // Викликаємо метод Print у створеного об'єкта
            type.GetMethod("Print").Invoke(person, null); // person.Print();

            string[] files = Directory.GetFiles(".", "DLL/*.dll");
            Assembly[] asm = new Assembly[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                asm[i] = Assembly.LoadFrom(files[i]);
            }

            List<Type> types = new List<Type>();
            for (int i = 0; i < asm.Length; i++)
            {
                types.AddRange(asm[i].GetTypes());
            }

            List<MethodInfo> methodinfo = new List<MethodInfo>();
            foreach (Type t in types)
                methodinfo.AddRange(t.GetMethods());

            Console.WriteLine("Методи:\n\n");

            foreach (MethodInfo info in methodinfo)
                Console.WriteLine(info.Name);

            int[] A = new int[10];

            methodinfo[0].Invoke(null, new object[] { A }); // public static void Init(int[] A)

            Console.WriteLine("\nПочатковий масив:\n");
            methodinfo[1].Invoke(null, new object[] { A }); // public static void Print(int[] A)

            methodinfo[6].Invoke(null, new object[] { A }); // public static void Reverse(int[] A)

            Console.WriteLine("Масив після реверсування:\n");
            methodinfo[1].Invoke(null, new object[] { A }); // public static void Print(int[] A)

            methodinfo[7].Invoke(null, new object[] { A }); // public static void Neighbor(int[] A)

            Console.WriteLine("Масив після перетворення:\n");
            methodinfo[1].Invoke(null, new object[] { A }); // public static void Print(int[] A)

            Console.WriteLine("Сума елементів масиву: {0,4}",
                (int)methodinfo[12].Invoke(null, new object[] { A })); // public static int SumOfElements(int[] A)

            Console.WriteLine("Середнє арифметичне елементів масиву: {0,4}",
                (double)methodinfo[13].Invoke(null, new object[] { A })); // public static double Average(int[] A)
        }
    }
}