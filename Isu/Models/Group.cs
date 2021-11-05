using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Models
{
    public class Group
    {
        private readonly List<Student> _students;
        private Group(string name, CourseNumber courseNumber, List<Student> students, int maxStudentCount)
        {
            if (name.Length != 5) throw new IsuException("Wrong group name");
            GroupName = name;
            CourseNumber = courseNumber;
            _students = new List<Student>(students);
            MaxStudentCount = maxStudentCount;
        }

        public int MaxStudentCount { get; }

        public string GroupName { get; }

        public CourseNumber CourseNumber { get; }

        public IReadOnlyList<Student> Students => _students;

        public GroupBuilder ToBuilder()
        {
            GroupBuilder groupBuilder = new ();
            groupBuilder.WithName(GroupName)
                .WithStudents((List<Student>)Students)
                .WithCourseNumber(CourseNumber)
                .WithMaxStudentCount(MaxStudentCount);
            return groupBuilder;
        }

        public class GroupBuilder
        {
            private string _groupName;
            private CourseNumber _courseNumber;
            private List<Student> _students = new ();
            private int _maxStudentCount;

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

            public GroupBuilder WithMaxStudentCount(int count)
            {
                _maxStudentCount = count;
                return this;
            }

            public Group Build()
            {
                Group finalGroup = new (_groupName, _courseNumber, _students, _maxStudentCount);
                return finalGroup;
            }
        }
    }
}