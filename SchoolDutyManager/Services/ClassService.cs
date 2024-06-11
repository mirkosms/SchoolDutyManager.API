using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SchoolDutyManager.Services
{
    public static class ClassService
    {
        private static List<Class> classes;
        private static readonly string filePath = "./Data/classes.json";

        static ClassService()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                classes = JsonSerializer.Deserialize<List<Class>>(json);
            }
            else
            {
                classes = new List<Class>();
            }
        }

        public static List<Class> GetAll() => classes;

        public static Class Get(int id) => classes.Find(c => c.Id == id);

        public static void Add(Class classObj)
        {
            classObj.Id = classes.Count > 0 ? classes[^1].Id + 1 : 1;
            classes.Add(classObj);
            SaveToFile();
        }

        public static void Update(Class classObj)
        {
            var index = classes.FindIndex(c => c.Id == classObj.Id);
            if (index != -1)
            {
                classes[index] = classObj;
                SaveToFile();
            }
        }

        public static void Delete(int id)
        {
            var classObj = Get(id);
            if (classObj != null)
            {
                classes.Remove(classObj);
                SaveToFile();
            }
        }

        private static void SaveToFile()
        {
            var json = JsonSerializer.Serialize(classes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
