using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System;

namespace SchoolDutyManager.Services
{
    public static class ClassService
    {
        private static List<Class> classes;
        private static readonly string filePath = "./Data/classes.json";

        static ClassService()
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing ClassService: {ex.Message}");
            }
        }

        public static List<Class> GetAll()
        {
            try
            {
                return classes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAll: {ex.Message}");
                return null;
            }
        }

        public static Class Get(int id)
        {
            try
            {
                return classes.Find(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Get: {ex.Message}");
                return null;
            }
        }

        public static void Add(Class classObj)
        {
            try
            {
                classObj.Id = classes.Count > 0 ? classes[^1].Id + 1 : 1;
                classes.Add(classObj);
                SaveToFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Add: {ex.Message}");
            }
        }

        public static void Update(Class classObj)
        {
            try
            {
                var index = classes.FindIndex(c => c.Id == classObj.Id);
                if (index != -1)
                {
                    classes[index] = classObj;
                    SaveToFile();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
            }
        }

        public static void Delete(int id)
        {
            try
            {
                var classObj = Get(id);
                if (classObj != null)
                {
                    classes.Remove(classObj);
                    SaveToFile();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}");
            }
        }

        private static void SaveToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(classes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving classes to file: {ex.Message}");
            }
        }
    }
}
