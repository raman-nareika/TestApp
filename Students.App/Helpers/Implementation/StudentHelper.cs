using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Students.App.Helpers.Interfaces;
using Students.App.ViewModel;
using Students.Data.Data;
using Students.Data.Services.Interfaces;

namespace Students.App.Helpers.Implementation
{
    public class StudentHelper : IStudentHelper
    {
        private readonly IStudentFacade _studentFacade;

        public StudentHelper(IStudentFacade studentFacade)
        {
            _studentFacade = studentFacade;
        }

        public List<StudentViewModel> LoadStudents()
        {
            var studs = _studentFacade.GetAllStudents();
            var students = Mapper.Map<IEnumerable<Student>, IEnumerable<StudentViewModel>>(studs);

            return students.ToList();
        }

        public void DeleteStudent(StudentViewModel studentViewModel)
        {
            _studentFacade.DeleteStudent(studentViewModel.Id);
        }

        public void DeleteStudents(IEnumerable<StudentViewModel> students)
        {
            throw new System.NotImplementedException();
        }

        public void Save(StudentViewModel studentViewModel)
        {
            var student = Mapper.Map<Student>(studentViewModel);

            _studentFacade.CreateStudent(student);
        }

        public void Update(StudentViewModel studentViewModel)
        {
            var student = Mapper.Map<Student>(studentViewModel);

            _studentFacade.UpdateStudent(student);
        }
    }
}
