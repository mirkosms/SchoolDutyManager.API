using SchoolDutyManager.Models;
using System.Collections.Generic;

namespace SchoolDutyManager.Services
{
    public interface IDutySwapService
    {
        List<DutySwap> GetAll();
        DutySwap Get(int id);
        void Add(DutySwap dutySwap);
        void Update(DutySwap dutySwap);
    }
}
