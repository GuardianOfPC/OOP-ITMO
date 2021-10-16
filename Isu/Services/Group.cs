using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Services
{
    public class Group
    {
        private readonly int _maxStudentCount = 30;
        private readonly string _groupName;
        private readonly CourseNumber _courseNumber;
        private readonly List<Student> _students;

        private Group(string name, CourseNumber courseNumber, List<Student> students)
        {
            if (name.Length != 5 && !name.Contains("M3"))
            {
                throw new IsuException("Wrong Group Name");
            }

            _groupName = name;
            _courseNumber = courseNumber;
            _students = new List<Student>();
            _students = students;
        }

        public IReadOnlyList<Student> Students => _students;

        public string GetGroupName()
        {
            return _groupName;
        }

        public int GetMaxStudentsCount()
        {
            return _maxStudentCount;
        }

        public CourseNumber GetCourseNumber()
        {
            return _courseNumber;
        }

        public GroupBuilder ToBuilder()
        {
            GroupBuilder groupBuilder = new ();
            groupBuilder.WithName(_groupName);
            groupBuilder.WithCourseNumber(_courseNumber);
            groupBuilder.WithStudents(_students);
            return groupBuilder;
        }

        public class GroupBuilder
        {
            private string _groupName;
            private CourseNumber _courseNumber;
            private List<Student> _students = new ();

            public GroupBuilder WithName(string name)
            {
                _groupName = name;
                return this;
            }

            public GroupBuilder WithCourseNumber(CourseNumber courseNumber)
            {
                _courseNumber = courseNumber;
                return this;
            }

            public GroupBuilder WithStudents(List<Student> students)
            {
                _students = students;
                return this;
            }

            public Group Build()
            {
                Group finalGroup = new (_groupName, _courseNumber, _students);
                return finalGroup;
            }
        }
    }
}