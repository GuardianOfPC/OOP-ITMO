using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Services
{
    public class Group
    {
        public Group(string name)
        {
            if (name.Length == 5 && name[0] == 'M')
            {
                 GroupName = name;
                 Students = new List<Student>();
                 CourseNumber.Number = name[2];
            }
            else
            {
                throw new IsuException("Wrong Name");
            }
        }

        public int StudentsCount { get; set; }
        public CourseNumber CourseNumber { get; } = new ();
        public List<Student> Students { get; }
        public string GroupName { get; set; }
    }
}