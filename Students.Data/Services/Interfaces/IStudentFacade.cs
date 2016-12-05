using System.Collections.Generic;
using Students.Data.Data;

namespace Students.Data.Services.Interfaces
{
    public interface IStudentFacade
    {
        void CreateStudent(Student student);
        Student GetStudent(int id);
        IEnumerable<Student> GetAllStudents();
        void UpdateStudent(Student student);
        void DeleteStudent(Student student);
        void DeleteStudent(int id);
    }
}
