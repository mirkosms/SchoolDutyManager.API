using SchoolDutyManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SchoolDutyManager.Services
{
    public class DutyService : IDutyService
    {
        private static List<Duty> duties;
        private static readonly string filePath = "./Data/duties.json";

        static DutyService()
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing DutyService: {ex.Message}");
            }
        }

        public List<Duty> GetAllDuties()
        {
            try
            {
                return duties;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllDuties: {ex.Message}");
                return null;
            }
        }

        public Duty GetDutyById(int id)
        {
            try
            {
                return duties.FirstOrDefault(d => d.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDutyById: {ex.Message}");
                return null;
            }
        }

        public void AddDuty(Duty duty)
        {
            try
            {
                duty.Id = duties.Count > 0 ? duties[^1].Id + 1 : 1;
                duties.Add(duty);
                SaveToFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddDuty: {ex.Message}");
            }
        }

        public void UpdateDuty(Duty duty)
        {
            try
            {
                var index = duties.FindIndex(d => d.Id == duty.Id);
                if (index != -1)
                {
                    duties[index] = duty;
                    SaveToFile();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateDuty: {ex.Message}");
            }
        }

        public void DeleteDuty(int id)
        {
            try
            {
                var duty = GetDutyById(id);
                if (duty != null)
                {
                    duties.Remove(duty);
                    SaveToFile();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteDuty: {ex.Message}");
            }
        }

        private static void SaveToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(duties, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving duties to file: {ex.Message}");
            }
        }
    }
}
