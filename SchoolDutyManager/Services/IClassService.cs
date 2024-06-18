using SchoolDutyManager.Models;
using System.Collections.Generic;

namespace SchoolDutyManager.Services
{
    public interface IClassService
    {
        List<Class> GetAll();
        Class Get(int id);
        void Add(Class classObj);
        void Update(Class classObj);
        void Delete(int id);
    }
}
