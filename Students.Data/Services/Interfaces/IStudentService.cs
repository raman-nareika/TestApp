using System.Collections.Generic;
using Students.Data.Data;

namespace Students.Data.Services.Interfaces
{
    public interface IStudentService
    {
        void CreateStudent(string path, Student student);
        Student GetStudent(string path, int id);
        IEnumerable<Student> GetAllStudents(string path);
        void UpdateStudent(string path, Student student);
        void DeleteStudent(Student student);
        void DeleteStudent(string path, int id);
    }
}
