using System;

namespace Students.Data.Data
{
    [Serializable]
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Last { get; set; }
        public byte Age { get; set; }
        public Gender Gender { get; set; }
    }
}
