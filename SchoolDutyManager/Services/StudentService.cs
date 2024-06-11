using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SchoolDutyManager.Services
{
    public static class StudentService
    {
        private static List<Student> students;
        private static readonly string filePath = "./Data/students.json";

        static StudentService()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                students = JsonSerializer.Deserialize<List<Student>>(json);
            }
            else
            {
                students = new List<Student>();
            }
        }

        public static List<Student> GetAll() => students;

        public static Student Get(int id) => students.Find(s => s.Id == id);

        public static void Add(Student student)
        {
            student.Id = students.Count > 0 ? students[^1].Id + 1 : 1;
            students.Add(student);
            SaveToFile();
        }

        public static void Update(Student student)
        {
            var index = students.FindIndex(s => s.Id == student.Id);
            if (index != -1)
            {
                students[index] = student;
                SaveToFile();
            }
        }

        public static void Delete(int id)
        {
            var student = Get(id);
            if (student != null)
            {
                students.Remove(student);
                SaveToFile();
            }
        }

        private static void SaveToFile()
        {
            var json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
