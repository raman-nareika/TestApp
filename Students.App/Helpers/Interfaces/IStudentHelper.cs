using System.Collections.Generic;
using Students.App.ViewModel;

namespace Students.App.Helpers.Interfaces
{
    public interface IStudentHelper
    {
        List<StudentViewModel> LoadStudents();
        void DeleteStudent(StudentViewModel student);
        void DeleteStudents(IEnumerable<StudentViewModel> students);
        void Save(StudentViewModel studentViewModel);
        void Update(StudentViewModel studentViewModel);
    }
}
