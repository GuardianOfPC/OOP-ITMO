﻿using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Services
{
    public class Group
    {
        private readonly string _groupName;
        private readonly CourseNumber _courseNumber;
        private readonly List<Student> _students;

        private Group(string name, CourseNumber courseNumber, List<Student> students)
        {
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

        public CourseNumber GetCourseNumber()
        {
            return _courseNumber;
        }

        public GroupBuilder ToBuilder()
        {
            GroupBuilder groupBuilder = new (_groupName);
            groupBuilder.WithCourseNumber(_courseNumber);
            groupBuilder.WithStudents(_students);
            return groupBuilder;
        }

        public class GroupBuilder
        {
            private const int MaxStudentCount = 30;
            private string _groupName;
            private CourseNumber _courseNumber;
            private List<Student> _students;

            public GroupBuilder(string name)
            {
                if (name.Length != 5 && !name.Contains("M3"))
                {
                    throw new IsuException("Wrong Group Name");
                }

                _courseNumber = (CourseNumber)name[1];

                _groupName = name;
                _students = new List<Student>();
            }

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

            public GroupBuilder AddStudentToGroup(Student student)
            {
                if (_students.Count > MaxStudentCount)
                {
                    throw new IsuException("Students exceeded");
                }

                _students.Add(student);
                return this;
            }

            public GroupBuilder RemoveStudent(Student student)
            {
                _students.Remove(student);
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