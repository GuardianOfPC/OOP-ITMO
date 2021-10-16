using System;
using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _groups = new ();
        public Group AddGroup(string name)
        {
            Group group = new Group.GroupBuilder().WithName(name).Build();
            _groups.Add(group);
            Group outGroup = group.ToBuilder().WithName(name).Build();
            return outGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            if (group.Students.Count > group.GetMaxStudentsCount())
            {
                throw new IsuException("Students exceeded");
            }

            Student student = new Student.StudentBuilder().WithName(name).WithGroup(group).Build();
            _groups.Remove(group);
            var oldStudents = (List<Student>)group.Students;
            oldStudents.Add(student);
            _groups.Add(group.ToBuilder().WithStudents(oldStudents).Build());
            return student;
        }

        public Student GetStudent(Guid id)
        {
            foreach (Group group in _groups)
            {
                foreach (Student student in group.Students)
                {
                    if (student.Id == id)
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

            _groups.Remove(oldGroup);
            _groups.Remove(newGroup);

            var oldStudents = (List<Student>)oldGroup.Students;
            oldStudents.Remove(student);
            Group temp = oldGroup.ToBuilder().WithStudents(oldStudents).Build();
            _groups.Add(temp);

            var newStudents = (List<Student>)newGroup.Students;
            newStudents.Add(student);
            Group temp1 = newGroup.ToBuilder().WithStudents(newStudents).Build();
            _groups.Add(temp1);
        }
    }
}