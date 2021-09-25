using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _groups = new ();
        public Group AddGroup(string name)
        {
            Group group = new (name);
            _groups.Add(group);
            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            if (group.StudentsCount > 30)
            {
                throw new IsuException("Group limit exceeded");
            }

            Student student = new (group, name);
            group.Students.Add(student);
            group.StudentsCount++;
            return student;
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
                    if (student.Name == name)
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
                if (group.GroupName == groupName)
                {
                    return group.Students;
                }
            }

            return new List<Student>();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            foreach (Group group in _groups)
            {
                if (group.CourseNumber == courseNumber)
                {
                    return group.Students;
                }
            }

            return new List<Student>();
        }

        public Group FindGroup(string groupName)
        {
            foreach (Group group in _groups)
            {
                if (group.GroupName == groupName)
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
                if (group.CourseNumber == courseNumber)
                {
                    groups.Add(group);
                }
            }

            return groups;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            foreach (Group group in _groups)
            {
                group.Students.Remove(student);
                AddStudent(newGroup, student.Name);
            }

            if (student.Group == newGroup)
            {
                throw new IsuException("Same group");
            }

            if (student.Group != newGroup)
            {
                throw new IsuException("Group didn't change");
            }
        }
    }
}