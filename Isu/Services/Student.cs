namespace Isu.Services
{
    public class Student
    {
        private static int _id = 100000;
        private readonly string _name;
        private readonly Group _group;

        private Student(string name, Group group)
        {
            _id++;
            _name = name;
            _group = group;
        }

        public static int GetId()
        {
            return _id;
        }

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
            StudentBuilder studentBuilder = new StudentBuilder(_name).WithGroup(_group);
            return studentBuilder;
        }

        public class StudentBuilder
        {
            private string _name;
            private Group _group;

            public StudentBuilder(string name)
            {
                _name = name;
            }

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