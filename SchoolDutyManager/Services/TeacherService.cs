using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SchoolDutyManager.Services
{
    public static class TeacherService
    {
        private static List<Teacher> teachers;
        private static readonly string filePath = "./Data/teachers.json";

        static TeacherService()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                teachers = JsonSerializer.Deserialize<List<Teacher>>(json);
            }
            else
            {
                teachers = new List<Teacher>();
            }
        }

        public static List<Teacher> GetAll() => teachers;

        public static Teacher Get(int id) => teachers.Find(t => t.Id == id);

        public static void Add(Teacher teacher)
        {
            teacher.Id = teachers.Count > 0 ? teachers[^1].Id + 1 : 1;
            teachers.Add(teacher);
            SaveToFile();
        }

        public static void Update(Teacher teacher)
        {
            var index = teachers.FindIndex(t => t.Id == teacher.Id);
            if (index != -1)
            {
                teachers[index] = teacher;
                SaveToFile();
            }
        }

        public static void Delete(int id)
        {
            var teacher = Get(id);
            if (teacher != null)
            {
                teachers.Remove(teacher);
                SaveToFile();
            }
        }

        private static void SaveToFile()
        {
            var json = JsonSerializer.Serialize(teachers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
