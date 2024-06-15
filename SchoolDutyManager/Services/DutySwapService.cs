using SchoolDutyManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SchoolDutyManager.Services
{
    public class DutySwapService : IDutySwapService
    {
        private readonly List<DutySwap> dutySwaps;
        private readonly string filePath = "./Data/dutySwaps.json";
        private readonly IDutyService _dutyService;
        private readonly IUserService _userService;

        public DutySwapService(IUserService userService, IDutyService dutyService)
        {
            _userService = userService;
            _dutyService = dutyService;
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

        public List<DutySwap> GetAllDutySwaps()
        {
            return dutySwaps;
        }

        public DutySwap GetDutySwapById(int id)
        {
            return dutySwaps.FirstOrDefault(ds => ds.Id == id);
        }

        public void CreateDutySwap(DutySwapRequestDto dutySwapRequestDto)
        {
            var dutySwap = new DutySwap
            {
                Id = dutySwaps.Count > 0 ? dutySwaps.Max(ds => ds.Id) + 1 : 1,
                OriginalDutyId = dutySwapRequestDto.OriginalDutyId,
                RequestedDutyId = dutySwapRequestDto.RequestedDutyId,
                Status = "Pending",
                InitiatingStudentId = dutySwapRequestDto.InitiatingStudentId,
                RespondingStudentId = dutySwapRequestDto.RespondingStudentId
            };
            dutySwaps.Add(dutySwap);
            SaveToFile();
        }

        public bool ApproveDutySwap(int id, string userEmail)
        {
            var dutySwap = GetDutySwapById(id);
            if (dutySwap == null)
            {
                return false;
            }

            var user = _userService.GetUserByEmail(userEmail);
            if (user == null)
            {
                return false;
            }

            if (user.Roles.Contains("Student") && dutySwap.RespondingStudentId != user.Id)
            {
                return false;
            }

            if (dutySwap.Status == "Pending" && user.Roles.Contains("Student"))
            {
                dutySwap.Status = "ApprovedByStudent";
            }
            else if (dutySwap.Status == "ApprovedByStudent" && (user.Roles.Contains("Teacher") || user.Roles.Contains("Admin")))
            {
                dutySwap.Status = "ApprovedByTeacher";
                UpdateDutiesFile(dutySwap); // Aktualizacja duties.json po zatwierdzeniu przez nauczyciela lub admina
            }
            else
            {
                return false;
            }

            SaveToFile();
            return true;
        }

        private void UpdateDutiesFile(DutySwap dutySwap)
        {
            var originalDuty = _dutyService.GetDutyById(dutySwap.OriginalDutyId);
            var requestedDuty = _dutyService.GetDutyById(dutySwap.RequestedDutyId);

            if (originalDuty != null && requestedDuty != null)
            {
                originalDuty.AssignedPeople.Remove(dutySwap.InitiatingStudentId);
                originalDuty.AssignedPeople.Add(dutySwap.RespondingStudentId);

                requestedDuty.AssignedPeople.Remove(dutySwap.RespondingStudentId);
                requestedDuty.AssignedPeople.Add(dutySwap.InitiatingStudentId);

                _dutyService.UpdateDuty(originalDuty);
                _dutyService.UpdateDuty(requestedDuty);
            }
        }

        public bool RejectDutySwap(int id, string userEmail)
        {
            var dutySwap = GetDutySwapById(id);
            if (dutySwap == null)
            {
                return false;
            }

            var user = _userService.GetUserByEmail(userEmail);
            if (user == null)
            {
                return false;
            }

            if (user.Roles.Contains("Student") && dutySwap.RespondingStudentId != user.Id)
            {
                return false;
            }

            dutySwap.Status = "Rejected";
            SaveToFile();
            return true;
        }

        public void DeleteDutySwap(int id)
        {
            var dutySwap = GetDutySwapById(id);
            if (dutySwap != null)
            {
                dutySwaps.Remove(dutySwap);
                SaveToFile();
            }
        }

        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(dutySwaps, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
