using SchoolDutyManager.Models;
using System.Collections.Generic;

namespace SchoolDutyManager.Services
{
    public interface IDutyService
    {
        List<Duty> GetAllDuties();
        Duty GetDutyById(int id);
        void AddDuty(Duty duty);
        void UpdateDuty(Duty duty);
        void DeleteDuty(int id);
    }
}
