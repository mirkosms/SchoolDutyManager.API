using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SchoolDutyManager.Services
{
    public class TeacherService : ITeacherService
    {
        private List<Teacher> teachers;
        private readonly string filePath = "./Data/teachers.json";

        public TeacherService()
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

        public List<Teacher> GetAllTeachers() => teachers;

        public Teacher GetTeacherById(int id) => teachers.Find(t => t.Id == id);

        public void AddTeacher(Teacher teacher)
        {
            teacher.Id = teachers.Count > 0 ? teachers[^1].Id + 1 : 1;
            teachers.Add(teacher);
            SaveToFile();
        }

        public bool UpdateTeacher(Teacher teacher)
        {
            var index = teachers.FindIndex(t => t.Id == teacher.Id);
            if (index != -1)
            {
                teachers[index] = teacher;
                SaveToFile();
                return true;
            }
            return false;
        }

        public bool DeleteTeacher(int id)
        {
            var teacher = GetTeacherById(id);
            if (teacher != null)
            {
                teachers.Remove(teacher);
                SaveToFile();
                return true;
            }
            return false;
        }

        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(teachers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
