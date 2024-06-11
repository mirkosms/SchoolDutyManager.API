using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SchoolDutyManager.Services
{
    public static class DutyService
    {
        private static List<Duty> duties;
        private static readonly string filePath = "./Data/duties.json";

        static DutyService()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                duties = JsonSerializer.Deserialize<List<Duty>>(json);
            }
            else
            {
                duties = new List<Duty>();
            }
        }

        public static List<Duty> GetAll() => duties;

        public static Duty Get(int id) => duties.Find(d => d.Id == id);

        public static void Add(Duty duty)
        {
            duty.Id = duties.Count > 0 ? duties[^1].Id + 1 : 1;
            duties.Add(duty);
            SaveToFile();
        }

        public static void Update(Duty duty)
        {
            var index = duties.FindIndex(d => d.Id == duty.Id);
            if (index != -1)
            {
                duties[index] = duty;
                SaveToFile();
            }
        }

        public static void Delete(int id)
        {
            var duty = Get(id);
            if (duty != null)
            {
                duties.Remove(duty);
                SaveToFile();
            }
        }

        private static void SaveToFile()
        {
            var json = JsonSerializer.Serialize(duties, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
