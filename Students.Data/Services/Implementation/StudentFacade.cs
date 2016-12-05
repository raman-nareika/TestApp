using System.Collections.Generic;
using Students.Data.Data;
using Students.Data.Services.Interfaces;

namespace Students.Data.Services.Implementation
{
    public class StudentFacade : IStudentFacade
    {
        private const string DefaultFileName = "Students.xml";

        private readonly ISettingsService _settingsService;
        private readonly IStudentService _studentService;

        public StudentFacade(ISettingsService settingsService, IStudentService studentService)
        {
            _settingsService = settingsService;
            _studentService = studentService;
        }

        public void CreateStudent(Student student)
        {
            var fileName = BuildFileName();

            _studentService.CreateStudent(fileName, student);
        }

        public Student GetStudent(int id)
        {
            var fileName = BuildFileName();
            var student = _studentService.GetStudent(fileName, id);

            return student;
        }

        public IEnumerable<Student> GetAllStudents()
        {
            var fileName = BuildFileName();
            var students = _studentService.GetAllStudents(fileName);

            return students;
        }

        public void UpdateStudent(Student student)
        {
            var fileName = BuildFileName();

            _studentService.UpdateStudent(fileName, student);
        }

        public void DeleteStudent(Student student)
        {
            DeleteStudent(student.Id);
        }

        public void DeleteStudent(int id)
        {
            var fileName = BuildFileName();

            _studentService.DeleteStudent(fileName, id);
        }

        private string BuildFileName()
        {
            return _settingsService.Open()?.SourceFilePath ?? DefaultFileName;
        }
    }
}
