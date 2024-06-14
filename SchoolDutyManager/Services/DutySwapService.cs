using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SchoolDutyManager.Services
{
    public class DutySwapService : IDutySwapService
    {
        private static List<DutySwap> dutySwaps;
        private static readonly string filePath = "./Data/dutySwaps.json";

        static DutySwapService()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                dutySwaps = JsonSerializer.Deserialize<List<DutySwap>>(json);
            }
            else
            {
                dutySwaps = new List<DutySwap>();
            }
        }

        public List<DutySwap> GetAll() => dutySwaps;

        public DutySwap Get(int id) => dutySwaps.FirstOrDefault(d => d.Id == id);

        public void Add(DutySwap dutySwap)
        {
            dutySwap.Id = dutySwaps.Count > 0 ? dutySwaps.Max(d => d.Id) + 1 : 1;
            dutySwaps.Add(dutySwap);
            SaveToFile();
        }

        public void Update(DutySwap dutySwap)
        {
            var index = dutySwaps.FindIndex(d => d.Id == dutySwap.Id);
            if (index != -1)
            {
                dutySwaps[index] = dutySwap;
                SaveToFile();
            }
        }

        private static void SaveToFile()
        {
            var json = JsonSerializer.Serialize(dutySwaps, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
