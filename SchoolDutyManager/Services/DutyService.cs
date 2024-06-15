using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SchoolDutyManager.Services
{
    public class DutyService : IDutyService
    {
        private readonly List<Duty> duties;
        private readonly string filePath = "./Data/duties.json";

        public DutyService()
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

        public List<Duty> GetAllDuties()
        {
            return duties;
        }

        public Duty GetDutyById(int id)
        {
            return duties.FirstOrDefault(d => d.Id == id);
        }

        public void AddDuty(Duty duty)
        {
            duty.Id = duties.Count > 0 ? duties.Max(d => d.Id) + 1 : 1;
            duties.Add(duty);
            SaveToFile();
        }

        public void UpdateDuty(Duty duty)
        {
            var index = duties.FindIndex(d => d.Id == duty.Id);
            if (index != -1)
            {
                duties[index] = duty;
                SaveToFile();
            }
        }

        public void DeleteDuty(int id)
        {
            var duty = GetDutyById(id);
            if (duty != null)
            {
                duties.Remove(duty);
                SaveToFile();
            }
        }

        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(duties, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
