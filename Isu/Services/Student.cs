using System;

namespace Isu.Services
{
    public class Student
    {
        private readonly Guid _id;
        private readonly string _name;
        private readonly Group _group;

        private Student(string name, Group group)
        {
            _id = Guid.NewGuid();
            _name = name;
            _group = group;
        }

        public Guid Id => _id;
        public string GetName()
        {
            return _name;
        }

        public Group GetGroup()
        {
            return _group;
        }

        public StudentBuilder ToBuilder()
        {
            StudentBuilder studentBuilder = new StudentBuilder().WithGroup(_group).WithName(_name);
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