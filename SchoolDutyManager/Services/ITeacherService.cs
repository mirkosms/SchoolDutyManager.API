using System.Collections.Generic;
using SchoolDutyManager.Models;

namespace SchoolDutyManager.Services
{
    public interface ITeacherService
    {
        List<Teacher> GetAllTeachers();
        Teacher GetTeacherById(int id);
        void AddTeacher(Teacher teacher);
        bool UpdateTeacher(Teacher teacher);
        bool DeleteTeacher(int id);
    }
}
