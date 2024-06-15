using SchoolDutyManager.Models;
using System.Collections.Generic;

namespace SchoolDutyManager.Services
{
    public interface IDutySwapService
    {
        List<DutySwap> GetAllDutySwaps();
        DutySwap GetDutySwapById(int id);
        void CreateDutySwap(DutySwapRequestDto dutySwapRequestDto);
        bool ApproveDutySwap(int id, string userEmail);
        bool RejectDutySwap(int id, string userEmail);
    }
}
