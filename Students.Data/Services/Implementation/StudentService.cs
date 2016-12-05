using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Students.Data.Data;
using Students.Data.Services.Interfaces;

namespace Students.Data.Services.Implementation
{
    public class StudentService : IStudentService
    {
        private const string RootXName = "Students";
        private const string StudentXName = "Student";
        private const string FirstNameElement = "FirstName";
        private const string LastNameElement = "Last";
        private const string IdElement = "Id";
        private const string AgeElement = "Age";
        private const string GenderElement = "Gender";

        public void CreateStudent(string path, Student student)
        {
            if (!File.Exists(path))
            {
                InitSourceFile(path, student);
            }
            else
            {
                try
                {
                    var doc = XDocument.Load(path);
                    var element = BuildXElementFromModel(student);

                    var root = doc.Element(RootXName);

                    if (root != null)
                    {
                        root.Add(element);
                        doc.Save(path);
                    }
                    else
                        InitSourceFile(path, student);
                }
                catch
                {
                    InitSourceFile(path, student);
                }
            }
        }

        public Student GetStudent(string path, int id)
        {
            if (!File.Exists(path))
                return null;

            var doc = XDocument.Load(path);
            var element = GetStudentXElementById(doc, id);
            var student = BuildFromXElement(element);

            return student;
        }

        public IEnumerable<Student> GetAllStudents(string path)
        {
            var students = new List<Student>();

            try
            {
                if (!File.Exists(path))
                    return students;

                var doc = XDocument.Load(path);

                foreach (var element in doc.Descendants(StudentXName))
                {
                    var student = BuildFromXElement(element);

                    if(student != null)
                        students.Add(student);
                }

                return students;
            }
            catch
            {
                return students;
            }
        }

        public void UpdateStudent(string path, Student student)
        {
            if (!File.Exists(path))
                return;

            var doc = XDocument.Load(path);
            var element = doc.Elements().Elements().FirstOrDefault(x => x?.Attribute(IdElement) != null &&
                                                                        student.Id.ToString().Equals(x.Attribute(IdElement)?.Value));

            if (element != null)
            {
                element.ReplaceWith(BuildXElementFromModel(student));
                doc.Save(path);
            }
        }

        public void DeleteStudent(Student student)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteStudent(string path, int id)
        {
            if (!File.Exists(path))
                return;

            var doc = XDocument.Load(path);
            var element = doc.Elements().Elements().FirstOrDefault(x => x?.Attribute(IdElement) != null && 
                                                    id.ToString().Equals(x.Attribute(IdElement)?.Value));

            if (element != null)
            {
                element.Remove();
                doc.Save(path);
            }
        }

        private XElement GetStudentXElementById(XDocument document, int id)
        {
            var element =
                document.Descendants(StudentXName).FirstOrDefault(x =>
                {
                    var xAttribute = x.Attribute(IdElement);

                    return xAttribute != null && x.Attribute(IdElement) != null && id.ToString().Equals(xAttribute.Value);
                });

            return element;
        }

        private Student BuildFromXElement(XElement xElement)
        {
            try
            {
                if (xElement == null) return null;

                var id = xElement.Attribute(IdElement)?.Value;
                var firstName = xElement.Element(FirstNameElement)?.Value;
                var gender = xElement.Element(GenderElement)?.Value;
                var age = xElement.Element(AgeElement)?.Value;
                var lastName = xElement.Element(LastNameElement)?.Value;

                if (age == null) throw new ArgumentNullException(nameof(age));
                if (firstName == null) throw new ArgumentNullException(nameof(firstName));
                if (gender == null) throw new ArgumentNullException(nameof(gender));
                if (id == null) throw new ArgumentNullException(nameof(id));
                if (lastName == null) throw new ArgumentNullException(nameof(lastName));

                return new Student
                {
                    Age = byte.Parse(age),
                    FirstName = firstName,
                    Gender = (Gender) int.Parse(gender),
                    Id = int.Parse(id),
                    Last = lastName
                };
            }
            catch(Exception e)
            {
                return null;
            }
        }

        private XElement BuildXElementFromModel(Student student)
        {
            try
            {
                var stud = new XElement(StudentXName, new XAttribute(IdElement, student.Id),
                    new XElement(FirstNameElement, student.FirstName),
                    new XElement(LastNameElement, student.Last),
                    new XElement(AgeElement, student.Age),
                    new XElement(GenderElement, (int) student.Gender));

                return stud;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        private void InitSourceFile(string fileName, Student student)
        {
            var element = BuildXElementFromModel(student);
            var newDoc = new XElement(RootXName, element);

            newDoc.Save(fileName);
        }
    }
}
