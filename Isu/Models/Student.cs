using System;

namespace Isu.Models
{
    public class Student
    {
        private Student(string name, Group group)
        {
            Id = Guid.NewGuid();
            Name = name;
            Group = group;
        }

        public string Name { get; }
        public Group Group { get; }
        public Guid Id { get; }

        public StudentBuilder ToBuilder()
        {
            StudentBuilder studentBuilder = new StudentBuilder()
                .WithGroup(Group)
                .WithName(Name);
            return studentBuilder;
        }

        public class StudentBuilder
        {
            private string _name;
            private Group _group;

            public StudentBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public StudentBuilder WithGroup(Group group)
            {
                _group = group;
                return this;
            }

            public Student Build()
            {
                Student finalStudent = new (_name, _group);
                return finalStudent;
            }
        }
    }
}