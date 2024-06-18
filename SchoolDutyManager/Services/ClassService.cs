using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System;

namespace SchoolDutyManager.Services
{
    public class ClassService : IClassService
    {
        private List<Class> classes;
        private readonly string filePath = "./Data/classes.json";

        public ClassService()
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

        public List<Class> GetAll()
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

        public Class Get(int id)
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

        public void Add(Class classObj)
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

        public void Update(Class classObj)
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

        public void Delete(int id)
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

        private void SaveToFile()
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
