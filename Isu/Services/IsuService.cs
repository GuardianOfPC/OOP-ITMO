﻿using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _groups = new ();
        public Group AddGroup(string name)
        {
            Group group = new Group.GroupBuilder(name).Build();
            _groups.Add(group);
            Group outGroup = group.ToBuilder().WithName(name).Build();
            return outGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            Student student = new Student.StudentBuilder(name).WithGroup(group).Build();
            group.AddStudentToGroup(student);
            Student outStudent = student.ToBuilder().WithGroup(group).Build();
            return outStudent;
        }

        public Student GetStudent(int id)
        {
            foreach (Group group in _groups)
            {
                foreach (Student student in group.Students)
                {
                    if (Student.GetId() == id)
                    {
                        return student;
                    }
                }
            }

            throw new IsuException("No such Student");
        }

        public Student FindStudent(string name)
        {
            foreach (Group group in _groups)
            {
                foreach (Student student in group.Students)
                {
                    if (student.GetName() == name)
                    {
                        return student;
                    }
                }
            }

            return null;
        }

        public List<Student> FindStudents(string groupName)
        {
            foreach (Group group in _groups)
            {
                if (group.GetGroupName() == groupName)
                {
                    return (List<Student>)group.Students;
                }
            }

            return new List<Student>();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            foreach (Group group in _groups)
            {
                if (group.GetCourseNumber() == courseNumber)
                {
                    return (List<Student>)group.Students;
                }
            }

            return new List<Student>();
        }

        public Group FindGroup(string groupName)
        {
            foreach (Group group in _groups)
            {
                if (group.GetGroupName() == groupName)
                {
                    return group;
                }
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            List<Group> groups = new ();
            foreach (Group group in _groups)
            {
                if (group.GetCourseNumber() == courseNumber)
                {
                    groups.Add(group);
                }
            }

            return groups;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            Group oldGroup = student.GetGroup();
            if (oldGroup.Equals(newGroup))
            {
                throw new IsuException("Same group");
            }

            foreach (Group group in _groups)
            {
                group.RemoveStudent(student);
                AddStudent(newGroup, student.GetName());
            }

            if (!oldGroup.Equals(newGroup))
            {
                throw new IsuException("Group didn't change");
            }
        }
    }
}