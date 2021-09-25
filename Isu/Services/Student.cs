namespace Isu.Services
{
    public class Student
    {
        private static int _id = 100000;

        public Student(Group group, string name)
        {
            Group = group;
            Name = name;
            _id++;
        }

        public string Name { get; }
        public Group Group { get; set; }

        public static int GetId()
        {
            return _id;
        }
    }
}